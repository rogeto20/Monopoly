﻿using System;

namespace Monopoly
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.init();
        }

        public void init()
        {
            Console.WriteLine("Welcome to Monopoly! Please follow the instructions to set up the game!");
            Board board = Board.GetBoard();
            Game monopoly = new Game(board);
            monopoly.play();
        }

    }
}
