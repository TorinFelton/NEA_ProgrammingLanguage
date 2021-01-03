using TreeTraversal;
using System.Collections.Generic;


namespace Evaluator_Module.ExpressionEvaluation
{
    class BinOp : TreeNode
    {
        public static Dictionary<string, int> precedences = new Dictionary<string, int>()
        {
            {"^", 4 },
            {"*", 3 },
            {"/", 3 },
            {"+", 2 },
            {"-", 2 },
            {")", 2 },
            {"(", 1 }
        }; // Static dictionary of precedence levels represented by ints for ease of comparison later on - used in Algorithms/Postfix.cs

        public BinOp(string operationValue)
        {
            this.value = operationValue;
        }

        public BinOp() // parameterless option
        {

        }
    }
}
