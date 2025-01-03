using System;
using System.Collections.Generic;
using static Ex02.Game;

namespace Ex02
{
    public class Player
    {
        public string m_Name { get; private set; }
        public char m_Symbol { get; private set; }
        public int m_Points { get; set; }

        public Player(string name, char symbol, int points)
        {
            m_Name = name;
            m_Symbol = symbol;
            m_Points = points;
        }

        public static string GetValidatePlayerName(string i_PlayerPrompt)
        {
            while (true)
            {
                Console.WriteLine($"{i_PlayerPrompt}, enter your name (max 20 characters, no spaces):");
                string name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name) && name.Length <= 20 && !name.Contains(" "))
                {
                    name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                    return name;
                }

                Console.WriteLine("Invalid name. Please try again.");
            }
        }

        public static Player GetPlayerOrComputer(ePlayerType i_Choice)
        {
            Player player = null;
            char playerSymbol = 'O';

            switch (i_Choice)
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

        public eMoveMade MakeComputerMove(ref Grid io_Grid, Player i_Player)
        {
            char playerSymbol = i_Player.m_Symbol;
            int size = io_Grid.m_Size;
            eMoveMade isMoveMade = new eMoveMade();
            bool isEat = false;
            int randomIndex;
            string nextMoveString;

            List<string> optionalEatMoves = getOptionalEatMoves(ref io_Grid, size, playerSymbol);
            List<string> optionalMoves = getOptionalMoves(ref io_Grid, size, playerSymbol);
            Random random = new Random();

            while (optionalEatMoves.Count > 0)
            {
                isEat = true;
                randomIndex = random.Next(0, optionalEatMoves.Count);
                nextMoveString = optionalEatMoves[randomIndex];

                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                Console.WriteLine("Computer's Turn (press 'enter' to see it's move)");
                Console.ReadLine();

                io_Grid = UpdatingBoard(nextMoveString, ref io_Grid, size, playerSymbol);
                Board.PrintBoard(ref io_Grid);
                Console.WriteLine($"Computer's move was ({i_Player.m_Symbol}): {nextMoveString}");
                isMoveMade = eMoveMade.Done;

                string eaterFinalPos = nextMoveString.Substring(3);

                optionalEatMoves = getOptionalEatMoves(ref io_Grid, size, playerSymbol);
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
                    Console.ReadKey(true);
                }

                Console.WriteLine("Computer's Turn (press 'enter' to see it's move)");
                Console.ReadLine();

                io_Grid = UpdatingBoard(nextMoveString, ref io_Grid, size, playerSymbol);
                Board.PrintBoard(ref io_Grid);
                Console.WriteLine($"Computer's move was ({i_Player.m_Symbol}): {nextMoveString}");
                isMoveMade = eMoveMade.Done;
            }

            if (optionalMoves.Count == 0 && optionalEatMoves.Count == 0)
            {
                isMoveMade = eMoveMade.None;
            }

            return isMoveMade;
        }

        public eMoveMade MakePlayerMove(ref Grid io_Grid, Player i_Player)
        {
            int size = io_Grid.m_Size;
            eMoveMade isMoveMade = eMoveMade.None;

            string currentPlayerValidatedMove = ValidateMove(Console.ReadLine());
            if (currentPlayerValidatedMove == "Q")
            {
                isMoveMade = eMoveMade.Quit;
            }

            char playerSymbol = i_Player.m_Symbol;
            List<string> optionalEatMoves = getOptionalEatMoves(ref io_Grid, size, playerSymbol);
            List<string> optionalMoves = getOptionalMoves(ref io_Grid, size, playerSymbol);

            if (optionalEatMoves.Count > 0)
            {
                isMoveMade = handleEatingMoves(ref io_Grid, i_Player, currentPlayerValidatedMove,
                    optionalEatMoves, size);
            }

            if (isMoveMade == eMoveMade.None && optionalMoves.Count > 0)
            {
                isMoveMade = handleRegularMoves(ref io_Grid, i_Player, currentPlayerValidatedMove,
                    optionalMoves, size);
            }

            if (optionalEatMoves.Count == 0 && optionalMoves.Count == 0)
            {
                isMoveMade = eMoveMade.None;
            }

            return isMoveMade;
        }

        private eMoveMade handleEatingMoves(ref Grid io_Grid, Player i_Player,
            string i_CurrentPlayerValidatedMove, List<string> i_OptionalEatMoves, int i_Size)
        {
            eMoveMade moveState = eMoveMade.None;
            string currentPlayerValidatedMove = i_CurrentPlayerValidatedMove;

            while (i_OptionalEatMoves.Count > 0)
            {
                if (!i_OptionalEatMoves.Contains(currentPlayerValidatedMove))
                {
                    Console.WriteLine("Invalid move. You must make an eating move.");
                    currentPlayerValidatedMove = ValidateMove(Console.ReadLine());
                    if (currentPlayerValidatedMove == "Q")
                    {
                        moveState = eMoveMade.Quit;
                    }

                    continue;
                }

                io_Grid = UpdatingBoard(currentPlayerValidatedMove, ref io_Grid, i_Size, i_Player.m_Symbol);
                Board.PrintBoard(ref io_Grid);
                Console.WriteLine($"{i_Player.m_Name}'s move was ({i_Player.m_Symbol}): {currentPlayerValidatedMove}");
                string eaterFinalPos = currentPlayerValidatedMove.Substring(3);

                i_OptionalEatMoves = getOptionalEatMoves(ref io_Grid, i_Size, i_Player.m_Symbol);
                List<string> filteredMoves = new List<string>();

                foreach (string move in i_OptionalEatMoves)
                {
                    if (move.StartsWith(eaterFinalPos))
                    {
                        filteredMoves.Add(move);
                    }
                }

                i_OptionalEatMoves = filteredMoves;

                if (i_OptionalEatMoves.Count == 0)
                {
                    moveState = eMoveMade.Done;
                    break;
                }

                Console.WriteLine("You have another eating move. Enter your next move:");
                currentPlayerValidatedMove = ValidateMove(Console.ReadLine());
                if (currentPlayerValidatedMove == "Q")
                {
                    moveState = eMoveMade.Quit;
                    break;
                }

                continue;
            }

            return moveState;
        }

        private eMoveMade handleRegularMoves(ref Grid io_Grid, Player i_Player,
            string i_CurrentPlayerValidatedMove, List<string> i_OptionalMoves, int i_Size)
        {
            eMoveMade moveState = eMoveMade.None;
            string currentPlayerValidatedMove = i_CurrentPlayerValidatedMove;

            while (true)
            {
                if (i_OptionalMoves.Contains(currentPlayerValidatedMove))
                {
                    io_Grid = UpdatingBoard(currentPlayerValidatedMove, ref io_Grid, i_Size,
                        i_Player.m_Symbol);
                    Board.PrintBoard(ref io_Grid);
                    Console.WriteLine($"{i_Player.m_Name}'s move was ({i_Player.m_Symbol}):" +
                        $" {currentPlayerValidatedMove}");
                    moveState = eMoveMade.Done;
                    break;
                }

                Console.WriteLine("Invalid move. Please enter a valid move.");
                currentPlayerValidatedMove = ValidateMove(Console.ReadLine());
                if (currentPlayerValidatedMove == "Q")
                {
                    moveState = eMoveMade.Quit;
                    break;
                }
            }

            return moveState;
        }

        private static List<string> getOptionalMoves(ref Grid io_Grid, int i_Size, char i_Symbol)
        {
            List<string> optionalMoves = new List<string>();
            ePieceType regularPiece = ePieceType.None, kingPiece = ePieceType.None;

            if (i_Symbol == 'O')
            {
                regularPiece = ePieceType.O;
                kingPiece = ePieceType.U;
            }
            else if (i_Symbol == 'X')
            {
                regularPiece = ePieceType.X;
                kingPiece = ePieceType.K;
            }

            for (int row = 0; row < i_Size; row++)
            {
                for (int col = 0; col < i_Size; col++)
                {
                    ePieceType piece = io_Grid.GetPieceAt(row, col);
                    if (piece == regularPiece || piece == kingPiece)
                    {
                        if (piece == ePieceType.O || piece == ePieceType.U)
                        {
                            if (row + 1 < i_Size && col - 1 >= 0 &&
                                io_Grid.GetPieceAt(row + 1, col - 1) == ePieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row + 1, col - 1);
                                optionalMoves.Add(move);
                            }

                            if (row + 1 < i_Size && col + 1 < i_Size &&
                                io_Grid.GetPieceAt(row + 1, col + 1) == ePieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row + 1, col + 1);
                                optionalMoves.Add(move);
                            }

                            if (piece == ePieceType.U)
                            {
                                if (row - 1 >= 0 && col - 1 >= 0 &&
                                    io_Grid.GetPieceAt(row - 1, col - 1) == ePieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row - 1, col - 1);
                                    optionalMoves.Add(move);
                                }

                                if (row - 1 >= 0 && col + 1 < i_Size &&
                                    io_Grid.GetPieceAt(row - 1, col + 1) == ePieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row - 1, col + 1);
                                    optionalMoves.Add(move);
                                }
                            }
                        }

                        if (piece == ePieceType.X || piece == ePieceType.K)
                        {
                            if (row - 1 >= 0 && col - 1 >= 0 &&
                                io_Grid.GetPieceAt(row - 1, col - 1) == ePieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row - 1, col - 1);

                                optionalMoves.Add(move);
                            }

                            if (row - 1 >= 0 && col + 1 < i_Size &&
                                io_Grid.GetPieceAt(row - 1, col + 1) == ePieceType.None)
                            {
                                string move = ConvertStepToString(row, col, row - 1, col + 1);

                                optionalMoves.Add(move);
                            }

                            if (piece == ePieceType.K)
                            {
                                if (row + 1 < i_Size && col - 1 >= 0 &&
                                    io_Grid.GetPieceAt(row + 1, col - 1) == ePieceType.None)
                                {
                                    string move = ConvertStepToString(row, col, row + 1, col - 1);
                                    optionalMoves.Add(move);
                                }

                                if (row + 1 < i_Size && col + 1 < i_Size &&
                                    io_Grid.GetPieceAt(row + 1, col + 1) == ePieceType.None)
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

        private static List<string> getOptionalEatMoves(ref Grid io_Grid, int i_Size, char i_PlayerSymbol)
        {
            List<string> optionalEatMoves = new List<string>();
            ePieceType regularPiece = ePieceType.None, kingPiece = ePieceType.None;
            ePieceType opponentRegular = ePieceType.None, opponentKing = ePieceType.None;

            if (i_PlayerSymbol == 'O')
            {
                regularPiece = ePieceType.O;
                kingPiece = ePieceType.U;
                opponentRegular = ePieceType.X;
                opponentKing = ePieceType.K;
            }
            else if (i_PlayerSymbol == 'X')
            {
                regularPiece = ePieceType.X;
                kingPiece = ePieceType.K;
                opponentRegular = ePieceType.O;
                opponentKing = ePieceType.U;
            }

            for (int row = 0; row < i_Size; row++)
            {
                for (int col = 0; col < i_Size; col++)
                {
                    if (io_Grid.GetPieceAt(row, col) == regularPiece || io_Grid.GetPieceAt(row, col) == kingPiece)
                    {
                        if (io_Grid.GetPieceAt(row, col) == regularPiece)
                        {
                            if (i_PlayerSymbol == 'O')
                            {
                                addEatMoveIfValid(optionalEatMoves, ref io_Grid, row, col, i_Size,
                                    1, -1, opponentRegular, opponentKing);
                                addEatMoveIfValid(optionalEatMoves, ref io_Grid, row, col, i_Size,
                                    1, 1, opponentRegular, opponentKing);
                            }
                            else
                            {
                                addEatMoveIfValid(optionalEatMoves, ref io_Grid, row, col, i_Size,
                                    -1, -1, opponentRegular, opponentKing);
                                addEatMoveIfValid(optionalEatMoves, ref io_Grid, row, col, i_Size,
                                    -1, 1, opponentRegular, opponentKing);
                            }
                        }

                        if (io_Grid.GetPieceAt(row, col) == kingPiece)
                        {
                            addEatMoveIfValid(optionalEatMoves, ref io_Grid, row, col, i_Size,
                                1, -1, opponentRegular, opponentKing);
                            addEatMoveIfValid(optionalEatMoves, ref io_Grid, row, col, i_Size,
                                1, 1, opponentRegular, opponentKing);
                            addEatMoveIfValid(optionalEatMoves, ref io_Grid, row, col, i_Size,
                                -1, -1, opponentRegular, opponentKing);
                            addEatMoveIfValid(optionalEatMoves, ref io_Grid, row, col, i_Size,
                                -1, 1, opponentRegular, opponentKing);
                        }
                    }
                }
            }

            return optionalEatMoves;
        }

        private static void addEatMoveIfValid(List<string> i_OptionalEatMoves, ref Grid io_Grid,
            int i_Row, int i_Col, int i_Size, int i_RowDir, int i_ColDir,
            ePieceType i_OpponentRegular, ePieceType i_OpponentKing)
        {
            int targetRow = i_Row + i_RowDir * 2;
            int targetCol = i_Col + i_ColDir * 2;
            int middleRow = i_Row + i_RowDir;
            int middleCol = i_Col + i_ColDir;

            if (targetRow >= 0 && targetRow < i_Size && targetCol >= 0 && targetCol < i_Size &&
                (io_Grid.GetPieceAt(middleRow, middleCol) == i_OpponentRegular 
                || io_Grid.GetPieceAt(middleRow, middleCol) == i_OpponentKing) &&
                io_Grid.GetPieceAt(targetRow, targetCol) == ePieceType.None)
            {
                string move = ConvertStepToString(i_Row, i_Col, targetRow, targetCol);
                i_OptionalEatMoves.Add(move);
            }
        }

        public static void PrintScoreBoard(Player i_Player1, Player i_Player2)
        {
            Console.WriteLine($"{Environment.NewLine}Score Board{Environment.NewLine}" +
                $"{makePointsString(i_Player1)}{Environment.NewLine}" +
                $"{makePointsString(i_Player2)}{Environment.NewLine}");
        }

        private static string makePointsString(Player player)
        {
            string playerPointsString = $"{player.m_Name}   ({player.m_Symbol}): {player.m_Points} points.";
            
            return playerPointsString;
        }
    }
}

