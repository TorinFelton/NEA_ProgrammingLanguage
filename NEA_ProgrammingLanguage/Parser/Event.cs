using System;
using System.Collections.Generic;
using System.Text;

namespace NEA_ProgrammingLanguage.Parser
{
    class Event : Step
    {
        private string type;
        public Event() { }
        public Event(List<string> operands, string type)
        {
            this.operands = operands;
            this.type = type;
        }

        public string Type()
        {
            return this.type;
        }

        public override string ToString()
        {
            return this.type + ": [" + String.Join(", ", this.operands.ToArray()) + "]"; // Same as Step ToString() but with type of Event first
        }
    }
}
