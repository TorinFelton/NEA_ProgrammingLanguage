using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    abstract class Event : Step
    {
        protected string type;
        public Event() { }

        public string Type()
        {
            return this.type;
        }

    }
}
