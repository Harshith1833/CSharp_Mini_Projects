while (true)
{
    
    Console.WriteLine("--- Simple Calculator ---");

    Console.WriteLine("Enter the first number: ");
    //double num1 = 0;
    double num1 = double.TryParse(Console.ReadLine(), out num1) ? num1 : 0;

    Console.WriteLine("Enter the second number: ");
    //double num2 = 0;
    double num2 = double.TryParse(Console.ReadLine(), out num2) ? num2 : 0;

    Console.WriteLine($"Entered numbers are: {num1} and {num2}");

    Console.WriteLine("Select an operation (+, -, *, /): ");
    string operation = Console.ReadLine();
    double result = 0;

    switch (operation)
    {
        case "+":
            result = num1 + num2;
            break;
        case "-":
            result = num1 - num2;
            break;
        case "*":
            result = num1 * num2;
            break;
        case "/":
            if (num2 != 0)
                result = num1 / num2;
            else
                Console.WriteLine("Error: Division by zero is not allowed.");
            break;
        default:
            Console.WriteLine("Invalid operation selected.");
            break;
    }
    Console.WriteLine($"Result: {result}");

    Console.WriteLine("Do you want to continue? (y/n): ");
    string continueChoice = Console.ReadLine();
    if (continueChoice.ToLower() != "y")
    {
        break;
    }

}