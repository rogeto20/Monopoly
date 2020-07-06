using System;

namespace Monopoly
{
    class Go : Property
    {
        public Go(string[] details) : base(details)
        {

        }

        public override void action(Player turn)
        {
            Console.WriteLine("\t::You get $400 more to spend!");
            turn.money += 400;
        }

        public override string getDetails()
        {
            throw new NotImplementedException();
        }
    }
}
