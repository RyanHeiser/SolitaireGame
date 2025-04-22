using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    public class Foundation
    {
        private Pile hearts = new Pile();
        private Pile diamonds = new Pile();
        private Pile clubs = new Pile();
        private Pile spades = new Pile();

        const int MAX_CARDS = 14;

        public Foundation()
        {
            AddSuitIcons();
        }

        // returns true if the piles in the foundation are full
        public Boolean Full()
        {
            return hearts.Cards.Count == MAX_CARDS && diamonds.Cards.Count == MAX_CARDS && clubs.Cards.Count == MAX_CARDS && spades.Cards.Count == MAX_CARDS;
        }

        // returns true if the foundation contains the card
        public Boolean Contains(Card card)
        {
            return hearts.Cards.Contains(card) || diamonds.Cards.Contains(card) || clubs.Cards.Contains(card) || spades.Cards.Contains(card);
        }

        // returns the pile of the card
        public Pile GetPile(Card card)
        {
            if (hearts.Cards.Contains(card))
            {
                return Hearts;
            }
            else if (diamonds.Cards.Contains(card))
            {
                return Diamonds;
            }
            else if (clubs.Cards.Contains(card))
            {
                return Clubs;
            }
            else if (spades.Cards.Contains(card))
            {
                return Spades;
            }
            else
            {
                return null;
            }
        }

        // returns the piles in the foundation
        public List<Pile> GetPiles()
        {
            List<Pile> piles = new List<Pile>();
            piles.Add(hearts);
            piles.Add(diamonds);
            piles.Add(clubs);
            piles.Add(spades);
            return piles;
        }

        // clears the foundation
        public void Clear()
        {
            hearts.Clear();
            diamonds.Clear();
            clubs.Clear();
            spades.Clear();
        }

        // adds the suit icon images to the foundation in the window
        public void AddSuitIcons()
        {
            hearts.AddCard(new Card(CardSuit.Hearts, CardValue.Null));
            diamonds.AddCard(new Card(CardSuit.Diamonds, CardValue.Null));
            clubs.AddCard(new Card(CardSuit.Clubs, CardValue.Null));
            spades.AddCard(new Card(CardSuit.Spades, CardValue.Null));
        }

        public Pile Hearts { get => hearts; set => hearts = value; }
        public Pile Diamonds { get => diamonds; set => diamonds = value; }
        public Pile Clubs { get => clubs; set => clubs = value; }
        public Pile Spades { get => spades; set => spades = value; }
    }
}
