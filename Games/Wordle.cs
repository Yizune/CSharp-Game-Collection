using CSharpCollection.Helpers;

namespace CSharpCollection.Games;

internal static class Wordle
{
    private const int WordLength = 5;
    private const int MaxAttempts = 6;
    private static readonly string WordFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "words.txt");
    private static readonly List<string> Words = [];
    private static readonly HashSet<string> ValidWords = [];
    private static readonly Queue<string> WordBag = [];

    private static string? lastWord;
    private static int wins;
    private static int losses;

    private enum LetterState { Absent, Present, Correct }
    private sealed record Guess(string Word, LetterState[] States);

    public static void Run()
    {
        if(!TryLoadWords())
        {
            return;
        }

        wins = 0;
        losses = 0;
        Menu.RunGameMenu("Wordle", PlayRound);
    }

    private static void PlayRound()
    {
        string answer = NextWord();
        List<Guess> guesses = [];
        string? message = null;

        while(guesses.Count < MaxAttempts)
        {
            DrawGame(guesses, message);
            Console.Write($"Enter guess {guesses.Count + 1}/{MaxAttempts}: ");

            string word = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();
            message = Validate(word);
            if(message != null)
            {
                continue;
            }

            guesses.Add(new Guess(word, Evaluate(word, answer)));
            if(word == answer)
            {
                wins++;
                DrawGame(guesses, $"You found the word: {answer}");
                ConsoleUI.Pause("Press any key to return to the Wordle menu...");
                return;
            }

            message = null;
        }

        losses++;
        DrawGame(guesses, $"You ran out of attempts. The word was {answer}.");
        ConsoleUI.Pause("Press any key to return to the Wordle menu...");
    }

    private static bool TryLoadWords()
    {
        if(Words.Count > 0)
        {
            return true;
        }

        try
        {
            foreach(string line in File.ReadLines(WordFilePath))
            {
                string word = line.Trim().ToUpperInvariant();
                if(word.Length == WordLength && word.All(char.IsLetter) && ValidWords.Add(word))
                {
                    Words.Add(word);
                }
            }

            if(Words.Count == 0)
            {
                throw new InvalidOperationException("The word list contains no valid five-letter words.");
            }

            return true;
        }
        catch(Exception exception)
        {
            ConsoleUI.ShowScreen("WORDLE");
            Console.WriteLine("Wordle could not load its word list.");
            Console.WriteLine(exception.Message);
            ConsoleUI.Pause("Press any key to return to the main menu...");
            return false;
        }
    }

    private static string NextWord()
    {
        if(WordBag.Count == 0)
        {
            RefillWordBag();
        }

        string word = WordBag.Dequeue();
        lastWord = word;
        return word;
    }

    private static void RefillWordBag()
    {
        List<string> shuffledWords = [.. Words];

        for(int index = shuffledWords.Count - 1; index > 0; index--)
        {
            int randomIndex = Random.Shared.Next(index + 1);
            (shuffledWords[index], shuffledWords[randomIndex]) = (shuffledWords[randomIndex], shuffledWords[index]);
        }

        if(shuffledWords.Count > 1 && shuffledWords[0] == lastWord)
        {
            int index = Random.Shared.Next(1, shuffledWords.Count);
            (shuffledWords[0], shuffledWords[index]) = (shuffledWords[index], shuffledWords[0]);
        }

        foreach(string word in shuffledWords)
        {
            WordBag.Enqueue(word);
        }
    }

    private static string? Validate(string word)
    {
        if(word.Length != WordLength)
        {
            return $"Your guess must contain exactly {WordLength} letters.";
        }

        if(!word.All(char.IsLetter))
        {
            return "Your guess may only contain letters.";
        }

        return ValidWords.Contains(word) ? null : $"{word} is not in the word list.";
    }

    private static LetterState[] Evaluate(string guess, string answer)
    {
        LetterState[] states = new LetterState[WordLength];
        bool[] usedLetters = new bool[WordLength];

        for(int index = 0; index < WordLength; index++)
        {
            if(guess[index] == answer[index])
            {
                states[index] = LetterState.Correct;
                usedLetters[index] = true;
            }
        }

        for(int guessIndex = 0; guessIndex < WordLength; guessIndex++)
        {
            if(states[guessIndex] == LetterState.Correct)
            {
                continue;
            }

            states[guessIndex] = LetterState.Absent;
            for(int answerIndex = 0; answerIndex < WordLength; answerIndex++)
            {
                if(guess[guessIndex] == answer[answerIndex] && !usedLetters[answerIndex])
                {
                    states[guessIndex] = LetterState.Present;
                    usedLetters[answerIndex] = true;
                    break;
                }
            }
        }

        return states;
    }

    private static void DrawGame(IReadOnlyList<Guess> guesses, string? message)
    {
        ConsoleUI.ShowScreen("WORDLE");
        Console.WriteLine($"Wins: {wins}   Losses: {losses}\n");

        for(int row = 0; row < MaxAttempts; row++)
        {
            if(row < guesses.Count)
            {
                DrawGuess(guesses[row]);
            }
            else
            {
                Console.WriteLine(" _   _   _   _   _ ");
            }
        }

        Console.WriteLine($"\nAttempts remaining: {MaxAttempts - guesses.Count}");
        if(message is not null)
        {
            Console.WriteLine($"\n{message}");
        }

        Console.WriteLine("\nGreen: correct position | Yellow: wrong position | Gray: not in word");
    }

    private static void DrawGuess(Guess guess)
    {
        for(int index = 0; index < WordLength; index++)
        {
            Console.BackgroundColor = guess.States[index] switch
            {
                LetterState.Correct => ConsoleColor.DarkGreen,
                LetterState.Present => ConsoleColor.DarkYellow,
                _ => ConsoleColor.DarkGray
            };
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" {guess.Word[index]} ");
            Console.ResetColor();
            Console.Write(' ');
        }

        Console.WriteLine();
    }
}
