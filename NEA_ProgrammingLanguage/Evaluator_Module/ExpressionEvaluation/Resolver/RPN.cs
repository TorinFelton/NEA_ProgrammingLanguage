using System;
using System.Collections.Generic;
using TreeTraversal;
using Errors;
using Parser_Module;

namespace Evaluator_Module.ExpressionEvaluation.Resolver
{
    class RPN
    {
        public static TreeNode Evaluate(List<TreeNode> nodes) // Input is a list of TreeNodes given in POSTFIX traversal order of the tree
        {
            Stack<TreeNode> nodeStack = new Stack<TreeNode>(); // Create stack to use for RPN, type TreeNode to encompass any possible type of node (all inherit from TreeNode)

            foreach (TreeNode treeNode in nodes)
            {
                if (treeNode is BoolOperand || treeNode is Number || treeNode is StringOperand) nodeStack.Push(treeNode); // if it is an operand, push to stack immediately

                else // Must be an 'operator' of some form - could be logical comparison or maths operation
                {
                    if (treeNode.value.Equals("!"))
                    {
                        BoolOperand arg = (BoolOperand)nodeStack.Pop();
                        nodeStack.Push(new BoolOperand(!arg.BoolValue())); // unary NOT, so flip the operand's boolean
                    } else if (treeNode.value.Equals("_"))
                    {
                        Number operand = (Number)nodeStack.Pop();
                        nodeStack.Push(new Number(operand.IntValue() * (-1))); // unary minus, so negate the operand
                    }
                    else
                    {
                        // Pop last two
                        TreeNode arg2 = nodeStack.Pop();
                        TreeNode arg1 = nodeStack.Pop(); // Note they are reversed, the first one to be popped is the second argument in the expression.

                        if (Syntax.IsComparator(treeNode.value))
                        {
                            nodeStack.Push(new BoolOperand(Compare(arg1, arg2, treeNode.value))); // add boolean result of comparison
                        } else
                        { // Must be an operation - can still be logical or mathematical
                            if (arg1 is Number && arg2 is Number) // mathematical operation
                            {
                                Number arg1N = (Number)arg1;
                                Number arg2N = (Number)arg2;

                                nodeStack.Push(CalculateNumeric(arg1N.IntValue(), arg2N.IntValue(), treeNode.value));
                            }
                            else if (arg1 is BoolOperand && arg2 is BoolOperand) // logical operation
                            {
                                BoolOperand arg1B = (BoolOperand)arg1;
                                BoolOperand arg2B = (BoolOperand)arg2;

                                nodeStack.Push(CalculateLogical(arg1B.BoolValue(), arg2B.BoolValue(), treeNode.value));
                            }
                            else if (arg1 is StringOperand && arg2 is StringOperand)
                            {
                                if (treeNode.value.Equals("+")) nodeStack.Push(new StringOperand(arg1.value + arg2.value));
                                else throw new TypeMatchError();
                            }
                            else throw new TypeMatchError();
                        }
                    }
                }
            }
            TreeNode result = nodeStack.Pop(); // Stack should be left with just one Num (Integer) as the final result
            return result;
        }

        public static bool Compare(TreeNode arg1, TreeNode arg2, string comparison)
        {
            bool toReturn = false; // default is false

            if ((arg1 is StringOperand && arg2 is StringOperand) || (arg1 is BoolOperand && arg2 is BoolOperand))
            {
                if (comparison.Equals("==")) toReturn = (arg1.value.Equals(arg2.value));
                else if (comparison.Equals("!=")) toReturn = !(arg1.value.Equals(arg2.value));
                else throw new ComparisonError(); // Cannot do any other comparison on strings.
            }
            else
            { // Any comparison that isn't == or != can ONLY be done on numbers
                int op1Integer; // We need to conver the number Tokens to actual Integers for comparison
                int op2Integer;
                if (arg1 is Number && arg2 is Number)
                {
                    // Value is stored as a string, hence the conversion to integer.
                    op1Integer = ((Number)arg1).IntValue();
                    op2Integer = ((Number)arg2).IntValue();
                }
                else throw new ComparisonError(); // Invalid types to do these comparisons on

                switch (comparison)
                {
                    case "==": // TokenEqual just checks type & value of Tokens are equal. No need to convert to C# Integer type.
                        toReturn = op1Integer == op2Integer;
                        break;
                    case "!=":
                        toReturn = !(op1Integer == op2Integer);
                        break;
                    case "<=":
                        // Check if equal or if less than. 
                        // Note TokenEqual takes in Tokens, OR we can compare their raw Integer values
                        toReturn = op1Integer <= op2Integer;
                        break;
                    case ">=":
                        // Check if equal or greater than. 
                        toReturn = op1Integer >= op2Integer;
                        break;
                    case "<":
                        toReturn = op1Integer < op2Integer;
                        break;
                    case ">":
                        toReturn = op1Integer > op2Integer;
                        break;
                    default:
                        toReturn = false;
                        break;
                }
            }

            return toReturn;
        }

        public static BoolOperand CalculateLogical(bool arg1, bool arg2, string comparison)
        {
            bool result = false; // Default is false

            switch (comparison)
            {
                case "&&":
                    result = arg1 && arg2;
                    break;
                case "||":
                    result = arg1 || arg2;
                    break;
                default:
                    break; // do nothing as result = 0 already
            }
            return new BoolOperand(result);
        }

        public static Number CalculateNumeric(int arg1, int arg2, string operation)
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
            return new Number(result);
        }

    }
}
