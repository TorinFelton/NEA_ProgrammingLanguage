using TreeTraversal;
using System.Collections.Generic;


namespace Evaluator_Module.ExpressionEvaluation.Resolver
{
    class MathsOperator : TreeNode
    {
        public static Dictionary<string, int> precedences = new Dictionary<string, int>()
        {
            {"^", 4 },
            {"_", 4 },
            {"*", 3 },
            {"/", 3 },
            {"+", 2 },
            {"-", 2 },
            {")", 2 },
            {"(", 1 }
        }; // Static dictionary of precedence levels represented by ints for ease of comparison later on - used in TreeBuilder.cs

        public MathsOperator(string operationValue)
        {
            this.value = operationValue;
        }

        public MathsOperator() // parameterless option
        {

        }
    }
}
