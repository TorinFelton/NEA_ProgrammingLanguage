# 'Creating an Interpreter' (Experimental)
Repo for my AQA A-Level Computer Science Non-Exam Assessment (coursework).

- Uses the implementation of the [ExpressionEvaluator](https://github.com/TorinFelton/ExpressionEvaluator) repository for evaluating mathematical expressions.
- Experimental for extra features, e.g Else statements, While, etc. these will not be added to the Master branch. 

### Experimental Version
This is the version that I have continued to develop beyond the NEA coursework requirements. 
- [Master Branch](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/master): NEA version, basic limited functionality
- [(Current) Experimental Branch](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/experimental): More complex and functional version

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

# Addition Notes

<details>
<summary>Subroutines</summary>

<details>
	<summary> Recursive fibonacci nth term </summary>
	Not the most efficient way, but it just tests the recursion and return statements.

	```c#
func fib(int n) returns int {
	if (n <= 1) {
		return n;
	}
	return fib(n-1) + fib(n-2);
}
	```
  
<details>
  <summary>Simple Recursive Counting</summary>
  
```c#

func Count(int start, int finish) {
	outputln(start);

	if (start < finish) {
		Count(start+1, finish);
	}
	else {
		outputln("Finished counting!");
	}
}

Count(1, 10);


```

Program running:
```
-------------------- PROGRAM STARTED --------------------
1
2
3
4
5
6
7
8
9
10
Finished counting!
-------------------- PROGRAM ENDED --------------------
```
</details>


<details>
  <summary>Guessing Game Subroutine Implementation</summary>
  
```c#

func GuessingGame(string toGuess, int maxGuesses) {
	int guessAmount = 0;
	string guess = "";

	while (guessAmount < maxGuesses && toGuess != guess) {
		outputln("Guess the password.");
		inputStr(guess);
		guessAmount = guessAmount + 1;
	}

	if (toGuess == guess) { outputStringInt("You guessed it! Attempts: ", guessAmount); }
	else {
		outputln("You didn't guess it");
	}

}

GuessingGame("abc123", 5);
```

Program running:
```
-------------------- PROGRAM STARTED --------------------
Guess the password.
> abc12
Guess the password.
> abc123
You guessed it! Attempts: 2
-------------------- PROGRAM ENDED --------------------

```
  
</details>
</details>

<details>
<summary>Shell Usage</summary>
I've implemented an interactive shell, similar to that of Python. You are able to type normal programming statements and flow control in, but you are also (like Python) able to just type an expression in and have it evaluated.
The shell has text colouring too, here is what it looks like on the Windows Terminal Preview: https://imgur.com/a/DC17xAZ

NOTE: '>>' signifies input to the shell, and '>' signifies input to the program.

<details>
  <summary>Statements</summary>
  
```c#

>> int x = 0;
>> outputln(x);
0

```
</details>


<details>
  <summary>Flow Control (if, while) & Auto Line Numbering</summary>
	Line numbers will continue until the code block is finished. Colouring is different in the shell.
  
```c#

>> if (x == 0) {
2       outputln("X is 0");
3       inputInt(x);
4       if (x == 0) {
5               outputln("Unchanged");
6       }
7  }
X is 0
> 0
Unchanged

```

  
</details>

<details>
  <summary>Boolean Variable Declaration & Usage</summary>
	
  
```c#

>> bool testing = true && false;
>> testing
False
>> if (testing) {
2       outputln("Testing is true");
3  } else {
4       outputln("Testing is false");
5  }
6
Testing is false
>>

```

  
</details>
</details>

  </details>

<details>
<summary>Booleans & Logical Expressions</summary>
The master branch contains no booleans and logical expressions are not evaluated as such - it only supports two comparisons between variables. This version is able to evaluate a complex logical expression and now accepts 'True' and 'False' as values, meaning boolean variables are now here too. To do this, I've just re-implemented the same algorithm that calculates maths expressions, and replaced the 'operators' with logical comparators. The '!' acts as an unary minus too.
  
<details>
  <summary>Simple Logic Expression (Shell)</summary>
  
```c#

>> true && true
True
>> false && false
False

```
</details>


<details>
  <summary>Complex Order of Operations Expressions</summary>
	Working as far as I know*, I'm unable to test every possible input but all the ones I've tested have worked so far...
  
```c#

>> true && !(false || !(true && true))
True
```

Just to check:

=> TRUE AND NOT(FALSE OR NOT(TRUE AND TRUE))
=> TRUE AND NOT(FALSE OR FALSE)
=> TRUE AND TRUE
= TRUE

  
</details>

<details>
  <summary>Boolean Variable Declaration & Usage</summary>
	
  
```c#

>> bool testing = true && false;
>> testing
False
>> if (testing) {
2       outputln("Testing is true");
3  } else {
4       outputln("Testing is false");
5  }
6
Testing is false
>>

```

  
</details>
</details>

  </details>

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



  </details>


# Points of Interest

- [Main Lexer Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Lexer_Module/Tokeniser.cs)
- [Main Parser Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Parser_Module/Parser.cs)
- [Main Evaluator Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/Evaluator.cs)
- [Mathematical Expression Evaluator](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation)
  - [Tree Traversal](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/TreeTraversal/Traversal.cs)
  - [Infix to Postfix via Shunting-yard Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/Postfix.cs)
  - [RPN Evaluation Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/RPN.cs)
