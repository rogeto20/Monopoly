using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class Game
    {
        private Board board;
        private Player turn;
        public static int turnNum;
        private List<Player> players;

        public Game(Board board)
        {
            this.board = board;
            this.players = this.board.Players;
            this.board.game = this;
            turnNum = 0;
        }

        public void play()
        {
            this.start();
            Console.WriteLine("The game has started!");
            while (!hasWinner())
            {
                changeTurn();
                turn.toString();
                Console.WriteLine("Press->\n'A' - Player Actions(Build|Trade)\n'P' - Play Round\n'X' - Declare Bankruptcy");
                var Key = Console.ReadKey().Key;
                var selected = false;
                while (!selected)
                {
                    if(Key == ConsoleKey.A)
                    {
                        turn.action();
                        playRound();
                        selected = true;
                    }
                    else if(Key == ConsoleKey.P)
                    {
                        playRound();
                        selected = true;
                    }
                    else if (Key == ConsoleKey.X)
                    {
                        turn.declareBankruptcyToBank();
                        selected = true;
                    }
                    else
                    {
                        Console.WriteLine("Press->\n'A' - Player Actions(Build|Trade)\n'P' - Play Round");
                        Key = Console.ReadKey().Key;
                    }
                }
            }

        }

        private void playRound()
        {
            if (!turn.inJail)
            {
                
                Console.WriteLine("Press 'r' to roll the dice");
                var Key = Console.ReadKey().Key;
                var selected = false;
                while (!selected)
                {
                    if (Key == ConsoleKey.R)
                    {
                        roll(0);
                        selected = true;
                    }
                    else
                    {
                        Console.WriteLine("INVALID INPUT!\nPress 'r' to roll the dice");
                        Key = Console.ReadKey().Key;
                    }
                }
            }
            else
            {
                Console.WriteLine("Currently in jail. Press-> \n\t'r':roll the dice\n\t'p':pay 50 to get out");
                if (turn.jailCard == true)
                {
                    Console.WriteLine("\t'f':use get out of jail free card");
                }

                var Key = Console.ReadKey().Key;
                var selected = false;
                while (!selected)
                {
                    if (Key == ConsoleKey.R)
                    {
                        rollForJail(turn.jailCount);
                        selected = true;
                    }
                    else if (Key == ConsoleKey.P)
                    {
                        var amount = turn.money;
                        turn.money -= 50;
                        Console.WriteLine("\nNew Balance = ${0}", turn.money);
                        turn.inJail = false;
                        Jail.prisoners.Remove(turn);
                        turn.jailCount = 0;
                        selected = true;
                    }
                    else if (Key == ConsoleKey.F && turn.jailCard == true)
                    {
                        getOutOfJailFree(0);
                        selected = true;
                    }
                    else
                    {
                        Console.WriteLine("INVALID INPUT!");
                        Console.WriteLine("Currently in jail. Press-> \n\t'r':roll the dice\n\t'p':pay 50 to get out");
                        if (turn.jailCard == true)
                        {
                            Console.WriteLine("\t'f':use get out of jail free card");
                        }
                        Key = Console.ReadKey().Key;
                    }
                }
            }
        }

        private bool hasWinner()
        {
            bool winner = false;
            for(int i = 0; i<players.Count; i++)
            {
                if (players[i].isBankrupt)
                {
                    this.players.Remove(players[i]);
                }
            }
            if(players.Count == 1)
            {
                Console.WriteLine("{0} has won the game!", players[0].name);
                Console.WriteLine("Press any key to see their standings!");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("WINNER = {0}", players[0].name);
                players[0].toString();
                Console.WriteLine("Press 'Enter' to end the game");
                Console.ReadLine();
                winner = true;
                return winner;
            }
            else
            {
                return winner;
            }
        }

        private void getOutOfJailFree(int v)
        {
            turn.jailCard = false;
            turn.inJail = false;
            Jail.prisoners.Remove(turn);
            turn.jailCount = 0;
            Console.WriteLine("You have used your get out of jail free card. You will roll the dice in your next round");
        }

        private void rollForJail(int v)
        {
            Random rand = new Random();

            var roll1 = rand.Next(1, 7);
            var roll2 = rand.Next(1, 7);
            

            if (roll1 == roll2)
            {
                Console.WriteLine("\nYou rolled a double {0}, you are out of jail.",roll1);
                move(roll1 + roll2);
                turn.inJail = false;
                Jail.prisoners.Remove(turn);
                turn.jailCount = 0;
            }
            else
            {
                turn.jailCount++;
                Console.WriteLine("\nYou rolled '{0}' & '{1}'", roll1, roll2);
                Console.WriteLine("Try No. {0}", turn.jailCount);
                if(turn.jailCount == 3 && turn.money>=50)
                {
                    Console.WriteLine("You paid the $50 fine. You are Free!");
                    var amount = turn.money;
                    turn.money -= 50;
                    Parking.piggyBank += 50;
                    Console.WriteLine("Bank Balance = {0}", turn.money);
                    turn.inJail = false;
                    Jail.prisoners.Remove(turn);
                    turn.jailCount = 0;
                } else if (turn.jailCount == 3 && turn.money < 50)
                {
                    Console.WriteLine("YOU ARE OUT OF TRYS AND MONEY! IT'S A LIFETIME IN JAIL FOR YOU!");
                    turn.declareBankruptcyToBank();
                }
            }
            
        }

        private void roll(int count)
        {
            Random rand = new Random();

            var roll1 = rand.Next(1, 7);
            var roll2 = rand.Next(1, 7);
            

            if (roll1 == roll2)
            {
                count++;
                if(count == 3)
                {
                    Console.WriteLine("\n{0} Rolled a double {1} times", turn.name, count);
                    GoToJail.sendToJail(turn);
                }
                else 
                {
                    Console.WriteLine("\nYou rolled doubles of '{0}'. {1} - double", roll1, count);
                    move(roll1+roll2);
                    if (turn.isBankrupt)
                    {
                        return;
                    }
                    if (turn.inJail)
                    {
                        Console.WriteLine("\n>>>R: Roll again to get out of jail");

                        var selected = false;
                        var Key = Console.ReadKey().Key;
                        while (!selected)
                        {
                            if (Key == ConsoleKey.R)
                            {
                                rollForJail(turn.jailCount);
                                return;
                            }
                            else
                            {
                                Console.WriteLine("\n>>>R: Roll again to get out of jail");
                                Key = Console.ReadKey().Key;
                            }
                        }

                    }
                    else
                    {
                    Console.WriteLine("\n>>>R: Roll again");

                    var selected = false;
                    var Key = Console.ReadKey().Key;
                    while (!selected)
                    {
                        if (Key == ConsoleKey.R)
                        {
                            roll(count);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("\n>>>R: Roll again");
                            Key = Console.ReadKey().Key;
                        }
                    }

                    }
                }
                
            }else
            {
                Console.WriteLine("\nYou rolled '{0}' & '{1}'", roll1, roll2);
                move(roll1 + roll2);
            }
        }

        private void changeTurn()
        {
            Console.WriteLine("\n-----------------------------------------------------\nPlease press ENTER to start new turn");
            Console.ReadLine();
            Console.Clear();

            turnNum++;
            if(turnNum == players.Count)
            {
                turnNum -= players.Count;
            }
            
            turn = players[turnNum];
            Console.WriteLine("{0}'s turn", turn.name);
        }

        public void move(int roll)
        {
            var position = turn.positionOnBoard.position;
            
            position += roll;
            if(position > board.Places.places.Count)
            {
                position = position - board.Places.places.Count;
                passGo();
            } else if (position == board.Places.places.Count)
            {
                position = position - board.Places.places.Count;
            } else if (position<0)
            {
                position = position + board.Places.places.Count;
            }
            turn.positionOnBoard = board.Places.places[position];
           Console.WriteLine("Moved {0} spots to ->\n\t::{1}", roll, turn.positionOnBoard.name);
           checkPlace();
        }

        private void passGo()
        {
            turn.money += 200;
            Console.WriteLine("You passed Go (+$200) \nNew balance = ${0}", turn.money);
        }

        private void checkPlace()
        {
            turn.positionOnBoard.action(turn);
        }

        private void start()
        {
            var players = "";
            var startMessage = new StringBuilder("\n");
            for (int i =0; i<this.board.Players.Count; i++)
            {
                players+= (i+1)+": "+this.board.Players[i].name + "\n";
            }
            startMessage.AppendFormat("Game setup successfully! \nThere are {0} players. \n{1}", this.board.Players.Count,players );
            Console.WriteLine(startMessage);
            this.turn = this.players[turnNum];
            
        }


    }
}
