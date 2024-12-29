using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02
{
    class Game
    {
        private Board _board;
        private Player _player1;
        private Player _player2;

        public Game(Player player1, Player player2, int boardSize)
        {
            _board = new Board(boardSize);
            _player1 = player1;
            _player2 = player2;
        }

        public void Start()
        {
            Player currentPlayer = _player1;

            while (true)
            {
               // Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol}):{Environment.NewLine}{currentPlayer.Name}, enter your move (fromRow fromCol > toRow toCol) or 'Q' to quit:");
                
                string input = Console.ReadLine();


                MakeMove(input, _board.Grid, _board.Size, currentPlayer);
                //if (quit)
                {
                    Console.WriteLine($"{currentPlayer.Name} quit the game. {(_player1 == currentPlayer ? _player2.Name : _player1.Name)} wins!");
                    break;
                }

                //_board.MovePiece(fromRow, fromCol, toRow, toCol);

                currentPlayer = currentPlayer == _player1 ? _player2 : _player1;
            }
        }


        char ConvertDigitToLowerLetter(int i_digit)
        {
            return (char)('a' + (i_digit - '0')); // Map 0->a, 1->b, ..., 9->j
        }

        char ConvertDigitToUpperLetter(int i_digit)
        {
            return (char)('A' + (i_digit - '0')); // Map 0->A, 1->B, ..., 9->J
        }

        public string ValidateMove(string i_nextMoveString, Player i_player) {

            // Loop for continuous input validation
            string returnedValue = "True";

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
                    Console.WriteLine($"{i_player.Name} has lost the game. :( ");
                    returnedValue = "Q"; // Exit the method
                    break;
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
                    break;
                }

                // If input is invalid, display a message and reset the loop
                Console.WriteLine("Invalid input. Please enter move in format of Xx>Yy, or 'Q' to quit.");
                i_nextMoveString = null; // Reset input to trigger the outer loop for new input
            }

            return returnedValue;
        }

        public Board.PieceType[,] MakeMove(string i_nextMoveString, Board.PieceType[,] i_grid, int i_size, Player i_player)
        {
            if (ValidateMove(i_nextMoveString, i_player) == "True")
            {
                // Lists for optional moves and eat moves
                List<string> OoptionalEatMoves = GetOptionalEatMoves(i_grid, i_size, 'O'); // צעדי אכילה של עיגול
                List<string> XoptionalEatMoves = GetOptionalEatMoves(i_grid, i_size, 'X'); // צעדי אכילה של איקס

                List<string> OoptionalMoves = GetOptionalMoves(i_grid, i_size, 'O'); // צעדים רגילים של עיגול
                List<string> XoptionalMoves = GetOptionalMoves(i_grid, i_size, 'X'); // צעדים רגילים של איקס

                bool isEat = false;

                while (OoptionalEatMoves.Count > 0)
                {
                    for (int i = 0; i < OoptionalEatMoves.Count; i++)
                    {
                        if (i_nextMoveString == OoptionalEatMoves[i])
                        {
                            isEat = true;
                            i_grid = UpdatingBoard(i_nextMoveString, i_grid, i_size, 'O');

                            //print
                            OoptionalEatMoves = GetOptionalEatMoves(i_grid, i_size, 'O');
                            if (OoptionalEatMoves.Count > 0)
                            {
                                if(ValidateMove(i_nextMoveString = Console.ReadLine(), i_player) == "True")
                                {

                                }
                                 
                                break;
                            }
                        }
                    }
                    break;
                }

                if (OoptionalMoves.Count > 0 && !isEat)
                {
                    for (int i = 0; i < OoptionalMoves.Count; i++)
                    {
                        if (i_nextMoveString == OoptionalMoves[i])
                        {
                            i_grid = UpdatingBoard(i_nextMoveString, i_grid, i_size, 'O');
                            break;
                        }
                    }
                }
                return i_grid;
            }
            else
            {
                // print lost
            }
        }


        // Helper function to perform the move (update grid)
        private Board.PieceType[,] UpdatingBoard(string move, Board.PieceType[,] i_grid, int i_size, char playerSymbol)
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

            // If the piece is not a king and reached the end of the board, make it a king
            if (playerSymbol == 'O' && toRow == i_size - 1)
            {
                i_grid[toRow, toCol] = Board.PieceType.U;  // Make 'O' a king
            }
            else if (playerSymbol == 'X' && toRow == 0)
            {
                i_grid[toRow, toCol] = Board.PieceType.K;  // Make 'X' a king
            }

            return i_grid;
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
                        if (i_symbol == 'O')
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

                            if (grid[row, col] == kingPiece)
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
                        }

                        // במקרה של 'X' ו-'K' - אכילה קדימה לכיוון למעלה
                        if (i_symbol == 'X')
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

                        // במקרה של 'K' (מלך 'X') - אכילה אחורה לכיוון למטה
                        if (grid[row, col] == kingPiece)
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
    }
}
