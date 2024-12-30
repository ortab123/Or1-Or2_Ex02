using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex02.Game;

namespace Ex02
{
    public class ComputerPlayer : Player
    {

        public ComputerPlayer(string name, char symbol) : base(name, symbol)
        {

        }


        public override MoveMade MakeMove(Board.PieceType[,] i_grid, Player i_player)
        {
            char playerSymbol = i_player.Symbol;
            int size = i_grid.GetLength(0);
            MoveMade isMoveMade = new MoveMade();

            List<string> optionalEatMoves = GetOptionalEatMoves(i_grid, size, playerSymbol);
            List<string> optionalMoves = GetOptionalMoves(i_grid, size, playerSymbol);
            int randomIndex;
            string nextMoveString;
            Random random = new Random();

            // need a random number to choose from optional moves

            while(optionalEatMoves.Count > 0)
            {
                randomIndex = random.Next(0, optionalEatMoves.Count);
                nextMoveString = optionalEatMoves[randomIndex];


                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true); // Discard all leftover key inputs
                }

                // Need to press "Enter" to reavel computer move - wait for player
                Console.WriteLine("Computer's Turn (press 'enter' to see it's move)");
                Console.ReadLine();


                i_grid = UpdatingBoard(nextMoveString, i_grid, size, playerSymbol);
                Board.PrintBoard(i_grid);
                Console.WriteLine($"Computer's move was ({i_player.Symbol}): {nextMoveString}");
                isMoveMade = MoveMade.Done;

                // Recalculate eat moves after the current move
                optionalEatMoves = GetOptionalEatMoves(i_grid, size, playerSymbol);

                if (optionalEatMoves.Count > 0)
                {
                    continue;
                }

                break;
            }

            if (optionalEatMoves.Count == 0 && optionalMoves.Count > 0)
            {
                randomIndex = random.Next(0, optionalMoves.Count);
                nextMoveString = optionalMoves[randomIndex];

                i_grid = UpdatingBoard(nextMoveString, i_grid, size, playerSymbol);
                Board.PrintBoard(i_grid);
                Console.WriteLine($"Computer's move was ({i_player.Symbol}): {nextMoveString}");
                isMoveMade = MoveMade.Done;
            }

            if (optionalMoves.Count == 0 && optionalEatMoves.Count == 0)
            {
                isMoveMade = MoveMade.None;
            }

            return isMoveMade;
        }


    }
}
