using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    static class Syntax
    {
        private static List<string> types = new List<string>() { "int", "string"};
        private static List<string> comparators = new List<string>() { "==", "!=", ">", "<", ">=", "<=" };
        
        public static bool IsType(string text)
        {
            return types.Contains(text.ToLower());
        }

        public static bool IsComparator(string tokenValue)
        {
            return comparators.Contains(tokenValue);
        }
    }
}
