using System;
using System.Collections.Generic;
using System.Text;

namespace Errors
{
    class SyntaxError : Exception
    {
        public SyntaxError() : base()
        {
            Error.ShowError("SYNTAX error. Re-read your program and check for spelling/keyword mistakes.");
        }
    }

    class TypeError : Exception
    {
        public TypeError() : base()
        {
            Error.ShowError("TYPE error. Declared type does not match value of expression type.");
        }
    }

    class TypeMatchError : Exception
    {
        public TypeMatchError() : base()
        {
            Error.ShowError("TYPE MATCH error. Types in expression are not compatible.");
        }
    }

    class ReferenceError : Exception
    {
        public ReferenceError() : base()
        {
            Error.ShowError("REFERENCE error. Variable, function or type referenced does not exist.");
        }
    }

    class DeclareError : Exception
    {
        public DeclareError() : base()
        {
            Error.ShowError("DECLARE ERROR. Variable or function already exists.");
        }
    }

    class ComparisonError : Exception
    {
        public ComparisonError() : base()
        {
            Error.ShowError("COMPARISON error. Cannot compare different types.");
        }
    }

    class ArgumentRangeError : Exception
    {
        public ArgumentRangeError() : base()
        {
            Error.ShowError("ARGUMENT RANGE error. Check your function calls have the required amount of arguments.");
        }
    }

    class ReturnTypeError : Exception
    {
        public ReturnTypeError() : base()
        {
            Error.ShowError("RETURN TYPE error. The returned type does not match that of the defined function.");
        }
    }

    class ReturnMissingError : Exception
    {
        public ReturnMissingError() : base()
        {
            Error.ShowError("RETURN MISSING error. You have a declared a function to return something, but there is no return statement.");
        }
    }

    class IntConvertError : Exception
    {
        public IntConvertError() : base()
        {
            Error.ShowError("CONVERSION (Int) error. Cannot convert value to Integer.");
        }
    }

    class BoolConvertError : Exception
    {
        public BoolConvertError() : base()
        {
            Error.ShowError("CONVERSION (Bool) error. Cannot conert value to Bool.");
        }
    }

    static class Error
    {
        public static void ShowError(string err) // Pause program (input prompt) then kill it.
        {
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(err);
            Console.ForegroundColor = ConsoleColor.Gray;

            if (!((new System.Diagnostics.StackTrace()).ToString().ToLower().Contains("shell")))
                // If this is NOT running in a Shell instance.
            {
                Console.ReadLine(); // Used to pause it, Enter required to move on
                Environment.Exit(1); // Exits process, killing program after pause from ReadLine()
            }
            // else if it is running in a Shell instance, then let the shell catch the error and restart.
        }
    }
}
