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

            // Test programs to run in case of problems:
              // Parser_Module.TestProgram.Run();
              // Lexer_Module.TestProgram.Run();
              // Evaluator_Module.ExpressionEvaluation.TestProgram.Run();
              // Evaluator_Module.ExpressionEvaluation.TestProgram.Run();

            // FILE INPUT
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
            // END OF FILE INPUT

            Console.WriteLine("-------------------- PROGRAM STARTED --------------------");

            Tokeniser tokeniser = new Tokeniser(toRun); // TOKENISE FILE CONTENTS
            List<Token> tokens = tokeniser.Tokenise().ToList(); // ToList() will put the output of the IEnumerable straight into 'tokens'

            Parser parseTok = new Parser(tokens); // PARSE TOKENISED PROGRAM
            List<Step> evalSteps = parseTok.ParseTokens(); // CREATE EvaluationSteps for EVALUATOR_MODULE

            Evaluator eval = new Evaluator(); // Init Evaluator
            eval.Evaluate(evalSteps); // EVALUATE EvaluationSteps

            Console.WriteLine("-------------------- PROGRAM ENDED --------------------");

        }
    }
}
