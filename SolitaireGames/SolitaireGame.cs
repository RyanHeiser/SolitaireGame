using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    class SolitaireGame
    {
        Tableau tableau = new Tableau();
        Deck deck = new Deck();

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
                    newPile.AddCard(Deck.draw());
                }
                Tableau.SetPile(newPile, i);
            }
        }

        internal Tableau Tableau { get => tableau; set => tableau = value; }
        internal Deck Deck { get => deck; set => deck = value; }

    }
}
