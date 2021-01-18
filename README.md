# 'Creating an Interpreter' NEA (Experimental Branch)
Repo for my AQA A-Level Computer Science Non-Exam Assessment (coursework). This branch is for continuing with the project past the actual content of the A-Level NEA - this is not going to be submitted. Features that I want to add for fun will be added here - the master branch is the actual NEA submission. 

- Uses the implementation of the [ExpressionEvaluator](https://github.com/TorinFelton/ExpressionEvaluator) repository for evaluating mathematical expressions.
- Experimental for extra features, e.g Else statements, While, etc. these will not be added to the Master branch. 

(NEA) Targets:
- [x] Implement Lexer
- [x] Implement Parser
- [x] Implement Evaluator
- [x] Error handling outside of C# default exceptions
- [x] File input

(Experimental) Targets:
- [x] Else statements
- [ ] Else if statements
- [x] While statements
- [ ] More advanced condition parsing (currently only supports 1 comparison)
  - [ ] Boolean variable type
  
The experimental targets are a lot less likely to be implemented and have not been designed prior like the NEA ones.

# Experimental Addition Notes

<details>
<summary>While Loops</summary>
I've just reused the template from the 'If' statements and modified it slightly to support while loops - the WhileStatement object directly inherits from the IfStatement one. 
As I've added 'While' statements, more complex programs can be created:
  
<details>
  <summary>Simple Guessing Game</summary>
  
```c#

string password = "abc123";
string guess = "";
int guessAmount = 0;

while (guess != password) {
	outputln("Guess the password.");
	inputStr(guess);
	guessAmount = guessAmount + 1;
}

output("You guessed it! Attempts: ");
outputln(guessAmount);

```

Program running:
```
-------------------- PROGRAM STARTED --------------------
Guess the password.
> abwd
Guess the password.
> abc
Guess the password.
> abc 123
Guess the password.
> I don't know!
Guess the password.
> abc123
You guessed it! Attempts: 5
-------------------- PROGRAM ENDED --------------------

```
</details>

<details>
  <summary>Number Search</summary>
	Note this is still a slightly weird implementation due to the limitations of the language so far.
  
```c#

int x = 10*(4/1+1)*27+1;
int y = 0;
int z = 99999;

int result = 0;

while (result != x) {
	if (y == x) {
		result = y;
		output("Found! Y");
	} else {
		if (z == x) {
			result = x;
			output("Found! Z");
		}
	}
	y = y + 1;
	z = z - 1;
}

outputln(result);
```

Program running:
```
-------------------- PROGRAM STARTED --------------------
Found! Y 1351
-------------------- PROGRAM ENDED --------------------

```
  
</details>
</details>

# Points of Interest

- [Main Lexer Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Lexer_Module/Tokeniser.cs)
- [Main Parser Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Parser_Module/Parser.cs)
- [Main Evaluator Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/Evaluator.cs)
- [Mathematical Expression Evaluator](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation)
  - [Tree Traversal](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/TreeTraversal/Traversal.cs)
  - [Infix to Postfix via Shunting-yard Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/Postfix.cs)
  - [RPN Evaluation Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/RPN.cs)
