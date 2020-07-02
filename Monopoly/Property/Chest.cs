using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class Chest : Property
    {
        public static List<Card> cards = new List<Card>();
        public static bool made = false;
        public static int counter=0;

        public Chest(string[] details) : base(details)
        {
            if (!made)
            {
                string path = @"..\..\Property\Lists\CommunityChest.txt";
                string[] lines = File.ReadAllLines(path);

                for (int i = 0; i < lines.Length; i++)
                {
                    cards.Add(new ChestCard(lines[i]));
                }
                made = true;
                Shuffle();
            }
            
        }
        public void Shuffle()
        {
            Random rand = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public override void action(Player turn)
        {
            if(counter < cards.Count)
            {
                cards[counter].action(turn);
                counter++;
            }
            else
            {
                counter = 0;
                Shuffle();
                cards[counter].action(turn);
                counter++;
            }
        }
        
        public override string getDetails()
        {
            throw new NotImplementedException();
        }
    }
}
