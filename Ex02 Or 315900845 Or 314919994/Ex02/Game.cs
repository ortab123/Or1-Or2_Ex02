using System;
using System.Collections.Generic;
using static Ex02.Player;


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
                Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol}):{Environment.NewLine}{currentPlayer.Name}, enter your move (fromRow fromCol > toRow toCol) or 'Q' to quit:");

                bool isGameOver = false;


                // handle which player is playing

                switch (currentPlayer.MakeMove(_board.Grid, currentPlayer))
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
                        int player1Tokens = CountPlayerTokens(_board.Grid, 'X');
                        int player2Tokens = CountPlayerTokens(_board.Grid, 'O');

                        if (player1Tokens == 0)
                        {
                            Console.WriteLine($"{_player2.Name} wins! No tokens left for {_player1.Name}.");
                            isGameOver = true;
                        }
                        else if (player2Tokens == 0)
                        {
                            Console.WriteLine($"{_player1.Name} wins! No tokens left for {_player2.Name}.");
                            isGameOver = true;
                        }

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

        private int CountPlayerTokens(Board.PieceType[,] grid, char playerSymbol)
        {
            int count = 0;

            // Determine the pieces to count based on the player symbol
            Board.PieceType regularPiece = playerSymbol == 'X' ? Board.PieceType.X : Board.PieceType.O;
            Board.PieceType kingPiece = playerSymbol == 'X' ? Board.PieceType.K : Board.PieceType.U;

            foreach (var cell in grid)
            {
                if (cell == regularPiece || cell == kingPiece)
                {
                    count++;
                }
            }

            return count;
        }

        public static string ConvertStepToString(int fromRow, int fromCol, int toRow, int toCol)
        {
            char fromRowChar = (char)('A' + fromRow);
            char fromColChar = (char)('a' + fromCol);
            char toRowChar = (char)('A' + toRow);
            char toColChar = (char)('a' + toCol);
            string stepString = fromRowChar.ToString() + fromColChar + '>' + toRowChar + toColChar;
            return stepString;
        }

        public static string ValidateMove(string i_nextMoveString)
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
                    return i_nextMoveString; // Input is valid
                }

                // If input is invalid, display a message
                Console.WriteLine("Invalid input. Please enter move in format of Xx>Yy, or 'Q' to quit.");
            }
        }

        public static Board.PieceType[,] UpdatingBoard(string move, Board.PieceType[,] i_grid, int i_size, char playerSymbol)
        {
            // Parse move string like "Aa>Bb"
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

            return i_grid;
        }

        public static List<string> GetOptionalMoves(Board.PieceType[,] i_grid, int i_size, char i_symbol)
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

        public static List<string> GetOptionalEatMoves(Board.PieceType[,] grid, int size, char playerSymbol)
        {
            List<string> optionalEatMoves = new List<string>();
            Board.PieceType regularPiece = Board.PieceType.None, kingPiece = Board.PieceType.None, opponentRegular = Board.PieceType.None, opponentKing = Board.PieceType.None;

            // Initialize piece types based on playerSymbol
            if (playerSymbol == 'O')
            {
                regularPiece = Board.PieceType.O;
                kingPiece = Board.PieceType.U;
                opponentRegular = Board.PieceType.X;
                opponentKing = Board.PieceType.K;
            }
            else if (playerSymbol == 'X')
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
                    // Check if the piece is regular or king
                    if (grid[row, col] == regularPiece || grid[row, col] == kingPiece)
                    {
                        // Regular pieces only move in specific directions
                        if (grid[row, col] == regularPiece)
                        {
                            // For 'O', check downward eats
                            if (playerSymbol == 'O')
                            {
                                AddEatMoveIfValid(optionalEatMoves, grid, row, col, size, 1, -1, opponentRegular, opponentKing); // Down-left
                                AddEatMoveIfValid(optionalEatMoves, grid, row, col, size, 1, 1, opponentRegular, opponentKing);  // Down-right
                            }
                            // For 'X', check upward eats
                            else if (playerSymbol == 'X')
                            {
                                AddEatMoveIfValid(optionalEatMoves, grid, row, col, size, -1, -1, opponentRegular, opponentKing); // Up-left
                                AddEatMoveIfValid(optionalEatMoves, grid, row, col, size, -1, 1, opponentRegular, opponentKing);  // Up-right
                            }
                        }

                        // Kings can move in all directions
                        if (grid[row, col] == kingPiece)
                        {
                            AddEatMoveIfValid(optionalEatMoves, grid, row, col, size, 1, -1, opponentRegular, opponentKing);  // Down-left
                            AddEatMoveIfValid(optionalEatMoves, grid, row, col, size, 1, 1, opponentRegular, opponentKing);   // Down-right
                            AddEatMoveIfValid(optionalEatMoves, grid, row, col, size, -1, -1, opponentRegular, opponentKing); // Up-left
                            AddEatMoveIfValid(optionalEatMoves, grid, row, col, size, -1, 1, opponentRegular, opponentKing);  // Up-right
                        }
                    }
                }
            }

            return optionalEatMoves;
        }

        public static void AddEatMoveIfValid(List<string> optionalEatMoves, Board.PieceType[,] grid, int row, int col, int size, int rowDir, int colDir, Board.PieceType opponentRegular, Board.PieceType opponentKing)
        {
            int targetRow = row + rowDir * 2;   // Target position after the jump
            int targetCol = col + colDir * 2;
            int middleRow = row + rowDir;      // Position of the opponent's piece
            int middleCol = col + colDir;

            if (targetRow >= 0 && targetRow < size && targetCol >= 0 && targetCol < size && // Ensure within bounds
                (grid[middleRow, middleCol] == opponentRegular || grid[middleRow, middleCol] == opponentKing) && // Opponent's piece present
                grid[targetRow, targetCol] == Board.PieceType.None) // Target cell is empty
            {
                string move = ConvertStepToString(row, col, targetRow, targetCol);
                optionalEatMoves.Add(move);
            }
        }

    }
}
