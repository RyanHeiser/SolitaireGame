using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    class Pile
    {
        protected ObservableCollection<Card> _cards = new ObservableCollection<Card>();

        public Pile()
        {
        }

        public Pile(ObservableCollection<Card> cards)
        {
            this.Cards = cards;
        }

        public Card getCard(int index)
        {
            return Cards[index];
        }

        public Card draw()
        {
            Card card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }

        public void AddCard(Card card, int index = 0)
        {
            Cards.Insert(index, card);
        }

        public ObservableCollection<Card> Cards { get => _cards; set => _cards = value; }
    }
}
