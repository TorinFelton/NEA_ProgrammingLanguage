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
                ShellTokeniser = new Tokeniser(toRun);
                ShellParser = new Parser(ShellTokeniser.Tokenise().ToList());
                ShellEvaluator.Evaluate(ShellParser.ParseTokens());
        }

        public string ExpressionResult(string inputLine)
        {
            ShellTokeniser = new Tokeniser(inputLine);
            return ShellEvaluator.ResolveExpression(ShellTokeniser.Tokenise().ToList()).Value();

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
