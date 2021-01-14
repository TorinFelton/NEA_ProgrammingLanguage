using DataStructures;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Errors;

namespace Lexer_Module
{
    class Tokeniser
    {
        StringQueue contents;

        public Tokeniser(string input)
        {
            contents = new StringQueue(input);
        }
        public IEnumerable<Token> Tokenise() 
            // IEnumerable allows us to use foreach in C#
            // Not REQUIRED, but means there is no need to explicitly create a list and return it, instead use 'yield return'
            // 'yield return' allows you to return each element one at a time
            // In our case, we are returning each Token object in a list of Tokens
        {
            while (contents.More())
            {
                char character = contents.MoveNext(); 
                if (" \n\t\r".Contains(character)) continue; // We do not care about spaces or new lines, skip iteration
                // Operators
                else if ("+-*/^".Contains(character)) yield return new Token("operator", character.ToString());
                // General Guideline Grammar
                else if ("(){};=<>!".Contains(character))
                {
                    // Check if more tokens past it then check if we've found a comparator operator like "==", ">=", "<="
                    if (contents.More() && "=<>".Contains(contents.Next())) 
                        yield return new Token("grammar", character.ToString() + contents.MoveNext().ToString());
                        // If the next token is ALSO "=" or "<" or ">", then add both tokens together as one to form "==", "!=", "<=", ">="

                    else yield return new Token("grammar", character.ToString());
                    // else just add single token
                }
                // Numbers (only supports integers)
                else if (Regex.IsMatch(character.ToString(), "[0-9]")) yield return new Token("number", Match_GrabChunk(character, "[0-9]"));
                // Identifiers - can have numbers, not at the beginning though
                else if (Regex.IsMatch(character.ToString(), "[a-zA-Z]")) yield return new Token("identifier", Match_GrabChunk(character, "[a-zA-Z0-9]"));
                // Strings
                else if ("\"'".Contains(character)) yield return new Token("string", Char_GrabChunk(character));
                // Any strange or unrecognisable characters throw an error.
                else throw new SyntaxError();
            }
        }

        public string Char_GrabChunk(char delimiter)
            // Returns a string of every next character up to delimiter
            // E.g if delimiter is " and string is 'testing 123" test' then returns 'testing 123'
        {
            string chunk = "";
            while (contents.More() && !(contents.Next().Equals(delimiter)))
            {
                chunk += contents.MoveNext();
            }
            contents.MoveNext();
            return chunk;
        }
        
        public string Match_GrabChunk(char first_char, string pattern)
            // Continue collecting characters until no longer matching input regex pattern, then return collected chunk
        {
            string chunk = first_char.ToString();
            while (contents.More() && Regex.IsMatch(contents.Next().ToString(), pattern))
            {
                chunk += contents.MoveNext();
            }
            return chunk;
        }
    }
}
