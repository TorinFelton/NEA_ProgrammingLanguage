using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    static class Syntax // This is just a class for definitions and checking
    {
        private static List<string> types = new List<string>() { "int", "string", "bool"};
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
