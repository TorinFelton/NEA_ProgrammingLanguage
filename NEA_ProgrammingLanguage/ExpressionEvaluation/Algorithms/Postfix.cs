using ExpressionEvaluation.TreeTraversal;
using System;
using System.Collections.Generic;
using System.Data;


namespace ExpressionEvaluation.Algorithms
{
    class Postfix
    {
        public static TreeNode InfixToPostfix(string infix)
        {
            Stack<BinOp> operatorStack = new Stack<BinOp>();
            Stack<TreeNode> numStack = new Stack<TreeNode>();

            foreach (ExpressionToken token in TokeniseExpression(infix))
            {
                if (token.value == "(")
                {
                    operatorStack.Push(new BinOp(token.value));
                }

                else if (token.isNumber) // If it is a number, add to numStack
                {
                    numStack.Push(new Num(Int32.Parse(token.value)));
                    // Simply create a new Num (inheritant from TreeNode) node with the number in the character.
                }

                else if (BinOp.precedences.ContainsKey(token.value) && token.value != ")")
                {
                    while (operatorStack.Count > 0 && BinOp.precedences[operatorStack.Peek().value] >= BinOp.precedences[token.value])
                    { // While the precedence of the top of the operatorStack is bigger than or equal to the precedence of the char
                        BinOp binOperator = operatorStack.Pop();
                        // Reversed as the second op was pushed at the end
                        binOperator.right = numStack.Pop();
                        binOperator.left = numStack.Pop();

                        numStack.Push(binOperator);
                    }
                    // Now push operator at the end 
                    operatorStack.Push(new BinOp(token.value));
                }

                else if (token.value == ")")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek().value != "(")
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

        public static List<ExpressionToken> TokeniseExpression(string expr) // Change a string to a list of tokens
        {
            List<ExpressionToken> exprTokens = new List<ExpressionToken>();
            string numCache = ""; // Cache of digits found, for numbers longer than one digit. 

            foreach (char character in expr.Replace(" ", "")) // Guarantees no white space found or distributed in tokens.
            {
                if (BinOp.precedences.ContainsKey(character.ToString())) // If the character is an operator.
                {
                    if (numCache.Length > 0) // If we still have digits in the cache then submit them to the token list as a single token (number) and clear it.
                    {
                        exprTokens.Add(new ExpressionToken(numCache, true)); // Add full integer (currently in string data type) to tokens, true meaning it is a number.
                        numCache = "";
                    }
                    exprTokens.Add(new ExpressionToken(character.ToString(), false)); // Add operator to tokens, false meaning it is not a number.
                }
                else if (Char.IsDigit(character)) // If it's a digit, add it to the numCache
                {
                    numCache += character;
                }
            }
            if (numCache.Length > 0) exprTokens.Add(new ExpressionToken(numCache, true)); // If the expression ends and we still have cached digits, add the digits collected to the tokens list.

            return exprTokens;
        }
    }
}
