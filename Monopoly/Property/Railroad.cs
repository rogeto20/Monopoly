using System;
using System.Collections.Generic;

namespace Monopoly
{
    class Railroad : Property
    {

        public int buyingCost;
        public int defaultRent;
        public int rent;
        public int count;

        public Railroad(string[] details) : base(details)
        {
            this.buyingCost = int.Parse(details[4]);
            this.owner = null;
            this.count = int.Parse(details[12]);
            this.rent = int.Parse(details[6]);
            this.defaultRent = this.rent = int.Parse(details[6]);
        }

        public override void action(Player turn)
        {
            if (this.owner == null)
            {
                Console.WriteLine("\t::(No Owner, Buying Cost = {0})", this.buyingCost);
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
            Console.WriteLine("Press:\n\t'P' -> Pay rent.\n\t'B'-> Declare bankruptcy");
            if (Console.ReadKey().Key == ConsoleKey.P)
            {
                turn.money -= this.rent;
                this.owner.money += this.rent;
                Console.WriteLine("You have ${0} left", turn.money);
            }
            else if (Console.ReadKey().Key == ConsoleKey.B)
            {
                turn.declareBankruptcyToPlayer(this.owner);
            }
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
            var prop = new List<Railroad>();
            prop.Add(this);

            for (int i = 0; i < places.Count; i++)
            {
                if (this.color == places[i].color)
                {
                    var temp = (Railroad)places[i];
                    if (this.owner == temp.owner && this.name != temp.name)
                    {
                        prop.Add(temp);
                    }
                }
            }

            for (int i = 0; i < prop.Count; i++)
            {
                prop[i].rent = defaultRent * prop.Count;
            }
        }

        public override string getDetails()
        {
            var details = this.name + "(Type = " + this.color + ")(Rent =" + this.rent + ")";
            return details;
        }
    }
}
