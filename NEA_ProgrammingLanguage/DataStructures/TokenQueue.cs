using Lexer_Module;
using System;
using System.Collections.Generic;

namespace DataStructures
{
    class TokenQueue
    {
        private List<Token> tokens;
        private int index = 0;
        public TokenQueue(List<Token> inputTokens)
        {
            tokens = inputTokens;
        }


        public Token Next()
        {
            if (tokens.Count > 0 && index < tokens.Count)
            {
                return tokens[index];
            }
            else throw new IndexOutOfRangeException();
        }

        public Token MoveNext()
        {
            if (!(tokens.Count > 0 && index < tokens.Count)) throw new IndexOutOfRangeException();
            return tokens[index++]; // Returns index THEN increments it
        }

        public bool More()
        {
            return index < tokens.Count;
        }
    }
}
