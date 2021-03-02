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
            /*
            //-------- MULTI-LINE INPUT --------
            string input = "";
            string newInput = "";
            do
            {
                newInput = Console.ReadLine();
                input += newInput;
            } while (newInput.Length > 0);
            //-------- END OF MULTI-LINE INPUT --------
            */

            // FILE INPUT
            bool invalid = true;
            string input = "";
            while (invalid)
            {
                try
                {
                    Console.Write("Enter a valid file name to run: ");

                    input = System.IO.File.ReadAllText(Console.ReadLine());
                    invalid = false;
                }
                catch
                {
                    Console.WriteLine("Invalid file name.");
                }
            }
            // END OF FILE INPUT

            Tokeniser tokeniser = new Tokeniser(input);

            foreach (Token tok in tokeniser.Tokenise()) // Loop through each returned element
            {
                Console.WriteLine(tok.ToString()); // Output makes use of overridden Token.ToString() method
            }
        }
    }
}
