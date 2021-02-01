using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    class WhileLoop : IfStatement
    {
        // As a while loop follows the same structure as an if statement, we can just inherit EVERYTHING from the IfStatement object
        // A while loop is just an if statement that runs again until the condition is false.
        public WhileLoop(List<Step> cbContents, List<Token> condition) : base(cbContents, condition)
            // base() refers to the IfStatement() constructor
        {
            this.type = "WHILE_LOOP"; // The ONLY difference between this class and the IfStatement.
        }
    }
}
