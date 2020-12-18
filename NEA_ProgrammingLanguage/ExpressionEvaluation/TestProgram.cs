﻿using ExpressionEvaluation.Algorithms;
using ExpressionEvaluation.TreeTraversal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluation
{
    class TestProgram
    {
        public static void Run()
        {
            TreeNode bin1 = Postfix.InfixToPostfix(Console.ReadLine()); // e.g "5 * 2 + 1" -> "5 2 * 1 +"
            // Using TreeNode type, not BinOp (Binary Operator) as we cannot guarantee the root node of the abstract syntax tree will be an operator.

            Console.WriteLine("To postfix:");
            foreach (TreeNode node in Traversal.postOrder(bin1))
            {
                Console.Write(node.value + " ");
            }
            Console.WriteLine("\nTo infix:");
            foreach (TreeNode node in Traversal.inOrder(bin1))
            {
                Console.Write(node.value + " ");
            }
            Console.WriteLine("\nTo prefix:");
            foreach (TreeNode node in Traversal.preOrder(bin1))
            {
                Console.Write(node.value + " ");
            }
            Console.WriteLine();

            // Now using reverse polish notation, calculate what the result is. This takes in a postfix-ordered list of TreeNodes.
            Console.WriteLine("Answer: " + RPN.Evaluate(Traversal.postOrder(bin1)));

        }
    }
}
