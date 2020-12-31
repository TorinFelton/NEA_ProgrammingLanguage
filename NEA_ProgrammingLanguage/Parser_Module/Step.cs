using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Parser_Module
{
    abstract class Step
    {
        protected List<Token> operands;

        public Step() { }
        public Step(List<Token> operands)
        {
            this.operands = operands;
        }


        public List<Token> GetOperands()
        {
            return this.operands;
        }

        abstract public override string ToString();
    }
}
