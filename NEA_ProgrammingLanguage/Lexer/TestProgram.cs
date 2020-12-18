using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lexer
{
    class TestProgram
    {
        public static void Run()
        {
            string input = "";
            string newInput = "";
            do
            {
                newInput = Console.ReadLine();
                input += newInput;
            } while (newInput.Length > 0);

            Tokeniser tokeniser = new Tokeniser(input);
            List<Token> rawTokens = tokeniser.Tokenise().ToList();

            foreach (Token tok in rawTokens)
            {
                Console.WriteLine(tok.ToString());
            }
        }
    }
}
