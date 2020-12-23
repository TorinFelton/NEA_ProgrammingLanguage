using System;
using System.Collections.Generic;
using System.Text;

namespace NEA_ProgrammingLanguage.Parser
{
    static class Syntax
    {
        private static List<string> types = new List<string>() { "int", "string", "bool" };
        public static bool IsType(string text)
        {
            return types.Contains(text.ToLower());
        }
    }
}
