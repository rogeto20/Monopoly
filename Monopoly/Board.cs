using System;
using System.Collections.Generic;

namespace Monopoly
{
    class Board
    {
        private static Board board;
        public Game game;
        public Places Places;
        public List<Player> Players = new List<Player>();

        private Board()
        {
            Places = new Places();
            Console.WriteLine(">Please enter the number of players!");
            var num = int.Parse(Console.ReadLine());
            for (int i = 0; i < num; i++)
            {
                Console.WriteLine(">>Please enter the name of player:" + (i + 1));
                var name = Console.ReadLine();
                this.Players.Add(new Player(name, this));
            }
            Console.WriteLine("**Players added to the game**");
        }

        public static Board GetBoard()
        {
            if (board == null)
            {
                board = new Board();
                return board;
            }
            else
            {
                return board;
            }
        }
    }
}
