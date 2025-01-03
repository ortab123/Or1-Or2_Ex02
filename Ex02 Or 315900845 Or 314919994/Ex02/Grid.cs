
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
            initializeGrid(i_Size);
        }

        private void initializeGrid(int i_Size)
        {
            int rowsForPieces;

            if(i_Size == 6)
            {
                rowsForPieces = 2;
            }
            else if(i_Size == 8)
            {
                rowsForPieces = 3;
            }
            else
            {
                rowsForPieces = 4;
            }

            for(int row = 0; row < i_Size; row++)
            {
                for(int col = 0; col < i_Size; col++)
                {
                    if(row < rowsForPieces)
                    {
                        if((row + col) % 2 == 1)
                        {
                            m_Grid[row, col] = ePieceType.O;
                        }
                        else
                        {
                            m_Grid[row, col] = ePieceType.None;
                        }

                    }
                    else if(row >= i_Size - rowsForPieces)
                    {
                        if((row + col) % 2 == 1)
                        {
                            m_Grid[row, col] = ePieceType.X;
                        }
                        else
                        {
                            m_Grid[row, col] = ePieceType.None;
                        }
                    }
                    else
                    {
                        m_Grid[row, col] = ePieceType.None;
                    }
                }
            }
        }

        public ePieceType GetPieceAt(int I_Row, int i_Col)
        {
            return m_Grid[I_Row, i_Col];
        }

        public void SetPieceAt(int i_Row, int i_Col, ePieceType i_PieceType)
        {
            m_Grid[i_Row, i_Col] = i_PieceType;
        }
    }
}