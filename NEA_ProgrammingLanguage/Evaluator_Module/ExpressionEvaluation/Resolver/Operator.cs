using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Evaluator_Module.ExpressionEvaluation.Resolver
{
    class Operator : TreeNode
    {
        public static readonly Dictionary<string, int> precedences = new Dictionary<string, int>()
        {
            {"!", 5 },
            {"^", 5 },
            {"_", 5 },
            {"*", 4 },
            {"/", 4 },
            {"||", 2 },
            {"&&", 2 },
            {"==", 2 },
            {"<",  2 },
            {">", 2 },
            {"!=", 2 },
            {"<=", 2 },
            {">=", 2 },
            {"+", 3 },
            {"-", 3 },
            {")", 3 },
            {"(", 1 }
        };
        public Operator() { }
        public Operator(string value) { this.value = value; }
    }
}
