using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace NEA_ProgrammingLanguage.Parser
{
    abstract class Step
    {
        protected List<string> operands;

        public Step() { }
        public Step(List<string> operands)
        {
            this.operands = operands;
        }


        public List<string> GetOperands()
        {
            return this.operands;
        }

        public void SetOperands(List<string> operands)
        {
            this.operands = operands;
        }

        public override string ToString()
        {
            return "[" + String.Join(", ", this.operands.ToArray()) + "]"; // Return each operand in a string array format
        }
    }
}
