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
                Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol}):");

                Console.WriteLine($"{currentPlayer.Name}, enter your move (fromRow fromCol > toRow toCol) or 'Q' to quit:");
                string input = Console.ReadLine();
                MakeMove();
                if (quit)
                {
                    Console.WriteLine($"{currentPlayer.Name} quit the game. {(_player1 == currentPlayer ? _player2.Name : _player1.Name)} wins!");
                    break;
                }

                //_board.MovePiece(fromRow, fromCol, toRow, toCol);

                currentPlayer = currentPlayer == _player1 ? _player2 : _player1;
            }
        }
    }
}
