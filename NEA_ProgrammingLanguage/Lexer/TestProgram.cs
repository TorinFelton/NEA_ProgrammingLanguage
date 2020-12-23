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
            do // Test multiple lines of input
            {
                newInput = Console.ReadLine();
                input += newInput;
            } while (newInput.Length > 0);

            Tokeniser tokeniser = new Tokeniser(input);

            foreach (Token tok in tokeniser.Tokenise())
            {
                Console.WriteLine(tok.ToString());
            }
        }
    }
}
