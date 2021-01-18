using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    abstract class Event : Step // This class serves no purpose other than to distinguish statements and control flow.
    {

        public Event() {  }


    }
}
