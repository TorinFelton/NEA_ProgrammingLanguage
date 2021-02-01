using DataStructures;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Errors;

namespace Lexer_Module
{
    class Tokeniser
    {
        private CharQueue contents;

        public Tokeniser(string input)
        {
            contents = new CharQueue(input);
        }

        // NOTE: The usage of RegEx here is STRICTLY for single character matching to make it quicker to write
        // Instead of saying 'is character abcdefghijkmno....' we can just use [a-zA-Z] and check for match of the CHARACTER
        // We are NOT using RegEx to find keywords or tokens themselves - that is inefficient for long programs and often gets complex.
        public IEnumerable<Token> Tokenise() 
            // IEnumerable allows us to use foreach with yielding in C#
            // Not REQUIRED, but means there is no need to explicitly create a list and return it, instead use 'yield return'
            // 'yield return' allows you to return each element one at a time - this is just more memory efficient
            //
            // As we are always going to be dealing with a large amount of tokens (think each typed character in a program), 
            // we should be considerate of efficiency for memory and looping - we do not need to create a list in this method.
            //
            // In our case, we are returning each Token object in a list of Tokens
        {
            while (contents.More()) // While elements left in queue of chars
            {
                char character = contents.MoveNext();

                // I'll be using the a shortcut to check what the character is: String.Contains(char)
                // This is faster to write than 'if character == "+" || character == "-" etc...'

                if (" \n\t\r".Contains(character)) continue; // We do not care about spaces or new lines, skip iteration

                // Operators
                else if ("+-*/^".Contains(character)) yield return new Token("operator", character.ToString());

                // General Guideline Grammar
                else if ("(){};=<>!|&".Contains(character))
                {
                    // Check if more tokens past it then check if we've found a comparator operator like "==", ">=", "<="
                    if (contents.More() && "=<>&|".Contains(contents.Next()))
                        yield return new Token("grammar", character.ToString() + contents.MoveNext().ToString());
                    // If the next token is ALSO "=" or "<" or ">", then add both tokens together as one to form "==", "!=", "<=", ">="

                    else yield return new Token("grammar", character.ToString());
                    // else just add single token
                }
                // Numbers (only supports integers)
                // These are not full RegEx statements - they are only to match a SINGLE CHARACTER AT A TIME.
                else if (Regex.IsMatch(character.ToString(), "[0-9]")) yield return new Token("number", Match_GrabChunk(character, "[0-9]"));

                // Identifiers - can have numbers, not at the beginning though
                // We look at first for a letter, then grab any following letters/numbers
                else if (Regex.IsMatch(character.ToString(), "[a-zA-Z]"))
                {
                    string identifier = Match_GrabChunk(character, "[a-zA-Z0-9]");
                    if (identifier.ToLower().Equals("true") || identifier.ToLower().Equals("false")) yield return new Token("bool", identifier);
                    else yield return new Token("identifier", identifier);
                }

                // Strings can be denoted by ' or " in our language. If we find one of those, grab the rest of the string:
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
            // Continue collecting characters until no longer matching input regex pattern (for single char!), then return collected chunk
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
