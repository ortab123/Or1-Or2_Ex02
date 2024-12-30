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
                    name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                    return name;
                }

                Console.WriteLine("Invalid name. Please try again.");
            }
        }

        private static int GetBoardSize()
        {
            while (true)
            {
                Console.WriteLine($"Please select board size:{Environment.NewLine}1. 6{Environment.NewLine}2. 8{Environment.NewLine}3. 10");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    return 6;
                }
                if (choice == "2")
                {
                    return 8;
                }
                if (choice == "3")
                {
                    return 10;
                }
                Console.WriteLine("Invalid board size. Please try again.");
            } 

        }

        private static string GetPlayersChoice()
        {
            while (true)
            {
                Console.WriteLine($"Do you want to play against another player or the computer?{Environment.NewLine}1. Player vs. player{Environment.NewLine}2. Player vs. computer");
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

    }
}
