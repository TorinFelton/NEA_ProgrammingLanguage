using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    class WhileLoop : IfStatement
    {
        public WhileLoop(List<Step> cbContents, List<Token> op1, List<Token> op2, string comparator) : base(cbContents, op1, op2, comparator) 
        {
            this.type = "WHILE_LOOP";
        }
    }
}
