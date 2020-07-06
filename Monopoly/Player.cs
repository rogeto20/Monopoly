using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    class Player
    {
        public string name;
        public List<Property> property;
        public int money;
        public Property positionOnBoard;
        public Board Board;
        public bool isBankrupt;
        public bool inJail;
        public bool jailCard;
        public int jailCount;

        public Player(string name, Board board)
        {
            this.name = name;
            this.property = new List<Property>();
            positionOnBoard = board.Places.places[0];
            this.money = 1500;
            this.Board = board;
            this.jailCount = 0;
        }

        public void toString()
        {
            this.property = this.property.OrderBy(o => o.position).ToList();
            var details = "-----------------------------------------------------\n";
            details += "$$$ = " + this.money + "\nPlace on Board: ---> " + this.positionOnBoard.name;
            if (this.inJail && this.positionOnBoard.name.Equals("Jail"))
            {
                details += "\nCurrently in JAIL!";
            }
            else if (!this.inJail && this.positionOnBoard.name.Equals("Jail"))
            {
                details += "\nJust Visiting";
            }
            details += "\nProperties owned: ";

            if (this.property.Count == 0)
            {
                details += "Does not own any property!";
            }
            else
            {
                for (int i = 0; i < this.property.Count; i++)
                {
                    updateProperty(this.property[i]);
                    details += "\n\t-| " + this.property[i].getDetails();
                }
            }

            details += "\n-----------------------------------------------------";
            Console.WriteLine(details);
        }

        public void declareBankruptcyToBank()
        {
            Console.WriteLine("Bankruptcy declared!");
            this.isBankrupt = true;
            for (int i = 0; i < this.property.Count; i++)
            {
                var prop = this.property[i];
                if (prop is Street)
                {
                    var street = (Street)prop;

                    if (street.houses > 0)
                    {
                        var refund = (street.houses * street.costToBuild) / 2;
                        street.houses = 0;
                        this.money += refund;
                        Console.WriteLine("{0} Houses sold back", street.name);
                    }

                    if (street.hotels > 0)
                    {
                        var refund = (street.hotels * street.costToBuild) / 2;
                        this.money += refund;
                        street.hotels = 0;
                        Console.WriteLine("{0} Hotels sold back", street.name);
                    }
                }
                Console.WriteLine(prop.name + " given back to bank");
                prop.owner = null;
            }
            if (this.money > 0)
            {
                Console.WriteLine(this.money + "put in the center pot");
                Parking.piggyBank += this.money;
            }
            Game.turnNum--;
            Console.WriteLine("\n\n!!!!{0} IS OUT OF THE GAME!!!!", this.name);
        }

        public void declareBankruptcyToPlayer(Player player)
        {
            Console.WriteLine("Bankruptcy declared!");
            this.isBankrupt = true;
            for (int i = 0; i < this.property.Count; i++)
            {
                var prop = this.property[i];
                if (prop is Street)
                {
                    var street = (Street)prop;

                    if (street.houses > 0)
                    {
                        var refund = (street.houses * street.costToBuild) / 2;
                        street.houses = 0;
                        this.money += refund;
                        Console.WriteLine("{0} Houses sold back", street.name);
                    }

                    if (street.hotels > 0)
                    {
                        var refund = (street.hotels * street.costToBuild) / 2;
                        this.money += refund;
                        street.hotels = 0;
                        Console.WriteLine("{0} Hotels sold back", street.name);
                    }
                }
                Console.WriteLine(prop.name + " given back to " + player.name);
                prop.owner = player;
                player.property.Add(prop);
            }
            if (this.money > 0)
            {
                Console.WriteLine(this.money + " given to " + player.name);
                player.money += this.money;
            }
            Game.turnNum--;
            Console.WriteLine("\n\n!!!!{0} IS OUT OF THE GAME!!!!", this.name);
        }

        private void updateProperty(Property prop)
        {
            if (prop is Street)
            {
                var temp = (Street)prop;
                temp.updateRent();
            }
            else if (prop is Railroad)
            {
                var temp = (Railroad)prop;
                temp.updateRent();
            }
            else if (prop is Utility)
            {
                var temp = (Utility)prop;
                temp.updateRent();
            }
        }

        public void build()
        {
            bool done = false;
            var monopoly = new List<Street>();
            for (int i = 0; i < this.property.Count; i++)
            {
                if (property[i] is Street)
                {
                    var prop = (Street)property[i];
                    if (prop.isMonopoly)
                    {
                        monopoly.Add(prop);
                    }
                }
            }
            if (monopoly.Count > 0)
            {

                while (!done)
                {
                    Console.Clear();
                    Console.WriteLine("\t-Monopolized Property-(Balance:${0})", this.money);
                    for (int i = 0; i < monopoly.Count; i++)
                    {
                        Console.WriteLine("\t[" + i + "] {0}\t(Cost to Build:(${1}), Houses:{2}, Hotels:{3})", monopoly[i].name, monopoly[i].costToBuild, monopoly[i].houses, monopoly[i].hotels);
                    }
                    Console.WriteLine("Enter the number of the Street you would like to build");
                    var num = int.Parse(Console.ReadLine());
                    var prop = monopoly[num];
                    addBuilding(prop);
                    Console.WriteLine("Are you done building?(Y/N)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        done = true;
                    }
                }
            }
            else
            {
                Console.WriteLine("You have no monopoly at the moment.\nPress Enter key to continue the game.");
                Console.ReadLine();
            }
        }

        private void addBuilding(Street prop)
        {
            var done = false;
            Console.WriteLine("Building --> {0}", prop.name);
            while (!done)
            {
                var buildings = prop.houses + prop.hotels;
                if (this.money < prop.costToBuild)
                {
                    Console.WriteLine("You have insufficient funds to build");
                }
                else if (this.money > prop.costToBuild && buildings < 4)
                {
                    Console.WriteLine("\t::Add House?(Y/N)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        prop.houses++;
                        this.money -= prop.costToBuild;
                        Console.WriteLine("\t::House Added\n\t::Money Left(${0})", this.money);
                    }
                }
                else if (this.money > prop.costToBuild && buildings < 5)
                {
                    Console.WriteLine("\t::Add Hotel?(Y/N)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        prop.hotels++;
                        this.money -= prop.costToBuild;
                        Console.WriteLine("\t::Hotel Added\n\t::Money Left(${0})", this.money);
                    }
                }
                else if (this.money > prop.costToBuild && buildings == 5)
                {
                    Console.WriteLine("Maximum number of buildings reached");
                    return;
                }

                Console.WriteLine("Are you done building {0}", prop.name);
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    done = true;
                }

            }
            Console.WriteLine("Update = {0}(Houses:{1}, Hotels:{2})", prop.name, prop.houses, prop.hotels);
        }

        public void action()
        {
            Console.WriteLine("Would you like to build?(Y/N)");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                build();
                Console.Clear();
                this.toString();
            }
        }
    }
}
