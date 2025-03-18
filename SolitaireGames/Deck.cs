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
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    if (i == 0)
                    {
                        AddCard(new Card(CardSuit.Hearts, (CardValue) j));
                    }
                    else if (i == 1)
                    {
                        AddCard(new Card(CardSuit.Diamonds, (CardValue)j));
                    }
                    else if (i == 2)
                    {
                        AddCard(new Card(CardSuit.Clubs, (CardValue) j));
                    }
                    else
                    {
                        AddCard(new Card(CardSuit.Spades, (CardValue) j));
                    }
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
