using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    class IfStatement : Step
    {
        private List<Step> codeBlockContents; // Every Step inside the if statement code block

        public IfStatement() { }

        public IfStatement(List<Token> operands, List<Step> cbContents)
        {
            this.operands = operands;
            this.codeBlockContents = cbContents;
        }

        public override string ToString()
        {
            List<string> operandStrings = new List<string>();
            foreach (Token tok in operands) operandStrings.Add(tok.Value());

            string codeBlockString = "";
            foreach (Step step in this.codeBlockContents) // Collect each Step in the codeblock and add to string
            {
                codeBlockString += step.ToString() + "\n";
            }
            return "(IF) CONDITION: [" + String.Join("", operandStrings) + "]\n CONTENTS: \n" + codeBlockString; // Display condition operands AND each Step operand
        }
    }
}
