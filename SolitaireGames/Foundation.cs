using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    class Foundation
    {
        private List<Card> hearts = new List<Card>();
        private List<Card> diamonds = new List<Card>();
        private List<Card> clubs = new List<Card>();
        private List<Card> spades = new List<Card>();

        public Foundation()
        {

        }

        public void AddToHearts(Card card)
        {
            hearts.Add(card);
        }

        public void AddToDiamonds(Card card)
        {
            diamonds.Add(card);
        }

        public void AddToClubs(Card card)
        {
            clubs.Add(card);
        }

        public void AddToSpades(Card card)
        {
            spades.Add(card);
        }

        public List<Card> Hearts { get => hearts; set => hearts = value; }
        public List<Card> Diamonds { get => diamonds; set => diamonds = value; }
        public List<Card> Clubs { get => clubs; set => clubs = value; }
        public List<Card> Spades { get => spades; set => spades = value; }
    }
}
