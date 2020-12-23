using DataStructures;
using Lexer;
using System;
using System.Collections.Generic;
using System.Text;

namespace NEA_ProgrammingLanguage.Parser
{
    static class Parse
    {
        public static List<Step> ParseTokens(List<Token> tokens)
        {
            TokenQueue tokQueue = new TokenQueue(tokens);
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
                        
                        // We need to create an Event object with operands: [varType, varName, varValue] and type "VAR_DECLARE"
                        List<string> operands = new List<string>();
                        operands.Add(nextTok.Value()); // This adds the TYPE of the variable being declared.

                        nextTok = tokQueue.MoveNext(); // Move to next token in Q

                        // We expect a variable name to be next, which will be token type 'identifier'. If it is not, we have a syntax error!
                        if (nextTok.Type().Equals("identifier")) operands.Add(nextTok.Value()); // Add it to the operands list
                        else throw new SystemException(); // TODO: Add error class and methods

                        // Next token after varName should be an "="
                        if (!tokQueue.MoveNext().Value().Equals("=")) throw new SystemException(); // Throw syntax error if this is not found, and also move along queue past it no matter what
                        // TODO: add error class and methods

                        // At this point, we have operands as [varType, varName] - we need to 'capture' the varValue expression in the tokens (e.g 2 + y*10)
                        string varValue = ""; // All values are stored as strings - the Evaluator module will decide whether they are valid for the varType.

                        while (tokQueue.More() && !tokQueue.Next().Value().Equals(";")) // Capture all tokens up to the end of the declaration (indicated by ";")
                        {
                            varValue += tokQueue.MoveNext().Value();
                        }
                        operands.Add(varValue);

                        tokQueue.MoveNext(); // Done to skip over the ";" - in the while loop we stopped when we 'peeked' it, but it isn't 'popped' out the queue yet.

                        // Operands should now have [varType, varName, varValue] - we are ready to create the Event object and add it to the EvaluationSteps list
                        Event varDeclare = new Event(operands, "VAR_DECLARE");

                        EvaluationSteps.Add(varDeclare);
                    }
                }
            }

            return EvaluationSteps;
        }
    }
}
