using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    abstract class Property
    {
        public  string space;
        public  string name;
        public int position;
        public string color;
        public Player owner;
     
        public Property(string [] details)
        {
            name = details[0];
            space = details[1];
            color = details[2];
            position = int.Parse(details[3]);
        }

        public abstract void action(Player turn);
        public abstract string getDetails();
    }
}
