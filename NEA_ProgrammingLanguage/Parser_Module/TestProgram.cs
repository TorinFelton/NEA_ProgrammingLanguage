using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser_Module
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

            Tokeniser tokeniser = new Tokeniser(input); // Use our Tokeniser to obtain a list of Tokens

            Parser parse = new Parser(tokeniser.Tokenise().ToList()); // Init parser with Tokens
            foreach (Step stepObj in parse.ParseTokens())
            {
                Console.WriteLine(stepObj.ToString()); // Output each Step in EvaluationSteps
            }
        }
    }
}
