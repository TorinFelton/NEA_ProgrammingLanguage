using Lexer_Module;
using Parser_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module.Events
{
    class VarChange : Event
    {
        private string varName;
        private List<Token> varValue;

        public VarChange(string varName, List<Token> varValue)
        {
            this.type = "VAR_CHANGE";
            this.varName = varName;
            this.varValue = varValue;
        }

        public string Name() { return varName; }

        public List<Token> Value() { return varValue; }

        public override string ToString()
        {
            List<string> valueTokens = new List<string>();
            foreach (Token tok in varValue) valueTokens.Add(tok.Value());

            return "VAR_CHANGE: {name: '" + varName + "', value tokens: [" + String.Join(", ", valueTokens) + "]}";
        }
    }
}
