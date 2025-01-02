using System;

namespace Ex02
{
    class Game
    {
        private Board m_Board;
        private Player m_Player1;
        private Player m_Player2;
        private ePlayerType m_PlayerModeChoice;

        public Game(Player i_Player1, Player i_Player2, int i_BoardSize, ePlayerType i_PlayerModeChoice)
        {
            m_Board = new Board(i_BoardSize);
            m_Player1 = i_Player1;
            m_Player2 = i_Player2;
            m_PlayerModeChoice = i_PlayerModeChoice;
        }

        public void Start()
        {
            Grid grid = m_Board.GetGrid();
            LittleGameProcess(ref grid, m_Player1, m_Player2, m_PlayerModeChoice);
            GameEndMenu(this);
        }

        private static void LittleGameProcess(ref Grid io_Grid, Player i_Player1, Player i_Player2, ePlayerType i_PlayerType)
        {
            Player currentPlayer = i_Player1;
            Player loser;

            while (true)
            {
                // if computer no need to write this line
                if (currentPlayer.Name != "Computer")
                {
                    Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol}):" +
                        $"{Environment.NewLine}{currentPlayer.Name}, enter your i_Move (FROMROWfromcol>TOROWtocol) or 'Q' to quit:");
                }
                bool isGameOver = false;

