using DataStructures;
using Lexer_Module;
using Parser_Module.Events;
using Evaluator_Module;
using System;
using System.Collections.Generic;
using Errors;
using System.Linq;

namespace Parser_Module
{
    class Parser
    {
        private TokenQueue tokQueue;
      
        public Parser(List<Token> tokens) { tokQueue = new TokenQueue(tokens); }

        public List<Step> ParseTokens() // Due to the size of this method I am not using IEnumerable 'yield return' as it is hard to track nested return statements.
        {
            List<Step> EvaluationSteps = new List<Step>();

            while (tokQueue.More()) // While more tokens in queue (returns bool)
            {
                Token nextTok = tokQueue.MoveNext(); // pop next out of TokenQueue

                if (nextTok.Type().Equals("identifier"))
                    // All statements in our language begin with identifiers.
                    // We do not know what we have at this point, so let's check the identifier to see which tokens should follow after.
                {
                    if (Syntax.IsType(nextTok.Value()))
                    // If it is a var type, e.g "int", "string" - if it is, this is a variable declaration ("int x = 0;")
                    {
                        /*
                         * EXPECTED PATTERN: varType varName = expr;
                         * e.g int x = 2 + y*10;
                         * e.g string testing = "Hello World!";
                         */

                        VarDeclare varDeclare = CaptureVarDeclare(nextTok.Value()); // Call method with argument storing the type of var being declared, e.g 'string'

                        EvaluationSteps.Add(varDeclare); // Add Event object to the overall list of 'Steps' for the Evaluator module
                    }
                    else if (nextTok.Value().ToLower().Equals("if"))
                    // Start of an if statement
                    {
                        /*
                         * EXPECTED PATTERN: if(operands) { codeblock }
                         * e.g if (x > 0) {
                         *      output(x);
                         *      x = 0;
                         *     }
                         */

                        IfStatement ifState = CaptureIfStatement(); // Capture all useful information of the following if statements

                        // We COULD have an else statement, so let's check the next token
                        // First check there are still MORE tokens to check to avoid out of range errors
                        // Then check it's an IDENTIFIER ('else')
                        if (tokQueue.More() && tokQueue.Next().Type().Equals("identifier") && tokQueue.Next().Value().Equals("else"))
                        {
                            // If next token is 'else' and an identifier
                            ElseStatement elseState = CaptureElseStatement();
                            EvaluationSteps.Add(ifState);
                            EvaluationSteps.Add(elseState);
                            // Add if state then else directly after (ordered list!)
                        }
                        else EvaluationSteps.Add(ifState); // if no 'else' statement exists just add the if statement

                    }

                    else if (nextTok.Value().ToLower().Equals("while"))
                    {
                        IfStatement template = CaptureIfStatement(); // Trick the program to think it's capturing an if statement
                        WhileLoop whileLoop = new WhileLoop(template.GetCBContents(), template.GetCondition());
                        // Reuse code from the if statement because while & if follow the exact same structure:
                        // while (condition) { codeblock }
                        // if (condition) { codeblock }
                        // We just captured an if statement 'template' then used the information it collected to create a while loop instead

                        EvaluationSteps.Add(whileLoop);
                    }

                    else if (nextTok.Value().ToLower().Equals("func"))
                    {
                        FuncDeclare funcDec = CaptureFunctionDeclare();
                        /*
                         * EXPECTED PATTERN: func funcName(params) {
                         *  <codeblock>
                         * }
                         * 
                         * OR
                         * 
                         * func funcName(params) returns returnType {
                         *      <codeblock>
                         * }
                         */
                        EvaluationSteps.Add(funcDec);
                    }

                    else if (nextTok.Value().ToLower().Equals("return"))
                    {
                        // e.g 'return x + 2;'
                        // Expect expr to proceed then a ';'
                        List<Token> expr = new List<Token>();

                        while (tokQueue.More() && !GrammarTokenCheck(tokQueue.Next(), ";")) // while there are more tokens in expression. Loops UP TO (not inclusive) the ';' token
                        {
                            expr.Add(tokQueue.MoveNext());
                        }

                        if (!GrammarTokenCheck(tokQueue.MoveNext(), ";")) throw new SyntaxError(); // did not end with a ';', cause error

                        EvaluationSteps.Add(new ReturnStatement(expr));
                    }

                    else if (GrammarTokenCheck(tokQueue.Next(), "("))
                      // This condition will also return true if it finds an if/while statement, so it is AFTER the check for those.
                      // As we're using else if, if the program didn't recognise a 'while' or 'if' statement, we will reach this check
                      // We can GUARANTEE now that this must be a function call as 'if(){}' and 'while(){}' have been ruled out
                    {
                        /*
                         * EXPECTED PATTERN: funcName(expr); // Can take any expression!
                         * e.g output("Testing");
                         * e.g output(1 + 23);
                         * e.g output(x);
                         */

                        tokQueue.MoveNext(); // Skip the '(' token
                        // Remember, nextTok still holds the value of the token before '('
                        // This is the name of our function ('funcName')

                        FuncCall funcCall = CaptureFunctionCall(nextTok.Value()); // Pass the function name, e.g 'output'
                        EvaluationSteps.Add(funcCall);
                    }
                    else if (GrammarTokenCheck(tokQueue.Next(), "=")) // .Next() is PEEK not POP.
                        // Check if the token AFTER this one is "="
                    {
                        /*
                         * EXPECTED PATTERN: varName = expr;
                         * e.g x = 2 + y*10;
                         * e.g testing = "Hello World!";
                         */

                        tokQueue.MoveNext(); // Skip the '=' token
                        // Remember, nextTok still holds the value of the token before the '='
                        // This is the name of our variable to change ('varName')

                        VarChange varChan = CaptureVarChange(nextTok.Value());
                        EvaluationSteps.Add(varChan);
                    }
                    else throw new SyntaxError();
                    // If there is a rogue 'else' statement it will be caught in this
                    // Else statements are not 'looked' for on there own, they are only recognised when an if statement is found
                } else throw new SyntaxError(); // Statement doesn't begin with identifier - throw error.
            }

            return EvaluationSteps;
        }

