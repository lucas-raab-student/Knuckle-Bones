using System;
using System.Collections.Generic;
using System.Linq;

class KnuckleBonesGame
{
    static Random rng = new Random();
    static List<int>[] playerBoard = { new List<int>(), new List<int>(), new List<int>() };
    static List<int>[] aiBoard = { new List<int>(), new List<int>(), new List<int>() };

    static void Main(string[] args)
    {
        for (int i = 0; i < 9; i++)
        {
            PlayerTurn();
            AIturn();
        }

        EndGame();
    }

    static void PlayerTurn()
    {
        int die = rng.Next(1, 7);
        Console.WriteLine(" You rolled a " + die);

        int col = -1;
        while (true)
        {
            Console.Write("Choose a column (0, 1, or 2): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out col) && col >= 0 && col <= 2 && playerBoard[col].Count < 3)
                break;

            Console.WriteLine(" Invalid column or column is full. Try again.");
        }

        playerBoard[col].Add(die);
        aiBoard[col].RemoveAll(d => d == die);
        PrintBoards();
    }

    static void AIturn()
    {
        int die = rng.Next(1, 7);
        Console.WriteLine($" AI rolled a {die}");

        List<int> validCols = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            if (aiBoard[i].Count < 3)
                validCols.Add(i);
        }

        if (validCols.Count == 0)
        {
            Console.WriteLine("AI has no valid columns to place the die.");
            return;
        }

        int col = validCols[rng.Next(validCols.Count)];
        aiBoard[col].Add(die);
        playerBoard[col].RemoveAll(d => d == die);

        Console.WriteLine($" AI placed {die} in column {col}");
        PrintBoards();
    }

    static void PrintBoards()
    {
        Console.WriteLine(" Your Board:");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Col {i}: {string.Join(", ", playerBoard[i])}");
        }

        Console.WriteLine(" AI Board:");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Col {i}: {string.Join(", ", aiBoard[i])}");
        }
    }

    static int CalculateScore(List<int>[] board)
    {
        int score = 0;
        foreach (var col in board)
        {
            var counts = col.GroupBy(d => d);
            foreach (var group in counts)
            {
                int val = group.Key;
                int count = group.Count();
                score += val * count * count;
            }
        }
        return score;
    }

    static void EndGame()
    {
        int playerScore = CalculateScore(playerBoard);
        int aiScore = CalculateScore(aiBoard);

        Console.WriteLine($"\n🏁 Final Scores:");
        Console.WriteLine($"You: {playerScore}");
        Console.WriteLine($"AI : {aiScore}");

        if (playerScore > aiScore) Console.WriteLine(" You Win!");
        else if (playerScore < aiScore) Console.WriteLine(" AI Wins!");
        else Console.WriteLine("It's a Tie!");
    }
}
