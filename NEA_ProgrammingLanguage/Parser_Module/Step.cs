using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Parser_Module
{
    abstract class Step // Every child must have a TYPE, the other attributed differ
    {
        protected string type;

        public Step(string type) { this.type = type; }

        public Step() {} // Parameterless option for child classes like Event

        public string Type()
        {
            return this.type;
        }

        abstract public override string ToString(); // Each one is guaranteed to have a unique ToString() 
    }
}
