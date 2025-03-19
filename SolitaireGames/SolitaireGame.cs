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
        private Pile stock = new Pile();
        private Pile talon = new Pile();

        public SolitaireGame() 
        {
            SetUpTableau();
            SetUpStock();
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

        private void SetUpStock()
        {
            for (int i = 0; i < Deck.Cards.Count; i++)
            {
                Card card = Deck.Draw();
                Stock.AddCard(card, i);
            }
        }

        public void MoveFromPileToTableau(Pile pile, Card moved, Card movedTo)
        {
            pile.RemoveCard(moved);
            Pile newPile = Tableau.GetPile(movedTo);
            newPile.AddCard(moved, newPile.GetIndexOfCard(movedTo) + 1);
            movedTo.Image.AllowDrop = false;
        }


        public Tableau Tableau { get => tableau; set => tableau = value; }
        public Deck Deck { get => deck; set => deck = value; }
        public Pile Stock { get => stock; set => stock = value; }
        public Pile Talon { get => talon; set => talon = value; }
    }
}
