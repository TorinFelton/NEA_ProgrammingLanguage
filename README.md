# 'Creating an Interpreter' (NEA)
Repo for my AQA A-Level Computer Science Non-Exam Assessment (coursework).

- Uses the implementation of the [ExpressionEvaluator](https://github.com/TorinFelton/ExpressionEvaluator) repository for evaluating mathematical expressions.
- Experimental for extra features, e.g Else statements, While, etc. these will not be added to the Master branch. 

(NEA) Targets:
- [x] Implement Lexer
- [x] Implement Parser
- [x] Implement Evaluator
- [x] Error handling outside of C# default exceptions
- [x] File input

# Recent Updates

### Experimental Version
This is the version that I have continued to develop beyond the NEA coursework requirements. 
- [Master Branch](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/master): NEA version, basic limited functionality
- [Experimental Branch](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/experimental): More complex and functional version

The experimental branch will continue to be supported, the master will only get occasional updates if something is urgent, but no additional features. These are some of the experimental targets, most of which have been met:

(Experimental) Targets:
- [x] Else statements
- [ ] Else if statements
- [x] While loops
- [x] More advanced condition parsing (currently only supports 1 comparison)
  - [x] Boolean variable type
- [x] Shell implementation
- [x] Subroutines
  
The experimental targets are a lot less likely to be implemented and have not been designed prior like the NEA ones.

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

  <summary>Running Example Program</summary>
'>' in the console indicates an input prompt.
  
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

# Addition Notes

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

  </details>
  


# Points of Interest

- [Main Lexer Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Lexer_Module/Tokeniser.cs)
- [Main Parser Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Parser_Module/Parser.cs)
- [Main Evaluator Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/Evaluator.cs)
- [Mathematical Expression Evaluator](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation)
  - [Tree Traversal](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/TreeTraversal/Traversal.cs)
  - [Infix to Postfix via Shunting-yard Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/Postfix.cs)
  - [RPN Evaluation Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/RPN.cs)
