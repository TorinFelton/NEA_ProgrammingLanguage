using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module
{
    class IfStatement : Step
    {
        private List<Step> codeBlockContents; // Every Step inside the if statement code block
        // Example condition: "x + 10 == 13"
        private List<Token> operand1; // Would be 'x + 10'
        private List<Token> operand2; // Would be '13'
        private string comparator; // Would be '=='

        public IfStatement() { }

        public IfStatement(List<Step> cbContents, List<Token> op1, List<Token> op2, string comparator)
        {
            this.type = "IF_STATEMENT";
            this.codeBlockContents = cbContents;
            this.operand1 = op1;
            this.operand2 = op2;
            this.comparator = comparator;
        }

        public List<Token> GetOp1() { return operand1; }
        public List<Token> GetOp2() { return operand2; }
        public string GetComparator() { return comparator; }
        public List<Step> GetCBContents() { return codeBlockContents; }
        public override string ToString()
        {
            List<string> operand1String = new List<string>();
            List<string> operand2String = new List<string>();
            foreach (Token tok in operand1) operand1String.Add(tok.Value()); // Just get their values into a list of strings to print
            foreach (Token tok in operand2) operand2String.Add(tok.Value());

            string codeBlockString = "";
            foreach (Step step in this.codeBlockContents) // Collect each Step in the codeblock and add to string
            {
                codeBlockString += step.ToString() + "\n";
            }
            return "(IF) CONDITION: (" + String.Join("", operand1String) + comparator + String.Join("", operand2String) + ")\n CONTENTS: \n" + codeBlockString; // Display condition operands AND each Step operand
        }
    }
}
