using Lexer_Module;
using Parser_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser_Module.Events
{
    class FuncDeclare : Event
    {
        private string funcName;
        private Dictionary<string, string> parameterScope; // name, type
        private List<Step> cbContents;
        // Arguments could be an expression or a single element referencing a variable name.

        public FuncDeclare(string funcName, Dictionary<string, string> parameters, List<Step> cbContents)
        {
            this.type = "FUNC_DECLARE";
            this.funcName = funcName.ToLower();
            this.parameterScope = parameters;
            this.cbContents = cbContents;
        }

        public string GetName() { return funcName; }

        public Dictionary<string, string> GetParameters() { return parameterScope; }

        public List<Step> GetcbContents() { return cbContents; }

        public override string ToString()
        {
            /*
            List<string> argumentTokens = new List<string>();
            foreach (Token tok in arguments) argumentTokens.Add(tok.Value());
            */

            return "FUNC_DECLARE: {name: '" + funcName + "', argument tokens: [" + String.Join(", ", parameterScope.Keys.Select(x => x.ToString() + ": " + parameterScope[x])) + "]} CODEBLOCK:\n"
                + String.Join("\n ", cbContents.Select(x => x.ToString()));
            
            // Return function name and argument (in form of expression represented by Tokens) and cbcontents
        }
    }
}