        public VarDeclare CaptureVarDeclare(string varType)
        {
            // Before creating a VarDeclare object, we should collect all the data we need while checking we actually have it in the right format.
            // We have already been given the 'type' token of the variable (it is an argument of this function)

            Token nextTok = tokQueue.MoveNext(); // Move to next token in Q

            // We expect a variable name to be next, which will be token type 'identifier'. If it is not, we have a syntax error!
            string varName; // Cannot declare a variable inside the if statement scope, declare it here

            if (nextTok.Type().Equals("identifier")) varName = nextTok.Value(); // Collect Token with name of the variable
            else throw new SyntaxError(); // Throw error if variable name is not an 'identifier' token (invalid syntax)

            // Next token after varName should be an "="
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "=")) throw new SyntaxError(); // Throw syntax error if this is not found, and also move along queue index

            // We have varType and varName, now we need the tokens that form an expression to represent varValue.
            // This can be any amount of tokens, so we must collect them all
            List<Token> varValue = new List<Token>();

            while (tokQueue.More() && !GrammarTokenCheck(tokQueue.Next(), ";")) // Capture all tokens up to the end of the declaration (indicated by ";")
            {
                varValue.Add(tokQueue.MoveNext()); // Move along queue index and add each element it returns
            }


            if (tokQueue.More()) tokQueue.MoveNext(); // Done! Now skip over the ";" - in the while loop we stopped when we 'peeked' it, but it isn't 'popped' out the queue yet.
            else throw new SyntaxError(); // Just make sure that it actually existed and loop didn't stop as it ran out of tokens

            // We have all the data we need - we can create a VarDeclare object now

