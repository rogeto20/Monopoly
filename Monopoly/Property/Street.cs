using System;
using System.Collections.Generic;

namespace Monopoly
{
    class Street : Property
    {

        public int buyingCost;
        public int costToBuild;
        public int[] rentPerBuilding = new int[6];
        public int rent;
        public int defaultRent;
        public int houses;
        public int hotels;
        public int count;
        public bool isMonopoly;


        public Street(string[] details) : base(details)
        {
            this.buyingCost = int.Parse(details[4]);
            this.costToBuild = int.Parse(details[5]);
            this.houses = 0;
            this.hotels = 0;
            this.owner = null;
            this.count = int.Parse(details[12]);
            for (int i = 0; i < rentPerBuilding.Length; i++)
            {
                this.rentPerBuilding[i] = int.Parse(details[i + 6]);
            }
            this.rent = rentPerBuilding[houses + hotels];
            this.defaultRent = rentPerBuilding[houses + hotels];
        }

        public override void action(Player turn)
        {
            if (this.owner == null)
            {
                Console.WriteLine("\t::(Color - {0}, No Owner, Buying Cost = {1})", this.color, this.buyingCost);
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
                if (turn.money > this.rent)
                {
                    turn.money -= this.rent;
                    this.owner.money += this.rent;
                    Console.WriteLine("You have ${0} left", turn.money);
                }
                else
                {
                    Console.WriteLine("You don't have enough money");
                    turn.declareBankruptcyToPlayer(this.owner);
                }

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
            var prop = new List<Street>();
            prop.Add(this);

            for (int i = 0; i < places.Count; i++)
            {
                if (this.color == places[i].color)
                {
                    var temp = (Street)places[i];
                    if (this.owner == temp.owner && this.name != temp.name)
                    {
                        prop.Add(temp);
                    }
                }
            }
            if (this.hotels != 0)
            {
                this.rent = this.rentPerBuilding[this.houses + this.hotels];
            }
            else if (this.houses > 0)
            {
                this.rent = this.rentPerBuilding[this.houses];
            }
            else if (prop.Count == this.count)
            {
                for (int i = 0; i < prop.Count; i++)
                {
                    prop[i].rent = prop[i].defaultRent * 2;
                    prop[i].isMonopoly = true;
                }
            }
            else
            {
                for (int i = 0; i < prop.Count; i++)
                {
                    prop[i].rent = prop[i].defaultRent;
                    prop[i].isMonopoly = false;
                }
            }
        }

        public override string getDetails()
        {
            var details = "";
            if (this.isMonopoly)
            {
                details = "(M)" + this.name + "(Color - " + this.color + ")(Rent =" + this.rent + ") ";
            }
            else
            {
                details = this.name + "(Color - " + this.color + ")(Rent =" + this.rent + ") ";
            }
            return details;
        }

    }
}
