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

        public void MoveCard(Card moved, Card movedTo)
        {
            Pile pile = GetPile(moved);
            int movedIndex = pile.GetIndexOfCard(moved);
            if (movedIndex > 0)
            {
                pile.GetCard(movedIndex - 1).FaceDown = false;
                pile.GetCard(movedIndex - 1).Image.AllowDrop = true;
                pile.GetCard(movedIndex - 1).Draggable = true;
            }
            Pile newPile = GetPile(movedTo);
            newPile.AddCard(moved, newPile.GetIndexOfCard(movedTo) + 1);
            pile.RemoveCard(moved);
            movedTo.Image.AllowDrop = false;

        }

        public Boolean Contains(Card c)
        {
            for (int i = 0; i < NumPiles; i++)
            {
                if (Piles[i].Cards.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }

        public Pile GetPile(Card c)
        {
            for (int i = 0; i < NumPiles; i++)
            {
                if (Piles[i].Cards.Contains(c))
                {
                    return Piles[i];
                }
            }
            return null;
        }

        public Pile[] Piles { get => piles; set => piles = value; }

        public static int NumPiles => numPiles;
    }
}
