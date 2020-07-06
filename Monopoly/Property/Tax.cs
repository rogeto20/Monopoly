using System;

namespace Monopoly
{
    class Tax : Property
    {
        private int tax;
        public Tax(string[] details) : base(details)
        {
            this.tax = int.Parse(details[6]);
        }

        public override void action(Player turn)
        {
            Console.WriteLine("\t::Tax = {0}", this.tax);
            Console.WriteLine("Press:\n\t::P: Pay taxes\n\t::B: Declare bankruptcy");
            if (Console.ReadKey().Key == ConsoleKey.P)
            {
                turn.money -= this.tax;
                Parking.piggyBank += this.tax;
            }
            else if (Console.ReadKey().Key == ConsoleKey.B)
            {
                Console.WriteLine("\n" + turn.name + " is now bankrupt and is out of the game!");
                turn.isBankrupt = true;
            }
        }

        public override string getDetails()
        {
            throw new NotImplementedException();
        }
    }
}
