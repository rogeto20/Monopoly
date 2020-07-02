using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class Card
    {
        public string type;
        public string category;
        public string name;
        public string desc;
        public int effect;
        public string tag;

        public Card(string details)
        {
            var list = details.Split('\t');
            tag = list[1];
            type = list[2];
            category = list[3];
            name = list[4];
            desc = list[5];
            effect= int.Parse(list[6]);
        }

        public void action(Player turn)
        {
            Console.WriteLine("\t::{0}\n\t::{1}",this.name, this.desc);
            switch (this.category)
            {
                case "move":
                    this.move(turn);
                    break;
                case "money":
                    this.money(turn);
                    break;
                case "item":
                    this.item(turn);
                    break;
            }
        }

        private void move(Player turn)
        {
            var position = turn.positionOnBoard.position;
            var roll = 0;
            
            if (this.tag.Contains("GOTOJ"))
            {
                GoToJail.sendToJail(turn);
                return;
            }
            else if (this.tag.Contains("ADVNR"))
            {
                int [] rails = {5,15,25,35};
                roll = checkNearest(position, rails);
            }
            else if (this.tag.Contains("ADVNU"))
            {
                int[] util = {12,28};
                roll = checkNearest(position, util);
            }
            else
            {
                roll = this.effect - position;
                if (this.effect < 0)
                {
                    turn.Board.game.move(roll);
                    return;
                }
            }

            if (roll < 0)
            {
                roll = 40 + roll;
            }
            turn.Board.game.move(roll);
        }

        private int checkNearest(int position, int[] place)
        {
            var num = 0;

            while( num < place.Length && position> place[num] )
            {
                num++;
            }
            if(num >= place.Length)
            {
                num = 0;
            }
            return place[num] - position;
        }

        private void money(Player turn)
        {
            if (this.tag.Contains("GENRP"))
            {
                makeRepairs(turn);
            }else if (this.tag.Contains("CHBRD"))
            {
                chairman(turn);
            }
            else if (this.tag.Contains("GRDON"))
            {
                collect(turn);
            }
            else if (this.tag.Contains("YBDAY"))
            {
                collect(turn);
            }
            else if (this.tag.Contains("STRRP"))
            {
                makeRepairs(turn);
            }
            else
            {
                if(this.effect > 0)
                {
                    turn.money += this.effect;
                    if (this.effect < 0)
                    {
                        Parking.piggyBank -= this.effect;
                    }
                    Console.WriteLine("You have ${0} left", turn.money);
                }
                else if(this.effect < 0)
                {
                    Console.WriteLine("Press:\n\t'P' -> Pay fee.\n\t'B'-> Declare bankruptcy");
                    if (Console.ReadKey().Key == ConsoleKey.P)
                    {
                        var fee = this.effect * -1;
                        if (turn.money > fee)
                        {
                            turn.money -= fee;
                            Parking.piggyBank += fee;
                            Console.WriteLine("You have ${0} left", turn.money);
                        }
                        else
                        {
                            Console.WriteLine("You don't have enough money and are now bankrupt to the bank");
                            turn.declareBankruptcyToBank();
                        }

                    }
                    else if (Console.ReadKey().Key == ConsoleKey.B)
                    {
                        turn.declareBankruptcyToBank();
                    }
                }
                
            }
        }

        
        private void collect(Player turn)
        {
            var players = turn.Board.Players;
            foreach (Player player in players)
            {
                if (player.money > this.effect)
                {
                    player.money -= this.effect;
                    turn.money += this.effect;
                }
                else
                {
                    Console.WriteLine("\t::{0} declared bankruptcy to you", player.name);
                    player.declareBankruptcyToPlayer(turn);
                }
                
            }
            Console.WriteLine("\t::New Balance: ${0}", turn.money);

        }

        private void chairman(Player turn)
        {
            var players = turn.Board.Players;
            var money = (players.Count-1) * 50;
            Console.WriteLine("\t::{0} to be paid in total.", money);
            if (money < turn.money)
            {
                foreach(Player player in players)
                {
                    player.money += this.effect;
                    turn.money -= this.effect;
                }
                Console.WriteLine("\t::New Balance: ${0}", turn.money);
            }
            else
            {
                Console.WriteLine("\t::You have insufficient funds! Bankrupt to the bank");
                turn.declareBankruptcyToBank();
            }
            
        }

        private void makeRepairs(Player turn)
        {
            var prop = turn.property;
            var repairs = 0;
            var houses = 0;
            var hotels = 0;
            var hotRepairs = 0;
            var houRepairs = 0;

            if (this.tag.Equals("STRRP"))
            {
                hotRepairs = 100;
                houRepairs = 25;
            } else if (this.tag.Equals("GENRP"))
            {
                hotRepairs = 115;
                houRepairs = 40;
            }


            for (int i = 0; i<prop.Count; i++)
            {
                if(prop[i] is Street)
                {
                    var street = (Street)prop[i];
                    if (street.hotels > 0)
                    {
                        repairs += hotRepairs;
                        hotels++;
                    }
                    else if(street.houses > 0)
                    {
                        repairs += houRepairs * street.houses;
                        houses+=street.houses;
                    }
                }
            }
            if(repairs == 0)
            {
                Console.WriteLine("\t::No Houses or Hotels to Repair!");
                return;
            }
            else
            {
                Console.WriteLine("\t::Houses = {0}, Hotels = {1}\n\t::Repairs = {2}", houses, hotels, repairs);
                Console.WriteLine("Press:\n\t'P' -> Pay repairs.\n\t'B'-> Declare bankruptcy");
                if (Console.ReadKey().Key == ConsoleKey.P)
                {
                    if (turn.money > repairs)
                    {
                        turn.money -= repairs;
                        Parking.piggyBank += repairs;
                        Console.WriteLine("You have ${0} left", turn.money);
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money and are now bankrupt to the bank");
                        turn.declareBankruptcyToBank();
                    }
                    
                }
                else if (Console.ReadKey().Key == ConsoleKey.B)
                {
                    turn.declareBankruptcyToBank();
                }
            }
        }

        private void item(Player turn)
        {
            turn.jailCard = true;
        }
    }
}
