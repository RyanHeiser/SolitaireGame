using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    public class Pile
    {
        protected ObservableCollection<Card> _cards = new ObservableCollection<Card>();

        public Pile()
        {
        }

        public Pile(ObservableCollection<Card> cards)
        {
            this.Cards = cards;
        }

        public Card GetCard(int index)
        {
            if (Cards.Count > index)
            {
                return Cards[index];
            } else
            {
                return null;
            }
        }

        public int GetIndexOfCard(Card card)
        {
            return Cards.IndexOf(card);
        }

        public Card Draw()
        {
            Card card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }

        public void AddCard(Card card, int index = 0)
        {
            Cards.Insert(index, card);
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public ObservableCollection<Card> Cards { get => _cards; set => _cards = value; }
    }
}
