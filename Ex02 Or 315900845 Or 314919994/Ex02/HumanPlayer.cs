using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ex02.Game;

namespace Ex02
{
    public class HumanPlayer : Player
    {

        public HumanPlayer(string name, char symbol) : base(name, symbol) { }

        public override MoveMade MakeMove(Board.PieceType[,] i_grid, Player i_player)
        {

            int size = i_grid.GetLength(0);
            string validationResult = ValidateMove(Console.ReadLine());

            MoveMade isMoveMade = new MoveMade();


            if (validationResult == "Q")
            {
                isMoveMade = MoveMade.Quit;
            }

            char playerSymbol = i_player.Symbol;
            List<string> optionalEatMoves = GetOptionalEatMoves(i_grid, size, playerSymbol);
            List<string> optionalMoves = GetOptionalMoves(i_grid, size, playerSymbol);

            bool isEat = false;

            // Handle eating moves
            while (optionalEatMoves.Count > 0 && isMoveMade != MoveMade.Quit)
            {
                // Check if the entered move is a valid eating move
                if (optionalEatMoves.Contains(validationResult))
                {
                    isEat = true;
                    i_grid = UpdatingBoard(validationResult, i_grid, size, playerSymbol);
                    Board.PrintBoard(i_grid);
                    Console.WriteLine($"{i_player.Name}'s move was ({i_player.Symbol}): {validationResult}");
                    isMoveMade = MoveMade.Done;

                    // Recalculate eat moves after the current move
                    optionalEatMoves = GetOptionalEatMoves(i_grid, size, playerSymbol);
                    if (optionalEatMoves.Count > 0)
                    {
                        Console.WriteLine("You have another eating move. Enter your next move:");
                        validationResult = ValidateMove(Console.ReadLine());

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
                    validationResult = ValidateMove(Console.ReadLine());

                    if (validationResult == "Q")
                    {
                        isMoveMade = MoveMade.Quit;
                    }
                }
            }

            // Handle regular moves if no eating moves are available
            while (!isEat && optionalMoves.Count > 0 && isMoveMade != MoveMade.Quit)
            {
                if (optionalMoves.Contains(validationResult))
                {
                    i_grid = UpdatingBoard(validationResult, i_grid, size, playerSymbol);
                    Board.PrintBoard(i_grid);
                    Console.WriteLine($"{i_player.Name}'s move was ({i_player.Symbol}) : {validationResult}");
                    isMoveMade = MoveMade.Done;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid move. Please enter a valid move.");
                    validationResult = ValidateMove(Console.ReadLine());

                    if (validationResult == "Q")
                    {
                        isMoveMade = MoveMade.Quit;
                        break;
                    }
                }
            }

            if (optionalMoves.Count == 0 && optionalEatMoves.Count == 0)
            {
                isMoveMade = MoveMade.None;
            }

            return isMoveMade;
        }


    }
}
