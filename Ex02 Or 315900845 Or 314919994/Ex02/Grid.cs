
namespace Ex02
{
    public class Grid
    {
        private ePieceType[,] m_Grid;

        public int m_Size { get; private set; }

        public Grid(int i_Size)
        {
            m_Size = i_Size;
            m_Grid = new ePieceType[i_Size, i_Size];
            InitializeGrid(i_Size);
        }

        private void InitializeGrid(int i_Size)
        {
            int rowsForPieces;
            if (i_Size == 6)
            {
                rowsForPieces = 2;
            }
            else if (i_Size == 8)
            {
                rowsForPieces = 3;
            }
            else
            {
                rowsForPieces = 4 ;
            }

            for (int row = 0; row < i_Size; row++)
            {
                for (int col = 0; col < i_Size; col++)
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
                    else if (row >= i_Size - rowsForPieces) // Bottom rows for X pieces
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

        public ePieceType GetPieceAt(int I_row, int i_Col)
        {
            return m_Grid[I_row, i_Col];
        }

        public void SetPieceAt(int i_Row, int i_Col, ePieceType i_PieceType)
        {
            m_Grid[i_Row, i_Col] = i_PieceType;
        }
    }
}
