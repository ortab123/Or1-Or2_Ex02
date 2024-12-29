using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ex02.Board;

namespace Ex02
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name, char symbol) : base(name, symbol) { }

        char ConvertDigitToLowerLetter(int i_digit)
        {
            return (char)('a' + (i_digit - '0')); // Map 0->a, 1->b, ..., 9->j
        }

        char ConvertDigitToUpperLetter(int i_digit)
        {
            return (char)('A' + (i_digit - '0')); // Map 0->A, 1->B, ..., 9->J
        }


        //public void MakeMove(string i_nextMoveString, Board.PieceType[,] i_grid, int i_size, Player i_player)
        //{
        //    while (true) // Infinite loop for continuous input validation
        //    {
        //        // Prompt for input if it's null or whitespace
        //        while (string.IsNullOrWhiteSpace(i_nextMoveString))
        //        {
        //            Console.WriteLine("Input cannot be empty. Try again.");
        //            i_nextMoveString = Console.ReadLine();
        //        }

        //        // Check if the user wants to quit
        //        if (i_nextMoveString.ToUpper() == "Q")
        //        {
        //            return; // Exit the method
        //        }

        //        // Convert to char array for validation
        //        char[] chars = i_nextMoveString.ToCharArray();

        //        // Validate input format: Xx>Yy
        //        if (chars.Length == 5 &&
        //            Char.IsUpper(chars[0]) &&
        //            Char.IsLower(chars[1]) &&
        //            chars[2] == '>' &&
        //            Char.IsUpper(chars[3]) &&
        //            Char.IsLower(chars[4]))
        //        {
        //            // Input is valid, break out of the loop
        //            Console.WriteLine("Valid input received.");
        //            break;
        //        }

        //        // If input is invalid, display a message and reset the loop
        //        Console.WriteLine("Invalid input. Please enter move in format of Xx>Yy, or 'Q' to quit.");
        //        i_nextMoveString = null; // Reset input to trigger the outer loop for new input
        //    }


        //    // פה יש לנו שתי רשימות עם צעדים אפשריים לאכילה בלבד, של שני המשתתפים
        //    List<string> OoptionalEatMoves = GetOptionalEatMoves(i_grid, i_size, 'O');
        //    List<string> XoptionalEatMoves = GetOptionalEatMoves(i_grid, i_size, 'X');

        //    // פה יש לנו שתי רשימות עם צעדים אפשריים (ללא אכילה) של שני המשתתפים
        //    List<string> OoptionalMoves = GetOptionalMoves(i_grid, i_size, 'O');
        //    List<string> XoptionalMoves = GetOptionalMoves(i_grid, i_size, 'X');

        //    // בדיקה האם הצעד הרצוי הוא באמת ברשימה
        //    if(i_player.Symbol == 'O')
        //    {
        //        // רק אם יש אופציות לאכול אז מבצעים אחרת, קופצים לשלב הבא
        //        if (OoptionalEatMoves.Count > 0)
        //        {
        //            for (int i = 0; i < OoptionalEatMoves.Count; i++)
        //            {
        //                if (OoptionalEatMoves[i] == i_nextMoveString)
        //                {
        //                    // Do the step
        //                }
        //                else
        //                {
        //                    // Console: Bad move! Try again!
        //                }
        //            }
        //        }
        //        // האחרת, כלומר אין צעדי אכילה בכלל, רק צעדים רגילים
        //        else if (OoptionalMoves.Count > 0)
        //        {
        //            for (int i = 0; i < OoptionalMoves.Count; i++)
        //            {
        //                if (OoptionalMoves[i] == i_nextMoveString)
        //                {
        //                    // Do the step
        //                }
        //                else
        //                {
        //                    // Console: Bad move! Try again!
        //                }
        //            }
        //        }
        //        // אין צעדים בכלל
        //        else
        //        {
        //            // No moves for O
        //        }
        //    }

        //    // X אותם הסברים עבור
        //    if (i_player.Symbol == 'X')
        //    {
        //        if (XoptionalEatMoves.Count > 0)
        //        {
        //            for (int i = 0; i < XoptionalEatMoves.Count; i++)
        //            {
        //                if (XoptionalEatMoves[i] == i_nextMoveString)
        //                {
        //                    // Do the step
        //                }
        //                else
        //                {
        //                    // Console: Bad move! Try again!
        //                }
        //            }
        //        }
        //        else if (XoptionalMoves.Count > 0)
        //        {
        //            for (int i = 0; i < XoptionalMoves.Count; i++)
        //            {
        //                if (XoptionalMoves[i] == i_nextMoveString)
        //                {
        //                    // Do the step
        //                }
        //                else
        //                {
        //                    // Console: Bad move! Try again!
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // No moves for O
        //        }
        //    }

        //    // לעשות צעד - ולוודא שהצעד עצמו תקין - במתודה נפרדת
        //    // לנקות את המסך


        //    //return (fromRow, fromCol, toRow, toCol, false);
        //}


        // NEW VERSIONS: VV

        public void MakeMove(string i_nextMoveString, Board.PieceType[,] i_grid, int i_size, Player i_player)
        {
            // Loop for continuous input validation
            while (true)
            {
                // Prompt for input if it's null or whitespace
                while (string.IsNullOrWhiteSpace(i_nextMoveString))
                {
                    Console.WriteLine("Input cannot be empty. Try again.");
                    i_nextMoveString = Console.ReadLine();
                }

                // Check if the user wants to quit
                if (i_nextMoveString.ToUpper() == "Q")
                {
                    return; // Exit the method
                }

                // Convert to char array for validation
                char[] chars = i_nextMoveString.ToCharArray();

                // Validate input format: Xx>Yy
                if (chars.Length == 5 &&
                    Char.IsUpper(chars[0]) &&
                    Char.IsLower(chars[1]) &&
                    chars[2] == '>' &&
                    Char.IsUpper(chars[3]) &&
                    Char.IsLower(chars[4]))
                {
                    // Input is valid, break out of the loop
                    Console.WriteLine("Valid input received.");
                    break;
                }

                // If input is invalid, display a message and reset the loop
                Console.WriteLine("Invalid input. Please enter move in format of Xx>Yy, or 'Q' to quit.");
                i_nextMoveString = null; // Reset input to trigger the outer loop for new input
            }

            // Lists for optional moves and eat moves
            List<string> OoptionalEatMoves = GetOptionalEatMoves(i_grid, i_size, 'O');
            List<string> XoptionalEatMoves = GetOptionalEatMoves(i_grid, i_size, 'X');

            List<string> OoptionalMoves = GetOptionalMoves(i_grid, i_size, 'O');
            List<string> XoptionalMoves = GetOptionalMoves(i_grid, i_size, 'X');

            bool validMove = false;

            // Check for 'O' moves
            if (i_player.Symbol == 'O')
            {
                validMove = CheckAndUpdatingBoard(OoptionalEatMoves, i_nextMoveString, i_grid, i_size, 'O') ||
                            CheckAndUpdatingBoard(OoptionalMoves, i_nextMoveString, i_grid, i_size, 'O');
            }

            // Check for 'X' moves
            if (i_player.Symbol == 'X' && !validMove)
            {
                validMove = CheckAndUpdatingBoard(XoptionalEatMoves, i_nextMoveString, i_grid, i_size, 'X') ||
                            CheckAndUpdatingBoard(XoptionalMoves, i_nextMoveString, i_grid, i_size, 'X');
            }

            if (!validMove)
            {
                Console.WriteLine("Invalid move. Try again.");
            }
        }

        // Helper function to check and perform the move
        private bool CheckAndUpdatingBoard(List<string> optionalMoves, string move, Board.PieceType[,] i_grid, int i_size, char playerSymbol)
        {
            foreach (var optionalMove in optionalMoves)
            {
                if (optionalMove == move)
                {
                    // Perform the move (update the grid)
                    // You need to implement the actual logic for updating the grid here
                    UpdatingBoard(move, i_grid, i_size, playerSymbol);
                    PrintBoard(i_grid);
                    return true;
                }
            }
            return false;
        }

        // Helper function to perform the move (update grid)
        private void UpdatingBoard(string move, Board.PieceType[,] i_grid, int i_size, char playerSymbol)
        {
            // Parse move string like "A2>B3"
            char fromRowChar = move[0];
            char fromColChar = move[1];
            char toRowChar = move[3];
            char toColChar = move[4];

            int fromRow = fromRowChar - 'A';  // Convert row letter to index
            int fromCol = fromColChar - 'a'; // Convert column letter to index
            int toRow = toRowChar - 'A';     // Convert row letter to index
            int toCol = toColChar - 'a';    // Convert column letter to index

            // Update the board based on the move
            i_grid[toRow, toCol] = i_grid[fromRow, fromCol];
            i_grid[fromRow, fromCol] = Board.PieceType.None;  // Clear the original position

            // If the piece is a king and reached the end of the board, make it a king
            if (playerSymbol == 'O' && toRow == i_size - 1)
            {
                i_grid[toRow, toCol] = Board.PieceType.U;  // Make 'O' a king
            }
            else if (playerSymbol == 'X' && toRow == 0)
            {
                i_grid[toRow, toCol] = Board.PieceType.K;  // Make 'X' a king
            }
        }

        public List<string> GetOptionalEatMoves(Board.PieceType[,] grid, int size, char i_symbol)
        {
            List<string> optionalEatMoves = new List<string>();
            Board.PieceType regularPiece = Board.PieceType.None, kingPiece = Board.PieceType.None, opponentRegular = Board.PieceType.None, opponentKing = Board.PieceType.None;

            // אתחול המשתנים על פי ערך 'i_symbol'
            if (i_symbol == 'O')
            {
                regularPiece = Board.PieceType.O;
                kingPiece = Board.PieceType.U;
                opponentRegular = Board.PieceType.X;
                opponentKing = Board.PieceType.K;
            }
            else if (i_symbol == 'X')
            {
                regularPiece = Board.PieceType.X;
                kingPiece = Board.PieceType.K;
                opponentRegular = Board.PieceType.O;
                opponentKing = Board.PieceType.U;
            }

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (grid[row, col] == regularPiece || grid[row, col] == kingPiece)
                    {
                        // במקרה של 'O' ו-'U' - האכלה קדימה לכיוון למטה
                        if (i_symbol == 'O' || i_symbol == 'U')
                        {
                            // Eating forward left
                            bool canEatForwardLeft = row + 2 < size && col - 2 >= 0 &&
                                                     (grid[row + 1, col - 1] == opponentRegular || grid[row + 1, col - 1] == opponentKing) &&
                                                     grid[row + 2, col - 2] == Board.PieceType.None;

                            if (canEatForwardLeft)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row + 2) + ConvertDigitToLowerLetter(col - 2);
                                optionalEatMoves.Add(move);
                            }

                            // Eating forward right
                            bool canEatForwardRight = row + 2 < size && col + 2 < size &&
                                                      (grid[row + 1, col + 1] == opponentRegular || grid[row + 1, col + 1] == opponentKing) &&
                                                      grid[row + 2, col + 2] == Board.PieceType.None;

                            if (canEatForwardRight)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row + 2) + ConvertDigitToLowerLetter(col + 2);
                                optionalEatMoves.Add(move);
                            }
                        }

                        // במקרה של 'X' ו-'K' - אכילה קדימה לכיוון למעלה
                        if (i_symbol == 'X' || i_symbol == 'K')
                        {
                            // Eating forward left
                            bool canEatForwardLeft = row - 2 >= 0 && col - 2 >= 0 &&
                                                     (grid[row - 1, col - 1] == opponentRegular || grid[row - 1, col - 1] == opponentKing) &&
                                                     grid[row - 2, col - 2] == Board.PieceType.None;

                            if (canEatForwardLeft)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row - 2) + ConvertDigitToLowerLetter(col - 2);
                                optionalEatMoves.Add(move);
                            }

                            // Eating forward right
                            bool canEatForwardRight = row - 2 >= 0 && col + 2 < size &&
                                                      (grid[row - 1, col + 1] == opponentRegular || grid[row - 1, col + 1] == opponentKing) &&
                                                      grid[row - 2, col + 2] == Board.PieceType.None;

                            if (canEatForwardRight)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row - 2) + ConvertDigitToLowerLetter(col + 2);
                                optionalEatMoves.Add(move);
                            }
                        }

                        // במקרה של 'U' (מלך 'O') - אכילה אחורה לכיוון למעלה
                        if (i_symbol == 'U')
                        {
                            // Eating backward left
                            bool canEatBackwardLeft = row - 2 >= 0 && col - 2 >= 0 &&
                                                      (grid[row - 1, col - 1] == opponentRegular || grid[row - 1, col - 1] == opponentKing) &&
                                                      grid[row - 2, col - 2] == Board.PieceType.None;

                            if (canEatBackwardLeft)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row - 2) + ConvertDigitToLowerLetter(col - 2);
                                optionalEatMoves.Add(move);
                            }

                            // Eating backward right
                            bool canEatBackwardRight = row - 2 >= 0 && col + 2 < size &&
                                                       (grid[row - 1, col + 1] == opponentRegular || grid[row - 1, col + 1] == opponentKing) &&
                                                       grid[row - 2, col + 2] == Board.PieceType.None;

                            if (canEatBackwardRight)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row - 2) + ConvertDigitToLowerLetter(col + 2);
                                optionalEatMoves.Add(move);
                            }
                        }

                        // במקרה של 'K' (מלך 'X') - אכילה אחורה לכיוון למטה
                        if (i_symbol == 'K')
                        {
                            // Eating backward left
                            bool canEatBackwardLeft = row + 2 < size && col - 2 >= 0 &&
                                                      (grid[row + 1, col - 1] == opponentRegular || grid[row + 1, col - 1] == opponentKing) &&
                                                      grid[row + 2, col - 2] == Board.PieceType.None;

                            if (canEatBackwardLeft)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row + 2) + ConvertDigitToLowerLetter(col - 2);
                                optionalEatMoves.Add(move);
                            }

                            // Eating backward right
                            bool canEatBackwardRight = row + 2 < size && col + 2 < size &&
                                                       (grid[row + 1, col + 1] == opponentRegular || grid[row + 1, col + 1] == opponentKing) &&
                                                       grid[row + 2, col + 2] == Board.PieceType.None;

                            if (canEatBackwardRight)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row + 2) + ConvertDigitToLowerLetter(col + 2);
                                optionalEatMoves.Add(move);
                            }
                        }
                    }
                }
            }
            return optionalEatMoves;
        }

        public List<string> GetOptionalMoves(Board.PieceType[,] i_grid, int i_size, char i_symbol)
        {
            List<string> optionalMoves = new List<string>();
            Board.PieceType regularPiece = Board.PieceType.None, kingPiece = Board.PieceType.None;

            // אתחול המשתנים על פי ערך 'i_symbol'
            if (i_symbol == 'O')
            {
                regularPiece = Board.PieceType.O;
                kingPiece = Board.PieceType.U;
            }
            else if (i_symbol == 'X')
            {
                regularPiece = Board.PieceType.X;
                kingPiece = Board.PieceType.K;
            }

            for (int row = 0; row < i_size; row++)
            {
                for (int col = 0; col < i_size; col++)
                {
                    if (i_grid[row, col] == regularPiece || i_grid[row, col] == kingPiece)
                    {
                        // עבור 'O' ו-'U' - דילוג קדימה לכיוון מטה
                        if (i_symbol == 'O' || i_symbol == 'U')
                        {
                            // Move Forward Left
                            if (row + 1 < i_size && col - 1 >= 0 && i_grid[row + 1, col - 1] == Board.PieceType.None)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row + 1) + ConvertDigitToLowerLetter(col - 1);
                                optionalMoves.Add(move);
                            }

                            // Move Forward Right
                            if (row + 1 < i_size && col + 1 < i_size && i_grid[row + 1, col + 1] == Board.PieceType.None)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row + 1) + ConvertDigitToLowerLetter(col + 1);
                                optionalMoves.Add(move);
                            }

                            // עבור 'U' (מלך 'O') - דילוג אחורה לכיוון למעלה
                            if (i_symbol == 'U')
                            {
                                // Move Backward Left
                                if (row - 1 >= 0 && col - 1 >= 0 && i_grid[row - 1, col - 1] == Board.PieceType.None)
                                {
                                    string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                                  ConvertDigitToUpperLetter(row - 1) + ConvertDigitToLowerLetter(col - 1);
                                    optionalMoves.Add(move);
                                }

                                // Move Backward Right
                                if (row - 1 >= 0 && col + 1 < i_size && i_grid[row - 1, col + 1] == Board.PieceType.None)
                                {
                                    string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                                  ConvertDigitToUpperLetter(row - 1) + ConvertDigitToLowerLetter(col + 1);
                                    optionalMoves.Add(move);
                                }
                            }
                        }

                        // עבור 'X' ו-'K' - דילוג קדימה לכיוון מעלה
                        if (i_symbol == 'X' || i_symbol == 'K')
                        {
                            // Move Forward Left
                            if (row - 1 >= 0 && col - 1 >= 0 && i_grid[row - 1, col - 1] == Board.PieceType.None)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row - 1) + ConvertDigitToLowerLetter(col - 1);
                                optionalMoves.Add(move);
                            }

                            // Move Forward Right
                            if (row - 1 >= 0 && col + 1 < i_size && i_grid[row - 1, col + 1] == Board.PieceType.None)
                            {
                                string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                              ConvertDigitToUpperLetter(row - 1) + ConvertDigitToLowerLetter(col + 1);
                                optionalMoves.Add(move);
                            }

                            // עבור 'K' (מלך 'X') - דילוג אחורה לכיוון למטה
                            if (i_symbol == 'K')
                            {
                                // Move Backward Left
                                if (row + 1 < i_size && col - 1 >= 0 && i_grid[row + 1, col - 1] == Board.PieceType.None)
                                {
                                    string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                                  ConvertDigitToUpperLetter(row + 1) + ConvertDigitToLowerLetter(col - 1);
                                    optionalMoves.Add(move);
                                }

                                // Move Backward Right
                                if (row + 1 < i_size && col + 1 < i_size && i_grid[row + 1, col + 1] == Board.PieceType.None)
                                {
                                    string move = ConvertDigitToUpperLetter(row) + ConvertDigitToLowerLetter(col) + ">" +
                                                  ConvertDigitToUpperLetter(row + 1) + ConvertDigitToLowerLetter(col + 1);
                                    optionalMoves.Add(move);
                                }
                            }
                        }
                    }
                }
            }
            return optionalMoves;
        }

        // OLD VERSIONS: VV
        //public List<StringBuilder> optionalMoves(Board.PieceType[,] i_grid, int i_size, Player i_player)
        //{
        //    StringBuilder optionalMove = new StringBuilder(5);
        //    List<StringBuilder> optionalMovesArray = new List<StringBuilder>();

        //    if (i_player.Symbol == 'O')
        //    {
        //        for (int row = 0; row < i_size; row++)
        //        {
        //            for (int col = 0; col < i_size; col++)
        //            {
        //                if (i_grid[row, col] != Board.PieceType.O || i_grid[row, col] != Board.PieceType.U)
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    if (col == 0) // First column
        //                    {
        //                        if (i_grid[row + 1, col + 1] == Board.PieceType.None && row != i_size - 1)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row + 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col + 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.U && row != 0 &&
        //                        i_grid[row - 1, col + 1] == Board.PieceType.None)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row - 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col + 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                    }
        //                    else if (col == i_size - 1) // Last column
        //                    {
        //                        if (i_grid[row + 1, col - 1] == Board.PieceType.None && row != i_size - 1)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row + 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col - 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.U && row != 0 &&
        //                        i_grid[row - 1, col - 1] == Board.PieceType.None)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row - 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col - 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                    }
        //                    else // Any other column
        //                    {
        //                        if (i_grid[row + 1, col + 1] == Board.PieceType.None && row != i_size - 1)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row + 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col + 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row + 1, col - 1] == Board.PieceType.None && row != i_size - 1)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row + 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col - 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.U && row != 0 &&
        //                        i_grid[row - 1, col - 1] == Board.PieceType.None)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row - 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col - 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.U && row != 0 &&
        //                        i_grid[row - 1, col + 1] == Board.PieceType.None)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row - 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col + 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int row = 0; row < i_size; row++)
        //        {
        //            for (int col = 0; col < i_size; col++)
        //            {
        //                if (i_grid[row, col] != Board.PieceType.X || i_grid[row, col] != Board.PieceType.K)
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    if (col == 0) // First column
        //                    {
        //                        if (i_grid[row - 1, col + 1] == Board.PieceType.None && row != 0)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row - 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col + 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.K && row != i_size - 1 &&
        //                        i_grid[row + 1, col + 1] == Board.PieceType.None)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row + 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col + 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                    }
        //                    else if (col == i_size - 1) // Last column
        //                    {
        //                        if (i_grid[row - 1, col - 1] == Board.PieceType.None && row != 0)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row - 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col - 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.K && row != i_size - 1 &&
        //                        i_grid[row + 1, col - 1] == Board.PieceType.None)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row + 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col - 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                    }
        //                    else // Any other column
        //                    {
        //                        if (i_grid[row - 1, col + 1] == Board.PieceType.None && row != 0)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row - 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col + 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row - 1, col - 1] == Board.PieceType.None && row != 0)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row - 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col - 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.K && row != i_size - 1 &&
        //                        i_grid[row + 1, col - 1] == Board.PieceType.None)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row + 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col - 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.K && row != i_size - 1 &&
        //                        i_grid[row + 1, col + 1] == Board.PieceType.None)
        //                        {
        //                            optionalMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalMove[2] = '>';
        //                            optionalMove[3] = ConvertDigitToUpperLetter(row + 1);
        //                            optionalMove[4] = ConvertDigitToLowerLetter(col + 1);
        //                            optionalMovesArray.Add(optionalMove);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return optionalMovesArray;
        //}

        //public List<StringBuilder> optionalEatMoves(Board.PieceType[,] i_grid, int i_size, Player i_player)
        //{
        //    StringBuilder optionalEatMove = new StringBuilder(5);
        //    List<StringBuilder> optionalEatMovesArray = new List<StringBuilder>();

        //    if (i_player.Symbol == 'O')
        //    {
        //        for (int row = 0; row < i_size; row++)
        //        {
        //            for (int col = 0; col < i_size; col++)
        //            {
        //                if (i_grid[row, col] != Board.PieceType.O || i_grid[row, col] != Board.PieceType.U)
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    if (col == 0 || col == 1) // First column
        //                    {
        //                        if (i_grid[row + 2, col + 2] == Board.PieceType.None && row != i_size - 1 && row != i_size - 2 &&
        //                            (i_grid[row + 1, col + 1] == Board.PieceType.X || i_grid[row + 1, col + 1] == Board.PieceType.K))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row + 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col + 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.U && row != 0 && row != 1 && 
        //                        i_grid[row - 2, col + 2] == Board.PieceType.None && (i_grid[row - 1, col + 1] == Board.PieceType.X ||
        //                        i_grid[row - 1, col + 1] == Board.PieceType.K))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row - 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col + 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                    }
        //                    else if (col == i_size - 1 || col == i_size - 2) // Last column
        //                    {
        //                        if (i_grid[row + 2, col - 2] == Board.PieceType.None && row != i_size - 1 && row != i_size - 2 &&
        //                            (i_grid[row + 1, col - 1] == Board.PieceType.X || i_grid[row + 1, col - 1] == Board.PieceType.K))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row + 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col - 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.U && row != 0 && row != 1 && 
        //                        i_grid[row - 2, col - 2] == Board.PieceType.None && (i_grid[row - 1, col - 1] == Board.PieceType.X ||
        //                        i_grid[row - 1, col - 1] == Board.PieceType.K))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row - 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col - 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                    }
        //                    else // Any other column
        //                    {
        //                        if (i_grid[row + 2, col + 2] == Board.PieceType.None && row != i_size - 1 && row != i_size - 2 &&
        //                            (i_grid[row + 1, col + 1] == Board.PieceType.X ||i_grid[row + 1, col + 1] == Board.PieceType.K))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row + 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col + 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row + 2, col - 2] == Board.PieceType.None && row != i_size - 1 && row != i_size - 2 &&
        //                            (i_grid[row + 1, col - 1] == Board.PieceType.X || i_grid[row + 1, col - 1] == Board.PieceType.K))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row + 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col - 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.U && row != 0 && row != 1 &&
        //                        i_grid[row - 2, col - 2] == Board.PieceType.None && (i_grid[row - 1, col - 1] == Board.PieceType.X ||
        //                        i_grid[row - 1, col - 1] == Board.PieceType.K))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row - 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col - 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.U && row != 0 && row != 1 &&
        //                        i_grid[row - 2, col + 2] == Board.PieceType.None && (i_grid[row - 1, col + 1] == Board.PieceType.X ||
        //                        i_grid[row - 1, col + 1] == Board.PieceType.K))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row - 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col + 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int row = 0; row < i_size; row++)
        //        {
        //            for (int col = 0; col < i_size; col++)
        //            {
        //                if (i_grid[row, col] != Board.PieceType.X || i_grid[row, col] != Board.PieceType.K)
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    if (col == 0 || col == 1) // First column
        //                    {
        //                        if (i_grid[row - 2, col + 2] == Board.PieceType.None && row != 0 && row != 1 && 
        //                            (i_grid[row - 1, col + 1] == Board.PieceType.O ||
        //                        i_grid[row - 1, col + 1] == Board.PieceType.U))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row - 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col + 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.K && row != i_size - 1 && row != i_size - 2 &&
        //                            i_grid[row + 2, col + 2] == Board.PieceType.None && (i_grid[row + 1, col + 1] == Board.PieceType.O ||
        //                            i_grid[row + 1, col + 1] == Board.PieceType.U))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row + 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col + 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                    }
        //                    else if (col == i_size - 1 || col == i_size - 2) // Last column
        //                    {
        //                        if (i_grid[row - 2, col - 2] == Board.PieceType.None && row != 0 && row != 1 &&
        //                            (i_grid[row - 1, col - 1] == Board.PieceType.O ||
        //                            i_grid[row - 1, col - 1] == Board.PieceType.U))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row - 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col - 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.K && row != i_size - 1 && row != i_size - 2 && 
        //                        i_grid[row + 2, col - 2] == Board.PieceType.None && (i_grid[row + 1, col - 1] == Board.PieceType.O ||
        //                            i_grid[row + 1, col - 1] == Board.PieceType.U))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row + 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col - 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                    }
        //                    else // Any other column
        //                    {
        //                        if (i_grid[row - 2, col + 2] == Board.PieceType.None && row != 0 && row != 1 &&
        //                            (i_grid[row - 1, col + 1] == Board.PieceType.O ||
        //                            i_grid[row - 1, col + 1] == Board.PieceType.U))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row - 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col + 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row - 2, col - 2] == Board.PieceType.None && row != 0 && row != 1 &&
        //                            (i_grid[row - 1, col - 1] == Board.PieceType.O ||
        //                            i_grid[row - 1, col - 1] == Board.PieceType.U))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row - 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col - 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.K && row != i_size - 1 && row != i_size - 2 &&
        //                            i_grid[row + 2, col + 2] == Board.PieceType.None && (i_grid[row + 1, col + 1] == Board.PieceType.O ||
        //                            i_grid[row + 1, col + 1] == Board.PieceType.U))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row + 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col + 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                        if (i_grid[row, col] == Board.PieceType.K && row != i_size - 1 && row != i_size - 2 &&
        //                            i_grid[row + 2, col - 2] == Board.PieceType.None && (i_grid[row + 1, col - 1] == Board.PieceType.O ||
        //                            i_grid[row + 1, col - 1] == Board.PieceType.U))
        //                        {
        //                            optionalEatMove[0] = ConvertDigitToUpperLetter(row);
        //                            optionalEatMove[1] = ConvertDigitToLowerLetter(col);
        //                            optionalEatMove[2] = '>';
        //                            optionalEatMove[3] = ConvertDigitToUpperLetter(row + 2);
        //                            optionalEatMove[4] = ConvertDigitToLowerLetter(col - 2);
        //                            optionalEatMovesArray.Add(optionalEatMove);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return optionalEatMovesArray;
        //}


    }
}
