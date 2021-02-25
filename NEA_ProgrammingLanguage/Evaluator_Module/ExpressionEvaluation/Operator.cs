using TreeTraversal;
using System.Collections.Generic;


namespace Evaluator_Module.ExpressionEvaluation
{
    class Operator : TreeNode
    {
        public static Dictionary<string, int> precedences = new Dictionary<string, int>()
        {
            {"_", 5 }, // unary minus has a higher precedence than all below
            {"^", 4 },
            {"*", 3 },
            {"/", 3 },
            {"+", 2 },
            {"-", 2 },
            {")", 2 },
            {"(", 1 }
        }; // Static dictionary of precedence levels represented by ints for ease of comparison later on - used in TreeBuilder.cs

        public Operator(string operationValue)
        {
            this.value = operationValue;
        }

        public Operator() // parameterless option
        {

        }
    }
}
