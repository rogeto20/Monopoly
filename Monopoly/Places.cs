using System.Collections.Generic;
using System.IO;

namespace Monopoly
{
    class Places
    {
        public List<Property> places;

        public Places()
        {
            this.places = getPlaces();
        }

        private List<Property> getPlaces()
        {
            List<Property> temp = new List<Property>();
            string path = @"..\..\Property\Lists\List.txt";
            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] details = lines[i].Split(',');

                temp.Add(placeBuilder(details));
            }

            return temp;
        }

        private Property placeBuilder(string[] details)
        {
            Property place = null;

            if (details[1].Equals("Go"))
            {
                place = new Go(details);
            }
            else if (details[1].Equals("Street"))
            {
                place = new Street(details);
            }
            else if (details[1].Equals("Chest"))
            {
                place = new Chest(details);
            }
            else if (details[1].Equals("Tax"))
            {
                place = new Tax(details);
            }
            else if (details[1].Equals("Railroad"))
            {
                place = new Railroad(details);
            }
            else if (details[1].Equals("Utility"))
            {
                place = new Utility(details);
            }
            else if (details[1].Equals("Chance"))
            {
                place = new Chance(details);
            }
            else if (details[1].Equals("Jail"))
            {
                place = new Jail(details);
            }
            else if (details[1].Equals("GoToJail"))
            {
                place = new GoToJail(details);
            }
            else if (details[1].Equals("Parking"))
            {
                place = new Parking(details);
            }
            return place;
        }
    }
}
