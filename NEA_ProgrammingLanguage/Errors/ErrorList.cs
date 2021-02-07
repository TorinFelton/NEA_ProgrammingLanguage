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
            Error.ShowError("TYPE_MATCH error. Types in expression are not compatible.");
        }
    }

    class ReferenceError : Exception
    {
        public ReferenceError() : base()
        {
            Error.ShowError("REFERENCE error. Variable or function referenced does not exist.");
        }
    }

    class DeclareError : Exception
    {
        public DeclareError() : base()
        {
            Error.ShowError("DECLARE_ERROR. Variable already exists.");
        }
    }

    class ComparisonError : Exception
    {
        public ComparisonError() : base()
        {
            Error.ShowError("COMPARISON error. Cannot compare different types.");
        }
    }

    class ArgumentRangeException : Exception
    {
        public ArgumentRangeException() : base()
        {
            Error.ShowError("ARGUMENT RANGE error. Check your function calls have the required amount of arguments.");
        }
    }

    static class Error
    {
        public static void ShowError(string err) // Pause program (input prompt) then kill it.
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(err);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadLine(); // Used to pause it, Enter required to move on
            Environment.Exit(1); // Exits process
        }
    }
}
