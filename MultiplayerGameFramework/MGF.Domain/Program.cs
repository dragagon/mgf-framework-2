using MGF.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MGFContext entities = new MGFContext())
            {
                Character character = new Character() { Name = "The Dude" };

                entities.Characters.Add(character);
                entities.SaveChanges();
            }
        }
    }
}
