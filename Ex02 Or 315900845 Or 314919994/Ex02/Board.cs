using System;

namespace Ex02
{
    public class Board
    {
        private static Grid m_Grid;
        private static int m_Size { get; set; } 

        public Board(int i_Size)
        {
            m_Size = i_Size;
            m_Grid = new Grid(i_Size);
            PrintBoard(ref m_Grid);
        }

        public static void PrintBoard(ref Grid io_Grid) 
        {
            ConsoleUtils.Screen.Clear();
            Console.Write("   ");

            for (int col = 0; col < m_Size; col++)
            {
                Console.Write($" {(char)('a' + col)}  ");
            }

            Console.WriteLine($"{Environment.NewLine}  " + new string('=', m_Size * 4 + 1));

            for (int row = 0; row < m_Size; row++)
            {
                Console.Write($"{(char)('A' + row)} |");

                for (int col = 0; col < m_Size; col++)
                {
                    if(io_Grid.GetPieceAt(row,col) == ePieceType.None)
                    {
                        Console.Write("   |");
                    }
                    else 
                    {
                        string pieceSymbol = io_Grid.GetPieceAt(row, col).ToString();
                        Console.Write($" {pieceSymbol} |");
                    }   
                }

                Console.WriteLine();

                if (row < m_Size)
                {
                    Console.WriteLine("  " + new string('=', m_Size * 4 + 1));
                }
            }
        }

        public static int SetBoardSize()
        {
            int sizeToReturn;

            while (true)
            {
                Console.WriteLine($"Please select board size:{Environment.NewLine}1. 6{Environment.NewLine}2. 8{Environment.NewLine}3. 10");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    sizeToReturn = 6;
                    break;
                }

                if (choice == "2")
                {
                    sizeToReturn = 8;
                    break;
                }

                if (choice == "3")
                {
                    sizeToReturn = 10;
                    break;
                }

                Console.WriteLine("Invalid board size. Please try again.");
            }

            return sizeToReturn;
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
            return m_Grid;
        }
    }
}
