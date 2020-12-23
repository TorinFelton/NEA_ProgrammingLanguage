using System;
using System.Collections.Generic;
using System.Linq;

namespace NEA_ProgrammingLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lexer.TestProgram.Run();
            // ExpressionEvaluation.TestProgram.Run();

            string input = "";
            string newInput = "";
            do // Test multiple lines of input
            {
                newInput = Console.ReadLine();
                input += newInput;
            } while (newInput.Length > 0);

            Lexer.Tokeniser tokeniser = new Lexer.Tokeniser(input);
            List<Lexer.Token> tokens = tokeniser.Tokenise().ToList();

            foreach (Parser.Step step in Parser.Parse.ParseTokens(tokens))
            {
                Console.WriteLine(step.ToString());
            }
        }
    }
}
