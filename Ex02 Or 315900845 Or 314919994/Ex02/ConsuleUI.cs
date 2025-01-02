using System;

namespace Ex02
{
    internal class ConsuleUI
    {
        public static void GameStartMenu()
        {
            Console.WriteLine("Welcome to Checkers!");
            string player1Name = Player.GetValidatePlayerName("Player 1");
            Player player1 = new Player(player1Name, 'X', 0);
            int boardSize = Board.SetBoardSize();
            ePlayerType playerModeChoice = Board.GetPlayerOrComputerGame();
            Player player2 = Player.GetPlayerOrComputer(playerModeChoice);

            Game game = new Game(player1, player2, boardSize, playerModeChoice);
            game.Start();
        }
    }
}
