using DataStructures;
using Lexer_Module;
using Parser_Module.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    class Parser
    {
        TokenQueue tokQueue;
        public Parser(List<Token> tokens) { tokQueue = new TokenQueue(tokens); }
        public List<Step> ParseTokens()
        {
            List<Step> EvaluationSteps = new List<Step>();

            while (tokQueue.More())
            {
                Token nextTok = tokQueue.MoveNext();
                

                if (nextTok.Type().Equals("identifier"))
                    // Could be variable declaration, assignment, function call, "if"
                {
                    if (Syntax.IsType(nextTok.Value()))
                        // If it is a var type, e.g "int" "string" "bool" - if it is, this is a variable declaration ("int x = 0;")
                    {
                        /*
                         * EXPECTED PATTERN: varType varName = expr;
                         * e.g int x = 2 + y*10;
                         * e.g string testing = "Hello World!";
                         * e.g bool testBool = True;
                         */

                        Event varDeclare = CaptureVarDeclare(nextTok.Value()); // Call method with argument storing the type of var being declared, e.g 'string'

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

                        IfStatement statement = CaptureIfStatement();

                        EvaluationSteps.Add(statement);
                    }

                    
                    else if (GrammarTokenCheck(tokQueue.Next(), "("))
                        // This condition will return true for an if statement, hence the else if and placement AFTER the 'if' statement check
                        // Therefore must be start of function
                    {
                        /*
                         * EXPECTED PATTERN: funcName(expr);
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
                    else if (GrammarTokenCheck(tokQueue.Next(), "="))
                    {
                        /*
                         * EXPECTED PATTERN: varName = expr;
                         * e.g x = 2 + y*10;
                         * e.g testing = "Hello World!";
                         * e.g testBool = True;
                         */

                        tokQueue.MoveNext(); // Skip the '=' token
                        // Remember, nextTok still holds the value of the token before the '='
                        // This is the name of our variable to change ('varName')

                        VarChange varChan = CaptureVarChange(nextTok.Value());
                        EvaluationSteps.Add(varChan);
                    }
                }
            }

            return EvaluationSteps;
        }

        public VarDeclare CaptureVarDeclare(string varType)
        {
            // Before creating a VarDeclare object, we should collect all the data we need while checking we actually have it in the right format.
            // We have already been given the 'type' token of the variable (it is an argument of this function)

            Token nextTok = tokQueue.MoveNext(); // Move to next token in Q

            // We expect a variable name to be next, which will be token type 'identifier'. If it is not, we have a syntax error!
            string varName;

            if (nextTok.Type().Equals("identifier")) varName = nextTok.Value(); // Collect Token with name of the variable
            else throw new SystemException(); // Throw error if variable name is not an 'identifier' token (invalid syntax)

            // Next token after varName should be an "="
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "=")) throw new SystemException(); // Throw syntax error if this is not found, and also move along queue index

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
            List<Token> operands = new List<Token>();
            List<Step> codeBlockContents = new List<Step>();

            // Next token after 'if' should be '('
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "(")) throw new SystemException(); // Check next token while simulatneously moving along queue

            // Next token(s) should be operands to form a 'condition' 
            // These token(s) will be inside ( )
            // e.g if (x > 0) {} Capture the "x > 0"
            operands = CollectInsideBrackets("(", ")");

            // Next token after ')' should be '{'
            if (!GrammarTokenCheck(tokQueue.MoveNext(), "{")) throw new SystemException(); // Check next token while simulatneously moving along queue

            // Next token(s) should all be programming statements inside the code block { }
            // Once again, we need to collect these so we can parse them
            List<Token> codeBlockTokens = CollectInsideBrackets("{", "}");
            
            Parser parseTokens = new Parser(codeBlockTokens); // Create new parser object with only codeblock tokens
            codeBlockContents = parseTokens.ParseTokens(); // (semi-recursion) Call parse function to parse the codeblock tokens and output a list of Step

            return new IfStatement(operands, codeBlockContents);
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
    }
}
