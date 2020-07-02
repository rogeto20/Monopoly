using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class GoToJail : Property
    {
        public GoToJail(string[] details) : base(details)
        {
            
        }

        public override void action(Player turn)
        {
            sendToJail(turn);
        }


        public static void sendToJail(Player turn)
        {
            var position = 10;
            turn.positionOnBoard = turn.Board.Places.places[position];
            turn.inJail = true;
            Jail.prisoners.Add(turn);
            Console.WriteLine("\nMoved to {0}", turn.positionOnBoard.name);
        }

        public override string getDetails()
        {
            throw new NotImplementedException();
        }
    }
}
