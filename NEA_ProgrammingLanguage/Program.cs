using Parser_Module;
using Lexer_Module;
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

            Tokeniser tokeniser = new Tokeniser(input);
            List<Token> tokens = tokeniser.Tokenise().ToList();
            
            // Console.WriteLine(String.Join("\n", tokens));

            Parser parseTok = new Parser(tokens);

            foreach (Step step in parseTok.ParseTokens())
            {
                Console.WriteLine(step.ToString());
            }
            
        }
    }
}
