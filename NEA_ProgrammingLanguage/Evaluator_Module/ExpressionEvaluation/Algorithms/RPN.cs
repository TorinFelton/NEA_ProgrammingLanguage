using TreeTraversal;
using System;
using System.Collections.Generic;


namespace Evaluator_Module.ExpressionEvaluation.Algorithms
{
    class RPN
    {
        public static int Evaluate(List<TreeNode> nodes)
        {
            Stack<TreeNode> nodeStack = new Stack<TreeNode>(); // Create stack to use for RPN, type TreeNode as it could be a BinOp or Num

            foreach (TreeNode treeNode in nodes)
            {
                if (treeNode is Num) nodeStack.Push(treeNode); // If it is a Num (operand) then just push onto stack
                else if (treeNode is BinOp) // If it is a BinOp (operator) then pop last two operands and calculate:
                {
                    // Pop last two
                    Num arg2 = (Num) nodeStack.Pop();
                    Num arg1 = (Num) nodeStack.Pop(); // Note they are reversed, the first one to be popped is the second argument in the expression.

                    nodeStack.Push(new Num(Calculate(arg1.intValue, arg2.intValue, treeNode.value))); // push result as a Num type
                }
            }
            Num result = (Num)nodeStack.Pop(); // Stack should be left with just one Num (operand) as the final result
            return result.intValue;
        }

        public static int Calculate(int arg1, int arg2, string operation)
        {
            int result = 0; // Default is 0
            switch (operation)
            {
                case "+":
                    result = arg1 + arg2;
                    break;
                case "-":
                    result = arg1 - arg2;
                    break;
                case "*":
                    result = arg1 * arg2;
                    break;
                case "/":
                    result = arg1 / arg2;
                    break;
                case "^":
                    result = (int)Math.Pow(arg1, arg2);
                    break;
                default:
                    break; // do nothing as result = 0 already
            }
            return result;
        }
    }
}
