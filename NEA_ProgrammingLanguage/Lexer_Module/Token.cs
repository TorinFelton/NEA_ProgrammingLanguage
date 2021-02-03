using System.Collections.Generic;

namespace Lexer_Module
{
    class Token
    {
        private string type;
        private string value;

        public Token(string type, string value)
        {
            this.type = type; this.value = value;
        }

        public override string ToString()
        {
            return "('" + type + "', \"" + value + "\")"; // e.g ("grammar", "+")
        }

        public string Type()
            // It is easier to name getters as simply Type() or Value() instead of GetType() or GetValue()
            // as these are built-in methods that shouldn't really be overriden
        {
            return type;
        }

        public string Value()
        {
            return value;
        }

        public static IEnumerable<List<Token>> TokenSplit(string toSplitBy, List<Token> toSplit)
        {
            List<Token> cache = new List<Token>();
            foreach (Token tok in toSplit)
            {
                if (tok.Type().Equals("grammar") && tok.Value().Equals(toSplitBy))
                {
                    yield return cache;
                    cache = new List<Token>(); // reset cache
                }
                else cache.Add(tok);
            }
            yield return cache; // return last (or only) section
        }
    }
}
