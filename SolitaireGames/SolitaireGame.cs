using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    public class SolitaireGame
    {
        private Tableau tableau = new Tableau();
        private Deck deck = new Deck();

        public SolitaireGame() 
        {
            SetUpTableau();
        }

       

        private void SetUpTableau()
        {
            for (int i = 0; i < 7; i++)
            {
                Pile newPile = new Pile();
                for (int j = 0; j <= i; j++)
                {
                    Card card = Deck.Draw();
                    newPile.AddCard(card, j);
                    if (j == i)
                    {
                        card.FaceDown = false;
                        card.Image.AllowDrop = true;
                    }
                }
                Tableau.SetPile(newPile, i);
            }
        }


        public Tableau Tableau { get => tableau; set => tableau = value; }
        public Deck Deck { get => deck; set => deck = value; }

    }
}
