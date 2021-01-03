using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Parser_Module
{
    abstract class Step
    {
        protected string type;
        public Step(string type) { this.type = type; }

        public Step() { } // Parmaterless option

        public string Type()
        {
            return this.type;
        }

        abstract public override string ToString();
    }
}
