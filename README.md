# 'Creating an Interpreter' (NEA)
Repo for my AQA A-Level Computer Science Non-Exam Assessment (coursework).

- Uses the implementation of the [ExpressionEvaluator](https://github.com/TorinFelton/ExpressionEvaluator) repository for evaluating mathematical expressions.
- Will publish full NEA document when completed - this will contain analysis, design and implementation of project.

Targets:
- [x] Implement Lexer
- [x] Implement Parser
- [x] Implement Evaluator
- [x] Error handling outside of C# default exceptions
- [x] File input

# Showcase Program
<details>
<summary>Example Program Source Code</summary>
This is the 'input' to the interpreter: 
  
```c++
int x = 10*(4+90);
int y = 10 * 4+90;
string helloWrld = "";

if (x == 940) {
	output("X is: ");
	outputln(x);

	if (y == 130) {
		output("Y is: ");
		outputln(y);

		outputln("Order of operations works!");
	}
}

outputln("Give a new value for X: ");
inputInt(x);
if (x > 0) { outputln("X is greater than 0!"); }

outputln("Give a string value: ");
inputStr(helloWrld);
outputln("Your value: '" + helloWrld + "'");

```
</details>

<details>
  <summary>Running Example Program</summary>
  
```
-------------------- PROGRAM STARTED --------------------
X is: 940
Y is: 130
Order of operations works!
Give a new value for X:
> 100
X is greater than 0!
Give a string value:
> Hello World
Your value: 'Hello World'
-------------------- PROGRAM ENDED --------------------
```
  </details>
  

# Points of Interest

- [Main Lexer Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Lexer_Module/Tokeniser.cs)
- [Main Parser Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Parser_Module/Parser.cs)
- [Main Evaluator Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/Evaluator.cs)
- [Mathematical Expression Evaluator](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation)
  - [Tree Traversal](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/TreeTraversal/Traversal.cs)
  - [Infix to Postfix via Shunting-yard Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/Postfix.cs)
  - [RPN Evaluation Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/RPN.cs)
