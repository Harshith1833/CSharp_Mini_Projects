# About the Project: P2_SimpleCalculator

## Project Overview
The P2_SimpleCalculator is a basic console-based calculator application developed in C# using the .NET framework. This project serves as a fundamental example of implementing arithmetic operations in a command-line interface, demonstrating core programming concepts such as user input handling, conditional logic, and loop structures.

## Project Structure
The project consists of the following key files:
- **Program.cs**: The main entry point containing the calculator logic

## Technical Details
- **Framework**: .NET 10.0
- **Output Type**: Executable (Console Application)
- **Language Features**: Implicit usings enabled, nullable reference types enabled
- **Platform**: Cross-platform (Windows, Linux, macOS via .NET)

## Functionality
The calculator provides the following arithmetic operations:
- **Addition (+)**: Adds two numbers
- **Subtraction (-)**: Subtracts the second number from the first
- **Multiplication (*)**: Multiplies two numbers
- **Division (/)**: Divides the first number by the second (with zero-division protection)

## Code Explanation
The application runs in an infinite loop until the user chooses to exit. Here's how it works:

1. **Input Collection**: The program prompts the user to enter two numbers. It uses `double.TryParse()` to safely convert string input to double values, defaulting to 0 if parsing fails.

2. **Operation Selection**: The user selects an operation by entering one of the four symbols: +, -, *, or /.

3. **Calculation Logic**: A switch statement evaluates the selected operation and performs the corresponding arithmetic calculation.

4. **Error Handling**: For division, the program checks if the second number is zero to prevent division by zero errors.

5. **Output Display**: The result is displayed to the console.

6. **Continuation Logic**: After each calculation, the user is asked if they want to continue. If they respond with anything other than 'y' or 'Y', the program terminates.

## Learning Objectives
This project demonstrates several important programming concepts:
- **User Input/Output**: Reading from and writing to the console
- **Data Type Conversion**: Safe parsing of user input
- **Control Structures**: Switch statements and while loops
- **Error Prevention**: Basic validation and error handling
- **Modular Design**: Separation of concerns in a simple application

## How to Run
1. Ensure you have .NET 10.0 SDK installed
2. Navigate to the project directory
3. Run `dotnet build` to compile the project
4. Execute `dotnet run` to start the calculator

## Potential Enhancements
While this is a basic implementation, the project could be extended with:
- Support for more advanced operations (exponentiation, square root, etc.)
- Input validation with error messages
- History of calculations
- GUI interface using Windows Forms or WPF
- Unit tests for the calculation logic
