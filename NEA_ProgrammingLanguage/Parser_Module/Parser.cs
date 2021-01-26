using DataStructures;
using Lexer_Module;
using Parser_Module.Events;
using System;
using System.Collections.Generic;
using Errors;

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
                        WhileLoop whileLoop = new WhileLoop(template.GetCBContents(), template.GetOp1(), template.GetOp2(), template.GetComparator());
                        // Reuse code from the if statement because while & if follow the exact same structure:
                        // while (condition) { codeblock }
                        // if (condition) { codeblock }
                        // We just captured an if statement 'template' then used the information it collected to create a while loop instead

                        EvaluationSteps.Add(whileLoop);
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


            tokQueue.MoveNext(); // Done! Now skip over the ";" - in the while loop we stopped when we 'peeked' it, but it isn't 'popped' out the queue yet.

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
            // e.g if (x > 0) {} Capture the "x > 0"
            List<Token> condition = CollectInsideBrackets("(", ")");
            // We need to separate these into OPERAND1, OPERAND2, COMPARATOR to go into the 'operands' list
            // OPERANDs can be any expression, such as 9+1*x, hence we have to collect them carefully
            // We can split the list of tokens by the comparator, but we need to find it first
            string comparator = CollectComparator(condition);
            if (comparator.Equals("")) throw new SyntaxError(); // If we didn't find a comparator we have a syntax error

            // We now have a comparator to split by
            (List<Token> Operand1, List<Token> Operand2) = CaptureOperands(condition, comparator);

            // Next token after ')' should be '{'
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "{")) throw new SyntaxError(); // Check next token while simulatneously moving along queue

            // Next token(s) should all be programming statements inside the code block { }
            // Once again, we need to collect these so we can parse them
            List<Token> codeBlockTokens = CollectInsideBrackets("{", "}");

            Parser parseTokens = new Parser(codeBlockTokens); // Create new parser object with only codeblock tokens
            codeBlockContents = parseTokens.ParseTokens(); // (semi-recursion) Call parse function to parse the codeblock tokens and output a list of Step

            return new IfStatement(codeBlockContents, Operand1, Operand2, comparator);
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
            List<Token> codeBlockTokens = CollectInsideBrackets("{", "}");

            Parser parseTokens = new Parser(codeBlockTokens); // Create new parser object with only codeblock tokens
            codeBlockContents = parseTokens.ParseTokens(); // (semi-recursion) Call parse function to parse the codeblock tokens and output a list of Step

            return new ElseStatement(codeBlockContents);
        }

        public FuncCall CaptureFunctionCall(string funcName)
        {
            // This is a simple capture - we just need to get all tokens inside the ( )
            // e.g output(x + 2); -> capture the "x+2"
            List<Token> arguments = CollectInsideBrackets("(", ")");

            // After collection has ended we should be at token ')' in example 'output("testing");'
            //                                                        We need to skip this token ^
            tokQueue.MoveNext(); // Skip the ";"

            return new FuncCall(funcName, arguments);
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


            tokQueue.MoveNext(); // Done! Now skip over the ";" - in the while loop we stopped when we 'peeked' it, but it isn't 'popped' out the queue yet.

            return new VarChange(varName, varValue);
        }


        public bool GrammarTokenCheck(Token tok, string toCheck)
            // It is important to check not just the value of the grammar token, but that it is a GRAMMAR token
            // If we do not do this, a string ";" would return true as it has the value ';', but it is not actually part of the grammar in the program
            // e.g int x = ";"; would break the program if we did not check the token ";" type and realise it's a string, not grammar.
        {
            return tok.Type().Equals("grammar") && tok.Value().Equals(toCheck);
        }

        public List<Token> CollectInsideBrackets(string openBracket, string closeBracket)
          // open & close bracket can technically be anything, but it is generally used for ( ) and { }
          // This method allows us to collect everything inside these brackets and can handle nested brackets by balancing them
          // e.g 'output(1 + (2*(1+2)));' has nested () brackets inside
          // This method, applied when the first '(' is found, will collect '1 + (2*(1+2))'
        {
            int bracket_depth = 0;
            bool keepCollectingTokens = true;
            List<Token> toCollect = new List<Token>();

            while (keepCollectingTokens)
            {
                Token collectedTok = tokQueue.MoveNext();

                if (GrammarTokenCheck(collectedTok, openBracket)) bracket_depth++;
                else if (GrammarTokenCheck(collectedTok, closeBracket))
                {
                    if (bracket_depth == 0)
                    // Brackets already balanced, we have found the closing bracket that marks the end of the condition operands
                    {
                        keepCollectingTokens = false; // Finish collecting, will cause the final closing bracket to NOT be added to operands and stop the loop.
                    }
                    else bracket_depth--;
                }

                if (keepCollectingTokens) toCollect.Add(collectedTok); // If statement prevents the closing bracket at the end being added to the tokens as it is not part of the expression inside

            }

            return toCollect;
        }

        public string CollectComparator(List<Token> condition)
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

        public (List<Token> Operand1, List<Token> Operand2) CaptureOperands(List<Token> condition, string comparator)
            // Split expression (below in token form) e.g:
            // Split ['2', '+', '1', '>', '1', '*', '3'] by '>'
        {
            List<Token> Operand1 = new List<Token>();
            List<Token> Operand2 = new List<Token>();
            bool passedSplitPoint = false;

            foreach (Token tok in condition)
            {
                if (tok.Type().Equals("grammar") && tok.Value().Equals(comparator)) // Make sure grammar token and not a string with value ">" or similar
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
