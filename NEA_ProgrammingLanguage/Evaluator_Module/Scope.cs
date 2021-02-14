using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluator_Module
{
    class Scope
    {
        private Dictionary<string, Token> variableScope;

        public Scope()
        {
            variableScope = new Dictionary<string, Token>();
        }

        public Scope(Dictionary<string, Token> scope)
        {
            variableScope = scope;
        }

        public bool VarExists(string varName) { return variableScope.ContainsKey(varName); }

        public string GetVarType(string varName) { return variableScope[varName].Type(); }
        public string GetVarValue(string varName) { return variableScope[varName].Value(); }
        public Token GetVarTokenValue(string varName) { return variableScope[varName]; }

        public void SetVarValue(string varName, Token newValue) { variableScope[varName] = newValue; }
        public void Add(string name, Token value) { variableScope.Add(name, value); }

    }
}
