using Lexer_Module;
using Parser_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module.Events
{
    class FuncCall : Event
    {
        private string funcName;
        private List<Token> arguments;

        public FuncCall(string funcName, List<Token> arguments)
        {
            this.type = "FUNC_CALL";
            this.funcName = funcName.ToLower();
            this.arguments = arguments;
        }

        public string Name() { return funcName; }

        public List<Token> Arguments() { return arguments; }

        public override string ToString()
        {
            List<string> argumentTokens = new List<string>();
            foreach (Token tok in arguments) argumentTokens.Add(tok.Value());

            return "FUNC_CALL: {name: '" + funcName + "', argument tokens: [" + String.Join(", ", argumentTokens) + "]}";
        }
    }
}
