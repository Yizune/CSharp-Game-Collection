namespace CSharpCollection.Helpers;

internal static class ConsoleUI
{
    public static void ShowScreen(string title)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
    }

    public static void Pause(string message = "Press any key to continue...")
    {
        Console.WriteLine();
        Console.WriteLine(message);
        Console.ReadKey(intercept: true);
    }

    public static void ShowFooter()
    {
        Console.WriteLine();
        Console.WriteLine($"© {DateTime.Now.Year} Stevan Likušić. All rights reserved.");
    }
}
