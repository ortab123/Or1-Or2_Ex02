using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ex02.Game;

namespace Ex02
{
    public abstract class Player
    {
        public enum MoveMade
        {
            None, // ערך ברירת מחדל
            Quit,
            Done,
        }

        public string Name { get; set; }
        public char Symbol { get; set; } // 'X' or 'O'

        public Player(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
        }

        public abstract MoveMade MakeMove(Board.PieceType[,] i_grid, Player i_player);

    }
}

