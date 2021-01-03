using TreeTraversal;
using System;
using System.Collections.Generic;
using System.Data;
using Lexer_Module;

namespace Evaluator_Module.ExpressionEvaluation.Algorithms
{
    class Postfix
    {
        public static TreeNode InfixToPostfix(List<Token> infix)
        {
            Stack<BinOp> operatorStack = new Stack<BinOp>();
            Stack<TreeNode> numStack = new Stack<TreeNode>();

            foreach (Token token in infix)
            {
                if (token.Value().Equals("("))
                {
                    operatorStack.Push(new BinOp(token.Value()));
                }

                else if (token.Type().Equals("number")) // If it is a number, add to numStack
                {
                    numStack.Push(new Num(Int32.Parse(token.Value())));
                    // Simply create a new Num (inheritant from TreeNode) node with the number in the character.
                }


                else if (BinOp.precedences.ContainsKey(token.Value()) && !token.Value().Equals(")"))
                {
                    while (operatorStack.Count > 0 && BinOp.precedences[operatorStack.Peek().value] >= BinOp.precedences[token.Value()])
                    { // While the precedence of the top of the operatorStack is bigger than or equal to the precedence of the char
                        BinOp binOperator = operatorStack.Pop();

                        // Reversed as the second op was pushed at the end
                        binOperator.right = numStack.Pop();
                        binOperator.left = numStack.Pop();

                        numStack.Push(binOperator);
                    }
                    // Now push operator at the end 
                    operatorStack.Push(new BinOp(token.Value()));
                }

                else if (token.Value().Equals(")"))
                {
                    while (operatorStack.Count > 0 && !operatorStack.Peek().value.Equals("("))
                    {
                        BinOp binOperator = operatorStack.Pop();
                        // Reversed as the second op was pushed at the end
                        binOperator.right = numStack.Pop();
                        binOperator.left = numStack.Pop();

                        numStack.Push(binOperator);
                    }
                    operatorStack.Pop();
                }

                else
                {
                    throw new SyntaxErrorException();
                }
            }

            while (operatorStack.Count > 0)
            {
                BinOp binOperator = operatorStack.Pop();
                // Reversed as the second op was pushed at the end
                binOperator.right = numStack.Pop();
                binOperator.left = numStack.Pop();

                numStack.Push(binOperator);
            }

            return numStack.Pop();
        }

    }
}
