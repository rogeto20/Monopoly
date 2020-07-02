using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class Jail : Property
    {
        public static List<Player> prisoners;
        public Jail(string[] details) : base(details)
        {
            prisoners = new List<Player>();
        }

        public override void action(Player turn)
        {
            if (turn.inJail)
            {
                Console.WriteLine("It's behind the bars for you!");
            }
            else
            {
                Console.WriteLine("Just visiting:");
                if(prisoners.Count == 0)
                {
                    Console.WriteLine("Prison is currently empty");
                }
                else
                {
                    for(int i = 0; i < prisoners.Count; i++)
                    {
                        Console.WriteLine(prisoners[i].name);
                    }
                    Console.WriteLine("Who are currently locked up! They look thin, prison life isn't treating them well.");
                }
                
            }
        }

        public override string getDetails()
        {
            throw new NotImplementedException();
        }
    }
}
