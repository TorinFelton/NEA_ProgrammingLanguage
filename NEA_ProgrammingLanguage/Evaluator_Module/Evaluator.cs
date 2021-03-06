using Errors;
using Evaluator_Module.ExpressionEvaluation.Algorithms;
using Lexer_Module;
using Parser_Module;
using Parser_Module.Events;
using System;
using System.Collections.Generic;
using TreeTraversal;

namespace Evaluator_Module
{
    class Evaluator
    {
        private Dictionary<string, Token> variableScope;

        public Evaluator()
        {
            this.variableScope = new Dictionary<string, Token>();
            // Variables are all stored in this dictionary
            // Their value is stored as a single Token object
            // Remember from Tokeniser - Tokens can have types like 'number' or 'string' (but all are *stored* as strings)
            // To check a variable type, we check the type of its Token (e.g if its 'number' we have an integer variable)
        }

        public void Evaluate(List<Step> evaluationSteps)
        {
            for (int index = 0; index < evaluationSteps.Count; index++)
            {
                Step evalStep = evaluationSteps[index];
                // .Type() can only be "VAR_DECLARE", "VAR_CHANGE", "FUNC_CALL", "IF_STATEMENT", "WHILE_LOOP"
                // It could also be "ELSE_STATEMENT", but we should only check for that DIRECTLY after an IF_STATEMENT
                if (evalStep.Type().Equals("IF_STATEMENT"))
                {
                    // Evaluate if statement - contains OPERAND1, OPERAND2, COMPARISON, codeBlockContents
                    IfStatement ifState = (IfStatement)evalStep; // Cast as we know it is now an IfStatement obj
                    bool conditionResult = CompareExpressions(ifState.GetOp1(), ifState.GetOp2(), ifState.GetComparator());
                    bool hasElse = index + 1 < evaluationSteps.Count && evaluationSteps[index + 1].Type().Equals("ELSE_STATEMENT"); // No chance of index out of range error as set to False before reaching it

                    if (conditionResult)
                    // If the 'IfStatement' condition is TRUE
                    {
                        Evaluate(ifState.GetCBContents()); // 'run' the contents of the if statement - this is RECURSIVE
                        if (hasElse) evaluationSteps.RemoveAt(index + 1);
                        // If we have an ELSE_STATEMENT after this, we need to remove it as the IF_STATEMENT has triggered (therefore the ELSE will not be triggered).
                    }
                    else if (hasElse)
                    {
                        // If the CONDITION is FALSE and the next Step obj is an ELSE_STATEMENT type

                        ElseStatement elseState = (ElseStatement)evaluationSteps[index+1];
                        // Cast to else
                        Evaluate(elseState.GetCBContents()); // 'run' the contents of the else (RECURSION)

                        evaluationSteps.RemoveAt(index + 1); // Remove ELSE_STATEMENT as we have used it and do not want to go over it again.
                    }
                }
                else if (evalStep.Type().Equals("WHILE_LOOP"))
                {
                    WhileLoop whileLoop = (WhileLoop)evalStep;
                    // Similar to if statement evaluation though no need to set a 'condition' variable because that condition may change
                    // Basically just reusing the C# while loop with the template of the Interpreted one
                    while (CompareExpressions(whileLoop.GetOp1(), whileLoop.GetOp2(), whileLoop.GetComparator()))
                    {
                        // While the condition is true, evaluate code inside
                        Evaluate(whileLoop.GetCBContents());
                    }

                }
                else if (evalStep.Type().Equals("VAR_DECLARE"))
                // Declare a variable in the variableScope
                {
                    VarDeclare varDecl = (VarDeclare)evalStep; // Cast as we know it's a VarDeclare obj
                    if (variableScope.ContainsKey(varDecl.GetName())) throw new DeclareError();
                    // If scope already has a variable that name, you cannot redeclare it as it already exists.
                    // Potential endpoint if variable exists - entire program will stop (crash).
                    Token varExpr = ResolveExpression(varDecl.Value());

                    if (!varExpr.Type().Equals(varDecl.GetVarType())) throw new TypeError();
                    // Value of variable does not match type with declared one. e.g 'int x = "Hello";'


                    variableScope.Add(varDecl.GetName(), varExpr);
                    // Type of variable can be found out by the .Type() of the key's Token.
                    // e.g 'int x = 1 + 2;'
                    // if we want to find variable 'x' type, we find variableScope[x].Type() which will return 'number', with variableScope[x].Value() being '3'
                }
                else if (evalStep.Type().Equals("VAR_CHANGE"))
                // Change a pre-existing variable
                {
                    VarChange varChan = (VarChange)evalStep; // Cast as we know it is a VarChange obj

                    if (!variableScope.ContainsKey(varChan.GetName())) throw new ReferenceError();
                    // If variable is NOT in the variableScope then we cannot change it as it doesn't exist.
                    // Potential endpoint for program crash
                    string varType = variableScope[varChan.GetName()].Type();
                    Token newValue = ResolveExpression(varChan.Value());

                    if (!varType.Equals(newValue.Type())) throw new TypeError();
                    // If the new value of the variable is not the right type, then crash.
                    // Potential endpoint
                    // e.g int x = 0; x = "hi"; will cause this error
                    variableScope[varChan.GetName()] = newValue; // Assign new value (Token)

                }
                else if (evalStep.Type().Equals("FUNC_CALL"))
                // Call a function
                {
                    FuncCall functionCall = (FuncCall)evalStep; // Cast as we know it is a FuncCall obj now
                    if (!functionCall.GetName().Equals("inputstr") && !functionCall.GetName().Equals("inputint")) // If NOT calling 'input' function
                    {
                        CallFunction(functionCall.GetName(), ResolveExpression(functionCall.GetArguments()));
                        // Call function with name and *resolved* list of arguments
                        // Resolve function always outputs a single token which is the result of an expression (list of tokens) being evaluated
                    } else
                        // SPECIAL CASE: Calling inputStr or inputInt functions indicates that the 'argument' is NOT an expression to be resolved, but rather a variable name to store input value in.
                        // This means functionCall.Argumnets() will only have 1 token:
                    {
                        CallFunction(functionCall.GetName(), functionCall.GetArguments()[0]); // Pass in first value in Arguments as there only should be one - the variable to be input to
                    }
                }
                else throw new SyntaxError(); // Unrecognised Step, crash program.
            }
        }

