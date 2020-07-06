using System;

namespace Monopoly
{
    class Parking : Property
    {
        public static int piggyBank;
        public Parking(string[] details) : base(details)
        {
            piggyBank = 500;
        }

        public override void action(Player turn)
        {
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$\nCenter pot = $" + piggyBank);
            turn.money += piggyBank;
            piggyBank = 500;
            Console.WriteLine("The center pot is all yours!\nNew balance = $" + turn.money);
            Console.WriteLine("There's nothing like free parking AMIRITE?");
        }

        public override string getDetails()
        {
            throw new NotImplementedException();
        }
    }
}
