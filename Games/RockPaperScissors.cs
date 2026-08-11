using CSharpCollection.Helpers;

namespace CSharpCollection.Games;

internal static class RockPaperScissors
{
    private enum Choice { Rock, Paper, Scissors }
    private enum Result { Draw, Won, Lost }

    private static int wins;
    private static int losses;

    public static void Run()
    {
        wins = 0;
        losses = 0;
        Menu.RunGameMenu("Rock Paper Scissors", PlayRound);
    }

    private static void PlayRound()
    {
        Choice player = (Choice)Menu.Select(
            $"Rock Paper Scissors\n\nWins: {wins}\nLosses: {losses}\n\nChoose your move:",
            "Rock",
            "Paper",
            "Scissors");
        Choice computer = (Choice)Random.Shared.Next(3);
        Result result = GetResult(player, computer);

        if(result == Result.Won)
        {
            wins++;
        }
        else if(result == Result.Lost)
        {
            losses++;
        }

        ConsoleUI.ShowScreen("ROCK PAPER SCISSORS");
        Console.WriteLine($"You chose:      {player}");
        Console.WriteLine($"Computer chose: {computer}");
        Console.WriteLine($"\n{ResultText(result)}");
        Console.WriteLine($"\nSession score - Wins: {wins}, Losses: {losses}");
        ConsoleUI.Pause("Press any key to return to the game menu...");
    }

    private static Result GetResult(Choice player, Choice computer)
    {
        if(player == computer)
        {
            return Result.Draw;
        }

        bool playerWon =
            (player == Choice.Rock && computer == Choice.Scissors) ||
            (player == Choice.Paper && computer == Choice.Rock) ||
            (player == Choice.Scissors && computer == Choice.Paper);

        return playerWon ? Result.Won : Result.Lost;
    }

    private static string ResultText(Result result) => result switch
    {
        Result.Won => "You won!",
        Result.Lost => "You lost!",
        _ => "It is a draw!"
    };
}
