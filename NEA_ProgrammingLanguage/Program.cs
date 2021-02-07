using Parser_Module;
using Lexer_Module;
using Evaluator_Module;
using System;
using System.Collections.Generic;
using System.Linq;
Added using System.Threading;

namespace NEA_ProgrammingLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parser_Module.TestProgram.Run();
            //Lexer_Module.TestProgram.Run();
            //Evaluator_Module.ExpressionEvaluation.TestProgram.Run();

            if (args.Length > 0 && args[0].Equals("-shell"))
            {
                Shell cmdShell = new Shell();
                while (true)
                {
                    cmdShell.OutputColour(">> ", ConsoleColor.Green);

                    string input = Console.ReadLine();
                    if (input.Equals("exit")) { Environment.Exit(0); }
                    if (input.EndsWith("{")) 
                    {
                        int balance = 1;
                        string total = input;
                        int lineNumber = 2;

                        while (balance != 0)
                        {
                            cmdShell.OutputColour(lineNumber++ + "  ", ConsoleColor.Green);
                            total += Console.ReadLine();
                            if (total.EndsWith("{")) balance++;
                            else if (total.EndsWith("}")) balance--;
                        }

                        cmdShell.Run(total);
                    } else if (!input.EndsWith(";"))
                    {
                        Console.WriteLine(cmdShell.ExpressionResult(input));
                    }
                    else
                    {
                        cmdShell.Run(input);
                    }
                }
            }
            else
            {
                bool invalid = true;
                string toRun = "";
                while (invalid)
                {
                    try
                    {
                        Console.Write("Enter a valid file name to run: ");

                        toRun = System.IO.File.ReadAllText(Console.ReadLine());
                        invalid = false;
                    }
                    catch
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
}
