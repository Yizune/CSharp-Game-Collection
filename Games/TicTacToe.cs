using CSharpCollection.Helpers;

namespace CSharpCollection.Games;

internal static class TicTacToe
{
    public static void Run()
    {
        Menu.RunGameMenu("Tic Tac Toe", PlayRound);
    }

    private static void PlayRound()
    {
        char[,] board =
        {
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' }
        };
        char player = 'X';
        int moves = 0;

        while(true)
        {
            DrawBoard(board);
            Console.Write($"\nPlayer {player}'s turn. Choose a position (1-9): ");

            if(!int.TryParse(Console.ReadLine(), out int position) || position is < 1 or > 9)
            {
                ConsoleUI.Pause("Enter a number from 1 to 9.");
                continue;
            }

            int row = (position - 1) / 3;
            int column = (position - 1) % 3;

            if(board[row, column] != ' ')
            {
                ConsoleUI.Pause("That position is already taken.");
                continue;
            }

            board[row, column] = player;
            moves++;

            if(HasWon(board, player))
            {
                DrawBoard(board);
                Console.WriteLine($"\nPlayer {player} won!");
                break;
            }

            if(moves == 9)
            {
                DrawBoard(board);
                Console.WriteLine("\nThe round ended in a draw!");
                break;
            }

            player = player == 'X' ? 'O' : 'X';
        }

        ConsoleUI.Pause("Press any key to return to the Tic Tac Toe menu...");
    }

    private static bool HasWon(char[,] board, char player)
    {
        for(int index = 0; index < 3; index++)
        {
            if((board[index, 0] == player && board[index, 1] == player && board[index, 2] == player)
                || (board[0, index] == player && board[1, index] == player && board[2, index] == player))
            {
                return true;
            }
        }

        return (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
            || (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player);
    }

    private static void DrawBoard(char[,] board)
    {
        ConsoleUI.ShowScreen("TIC TAC TOE");

        for(int row = 0; row < 3; row++)
        {
            Console.WriteLine($" {board[row, 0]} | {board[row, 1]} | {board[row, 2]} ");

            if(row < 2)
            {
                Console.WriteLine("---+---+---");
            }
        }
    }
}
