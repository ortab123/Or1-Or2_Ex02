using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to Checkers!");


            string player1Name = GetPlayerName("Player 1");
            Player player1 = new HumanPlayer(player1Name, 'X');

            int boardSize = GetBoardSize();

            // אפשרות לבחור אם לשחק נגד שחקן נוסף או נגד המחשב
            Console.WriteLine($"Do you want to play against another player or the computer?{Environment.NewLine}1. Player vs. player{Environment.NewLine}2. Player vs. computer");
            string playerModeChoice = GetPlayersChoice();

            Player player2;

            if (playerModeChoice == "1")
            {
                string player2Name = GetPlayerName("Player 2");
                player2 = new HumanPlayer(player2Name, 'O');
            }
            else
            {
                player2 = new ComputerPlayer("Computer", 'O');
            }

            Game game = new Game(player1, player2, boardSize);
            game.Start();
        }

        private static string GetPlayerName(string playerPrompt)
        {
            while (true)
            {
                Console.WriteLine($"{playerPrompt}, enter your name (max 20 characters, no spaces):");
                string name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name) && name.Length <= 20 && !name.Contains(" "))
                {
                    return name;
                }

                Console.WriteLine("Invalid name. Please try again.");
            }
        }

        private static int GetBoardSize()
        {
            while (true)
            {
                Console.WriteLine("Enter board size (6, 8, or 10):");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int size) && (size == 6 || size == 8 || size == 10))
                    return size;

                Console.WriteLine("Invalid board size. Please enter 6, 8, or 10.");
            }
        }
        private static string GetPlayersChoice()
        {
            while (true)
            {
                string choice = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(choice))
                {
                    if (choice == "1" || choice == "2")
                    {
                        return choice;
                    }
                }

                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        // מחלקת שחקן ממוחשב
        public class ComputerPlayer : Player
        {
            public ComputerPlayer(string name, char symbol) : base(name, symbol)
            {

            }

            // מתוד הלקיחה של מהלך אוטומטי מהמחשב לפי רמת הקושי
            public void MakeMove(Board board)
            {

            }


        }
    }
}
