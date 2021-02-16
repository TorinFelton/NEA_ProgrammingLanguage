using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Evaluator_Module.ExpressionEvaluation.Resolver
{
    class BoolOperand : TreeNode
    {
        private bool boolValue;
        public BoolOperand(bool boolValue)
        {
            this.boolValue = boolValue;
            this.value = boolValue.ToString();
        }

        public BoolOperand() // parameterless option
        {

        }

        public bool BoolValue() { return boolValue; }
    }
}
