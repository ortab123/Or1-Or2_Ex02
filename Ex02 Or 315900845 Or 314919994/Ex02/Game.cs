using System;
using System.Collections.Generic;
using static Ex02.Game;


namespace Ex02
{
    class Game
    {
        private Board _board;
        private Player _player1;
        private Player _player2;
        public enum MoveMade
        {
            None, // ערך ברירת מחדל
            Quit,
            Done,
        }

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
                Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol}):{Environment.NewLine}{currentPlayer.Name}, enter your move (fromRow fromCol > toRow toCol) or 'Q' to quit:");

                string input = Console.ReadLine();
                bool isGameOver = false;
                switch (MakeMove(input, _board.Grid, currentPlayer))
                {
                    case MoveMade.Quit:
                        Console.WriteLine($"{currentPlayer.Name} quit the game. {(_player1 == currentPlayer ? _player2.Name : _player1.Name)} wins!");
                        isGameOver = true;
                        break;
                    case MoveMade.None:
                        Console.WriteLine($"No possible moves for: {currentPlayer.Name}. {(_player1 == currentPlayer ? _player2.Name : _player1.Name)} wins!");
                        isGameOver = true;
                        break;
                    case MoveMade.Done:
                        break;
                }
                if (isGameOver)
                {
                    break;
                }
                else
                {
                    currentPlayer = (currentPlayer == _player1) ? _player2 : _player1;
                }


            }
        }


        public string ConvertStepToString(int fromRow, int fromCol, int toRow, int toCol)
        {
            char fromRowChar = (char)('A' + fromRow);
            char fromColChar = (char)('a' + fromCol);
            char toRowChar = (char)('A' + toRow);
            char toColChar = (char)('a' + toCol);
            string stepString = fromRowChar.ToString() + fromColChar + '>' + toRowChar + toColChar;
            return stepString;
        }

        public string ValidateMove(string i_nextMoveString, Player i_player)
        {
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
                    return "Q"; // Return quit signal
                }

                // Validate input format: Xx>Yy
                if (i_nextMoveString.Length == 5 &&
                    char.IsUpper(i_nextMoveString[0]) &&
                    char.IsLower(i_nextMoveString[1]) &&
                    i_nextMoveString[2] == '>' &&
                    char.IsUpper(i_nextMoveString[3]) &&
                    char.IsLower(i_nextMoveString[4]))
                {
                    return "True"; // Input is valid
                }

                // If input is invalid, display a message
                Console.WriteLine("Invalid input. Please enter move in format of Xx>Yy, or 'Q' to quit.");
                i_nextMoveString = null; // Reset input to trigger the loop for new input
            }
        }

        public MoveMade MakeMove(string i_nextMoveString, Board.PieceType[,] i_grid, Player i_player)
        {

            int size = i_grid.GetLength(0);
            string validationResult = ValidateMove(i_nextMoveString, i_player);
            MoveMade isMoveMade = new MoveMade();


            if (validationResult == "Q")
            {
                isMoveMade = MoveMade.Quit;
            }

            // Determine the piece type for the player
            char playerSymbol = i_player.Symbol;
            List<string> optionalEatMoves = GetOptionalEatMoves(i_grid, size, playerSymbol);
            List<string> optionalMoves = GetOptionalMoves(i_grid, size, playerSymbol);

            bool isEat = false;

            // Handle eating moves
            while (optionalEatMoves.Count > 0)
            {
                // Check if the entered move is a valid eating move
                if (optionalEatMoves.Contains(i_nextMoveString))
                {
                    isEat = true;
                    i_grid = UpdatingBoard(i_nextMoveString, i_grid, size, playerSymbol);
                    Board.PrintBoard(i_grid);
                    Console.WriteLine($"{i_player.Name}'s move was ({i_player.Symbol}): {i_nextMoveString}");
                    isMoveMade = MoveMade.Done;

                    // Recalculate eat moves after the current move
                    optionalEatMoves = GetOptionalEatMoves(i_grid, size, playerSymbol);
                    if (optionalEatMoves.Count > 0)
                    {
                        Console.WriteLine("You have another eating move. Enter your next move:");
                        i_nextMoveString = Console.ReadLine();
                        validationResult = ValidateMove(i_nextMoveString, i_player);

                        if (validationResult == "Q")
                        {
                            isMoveMade = MoveMade.Quit;
                        }

                        continue;
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("Invalid move. You must make an eating move.");
                    i_nextMoveString = Console.ReadLine();
                    validationResult = ValidateMove(i_nextMoveString, i_player);

                    if (validationResult == "Q")
                    {
                        isMoveMade = MoveMade.Quit;
                    }
                }
            }

            // Handle regular moves if no eating moves are available
            while (!isEat && optionalMoves.Count > 0)
            {
                if (optionalMoves.Contains(i_nextMoveString))
                {
                    i_grid = UpdatingBoard(i_nextMoveString, i_grid, size, playerSymbol);
                    Board.PrintBoard(i_grid);
                    Console.WriteLine($"{i_player.Name}'s move was ({i_player.Symbol}) : {i_nextMoveString}");
                    isMoveMade = MoveMade.Done;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid move. Please enter a valid move.");
                    i_nextMoveString = Console.ReadLine();
                    validationResult = ValidateMove(i_nextMoveString, i_player);

                    if (validationResult == "Q")
                    {
                        isMoveMade = MoveMade.Quit;
                    }
                }
            }
            if (optionalMoves.Count == 0 && optionalEatMoves.Count == 0)
            {
                isMoveMade = MoveMade.None;
            }

            return isMoveMade;
        }

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

            // Check if this is an eating move (jumping over an opponent's piece)
            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);

            if (rowDiff == 2 && colDiff == 2) // Eating move
            {
                int eatenRow = (fromRow + toRow) / 2;
                int eatenCol = (fromCol + toCol) / 2;

                // Erase the eaten token
                i_grid[eatenRow, eatenCol] = Board.PieceType.None;
            }

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

            // count

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
                                string move = ConvertStepToString(row, col, row + 2, col - 2);
                                optionalEatMoves.Add(move);
                            }

                            // Eating forward right
                            bool canEatForwardRight = row + 2 < size && col + 2 < size &&
                                                      (grid[row + 1, col + 1] == opponentRegular || grid[row + 1, col + 1] == opponentKing) &&
                                                      grid[row + 2, col + 2] == Board.PieceType.None;

                            if (canEatForwardRight)
                            {
                                string move = ConvertStepToString(row, col, row + 2, col + 2);
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
                                    string move = ConvertStepToString(row, col, row - 2, col - 2);
                                    optionalEatMoves.Add(move);
                                }

                                // Eating backward right
                                bool canEatBackwardRight = row - 2 >= 0 && col + 2 < size &&
                                                           (grid[row - 1, col + 1] == opponentRegular || grid[row - 1, col + 1] == opponentKing) &&
                                                           grid[row - 2, col + 2] == Board.PieceType.None;
                                if (canEatBackwardRight)
                                {
                                    string move = ConvertStepToString(row, col, row - 2, col + 2);
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
                                string move = ConvertStepToString(row, col, row - 2, col - 2);
                                optionalEatMoves.Add(move);
                            }

                            // Eating forward right
                            bool canEatForwardRight = row - 2 >= 0 && col + 2 < size &&
                                                      (grid[row - 1, col + 1] == opponentRegular || grid[row - 1, col + 1] == opponentKing) &&
                                                      grid[row - 2, col + 2] == Board.PieceType.None;

                            if (canEatForwardRight)
                            {
                                string move = ConvertStepToString(row, col, row - 2, col + 2);
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
                                string move = ConvertStepToString(row, col, row + 2, col - 2);
                                optionalEatMoves.Add(move);
                            }

                            // Eating backward right
                            bool canEatBackwardRight = row + 2 < size && col + 2 < size &&
                                                       (grid[row + 1, col + 1] == opponentRegular || grid[row + 1, col + 1] == opponentKing) &&
                                                       grid[row + 2, col + 2] == Board.PieceType.None;

                            if (canEatBackwardRight)
                            {
                                string move = ConvertStepToString(row, col, row + 2, col + 2);
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
                        if (regularPiece == Board.PieceType.O || kingPiece == Board.PieceType.U)
                        {
                            // Move Forward Left
                            if (row + 1 < i_size && col - 1 >= 0 && i_grid[row + 1, col - 1] == Board.PieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row + 1, col - 1);
                                optionalMoves.Add(move);
                            }

                            // Move Forward Right
                            if (row + 1 < i_size && col + 1 < i_size && i_grid[row + 1, col + 1] == Board.PieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row + 1, col + 1);
                                optionalMoves.Add(move);
                            }

                            // עבור 'U' (מלך 'O') - דילוג אחורה לכיוון למעלה
                            if (kingPiece == Board.PieceType.U)
                            {
                                // Move Backward Left
                                if (row - 1 >= 0 && col - 1 >= 0 && i_grid[row - 1, col - 1] == Board.PieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row - 1, col - 1);
                                    optionalMoves.Add(move);
                                }

                                // Move Backward Right
                                if (row - 1 >= 0 && col + 1 < i_size && i_grid[row - 1, col + 1] == Board.PieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row - 1, col + 1);
                                    optionalMoves.Add(move);
                                }
                            }
                        }

                        // עבור 'X' ו-'K' - דילוג קדימה לכיוון מעלה
                        if (regularPiece == Board.PieceType.X || kingPiece == Board.PieceType.K)
                        {
                            // Move Forward Left
                            if (row - 1 >= 0 && col - 1 >= 0 && i_grid[row - 1, col - 1] == Board.PieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row - 1, col - 1);

                                optionalMoves.Add(move);
                            }

                            // Move Forward Right
                            if (row - 1 >= 0 && col + 1 < i_size && i_grid[row - 1, col + 1] == Board.PieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row - 1, col + 1);

                                optionalMoves.Add(move);
                            }

                            // עבור 'K' (מלך 'X') - דילוג אחורה לכיוון למטה
                            if (kingPiece == Board.PieceType.K)
                            {
                                // Move Backward Left
                                if (row + 1 < i_size && col - 1 >= 0 && i_grid[row + 1, col - 1] == Board.PieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row + 1, col - 1);
                                    optionalMoves.Add(move);
                                }

                                // Move Backward Right
                                if (row + 1 < i_size && col + 1 < i_size && i_grid[row + 1, col + 1] == Board.PieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row + 1, col + 1);
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
