using Parser_Module;
using Lexer_Module;
using Evaluator_Module;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NEA_ProgrammingLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
             //Lexer_Module.TestProgram.Run();
            // ExpressionEvaluation.TestProgram.Run();
            // Evaluator_Module.ExpressionEvaluation.TestProgram.Run();


            bool invalid = true;
            string toRun = "";
            while (invalid) {
                try
                {
                    Console.Write("Enter a valid file name to run: ");

                    toRun = System.IO.File.ReadAllText(Console.ReadLine());
                    invalid = false;
                } catch
                {
                    Console.WriteLine("Invalid file name.");
                }
            }

            Console.WriteLine("-------------------- PROGRAM STARTED --------------------");
            Tokeniser tokeniser = new Tokeniser(toRun);
            List<Token> tokens = tokeniser.Tokenise().ToList();

            //Console.WriteLine(String.Join("\n", tokens));


            Parser parseTok = new Parser(tokens);
            List<Step> evalSteps = parseTok.ParseTokens();

            Evaluator eval = new Evaluator();
            eval.Evaluate(evalSteps);
            Console.WriteLine("-------------------- PROGRAM ENDED --------------------");

        }
    }
}
