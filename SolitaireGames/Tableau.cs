using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    public class Tableau
    {
        private const int numPiles = 7;
        Pile[] piles = new Pile[NumPiles];

        public Tableau()
        {

        }

        public Boolean SetPile(Pile pile, int index)
        {
            if (index < 0 || index >= piles.Length)
            {
                return false;
            }
            Piles[index] = pile;
            return true;
        }

        public Pile[] Piles { get => piles; set => piles = value; }

        public static int NumPiles => numPiles;
    }
}
