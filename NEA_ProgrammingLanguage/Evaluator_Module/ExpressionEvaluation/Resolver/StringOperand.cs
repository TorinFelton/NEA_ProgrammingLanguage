using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Evaluator_Module.ExpressionEvaluation.Resolver
{
    class StringOperand : TreeNode
    {
        public StringOperand() { }
        public StringOperand(string value) { this.value = value; }
    }
}
