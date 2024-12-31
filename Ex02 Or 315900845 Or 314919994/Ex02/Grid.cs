using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class Grid
    {
        private ePieceType[,] m_Grid;
        public int Size { get; private set; }

        public Grid(int size)
        {
            Size = size;
            m_Grid = new ePieceType[size, size];
            InitializeGrid(size);
        }
        private void InitializeGrid(int size)
        {
            int rowsForPieces;
            if (size == 6)
            {
                rowsForPieces = 2;
            }
            else if (size == 8)
            {
                rowsForPieces = 3;
            }
            else
            {
                rowsForPieces = 4 ;
            }

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (row < rowsForPieces) // Top rows for O pieces
                    {
                        if ((row + col) % 2 == 1) // Alternate placement
                        {
                            m_Grid[row, col] = ePieceType.O;
                        }
                        else
                        {
                            m_Grid[row, col] = ePieceType.None;
                        }
                    }
                    else if (row >= size - rowsForPieces) // Bottom rows for X pieces
                    {
                        if ((row + col) % 2 == 1) // Alternate placement
                        {
                            m_Grid[row, col] = ePieceType.X;
                        }
                        else
                        {
                            m_Grid[row, col] = ePieceType.None;
                        }
                    }
                    else // Middle rows are empty
                    {
                        m_Grid[row, col] = ePieceType.None;
                    }
                }
            }
        }
        public ePieceType GetPieceAt(int row, int col)
        {
            return m_Grid[row, col];
        }
        public void SetPieceAt(int row, int col, ePieceType pieceType)
        {
            m_Grid[row, col] = pieceType;
        }
    }
}
