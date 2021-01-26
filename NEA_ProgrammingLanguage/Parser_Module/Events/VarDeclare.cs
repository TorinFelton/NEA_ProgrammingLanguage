using Lexer_Module;
using Parser_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module.Events
{
    class VarDeclare : Event
    {
        private string varType;
        private string varName;
        private List<Token> varValue;

        public VarDeclare(string varType, string varName, List<Token> varValue)
        {
            this.type = "VAR_DECLARE";
            this.varType = varType;
            this.varName = varName;
            this.varValue = varValue;
        }

        public string GetName() { return varName; }
        public string GetVarType() 
        {
            if (varType.Equals("int")) return "number"; 
            else return varType;
            // Syntax physically written as int, but we generalise to 'number' as we only support Integers.
            // Programmatically, they are treated as 'number' type. They are just called 'int' in the interpretation
            // to give a similar syntax style to C++/Java/C# for learning.
        }
        public List<Token> Value() { return varValue; } // Naming conflict if called GetValue()

        public override string ToString()
        {
            List<string> valueTokens = new List<string>();
            foreach (Token tok in varValue) valueTokens.Add(tok.Value());

            return "VAR_DECLARE: {type: '" + varType + "', name: '" + varName + "', value tokens: [" + String.Join(", ", valueTokens) + "]}";
            // Type of variable, name, value
        }
    }
}
