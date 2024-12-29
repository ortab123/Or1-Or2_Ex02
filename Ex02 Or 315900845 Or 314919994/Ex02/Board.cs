using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public int Size { get; private set; }  // Here added static!

        public Board(int size) //Ctor
        {
            Size = size;
            Grid = new PieceType[Size, Size];
            Board.PieceType[,] pieceGrid = FillUpGrid(Grid, Size);
            PrintBoard(pieceGrid);
        }

        public void PrintBoard(Board.PieceType[,] grid) // Here added static!
        {
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

        //public static (int fromRow, int fromCol, int toRow, int toCol) ParseMove(string input)
        //{
        //    if (string.IsNullOrWhiteSpace(input) || input.Length != 5 || input[2] != '>')
        //    {
        //        throw new ArgumentException("Invalid move format. Expected format: Bc>Cd.");
        //    }

        //    int fromRow = input[0] - 'A'; // האות הראשונה מייצגת את השורה ההתחלתית
        //    int fromCol = input[1] - 'a'; // האות השנייה מייצגת את העמודה ההתחלתית
        //    int toRow = input[3] - 'A';   // האות הרביעית מייצגת את השורה הסופית
        //    int toCol = input[4] - 'a';   // האות החמישית מייצגת את העמודה הסופית

        //    // בדיקה שהקלט נמצא בגבולות הלוח
        //    if (fromRow < 0 || fromCol < 0 || toRow < 0 || toCol < 0)
        //    {
        //        throw new ArgumentException("Move coordinates out of bounds.");
        //    }

        //    return (fromRow, fromCol, toRow, toCol);
        //}

        public void MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            PieceType chosenPieceforNextMove = Grid[fromRow, fromCol];
            Grid[fromRow, fromCol] = PieceType.None;
            Grid[toRow, toCol] = chosenPieceforNextMove;

            // בדיקה אם החייל הופך ל"מלך"
            if (toRow == 0 && chosenPieceforNextMove == PieceType.O)
            {
                Grid[toRow, toCol] = PieceType.U; // הופך ל"מלך"
            }
            else if (toRow == Size - 1 && chosenPieceforNextMove == PieceType.X)
            {
                Grid[toRow, toCol] = PieceType.K; // הופך ל"מלך"
            }
        }
    }
}
