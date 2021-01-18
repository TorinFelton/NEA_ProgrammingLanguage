using Lexer_Module;
using Parser_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    class ElseStatement : Step
    {
        private List<Step> codeBlockContents; // Every Step inside the else statement code block

        public ElseStatement() { }

        public ElseStatement(List<Step> cbContents)
        {
            this.type = "ELSE_STATEMENT";
            this.codeBlockContents = cbContents;
        }

        public List<Step> GetCBContents() { return codeBlockContents; }
        public override string ToString()
        {

            string codeBlockString = "";
            foreach (Step step in this.codeBlockContents) // Collect each Step in the codeblock and add to string
            {
                codeBlockString += step.ToString() + "\n";
            }
            return "(ELSE) CONTENTS: \n" + codeBlockString; // Display condition operands AND each Step operand
        }
    }
}
