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
- [ ] While statements
- [ ] More advanced condition parsing (currently only supports 1 comparison)
  - [ ] Boolean variable type
  
The experimental targets are a lot less likely to be implemented and have not been designed prior like the NEA ones.

# Points of Interest

- [Main Lexer Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Lexer_Module/Tokeniser.cs)
- [Main Parser Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Parser_Module/Parser.cs)
- [Main Evaluator Method](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/Evaluator.cs)
- [Mathematical Expression Evaluator](https://github.com/TorinFelton/NEA_ProgrammingLanguage/tree/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation)
  - [Tree Traversal](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/TreeTraversal/Traversal.cs)
  - [Infix to Postfix via Shunting-yard Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/Postfix.cs)
  - [RPN Evaluation Algorithm](https://github.com/TorinFelton/NEA_ProgrammingLanguage/blob/master/NEA_ProgrammingLanguage/Evaluator_Module/ExpressionEvaluation/Algorithms/RPN.cs)
