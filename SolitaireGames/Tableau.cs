using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    class Tableau
    {
        Pile[] piles = new Pile[7];

        public Tableau()
        {

        }

        public void SetPile(Pile pile, int index)
        {
            Piles[index] = pile;
        }

        public Pile[] Piles { get => piles; set => piles = value; }
    }
}
