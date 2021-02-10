using TreeTraversal;
using System;
using System.Collections.Generic;


namespace Evaluator_Module.ExpressionEvaluation.Algorithms
{
    class RPN
    {
        public static int Evaluate(List<TreeNode> nodes) // Input is a list of TreeNodes given in POSTFIX traversal order of the tree
        {
            Stack<TreeNode> nodeStack = new Stack<TreeNode>(); // Create stack to use for RPN, type TreeNode as it could be a BinOp or Num

            foreach (TreeNode treeNode in nodes)
            {
                if (treeNode is Num) nodeStack.Push(treeNode); // If it is a Num (Integer) then just push onto stack
                else if (treeNode is BinOp) // If it is a BinOp (operator, root of leaf) then pop last two operands and calculate:
                {
                    if (treeNode.value.Equals("_")) // unary minus
                    {
                        Num arg = (Num)nodeStack.Pop(); // Only pop one off the stack, unary minus only operates on one operand

                        nodeStack.Push(new Num(arg.IntValue() * (-1))); // unary minus, so negate the operand
                    }
                    else
                    {
                        // Pop last two
                        Num arg2 = (Num)nodeStack.Pop();
                        Num arg1 = (Num)nodeStack.Pop(); // Note they are reversed, the first one to be popped is the second argument in the expression.

                        nodeStack.Push(new Num(Calculate(arg1.IntValue(), arg2.IntValue(), treeNode.value))); // Create new Num (Integer) with result of calc
                    }
                }
            }
            Num result = (Num)nodeStack.Pop(); // Stack should be left with just one Num (Integer) as the final result
            return result.IntValue();
        }

        public static int Calculate(int arg1, int arg2, string operation) // Simplest way to 'act out' operators that are in string-form
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
