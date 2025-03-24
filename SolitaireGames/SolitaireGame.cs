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
        private Foundation foundation = new Foundation();
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
                        card.Draggable = true;
                    }
                }
                Tableau.SetPile(newPile, i);
            }
        }

        private void SetUpStock()
        {
            while(Deck.Cards.Count > 0)
            {
                Card card = Deck.Draw();
                Stock.AddCard(card);
            }
        }

        public void MoveFromPileToTableau(Pile pile, Card moved, Card movedTo)
        {
            pile.RemoveCard(moved);
            Pile newPile = Tableau.GetPile(movedTo);
            newPile.AddCard(moved, newPile.GetIndexOfCard(movedTo) + 1);
            moved.Image.AllowDrop = true;
            movedTo.Image.AllowDrop = false;
        }

        public void MoveFromPileToTableau(Pile pile, Card moved, int columnIndex)
        {
            pile.RemoveCard(moved);
            Pile newPile = Tableau.Piles[columnIndex];
            if (newPile.Cards.Count > 0)
            {
                newPile.AddCard(moved, newPile.Cards.Count - 1);
            } else
            {
                newPile.AddCard(moved, 0);
            }
            moved.Image.AllowDrop = true;
        }

        public void MoveFromTableauToPile(Pile pile, Card moved, Card movedTo)
        {
            Pile oldPile = Tableau.GetPile(moved);
            if (oldPile.Cards.Count > 1) {
                Card card = oldPile.GetCard(oldPile.GetIndexOfCard(moved) - 1);
                card.Image.AllowDrop = true;
                card.FaceDown = false;
                card.Draggable = true;
            }
            oldPile.RemoveCard(moved);
            pile.AddCard(moved, pile.GetIndexOfCard(movedTo));
        }


        public Tableau Tableau { get => tableau; set => tableau = value; }
        public Deck Deck { get => deck; set => deck = value; }
        public Pile Stock { get => stock; set => stock = value; }
        public Pile Talon { get => talon; set => talon = value; }
        public Foundation Foundation { get => foundation; set => foundation = value; }
    }
}
