using System;
using System.Collections.Generic;
using System.Text;
using Evaluator_Module;
using Parser_Module;
using Lexer_Module;
using System.Linq;

namespace NEA_ProgrammingLanguage
{
    class Shell
    {
        Evaluator ShellEvaluator = new Evaluator();
        Parser ShellParser;
        Tokeniser ShellTokeniser;
        public Shell()
        {
            
        }

        public void Run(string toRun)
        {
            try
            {
                ShellTokeniser = new Tokeniser(toRun);
                ShellParser = new Parser(ShellTokeniser.Tokenise().ToList());
                ShellEvaluator.Evaluate(ShellParser.ParseTokens());
            } catch
            {
                //ShellEvaluator = new Evaluator();
                // Evaluator would be reset by above, but no need. Shell can keep running.
            }
        }

        public string ExpressionResult(string inputLine)
        {
            try
            {
                ShellTokeniser = new Tokeniser(inputLine);
                return ShellEvaluator.ResolveExpression(ShellTokeniser.Tokenise().ToList()).Value();
            } catch
            {
                //ShellEvaluator = new Evaluator();
                // Would be used to reset variable & function scope on error encounter, but it isn't needed
                // Shell can just chew up errors and continue
                return "";
            }

        }

        public void OutputColour(string toOutput, ConsoleColor colour)
        {
            ConsoleColor prevCol = Console.ForegroundColor;

            Console.ForegroundColor = colour;

            Console.Write(toOutput);

            Console.ForegroundColor = prevCol;
        }
    }
}
