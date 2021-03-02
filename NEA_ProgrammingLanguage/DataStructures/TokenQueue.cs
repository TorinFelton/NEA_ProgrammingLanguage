using Errors;
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


        public Token Next() // peek
        {
            if (!(tokens.Count > 0 && index < tokens.Count)) throw new SyntaxError();
            return tokens[index]; // Returns index
        }

        public Token MoveNext() // pop
        {
            if (!(tokens.Count > 0 && index < tokens.Count)) throw new SyntaxError();
            return tokens[index++]; // Returns index THEN increments it
        }

        public bool More() // check if able to peek or pop
        {
            return index < tokens.Count;
        }

        public List<Token> Contents() { return tokens; }
    }
}
