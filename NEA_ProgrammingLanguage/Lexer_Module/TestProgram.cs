using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lexer_Module
{
    class TestProgram
    {
        public static void Run()
        {
            //-------- MULTI-LINE INPUT --------
            string input = "";
            string newInput = "";
            do
            {
                newInput = Console.ReadLine();
                input += newInput;
            } while (newInput.Length > 0);
            //-------- END OF MULTI-LINE INPUT --------

            Tokeniser tokeniser = new Tokeniser(input);

            foreach (Token tok in tokeniser.Tokenise()) // Loop through each returned element
            {
                Console.WriteLine(tok.ToString()); // Output makes use of overridden Token.ToString() method
            }
        }
    }
}
