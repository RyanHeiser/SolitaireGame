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

        // returns the card at the specified index
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

        // returns true iff the pile contains the card
        public Boolean Contains(Card card)
        {
            return Cards.Contains(card);
        }

        // returns the index of the card
        public int GetIndexOfCard(Card card)
        {
            return Cards.IndexOf(card);
        }

        // removes and returns the top card in the pile
        public Card Draw()
        {
            Card card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }

        // adds a card to the bottom of the pile
        public void AddCard(Card card, int index = 0)
        {
            Cards.Insert(index, card);
        }

        // removes a card from the pile
        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        // clears the pile
        public void Clear()
        {
            Cards.Clear();
        }

        public ObservableCollection<Card> Cards { get => _cards; set => _cards = value; }
    }
}
