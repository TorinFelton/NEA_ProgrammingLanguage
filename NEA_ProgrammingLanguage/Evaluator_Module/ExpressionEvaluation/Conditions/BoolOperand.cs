using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Evaluator_Module.ExpressionEvaluation.Conditions
{
    class BoolOperand : TreeNode
    {
        public bool boolValue;
        public BoolOperand(bool boolValue)
        {
            this.boolValue = boolValue;
            this.value = boolValue.ToString();
        }

        public BoolOperand() // parameterless option
        {

        }
    }
}