        public Token ResolveExpression(List<Token> expr) // TODO
        {
            Token toReturn = new Token("", "");

            // First, replace variable name references with their values.
            expr = VariablesToValues(expr);

            // Now check tokens are all the same type in the expression (except grammar tokens)

            string exprResultType = CheckTypes(expr); // This func will throw error if they aren't
            // exprResultType now stores the final expected type for when expression is resolved to one token
            // e.g 1 + 1 => resolves to 'number'
            // e.g "1" + "1" => resolves to 'string'

            if (exprResultType.Equals("string"))
                // Indicates that we are dealing with a string expression
            {
                // The only operation that can be done to strings in an expression is '+' for concat
                if (expr.Count == 1) toReturn = new Token("string", expr[0].Value());
                // If there is only one token in the whole expression, it must just be a string
                // Therefore we can just return the string as it's 1 token
                else
                {
                    // We must be dealing with concatenation
                    if (!expr[0].Type().Equals("string")) throw new SyntaxError();
                    // Concatenation expressions MUST start with a string
                    // e.g string x = + "Hello World"; will cause ERROR as expr starts with '+'

                    string finalResult = expr[0].Value(); // First string in expression
                    int index = 1;

                    while (index < expr.Count)
                    {
                        if (expr[index].Type().Equals("operator"))
                        {
                            if (expr[index].Value().Equals("+") && index < expr.Count - 1)
                            {
                                finalResult += expr[index + 1].Value(); // Add NEXT string to final result
                            }
                            else throw new TypeMatchError(); // Cannot do any other operation than '+' on strings
                        }

                        index++;
                    }
                    toReturn = new Token("string", finalResult);
                }
            }
            else if (exprResultType.Equals("number"))
                // Indicates we are dealing with a mathematical expression
            {
                TreeNode root = TreeBuilder.BuildAST(expr); // Create abstract syntax tree of mathematical expression

                int result = RPN.Evaluate(Traversal.postOrder(root)); // Calculate result of RPN algorithm calculation

                toReturn = new Token("number", result.ToString());
            }
            else throw new SyntaxError(); // invalid expression type has somehow made it through, we cannot evaluate it so throw error.

            return toReturn;
        }

        public List<Token> VariablesToValues(List<Token> expr)
        {
            // Replace each variable in an expression with the value it references

            List<Token> newExpr = new List<Token>();
            Token toAdd;
            foreach (Token tok in expr)
            {
                if (tok.Type().Equals("identifier"))
                // Must be variable reference
                {
                    if (variableScope.ContainsKey(tok.Value()))
                    // If that variable exists
                    {
                        toAdd = variableScope[tok.Value()];
                        // Add referenced variable's VALUE token to expression instead of the actual reference to the variable
                    }
                    else throw new ReferenceError(); // Referencing non-existent variable, throw error
                }
                else toAdd = tok;

                newExpr.Add(toAdd);
            }

            return newExpr;
        }

        public string CheckTypes(List<Token> expr)
        {
            // Check all types in an expression can work together
            // e.g 1 + 1 will work
            // e.g "1" + "1" will work
            // e.g "1" + 1 will cause an ERROR.
            bool foundNumber = false;
            bool foundString = false;

            foreach (Token tok in expr)
            {
                if (!tok.Type().Equals("grammar"))
                {
                    if (tok.Type().Equals("number")) foundNumber = true;
                    else if (tok.Type().Equals("string")) foundString = true;
                }
            }

            if (foundString == foundNumber) throw new TypeMatchError(); // Found either both or neither "string" and "number" types in same expression, causes error.
            else if (foundNumber) return "number";
            else return "string"; // Must be a string if not foundNumber and not foundString == foundNumber == false.
        }