            return new VarDeclare(varType, varName, varValue);
        }

        public IfStatement CaptureIfStatement()
        {
            List<Step> codeBlockContents = new List<Step>();

            // Next token after 'if' should be '('
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "(")) throw new SyntaxError(); // Check next token while simulatneously moving along queue

            // Next token(s) should be operands to form a 'condition'
            // These token(s) will be inside ( )
            // e.g 'if (x > 0) {}' ==> Capture the "x > 0"
            List<Token> condition = CollectInsideBrackets("(", ")", ref tokQueue);


            // Next token after ')' should be '{'
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "{")) throw new SyntaxError(); 
            // Check next token while simulatneously moving along queue

            // Next token(s) should be all be EVERYTHING inside the code block { }
            // Once again, we need to collect these so we can parse them
            List<Token> codeBlockTokens = CollectInsideBrackets("{", "}", ref tokQueue);

            Parser parseTokens = new Parser(codeBlockTokens); // Create new parser object with only codeblock tokens
            codeBlockContents = parseTokens.ParseTokens(); 
            // (recursion) Call parse function to parse the codeblock tokens and output a list of Step
            // This will parse codeblock nests from the deepest nest to the furthest out

            return new IfStatement(codeBlockContents, condition);
        }

        public ElseStatement CaptureElseStatement()
        {
            List<Step> codeBlockContents = new List<Step>();
            // Called when Next() token is 'else' so we need to skip it:
            tokQueue.MoveNext();


            // We should be at the beginning of the else codeblock now
            // e.g: else { output("Hi"); }
            //           ^ we are here

            // Check next tok is the '{':
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "{")) throw new SyntaxError(); // Check next token while simulatneously moving along queue

            // Next token(s) should all be programming statements inside the code block { }
            // We need to collect these so we can parse them
            List<Token> codeBlockTokens = CollectInsideBrackets("{", "}", ref tokQueue);

            Parser parseTokens = new Parser(codeBlockTokens); // Create new parser object with only codeblock tokens
            codeBlockContents = parseTokens.ParseTokens(); // (recursion) Call parse function to parse the codeblock tokens and output a list of Step

            return new ElseStatement(codeBlockContents);
        }

        public FuncCall CaptureFunctionCall(string funcName)
        {
            // This is a simple capture - we just need to get all tokens inside the ( )
            // e.g output(x + 2); -> capture the "x+2"
            List<Token> arguments = CollectInsideBrackets("(", ")", ref tokQueue);


            // After collection has ended we should be at token ')' in example 'output("testing");'
            //                                                        We need to skip this token ^
            if (tokQueue.More()) tokQueue.MoveNext(); // Skip the ";"
            else throw new SyntaxError(); // error if line doesn't end with ';'

            return new FuncCall(funcName, arguments);
        }

        public FuncDeclare CaptureFunctionDeclare()
        {
            Token funcName = tokQueue.MoveNext();

            if (!funcName.Type().Equals("identifier")) throw new SyntaxError(); // function declare with no funcname => syntax error

            // We are at the '(' in "func testFunc(params) { }"
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "(")) throw new SyntaxError(); // if next Token isn't ( then throw error


            // -------------- PARSE DEFINED PARAMETERS --------------------
            List<Token> parameterTokens = CollectInsideBrackets("(", ")", ref tokQueue);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (parameterTokens.Count > 0)
            {
                foreach (List<Token> paramTok in Token.TokenSplit(",", parameterTokens))
                // Each paramTok should be something like 
                // [
                //  ('identifier', 'string'), 
                //  ('identifier', 'x')
                // ]
                // This is from: 'func testFunc(string x)'
                {
                    if (paramTok.Count != 2) throw new SyntaxError(); // too many or too few tokens

                    if (paramTok[0].Type().Equals("identifier") && paramTok[1].Type().Equals("identifier")) // e.g 'string x' => IDENTIFIER IDENTIFIER
                    {
                        if (Syntax.IsType(paramTok[0].Value()))
                        {
                            parameters.Add(paramTok[1].Value(), paramTok[0].Value().Replace("int", "number")); // (varname, type)
                        }
                        else throw new SyntaxError(); // parameter type undefined
                    }
                    else throw new SyntaxError(); // should be defining parameters, not any other type of token allowed other than identifiers
                }
            }

            Token nextTok = tokQueue.MoveNext();
            string returnType = "";

            if (nextTok.Type().Equals("identifier") && nextTok.Value().ToLower().Equals("returns"))
                // We now know it returns a value
                // PATTERN EXPECTED: 'returns <type>'
            {
                nextTok = tokQueue.MoveNext(); // Should now be the <type>

                if (Syntax.IsType(nextTok.Value())) 
                {
                    returnType = nextTok.Value();

                    nextTok = tokQueue.MoveNext(); // should move onto the { now
                }
                else throw new ReferenceError();
            }

            // next token should be {
            if (!GrammarTokenCheck(nextTok, "{")) throw new SyntaxError();



            // Next token(s) should all be programming statements inside the code block { }
            // We need to collect these so we can parse them
            List<Token> codeBlockTokens = CollectInsideBrackets("{", "}", ref tokQueue);

            Parser parseTokens = new Parser(codeBlockTokens); // Create new parser object with only codeblock tokens
            List<Step> codeBlockContents = parseTokens.ParseTokens(); // (recursion) Call parse function to parse the codeblock tokens and output a list of Step

            return new FuncDeclare(funcName.Value(), parameters, codeBlockContents, returnType);
        }

        public VarChange CaptureVarChange(string varName)
        {
            // We have already been given the name of the variable to change
            // We need to capture the value to change it to ('varValue') e.g '3 + x +1'

            // This can be any amount of tokens, so we must collect them all
            List<Token> varValue = new List<Token>();

            while (tokQueue.More() && !GrammarTokenCheck(tokQueue.Next(), ";")) // Capture all tokens up to the end of the declaration (indicated by ";")
            {
                varValue.Add(tokQueue.MoveNext()); // Move along queue index and add each element it returns
            }


            if (tokQueue.More()) tokQueue.MoveNext(); // Done! Now skip over the ";" - in the while loop we stopped when we 'peeked' it, but it isn't 'popped' out the queue yet.
            else throw new SyntaxError(); // if line doesn't end with ';', error

            return new VarChange(varName, varValue);
        }


        public static bool GrammarTokenCheck(Token tok, string toCheck)
            // It is important to check not just the value of the grammar token, but that it is a GRAMMAR token
            // If we do not do this, a string ";" would return true as it has the value ';', but it is not actually part of the grammar in the program
            // e.g int x = ";"; would break the program if we did not check the token ";" type and realise it's a string, not grammar.
        {
            return tok.Type().Equals("grammar") && tok.Value().Equals(toCheck);
        }

        public static List<Token> CollectInsideBrackets(string openBracket, string closeBracket, ref TokenQueue tokQueue)
          // open & close bracket can technically be anything, but it is generally used for '()' and '{}'
          // This method allows us to collect everything inside these brackets and can handle nested brackets by balancing them
          // e.g 'output(1 + (2*(1+2)));' has nested () brackets inside
          // This method, applied when the first '(' is found, will collect '1 + (2*(1+2))'
        {
            int bracket_depth = 0;
            bool keepCollectingTokens = true;
            List<Token> toCollect = new List<Token>();

            while (keepCollectingTokens && tokQueue.More())
            {
                Token collectedTok = tokQueue.MoveNext(); // Pop next Token out queue

                if (GrammarTokenCheck(collectedTok, openBracket)) bracket_depth++; // If a new codeblock, increase depth of nesting
                else if (GrammarTokenCheck(collectedTok, closeBracket)) // If end of code block
                {
                    if (bracket_depth == 0)
                    // Brackets already balanced, we have found the closing bracket that marks the end of the condition operands
                    {
                        keepCollectingTokens = false; // Finish collecting, will cause the final closing bracket to NOT be added to operands and stop the loop.
                    }
                    else bracket_depth--; // If we are still nested in but have ended a codeblock, decrease nesting depth
                }

                if (keepCollectingTokens) toCollect.Add(collectedTok); 
                    // If keepCollectingTokens is TRUE, we are still in the code block nest
                    // If NOT, we have finished collecting then collectedTok is a closing bracket - we do not want this added.

            }
            if (keepCollectingTokens) throw new SyntaxError(); // Loop must've stopped due to tokQueue.More() returning false (no more tokens left) therefore syntax error
            // e.g caused by 'testingFunc(02902 + 1092' with no balanced ')' on the end.

            return toCollect;
        }

        public static string CollectComparator(List<Token> condition)
        {
            bool found = false;
            int index = 0;
            string toReturn = "";

            while (!found && index < condition.Count)
            {
                if (condition[index].Type().Equals("grammar"))
                {
                    if (Syntax.IsComparator(condition[index].Value())) // If it is e.g "==" or "!=", etc.
                    {
                        toReturn = condition[index].Value(); // Return which comparator it is, e.g "=="
                        found = true; // stop loop
                    }
                }
                index++;
            }
            return toReturn;
        }

        public static (List<Token> Operand1, List<Token> Operand2) CaptureOperands(List<Token> condition, string comparator)
            // We can't use a built-in Split List method as it won't check our Token objects are type 'grammar'
            // Split expression (below in token form) e.g:
            // Split ['2', '+', '1', '>', '1', '*', '3'] by '>'
        {
            List<Token> Operand1 = new List<Token>();
            List<Token> Operand2 = new List<Token>();
            bool passedSplitPoint = false;

            foreach (Token tok in condition)
            {
                if (GrammarTokenCheck(tok, comparator)) // Make sure grammar token and not a string with value ">" or similar
                {
                    passedSplitPoint = true;
                }
                else if (passedSplitPoint) // Passed the point to split by, add to second list
                {
                    Operand2.Add(tok);
                }
                else
                {
                    Operand1.Add(tok);
                }
            }
            return (Operand1, Operand2); // Return two halves of the list of Tokens
        }
    }
}
