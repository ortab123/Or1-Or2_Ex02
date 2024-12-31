using System;
using System.Runtime.CompilerServices;

namespace Ex02
{
    public class Board
    {
        private static Grid grid;

        public static int Size { get; private set; } 

        public Board(int size) //Ctor
        {
            Size = size;
            grid = new Grid(size);
            PrintBoard(ref grid);
        }

        public static void PrintBoard(ref Grid grid) 
        {
            Ex02.ConsoleUtils.Screen.Clear();

            Console.Write("   ");

            for (int col = 0; col < Size; col++)
            {
                Console.Write($" {(char)('a' + col)}  ");
            }

            Console.WriteLine($"{Environment.NewLine}  " + new string('=', Size * 4 + 1));

            for (int row = 0; row < Size; row++)
            {

                Console.Write($"{(char)('A' + row)} |");

                for (int col = 0; col < Size; col++)
                {
                    if(grid.GetPieceAt(row,col) == ePieceType.None)
                    {
                        Console.Write("   |");
                    }
                    else 
                    {
                        string pieceSymbol = grid.GetPieceAt(row, col).ToString();
                        Console.Write($" {pieceSymbol} |");
                    }   
                }
                Console.WriteLine();

                if (row < Size)
                {
                    Console.WriteLine("  " + new string('=', Size * 4 + 1));
                }
            }
        }

        public static int SetBoardSize()
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

        public static ePlayerType GetPlayerOrComputerGame()
        {
            while (true)
            {
                ePlayerType eGameType = new ePlayerType();
                Console.WriteLine($"Do you want to play against another player or the computer?{Environment.NewLine}1. Player vs. player{Environment.NewLine}2. Player vs. computer");
                string choice = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(choice))
                {
                    if (choice == "1")
                    {
                        eGameType = ePlayerType.Regular;
                    }
                    if (choice == "2")
                    {
                        eGameType = ePlayerType.Computer;
                    }
                    return eGameType;
                }

                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        public Grid GetGrid()
        {
            return grid;
        }

    }
}
