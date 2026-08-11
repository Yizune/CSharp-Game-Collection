namespace CSharpCollection.Helpers;

internal static class Menu
{
    public static int Select(string title, params string[] options)
    {
        ArgumentOutOfRangeException.ThrowIfZero(options.Length);

        int selectedIndex = 0;

        while(true)
        {
            Draw(title, options, selectedIndex);

            switch(Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % options.Length;
                    break;
                case ConsoleKey.Enter:
                    return selectedIndex;
            }
        }
    }

    public static void RunGameMenu(string title, Action playRound)
    {
        while(true)
        {
            int choice = Select(title, "Play", "Back to main menu", "Exit");

            if(choice == 0)
            {
                playRound();
            }
            else if(choice == 1)
            {
                return;
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }

    private static void Draw(string title, IReadOnlyList<string> options, int selectedIndex)
    {
        ConsoleUI.ShowScreen(title);
        Console.WriteLine("Use the arrow keys and press Enter to confirm.\n");

        for(int index = 0; index < options.Count; index++)
        {
            string prefix = index == selectedIndex ? "> " : "  ";
            Console.WriteLine($"{prefix}{options[index]}");
        }

        ConsoleUI.ShowFooter();
    }
}
