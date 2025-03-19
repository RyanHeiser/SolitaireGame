using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    public class Deck : Pile
    {
        public Deck()
        {
            ResetDeck();
            Shuffle();
        }

        public void ResetDeck()
        {
            Cards.Clear();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    Card card;
                    if (i == 0)
                    {
                        card = new Card(CardSuit.Hearts, (CardValue)j);
                    }
                    else if (i == 1)
                    {
                        card = new Card(CardSuit.Diamonds, (CardValue)j);
                    }
                    else if (i == 2)
                    {
                        card = new Card(CardSuit.Clubs, (CardValue) j);
                    }
                    else
                    {
                        card = new Card(CardSuit.Spades, (CardValue)j);
                    }
                    AddCard(card);
                }
            }
        }

        public void Shuffle()
        {
            Random rand = new Random();
            for (int i = 0; i < 52; i++)
            {
                int index = rand.Next(52 - i) + i;
                Card c = Cards[index];
                Cards.RemoveAt(index);
                AddCard(c, i);
            }
        }
    }
}
