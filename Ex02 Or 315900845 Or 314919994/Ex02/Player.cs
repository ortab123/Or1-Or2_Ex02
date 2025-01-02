using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ex02.Game;

namespace Ex02
{
    public class Player
    {
        public string Name { get; private set; }
        public char Symbol { get; private set; } // 'X' or 'O'
        public int Points { get; set; }

        public Player(string name, char symbol, int points)
        {
            Name = name;
            Symbol = symbol;
            Points = points;
        }

        public static string GetValidatePlayerName(string playerPrompt)
        {
            while (true)
            {
                Console.WriteLine($"{playerPrompt}, enter your name (max 20 characters, no spaces):");
                string name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name) && name.Length <= 20 && !name.Contains(" "))
                {
                    name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                    return name;
                }

                Console.WriteLine("Invalid name. Please try again.");
            }
        }

        public static Player GetPlayerOrComputer(ePlayerType choice)
        {
            Player player = null;
            char playerSymbol = 'O';

            switch (choice)
            {
                case ePlayerType.Regular:
                    string player2Name = Player.GetValidatePlayerName("Player 2");
                    player = new Player(player2Name, playerSymbol, 0);
                    break;
                case ePlayerType.Computer:
                    player = new Player("Computer", playerSymbol, 0);
                    break;
            }

            return player;
        }

        public eMoveMade MakeComputerMove(ref Grid i_grid, Player i_player)
        {
            char playerSymbol = i_player.Symbol;
            int size = i_grid.Size;
            eMoveMade isMoveMade = new eMoveMade();

            List<string> optionalEatMoves = GetOptionalEatMoves(ref i_grid, size, playerSymbol);
            List<string> optionalMoves = GetOptionalMoves(ref i_grid, size, playerSymbol);
            bool isEat = false;

            int randomIndex;
            string nextMoveString;
            Random random = new Random();

            // need a random number to choose from optional moves

            while (optionalEatMoves.Count > 0)
            {
                isEat = true;
                randomIndex = random.Next(0, optionalEatMoves.Count);
                nextMoveString = optionalEatMoves[randomIndex];


                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true); // Discard all leftover key inputs
                }

                // Need to press "Enter" to reavel computer move - wait for player
                Console.WriteLine("Computer's Turn (press 'enter' to see it's move)");
                Console.ReadLine();


                i_grid = UpdatingBoard(nextMoveString, ref i_grid, size, playerSymbol);
                Board.PrintBoard(ref i_grid);
                Console.WriteLine($"Computer's move was ({i_player.Symbol}): {nextMoveString}");
                isMoveMade = eMoveMade.Done;

                string eaterFinalPos = nextMoveString.Substring(3);

                // Recalculate eat moves after the current move
                optionalEatMoves = GetOptionalEatMoves(ref i_grid, size, playerSymbol);
                bool hasExtraMove = false;


                foreach (string move in optionalEatMoves)
                {
                    if (move.StartsWith(eaterFinalPos))
                    {
                        hasExtraMove = true;
                        break;
                    }
                }

                if (!hasExtraMove)
                {
                    break;
                }
            }

            if (!isEat && optionalEatMoves.Count == 0 && optionalMoves.Count > 0)
            {
                randomIndex = random.Next(0, optionalMoves.Count);
                nextMoveString = optionalMoves[randomIndex];
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true); // Discard all leftover key inputs
                }

                // Need to press "Enter" to reavel computer move - wait for player
                Console.WriteLine("Computer's Turn (press 'enter' to see it's move)");
                Console.ReadLine();

                i_grid = UpdatingBoard(nextMoveString, ref i_grid, size, playerSymbol);
                Board.PrintBoard(ref i_grid);
                Console.WriteLine($"Computer's move was ({i_player.Symbol}): {nextMoveString}");
                isMoveMade = eMoveMade.Done;
            }

            if (optionalMoves.Count == 0 && optionalEatMoves.Count == 0)
            {
                isMoveMade = eMoveMade.None;
            }

            return isMoveMade;
        }

        public eMoveMade MakePlayerMove(ref Grid i_grid, Player i_player)
        {

            int size = i_grid.Size;
            string validationResult = ValidateMove(Console.ReadLine());

            eMoveMade isMoveMade = new eMoveMade();


            if (validationResult == "Q")
            {
                isMoveMade = eMoveMade.Quit;
            }

            char playerSymbol = i_player.Symbol;
            List<string> optionalEatMoves = GetOptionalEatMoves(ref i_grid, size, playerSymbol);
            List<string> optionalMoves = GetOptionalMoves(ref i_grid, size, playerSymbol);

            bool isEat = false;

            // Handle eating moves
            while (optionalEatMoves.Count > 0 && isMoveMade != eMoveMade.Quit)
            {

                // Check if the entered move is a valid eating move
                if (optionalEatMoves.Contains(validationResult))
                {
                    isEat = true;
                    i_grid = UpdatingBoard(validationResult, ref i_grid, size, playerSymbol);
                    Board.PrintBoard(ref i_grid);
                    Console.WriteLine($"{i_player.Name}'s move was ({i_player.Symbol}): {validationResult}");
                    isMoveMade = eMoveMade.Done;

                    string eaterFinalPos = validationResult.Substring(3);
  
                    // Recalculate eat moves after the current move
                    optionalEatMoves = GetOptionalEatMoves(ref i_grid, size, playerSymbol);
                    bool hasExtraMove = false;

                    foreach (string move in optionalEatMoves)
                    {
                        if (move.StartsWith(eaterFinalPos))
                        {
                            Console.WriteLine("You have another eating move. Enter your next move:");
                            validationResult = ValidateMove(Console.ReadLine());
                            while (move != validationResult)
                            {
                                Console.WriteLine("Wrong move, You have to you the same piece you used prevoiusly.");
                                validationResult = ValidateMove(Console.ReadLine());
                            }
                            // בדיקה רק אם הצעד שהכניס הוא הצעד move אז תעשה
                            if (validationResult == "Q")
                            {
                                return eMoveMade.Quit;
                            }
                            if (validationResult.StartsWith(eaterFinalPos))
                            {
                                hasExtraMove = true;
                                i_grid = UpdatingBoard(validationResult, ref i_grid, size, playerSymbol);
                                Board.PrintBoard(ref i_grid);
                                Console.WriteLine($"{i_player.Name}'s move was ({i_player.Symbol}): {validationResult}");
                                isMoveMade = eMoveMade.Done;
                                break;
                            }
                        }
                    }
                    if (hasExtraMove)
                    {
                        break;
                    }

                }
                else
                {
                    Console.WriteLine("Invalid move. You must make an eating move.");
                    validationResult = ValidateMove(Console.ReadLine());

                    if (validationResult == "Q")
                    {
                        isMoveMade = eMoveMade.Quit;
                    }
                }
            }

            // Handle regular moves if no eating moves are available
            while (!isEat && optionalMoves.Count > 0 && isMoveMade != eMoveMade.Quit)
            {
                if (optionalMoves.Contains(validationResult))
                {
                    i_grid = UpdatingBoard(validationResult, ref i_grid, size, playerSymbol);
                    Board.PrintBoard(ref i_grid);
                    Console.WriteLine($"{i_player.Name}'s move was ({i_player.Symbol}): {validationResult}");
                    isMoveMade = eMoveMade.Done;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid move. Please enter a valid move.");
                    validationResult = ValidateMove(Console.ReadLine());

                    if (validationResult == "Q")
                    {
                        isMoveMade = eMoveMade.Quit;
                        break;
                    }
                }
            }

            if (optionalMoves.Count == 0 && optionalEatMoves.Count == 0)
            {
                isMoveMade = eMoveMade.None;
            }

            return isMoveMade;
        }

        public static List<string> GetOptionalMoves(ref Grid i_grid, int i_size, char i_symbol)
        {
            List<string> optionalMoves = new List<string>();
            ePieceType regularPiece = ePieceType.None, kingPiece = ePieceType.None;

            if (i_symbol == 'O')
            {
                regularPiece = ePieceType.O;
                kingPiece = ePieceType.U;
            }
            else if (i_symbol == 'X')
            {
                regularPiece = ePieceType.X;
                kingPiece = ePieceType.K;
            }

            for (int row = 0; row < i_size; row++)
            {
                for (int col = 0; col < i_size; col++)
                {
                    ePieceType piece = i_grid.GetPieceAt(row, col);
                    if (piece == regularPiece || piece == kingPiece)
                    {
                        // עבור 'O' ו-'U' - דילוג קדימה לכיוון מטה
                        if (piece == ePieceType.O || piece == ePieceType.U)
                        {
                            // Move Forward Left
                            if (row + 1 < i_size && col - 1 >= 0 &&
                                i_grid.GetPieceAt(row + 1, col - 1) == ePieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row + 1, col - 1);
                                optionalMoves.Add(move);
                            }

                            // Move Forward Right
                            if (row + 1 < i_size && col + 1 < i_size &&
                                i_grid.GetPieceAt(row + 1, col + 1) == ePieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row + 1, col + 1);
                                optionalMoves.Add(move);
                            }

                            // עבור 'U' (מלך 'O') - דילוג אחורה לכיוון למעלה
                            if (piece == ePieceType.U)
                            {
                                // Move Backward Left
                                if (row - 1 >= 0 && col - 1 >= 0 &&
                                    i_grid.GetPieceAt(row - 1, col - 1) == ePieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row - 1, col - 1);
                                    optionalMoves.Add(move);
                                }

                                // Move Backward Right
                                if (row - 1 >= 0 && col + 1 < i_size &&
                                    i_grid.GetPieceAt(row - 1, col + 1) == ePieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row - 1, col + 1);
                                    optionalMoves.Add(move);
                                }
                            }
                        }

                        // עבור 'X' ו-'K' - דילוג קדימה לכיוון מעלה
                        if (piece == ePieceType.X || piece == ePieceType.K)
                        {
                            // Move Forward Left
                            if (row - 1 >= 0 && col - 1 >= 0 &&
                                i_grid.GetPieceAt(row - 1, col - 1) == ePieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row - 1, col - 1);

                                optionalMoves.Add(move);
                            }

                            // Move Forward Right
                            if (row - 1 >= 0 && col + 1 < i_size &&
                                i_grid.GetPieceAt(row - 1, col + 1) == ePieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row - 1, col + 1);

                                optionalMoves.Add(move);
                            }

                            // עבור 'K' (מלך 'X') - דילוג אחורה לכיוון למטה
                            if (piece == ePieceType.K)
                            {
                                // Move Backward Left
                                if (row + 1 < i_size && col - 1 >= 0 &&
                                    i_grid.GetPieceAt(row + 1, col - 1) == ePieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row + 1, col - 1);
                                    optionalMoves.Add(move);
                                }

                                // Move Backward Right
                                if (row + 1 < i_size && col + 1 < i_size &&
                                    i_grid.GetPieceAt(row + 1, col + 1) == ePieceType.None)
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

        public static List<string> GetOptionalEatMoves(ref Grid grid, int size, char playerSymbol)
        {
            List<string> optionalEatMoves = new List<string>();
            ePieceType regularPiece = ePieceType.None, kingPiece = ePieceType.None;
            ePieceType opponentRegular = ePieceType.None, opponentKing = ePieceType.None;

            // Initialize piece types based on playerSymbol
            if (playerSymbol == 'O')
            {
                regularPiece = ePieceType.O;
                kingPiece = ePieceType.U;
                opponentRegular = ePieceType.X;
                opponentKing = ePieceType.K;
            }
            else if (playerSymbol == 'X')
            {
                regularPiece = ePieceType.X;
                kingPiece = ePieceType.K;
                opponentRegular = ePieceType.O;
                opponentKing = ePieceType.U;
            }

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    // Check if the piece is regular or king
                    if (grid.GetPieceAt(row, col) == regularPiece || grid.GetPieceAt(row, col) == kingPiece)
                    {
                        // Regular pieces only move in specific directions
                        if (grid.GetPieceAt(row, col) == regularPiece)
                        {
                            // For 'O', check downward eats
                            if (playerSymbol == 'O')
                            {
                                AddEatMoveIfValid(optionalEatMoves, ref grid, row, col, size,
                                    1, -1, opponentRegular, opponentKing);
                                AddEatMoveIfValid(optionalEatMoves, ref grid, row, col, size,
                                    1, 1, opponentRegular, opponentKing);
                            }
                            // For 'X', check upward eats
                            else if (playerSymbol == 'X')
                            {
                                AddEatMoveIfValid(optionalEatMoves, ref grid, row, col, size,
                                    -1, -1, opponentRegular, opponentKing);
                                AddEatMoveIfValid(optionalEatMoves, ref grid, row, col, size,
                                    -1, 1, opponentRegular, opponentKing);
                            }
                        }

                        // Kings can move in all directions
                        if (grid.GetPieceAt(row, col) == kingPiece)
                        {
                            AddEatMoveIfValid(optionalEatMoves, ref grid, row, col, size,
                                1, -1, opponentRegular, opponentKing);
                            AddEatMoveIfValid(optionalEatMoves, ref grid, row, col, size,
                                1, 1, opponentRegular, opponentKing);
                            AddEatMoveIfValid(optionalEatMoves, ref grid, row, col, size,
                                -1, -1, opponentRegular, opponentKing);
                            AddEatMoveIfValid(optionalEatMoves, ref grid, row, col, size,
                                -1, 1, opponentRegular, opponentKing);
                        }
                    }
                }
            }

            return optionalEatMoves;
        }

        private static void AddEatMoveIfValid(List<string> optionalEatMoves, ref Grid grid, int row, int col,
            int size, int rowDir, int colDir, ePieceType opponentRegular, ePieceType opponentKing)
        {
            int targetRow = row + rowDir * 2;   // Target position after the jump
            int targetCol = col + colDir * 2;
            int middleRow = row + rowDir;      // Position of the opponent's piece
            int middleCol = col + colDir;

            if (targetRow >= 0 && targetRow < size && targetCol >= 0 && targetCol < size && // Ensure within bounds
                (grid.GetPieceAt(middleRow, middleCol) == opponentRegular || grid.GetPieceAt(middleRow, middleCol) == opponentKing) && // Opponent's piece present
                grid.GetPieceAt(targetRow, targetCol) == ePieceType.None) // Target cell is empty
            {
                string move = ConvertStepToString(row, col, targetRow, targetCol);
                optionalEatMoves.Add(move);
            }
        }

        public static void PrintScoreBoard(Player player1, Player player2)
        {
            Console.WriteLine($"{Environment.NewLine}Score Board{Environment.NewLine}" +
                $"{MakePointsString(player1)}{Environment.NewLine}" +
                $"{MakePointsString(player2)}{Environment.NewLine}");
        }

        private static string MakePointsString(Player player)
        {
            string playerPointsString = $"{player.Name}   ({player.Symbol}): {player.Points} points.";
            return playerPointsString;
        }
    }
}

