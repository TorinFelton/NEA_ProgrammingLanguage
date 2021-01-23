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


        public Token Next() // Peek function
        {
            if (tokens.Count > 0 && index < tokens.Count)
            {
                return tokens[index];
            }
            else throw new IndexOutOfRangeException();
        }

        public Token MoveNext() // Pop function
        {
            if (!(tokens.Count > 0 && index < tokens.Count)) throw new IndexOutOfRangeException();
            return tokens[index++]; // Returns index THEN increments it
        }

        public bool More() // Check if still more tokens to pop off queue
        {
            return index < tokens.Count;
        }
    }
}
