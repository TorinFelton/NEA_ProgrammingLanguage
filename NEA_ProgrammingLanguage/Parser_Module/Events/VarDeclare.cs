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

        public string Name() { return varName; }
        public string VarType() { return varType; }
        public List<Token> Value() { return varValue; }

        public override string ToString()
        {
            List<string> valueTokens = new List<string>();
            foreach (Token tok in varValue) valueTokens.Add(tok.Value());

            return "VAR_DECLARE: {type: '" + varType + "', name: '" + varName + "', value tokens: [" + String.Join(", ", valueTokens) + "]}";
        }
    }
}
