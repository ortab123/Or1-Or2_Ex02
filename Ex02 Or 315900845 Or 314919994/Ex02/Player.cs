using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02
{
    public abstract class Player
    {
        public string Name { get; set; }
        public char Symbol { get; set; } // 'X' or 'O'

        public Player(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }
}