        public bool CompareExpressions(List<Token> op1, List<Token> op2, string comparison)
        {
            bool toReturn;
            Token resolvedOp1 = ResolveExpression(op1);
            Token resolvedOp2 = ResolveExpression(op2);

            if (resolvedOp1.Type().Equals("string") && resolvedOp2.Type().Equals("string"))
            {
                if (comparison.Equals("==")) toReturn = TokenEqual(resolvedOp1, resolvedOp2);
                else if (comparison.Equals("!=")) toReturn = !TokenEqual(resolvedOp1, resolvedOp2);
                else throw new ComparisonError(); // Cannot do any other comparison on strings.

            } else { // Any comparison that isn't == or != can ONLY be done on numbers
                int op1Integer; // We need to conver the number Tokens to actual Integers for comparison
                int op2Integer;
                if (resolvedOp1.Type().Equals("number") && resolvedOp2.Type().Equals("number"))
                {
                    // Value is stored as a string, hence the conversion to integer.
                    op1Integer = int.Parse(resolvedOp1.Value());
                    op2Integer = int.Parse(resolvedOp2.Value());
                } else throw new ComparisonError(); // Invalid types to do these comparisons on

                switch (comparison)
                {
                    case "==": // TokenEqual just checks type & value of Tokens are equal. No need to convert to C# Integer type.
                        toReturn = TokenEqual(resolvedOp1, resolvedOp2);
                        break;
                    case "!=":
                        toReturn = !TokenEqual(resolvedOp1, resolvedOp2);
                        break;
                    case "<=":
                        // Check if equal or if less than.
                        // Note TokenEqual takes in Tokens, OR we can compare their raw Integer values
                        toReturn = TokenEqual(resolvedOp1, resolvedOp2) || (op1Integer < op2Integer);
                        break;
                    case ">=":
                        // Check if equal or greater than.
                        toReturn = TokenEqual(resolvedOp1, resolvedOp2) || (op1Integer > op2Integer);
                        break;
                    case "<":
                        toReturn = (op1Integer < op2Integer);
                        break;
                    case ">":
                        toReturn = (op1Integer > op2Integer);
                        break;
                    default:
                        toReturn = false;
                        break;
                }
            }

            return toReturn;
        }

        public bool TokenEqual(Token tok1, Token tok2) { return tok1.Type().Equals(tok2.Type()) && tok1.Value().Equals(tok2.Value()); }
        // Checks both type and value of a token are equal
        // simplified: return tok1.Type == tok2.Type && tok1.Value == tok2.Value
        // Using .Equals instead of == to avoid wrong definition of 'equal', .Equals is for literal equality in value

        public void CallFunction(string funcName, Token argument)
            // As this is a simple language, functions can only take one argument.
        {
            funcName = funcName.ToLower(); // We do not need to be case sensitive for our language.
            if (funcName.Equals("output")) Console.Write(argument.Value()); // Write to console with no new line
            else if (funcName.Equals("outputln")) Console.WriteLine(argument.Value()); // Write with new line
            else if (funcName.Equals("inputstr"))
            {
                string varName = argument.Value(); // Argument passed into input() function is a VARIABLE NAME REFERENCE

                if (!variableScope.ContainsKey(varName)) throw new ReferenceError();
                // If variable to input to doesn't exist there is an error

                if (!variableScope[varName].Type().Equals("string")) throw new TypeError();
                // inputStr() being done to a non-string variable causes an error

                Console.Write("> "); // Automatic input prompt
                string input = Console.ReadLine();

                variableScope[varName] = new Token("string", input); // Change value of variable to the input string
            }
            else if (funcName.Equals("inputint"))
            {
                string varName = argument.Value();

                if (!variableScope.ContainsKey(varName)) throw new ReferenceError();
                // If variable to input to doesn't exist there is an error

                if (!variableScope[varName].Type().Equals("number")) throw new TypeError();
                // inputInt() being done to a non-number variable causes an error

                Console.Write("> "); // Automatic input prompt
                int input;
                try
                {
                    input = int.Parse(Console.ReadLine());
                }
                catch
                {
                    // if input is not a number, cause error
                    throw new TypeError();
                }
                // It looks weird to catch and then throw an error anyway, but I've done it so I can use my custom TypeError() instead of C#'s one
                // My TypeError() will come up with a simple message and pause, the C# in-built error will kill the console window instead.

                variableScope[varName] = new Token("number", input.ToString());
                // The input is converted to an integer originally to check it is ACTUALLY a valid integer
                // Once we know that, we can store it as a 'number' token with string value and convert it without errors later
            }
            else throw new ReferenceError(); // No function name recognised, throw error.
        }
    }
}