                if (i_PlayerType == ePlayerType.Regular)
                {
                    switch (currentPlayer.MakePlayerMove(ref io_Grid, currentPlayer))
                    {
                        case eMoveMade.Quit:
                            Console.WriteLine($"{currentPlayer.Name} quit the game. {(i_Player1 == currentPlayer ? i_Player2.Name : i_Player1.Name)} wins!");
                            isGameOver = true;
                            loser = currentPlayer;
                            break;
                        case eMoveMade.None:
                            Console.WriteLine($"No possible moves for: {currentPlayer.Name}. {(i_Player1 == currentPlayer ? i_Player2.Name : i_Player1.Name)} wins!");
                            isGameOver = true;
                            loser = currentPlayer;
                            break;
                        case eMoveMade.Done:
                            int player1Tokens = countPlayerTokens(ref io_Grid, 'X');
                            int player2Tokens = countPlayerTokens(ref io_Grid, 'O');

                            if (player1Tokens == 0)
                            {
                                Console.WriteLine($"{i_Player2.Name} wins! No tokens left for {i_Player1.Name}.");
                                isGameOver = true;
                                loser = currentPlayer;
                            }
                            else if (player2Tokens == 0)
                            {
                                Console.WriteLine($"{i_Player1.Name} wins! No tokens left for {i_Player2.Name}.");
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
                        moveMade = currentPlayer.MakeComputerMove(ref io_Grid, i_Player2);
                    }
                    else
                    {
                        moveMade = currentPlayer.MakePlayerMove(ref io_Grid, i_Player1);
                    }
                    switch (moveMade)
                    {
                        case eMoveMade.Quit:
                            Console.WriteLine($"{currentPlayer.Name} quit the game. {(i_Player1 == currentPlayer ? i_Player2.Name : i_Player1.Name)} wins!");
                            isGameOver = true;
                            break;
                        case eMoveMade.None:
                            Console.WriteLine($"No possible moves for: {currentPlayer.Name}. {(i_Player1 == currentPlayer ? i_Player2.Name : i_Player1.Name)} wins!");
                            isGameOver = true;
                            break;
                        case eMoveMade.Done:
                            int player1Tokens = countPlayerTokens(ref io_Grid, 'X');
                            int player2Tokens = countPlayerTokens(ref io_Grid, 'O');

                            if (player1Tokens == 0)
                            {
                                Console.WriteLine($"{i_Player2.Name} wins! No tokens left for {i_Player1.Name}.");
                                isGameOver = true;
                            }
                            else if (player2Tokens == 0)
                            {
                                Console.WriteLine($"{i_Player1.Name} wins! No tokens left for {i_Player2.Name}.");
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

                currentPlayer = (currentPlayer == i_Player1) ? i_Player2 : i_Player1;
            }

            CalculateScore(ref io_Grid,ref i_Player1,ref i_Player2, loser);
            Player.PrintScoreBoard(i_Player1,i_Player2);
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

        private static int countPlayerPoints(ref Grid io_Grid, Player i_Player)
        {
            int score = 0;
            char playerSymbol = i_Player.Symbol;

            ePieceType regularPiece = playerSymbol == 'X' ? ePieceType.X : ePieceType.O;
            ePieceType kingPiece = playerSymbol == 'X' ? ePieceType.K : ePieceType.U;

            for (int row = 0; row < io_Grid.Size; row++)
            {
                for (int col = 0; col < io_Grid.Size; col++)
                {
                    ePieceType piece = io_Grid.GetPieceAt(row, col);
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

        public static void CalculateScore(ref Grid io_Grid, ref Player player1,
            ref Player player2, Player i_Loser)//check again parameters name
        {
            int firstPlayerPoints = countPlayerPoints(ref io_Grid, player1);
            int secondPlayerPoints = countPlayerPoints(ref io_Grid, player2);
            int scoreDifference = Math.Abs(firstPlayerPoints - secondPlayerPoints);
            
            if (player1.Name == i_Loser.Name)
            {
                player1.Points += scoreDifference;
            }
            else
            {
                player2.Points += scoreDifference;
            }
        }

        public static void GameEndMenu(Game i_Game)
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
                        Grid newGrid = new Grid(i_Game.m_Board.GetGrid().Size);
                        ConsoleUtils.Screen.Clear();
                        Board.PrintBoard(ref newGrid);
                        LittleGameProcess(ref newGrid, i_Game.m_Player1, i_Game.m_Player2, i_Game.m_PlayerModeChoice);
                        break;
                }
            }
        }

        private static int countPlayerTokens(ref Grid io_Grid, char i_playerSymbol)
        {
            int count = 0;

            // Determine the pieces to count based on the player symbol
            ePieceType regularPiece = i_playerSymbol == 'X' ? ePieceType.X : ePieceType.O;
            ePieceType kingPiece = i_playerSymbol == 'X' ? ePieceType.K : ePieceType.U;

            for (int row = 0; row < io_Grid.Size; row++)
            {
                for (int col = 0; col < io_Grid.Size; col++)
                {
                    ePieceType piece = io_Grid.GetPieceAt(row, col);
                    if (piece == regularPiece || piece == kingPiece)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static string ConvertStepToString(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            char fromRowChar = (char)('A' + i_FromRow);
            char fromColChar = (char)('a' + i_FromCol);
            char toRowChar = (char)('A' + i_ToRow);
            char toColChar = (char)('a' + i_ToCol);
            string stepString = fromRowChar.ToString() + fromColChar + '>' + toRowChar + toColChar;
            return stepString;
        }

        public static string ValidateMove(string i_NextMoveString)
        {
            string validatedMove = null;

            while (validatedMove == null)
            {
                // Prompt for input if it's null or whitespace
                if (string.IsNullOrWhiteSpace(i_NextMoveString))
                {
                    Console.WriteLine("Input cannot be empty. Try again.");
                }

                // Check if the user wants to quit
                else if (i_NextMoveString.ToUpper() == "Q")
                {
                    validatedMove = "Q"; // Return quit signal
                }

                // Validate input format: Xx>Yy
                else if (i_NextMoveString.Length == 5 &&
                    char.IsUpper(i_NextMoveString[0]) &&
                    char.IsLower(i_NextMoveString[1]) &&
                    i_NextMoveString[2] == '>' &&
                    char.IsUpper(i_NextMoveString[3]) &&
                    char.IsLower(i_NextMoveString[4]))
                {
                    validatedMove = i_NextMoveString;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter i_Move in format of Xx>Yy, or 'Q' to quit.");
                }

                if (validatedMove == null)
                {
                    i_NextMoveString = Console.ReadLine();
                }
            }
            return validatedMove;
        }

        public static Grid UpdatingBoard(string i_Move,ref Grid io_Grid, int i_size, char i_PlayerSymbol)
        {
            // Parse Move string like "Aa>Bb"
            char fromRowChar = i_Move[0];
            char fromColChar = i_Move[1];
            char toRowChar = i_Move[3];
            char toColChar = i_Move[4];

            int fromRow = fromRowChar - 'A';  // Convert row letter to index
            int fromCol = fromColChar - 'a'; // Convert column letter to index
            int toRow = toRowChar - 'A';     // Convert row letter to index
            int toCol = toColChar - 'a';    // Convert column letter to index

            // Check if this is an eating Move (jumping over an opponent's piece)
            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);

            if (rowDiff == 2 && colDiff == 2) // Eating iMove
            {
                int eatenRow = (fromRow + toRow) / 2;
                int eatenCol = (fromCol + toCol) / 2;

                // Erase the eaten token
                io_Grid.SetPieceAt(eatenRow, eatenCol, ePieceType.None);
            }

            // Update the board based on the Move
            io_Grid.SetPieceAt(toRow, toCol, io_Grid.GetPieceAt(fromRow, fromCol));
            io_Grid.SetPieceAt(fromRow, fromCol, ePieceType.None);  // Clear the original position

            // If the piece is not a king and reached the end of the board, make it a king
            if (i_PlayerSymbol == 'O' && toRow == i_size - 1)
            {
                io_Grid.SetPieceAt(toRow, toCol, ePieceType.U);  // Make 'O' a king
            }
            else if (i_PlayerSymbol == 'X' && toRow == 0)
            {
                io_Grid.SetPieceAt(toRow, toCol, ePieceType.K);  // Make 'X' a king         
            }

            return io_Grid;
        }
    }
}
