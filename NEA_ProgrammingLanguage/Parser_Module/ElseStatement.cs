using Lexer_Module;
using Parser_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser_Module
{
    class ElseStatement : Step
    {
        private List<Step> codeBlockContents; // Every Step inside the else statement code block

        public ElseStatement(List<Step> cbContents)
        {
            this.type = "ELSE_STATEMENT";
            this.codeBlockContents = cbContents;
        }

        public List<Step> GetCBContents() { return codeBlockContents.ToList(); } 
        // Why .ToList() when it's already a List? Because this forces it to pass effectively by Value instead of by reference - List<> is a reference type.


        public override string ToString()
        {

            string codeBlockString = "";
            foreach (Step step in this.codeBlockContents) // Collect each Step in the codeblock and add to string
            {
                codeBlockString += step.ToString() + "\n";
            }
            return "(ELSE) CONTENTS: \n" + codeBlockString; // Display each Step in codeblock
        }
    }
}
