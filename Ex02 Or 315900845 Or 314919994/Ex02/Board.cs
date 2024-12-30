using System;

namespace Ex02
{
    public class Board
    {
        public enum PieceType
        {
            None, // ערך ברירת מחדל
            X,
            O,
            K,
            U
        }

        internal PieceType[,] Grid { get; private set; }
        public static int Size { get; private set; }  // Here added static!

        public Board(int size) //Ctor
        {
            Size = size;
            Grid = new PieceType[Size, Size];
            Board.PieceType[,] pieceGrid = FillUpGrid(Grid, Size);
            PrintBoard(pieceGrid);
        }

        public static void PrintBoard(Board.PieceType[,] grid)  // Here added static
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
                    if (grid[row, col] == PieceType.None)
                    {
                        Console.Write("   |");
                    }
                    else 
                    {
                        string pieceSymbol = grid[row, col].ToString();
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

        internal Board.PieceType[,] FillUpGrid(Board.PieceType[,] i_grid, int i_size)
        {
            if(i_size == 6)
            {
                for(int row=0; row<i_size; row++)
                {
                    for (int col = 0; col < i_size; col++)
                    {
                        if (row == 0 && col % 2 == 1)
                        {
                            i_grid[row, col] = PieceType.O;
                        }
                        else if (row == 1 && col % 2 == 0)
                        {
                            i_grid[row, col] = PieceType.O;
                        }
                        else if (row == 4 && col % 2 == 1)
                        {
                            i_grid[row, col] = PieceType.X;
                        }
                        else if (row == 5 && col % 2 == 0)
                        {
                            i_grid[row, col] = PieceType.X;
                        }
                        else
                        {
                            i_grid[row, col] = PieceType.None;
                        }
                    }
                } 
            }
            else if(i_size == 8)
            {
                for (int row = 0; row < i_size; row++)
                {
                    for (int col = 0; col < i_size; col++)
                    {
                        if ((row == 0 || row == 2) && col % 2 == 1)
                        {
                            i_grid[row, col] = PieceType.O;
                        }
                        else if (row == 1 && col % 2 == 0)
                        {
                            i_grid[row, col] = PieceType.O;
                        }
                        else if ((row == 5 || row == 7) && col % 2 == 1)
                        {
                            i_grid[row, col] = PieceType.X;
                        }
                        else if (row == 6 && col % 2 == 0)
                        {
                            i_grid[row, col] = PieceType.X;
                        }
                        else
                        {
                            i_grid[row, col] = PieceType.None;
                        }
                    }
                }

            }
            else if(i_size == 10)
            {
                for (int row = 0; row < i_size; row++)
                {
                    for (int col = 0; col < i_size; col++)
                    {
                        if ((row == 0 || row == 2) && col % 2 == 1)
                        {
                            i_grid[row, col] = PieceType.O;
                        }
                        else if ((row == 1 || row == 3) && col % 2 == 0)
                        {
                            i_grid[row, col] = PieceType.O;
                        }
                        else if ((row == 6 || row == 8) && col % 2 == 1)
                        {
                            i_grid[row, col] = PieceType.X;
                        }
                        else if ((row == 7 || row == 9)&& col % 2 == 0)
                        {
                            i_grid[row, col] = PieceType.X;
                        }
                        else
                        {
                            i_grid[row, col] = PieceType.None;
                        }
                    }
                }

            }
            return i_grid;
        }

    }
}
