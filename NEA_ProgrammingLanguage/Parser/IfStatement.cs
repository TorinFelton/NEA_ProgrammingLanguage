using System;
using System.Collections.Generic;
using System.Text;

namespace NEA_ProgrammingLanguage.Parser
{
    class IfStatement : Step
    {
        private List<Step> codeBlockContents; // Every Step inside the if statement code block

        public IfStatement() { }

        public IfStatement(List<string> operands, List<Step> cbContents)
        {
            this.operands = operands;
            this.codeBlockContents = cbContents;
        }

        public override string ToString()
        {
            string toReturn = "";
            foreach (Step step in this.codeBlockContents) // Collect each Step in the codeblock and add to string
            {
                toReturn += step.ToString() + "\n";
            }
            return "(IF) CONDITION: [" + String.Join(", ", this.operands.ToArray()) + "]\n CONTENTS: " + toReturn; // Display condition operands AND each Step operand
        }
    }
}
