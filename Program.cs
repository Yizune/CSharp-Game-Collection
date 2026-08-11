using CSharpCollection.Games;
using CSharpCollection.Helpers;

namespace CSharpCollection
{
    internal static class Program
    {
        public static void Main()
        {
            while(true)
            {
                int selectedGame = Menu.Select("C# Mini Games",
                    "Rock Paper Scissors",
                    "Tic Tac Toe",
                    "Wordle",
                    "Exit");

                switch(selectedGame)
                {
                    case 0:
                        RockPaperScissors.Run();
                        break;

                    case 1:
                        TicTacToe.Run();
                        break;

                    case 2:
                        Wordle.Run();
                        break;

                    case 3:
                        return;
                }
            }
        }
    }
}
