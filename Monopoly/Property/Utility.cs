using System;
using System.Collections.Generic;

namespace Monopoly
{
    class Utility : Property
    {
        public int buyingCost;
        public int rent;
        public int defaultRent;
        public int count;
        public bool isMonopoly;


        public Utility(string[] details) : base(details)
        {
            this.buyingCost = int.Parse(details[4]);
            this.owner = null;
            this.count = int.Parse(details[12]);
            this.rent = int.Parse(details[6]);
            this.defaultRent = rent;
        }

        public override void action(Player turn)
        {
            if (this.owner == null)
            {
                Console.WriteLine("\t::(Type - {0}, No Owner, Buying Cost = {1})", this.color, this.buyingCost);
                buy(turn);
            }
            else
            {
                if (this.owner != turn)
                {
                    Console.WriteLine("\t::(Owner-> {0}, Rent ->{1})", this.owner.name, this.rent);
                    payOwner(turn);
                }
                else
                {
                    Console.WriteLine("\t::(Yours)");
                }
            }

        }

        public void payOwner(Player turn)
        {
            var rent = 0;
            var roll = 0;
            Console.WriteLine("Property is a utility. Press 'r' to roll dice");
            if (Console.ReadKey().Key == ConsoleKey.R)
            {
                roll = rollDice();
                rent = this.rent * roll;
            }

            Console.WriteLine("Amount due-> (Rent:{0}) * (Roll:{1}) = {2}", this.rent, roll, rent);
            Console.WriteLine("Press:\n\t'P' -> Pay\n\t'B'->Declare bankruptcy");
            if (Console.ReadKey().Key == ConsoleKey.P)
            {
                turn.money -= rent;
                this.owner.money += rent;
                Console.WriteLine("You have ${0} left", turn.money);
            }
            else if (Console.ReadKey().Key == ConsoleKey.B)
            {
                turn.declareBankruptcyToPlayer(this.owner);
            }

        }

        public int rollDice()
        {

            Random rand = new Random();
            var roll1 = rand.Next(1, 7);
            var roll2 = rand.Next(1, 7);

            return roll1 + roll2;
        }

        public void buy(Player turn)
        {
            Console.WriteLine("Press-> \n\t'B':Buy \n\t'X':Don't buy");
            if (Console.ReadKey().Key == ConsoleKey.B)
            {
                purchase(turn);
            }
            else if (Console.ReadKey().Key == ConsoleKey.X)
            {
                return;
            }
        }

        private void purchase(Player turn)
        {
            if (turn.money > this.buyingCost)
            {
                turn.property.Add(this);
                this.owner = turn;
                turn.money -= this.buyingCost;
                updateRent();
                Console.WriteLine("\nYou now own " + this.name);
            }
            else
            {
                Console.WriteLine("You have ${0} not enough to buy this property", turn.money);
            }
        }
        public void updateRent()
        {
            var places = this.owner.Board.Places.places;
            var prop = new List<Utility>();
            prop.Add(this);

            for (int i = 0; i < places.Count; i++)
            {
                if (this.color == places[i].color)
                {
                    var temp = (Utility)places[i];
                    if (this.owner == temp.owner && this.name != temp.name)
                    {
                        prop.Add(temp);
                    }
                }
            }

            if (prop.Count == this.count)
            {
                for (int i = 0; i < prop.Count; i++)
                {
                    prop[i].isMonopoly = true;
                    prop[i].rent = 10;
                }
            }
            else
            {
                for (int i = 0; i < prop.Count; i++)
                {
                    prop[i].isMonopoly = false;
                    prop[i].rent = prop[i].defaultRent;
                }
            }
        }

        public override string getDetails()
        {
            var details = "";
            if (this.isMonopoly)
            {
                details = "(M)" + this.name + "(" + this.color + ")(Rent =" + this.rent + ") ";
            }
            else
            {
                details = this.name + "(" + this.color + ")(Rent =" + this.rent + ") ";
            }
            return details;
        }
    }
}
