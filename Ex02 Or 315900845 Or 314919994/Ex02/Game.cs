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
        private ePlayerType _playerModeChoice;

        public Game(Player player1, Player player2, int boardSize, ePlayerType playerModeChoice)
        {
            _board = new Board(boardSize);
            _player1 = player1;
            _player2 = player2;
            _playerModeChoice = playerModeChoice;
        }

        public void Start()
        {
            Grid grid = _board.GetGrid();
            LittleGameProcess(ref grid, _player1, _player2, _playerModeChoice);
            GameEndMenu(this);
        }

        private static void LittleGameProcess(ref Grid i_grid, Player player1, Player player2, ePlayerType playerType)
        {
            Player currentPlayer = player1;
            Player loser;

            while (true)
            {
                // if computer no need to write this line
                if (currentPlayer.Name != "Computer")
                {
                    Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol}):" +
                        $"{Environment.NewLine}{currentPlayer.Name}, enter your move (fromRow fromCol > toRow toCol) or 'Q' to quit:");
                }
                bool isGameOver = false;

                if (playerType == ePlayerType.Regular)
                {
                   switch(currentPlayer.MakePlayerMove(ref i_grid, player1))
                    {
                        case eMoveMade.Quit:
                            Console.WriteLine($"{currentPlayer.Name} quit the game. {(player1 == currentPlayer ? player2.Name : player1.Name)} wins!");
                            isGameOver = true;
                            loser = currentPlayer;
                            break;
                        case eMoveMade.None:
                            Console.WriteLine($"No possible moves for: {currentPlayer.Name}. {(player1 == currentPlayer ? player2.Name : player1.Name)} wins!");
                            isGameOver = true;
                            loser = currentPlayer;
                            break;
                        case eMoveMade.Done:
                            int player1Tokens = CountPlayerTokens(ref i_grid, 'X');
                            int player2Tokens = CountPlayerTokens(ref i_grid, 'O');

                            if (player1Tokens == 0)
                            {
                                Console.WriteLine($"{player2.Name} wins! No tokens left for {player1.Name}.");
                                isGameOver = true;
                                loser = currentPlayer;
                            }
                            else if (player2Tokens == 0)
                            {
                                Console.WriteLine($"{player1.Name} wins! No tokens left for {player2.Name}.");
                                isGameOver = true;
                            }

                            break;
                    }

                }
                else
                {
                    eMoveMade moveMade = new eMoveMade();
                    if(currentPlayer.Name == "Computer")
                    {
                        moveMade = currentPlayer.MakeComputerMove(ref i_grid, player2);
                    }
                    else
                    {
                        moveMade = currentPlayer.MakePlayerMove(ref i_grid, player1);
                    }
                    switch (moveMade)
                    {
                        case eMoveMade.Quit:
                            Console.WriteLine($"{currentPlayer.Name} quit the game. {(player1 == currentPlayer ? player2.Name : player1.Name)} wins!");
                            isGameOver = true;
                            break;
                        case eMoveMade.None:
                            Console.WriteLine($"No possible moves for: {currentPlayer.Name}. {(player1 == currentPlayer ? player2.Name : player1.Name)} wins!");
                            isGameOver = true;
                            break;
                        case eMoveMade.Done:
                            int player1Tokens = CountPlayerTokens(ref i_grid, 'X');
                            int player2Tokens = CountPlayerTokens(ref i_grid, 'O');

                            if (player1Tokens == 0)
                            {
                                Console.WriteLine($"{player2.Name} wins! No tokens left for {player1.Name}.");
                                isGameOver = true;
                            }
                            else if (player2Tokens == 0)
                            {
                                Console.WriteLine($"{player1.Name} wins! No tokens left for {player2.Name}.");
                                isGameOver = true;
                            }

                            break;
                    }
                }

                if (isGameOver)
                {
                    loser = currentPlayer;
                    break;
                }

                currentPlayer = (currentPlayer == player1) ? player2 : player1;
            }

            CalculateScore(ref i_grid,ref player1,ref player2, loser);
            Player.PrintScoreBoard(player1,player2);
        }

        public static eAnotherGame GetPlayerChoiceForRemacth()
        {
            while (true)
            {
                eAnotherGame eRematch = new eAnotherGame();
                Console.WriteLine($"1. No{Environment.NewLine}2. Yes");
                string choice = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(choice))
                {
                    if (choice == "1")
                    {
                        eRematch = eAnotherGame.No;
                    }
                    if (choice == "2")
                    {
                        eRematch = eAnotherGame.Yes;
                    }
                    return eRematch;
                }

                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        private static int CountPlayerPoints(ref Grid i_grid, Player player)
        {
            int score = player.Points;
            char playerSymbol = player.Symbol;

            ePieceType regularPiece = playerSymbol == 'X' ? ePieceType.X : ePieceType.O;
            ePieceType kingPiece = playerSymbol == 'X' ? ePieceType.K : ePieceType.U;

            for (int row = 0; row < i_grid.Size; row++)
            {
                for (int col = 0; col < i_grid.Size; col++)
                {
                    ePieceType piece = i_grid.GetPieceAt(row, col);
                    if (piece == regularPiece)
                    {
                        score++;
                    }
                    if (piece == kingPiece)
                    {
                        score += 4;
                    }
                }
            }

            return score;
        }

        public static void CalculateScore(ref Grid i_grid,ref Player player1,
            ref Player player2, Player loser)
        {
            int firstPlayerPoints = CountPlayerPoints(ref i_grid, player1);
            int secondPlayerPoints = CountPlayerPoints(ref i_grid, player2);
            int score = Math.Abs(firstPlayerPoints - secondPlayerPoints);
            
            if (player1.Name == loser.Name)
            {
                player1.Points += score;
            }
            else
            {
                player2.Points += score;
            }
        }
        public static void GameEndMenu(Game i_game)
        {
            bool keepPlaying = true;

            while (keepPlaying)
            {
                Console.WriteLine($"Thanks for playing Checkers with us!{Environment.NewLine}" +
                    $"Do you want to play another game?{Environment.NewLine}" +
                    $"Names, size, type and score will remain the same.");

                eAnotherGame playerChoiceForRematch = GetPlayerChoiceForRemacth();
                switch (playerChoiceForRematch)
                {
                    case eAnotherGame.No:
                        Console.WriteLine("Have a nice day!");
                        keepPlaying = false;
                        break;
                    case eAnotherGame.Yes:
                        Grid newGrid = new Grid(i_game._board.GetGrid().Size);
                        ConsoleUtils.Screen.Clear();
                        Board.PrintBoard(ref newGrid);
                        LittleGameProcess(ref newGrid, i_game._player1, i_game._player2, i_game._playerModeChoice);
                        break;
                }
            }
        }

        private static int CountPlayerTokens(ref Grid i_grid, char playerSymbol)
        {
            int count = 0;

            // Determine the pieces to count based on the player symbol
            ePieceType regularPiece = playerSymbol == 'X' ? ePieceType.X : ePieceType.O;
            ePieceType kingPiece = playerSymbol == 'X' ? ePieceType.K : ePieceType.U;

            for (int row = 0; row < i_grid.Size; row++)
            {
                for (int col = 0; col < i_grid.Size; col++)
                {
                    ePieceType piece = i_grid.GetPieceAt(row, col);
                    if (piece == regularPiece || piece == kingPiece)
                    {
                        count++;
                    }
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
            string validatedMove = null;
            while (validatedMove == null)
            {
                // Prompt for input if it's null or whitespace
                if (string.IsNullOrWhiteSpace(i_nextMoveString))
                {
                    Console.WriteLine("Input cannot be empty. Try again.");
                }

                // Check if the user wants to quit
                else if (i_nextMoveString.ToUpper() == "Q")
                {
                    validatedMove = "Q"; // Return quit signal
                }

                // Validate input format: Xx>Yy
                else if (i_nextMoveString.Length == 5 &&
                    char.IsUpper(i_nextMoveString[0]) &&
                    char.IsLower(i_nextMoveString[1]) &&
                    i_nextMoveString[2] == '>' &&
                    char.IsUpper(i_nextMoveString[3]) &&
                    char.IsLower(i_nextMoveString[4]))
                {
                    validatedMove = i_nextMoveString;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter move in format of Xx>Yy, or 'Q' to quit.");
                }

                if (validatedMove == null)
                {
                    i_nextMoveString = Console.ReadLine();
                }
            }
            return validatedMove;
        }

        public static Grid UpdatingBoard(string move,ref Grid i_grid, int i_size, char playerSymbol)
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
                i_grid.SetPieceAt(eatenRow, eatenCol, ePieceType.None);
            }

            // Update the board based on the move
            i_grid.SetPieceAt(toRow, toCol, i_grid.GetPieceAt(fromRow, fromCol));
            i_grid.SetPieceAt(fromRow, fromCol, ePieceType.None);  // Clear the original position

            // If the piece is not a king and reached the end of the board, make it a king
            if (playerSymbol == 'O' && toRow == i_size - 1)
            {
                i_grid.SetPieceAt(toRow, toCol, ePieceType.U);  // Make 'O' a king
            }
            else if (playerSymbol == 'X' && toRow == 0)
            {
                i_grid.SetPieceAt(toRow, toCol, ePieceType.K);  // Make 'X' a king
            }

            return i_grid;
        }


    }
}
