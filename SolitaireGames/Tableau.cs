﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireGames
{
    public class Tableau
    {
        private const int numPiles = 7;
        Pile[] piles = new Pile[NumPiles];

        public Tableau()
        {

        }

        // sets a pile in the tableau at the specified index
        public Boolean SetPile(Pile pile, int index)
        {
            if (index < 0 || index >= piles.Length)
            {
                return false;
            }
            Piles[index] = pile;
            return true;
        }

        // moves a card to onto a card in the tableau
        public void MoveCard(Card moved, Card movedTo)
        {
            Pile pile = GetPile(moved);
            int movedIndex = pile.GetIndexOfCard(moved);
            if (movedIndex > 0)
            {
                pile.GetCard(movedIndex - 1).FaceDown = false;
                pile.GetCard(movedIndex - 1).Image.AllowDrop = true;
                pile.GetCard(movedIndex - 1).Draggable = true;
            }
            Pile newPile = GetPile(movedTo);
            System.Diagnostics.Debug.WriteLine(movedTo.Value);
            newPile.AddCard(moved, newPile.GetIndexOfCard(movedTo) + 1);
            pile.RemoveCard(moved);
            movedTo.Image.AllowDrop = false;

        }

        // moves a card to the end of the pile at the specified column index
        public void MoveCard(Card moved, int columnIndex)
        {
            Pile pile = GetPile(moved);
            int movedIndex = pile.GetIndexOfCard(moved);
            if (movedIndex > 0)
            {
                pile.GetCard(movedIndex - 1).FaceDown = false;
                pile.GetCard(movedIndex - 1).Image.AllowDrop = true;
                pile.GetCard(movedIndex - 1).Draggable = true;
            }
            Pile newPile = Piles[columnIndex];
            newPile.AddCard(moved, newPile.Cards.Count);
            pile.RemoveCard(moved);
        }

        // gets the size of the largest pile
        public int GetMaxPileSize()
        {
            int max = 0;
            for (int i = 0; i < NumPiles; i++)
            {
                if (Piles[i].Cards.Count > max)
                {
                    max = Piles[i].Cards.Count;
                }
            }
            return max;
        }

        // returns true if the tableau contains the card
        public Boolean Contains(Card c)
        {
            for (int i = 0; i < NumPiles; i++)
            {
                if (Piles[i].Cards.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }

        // returns the pile of the card
        public Pile GetPile(Card c)
        {
            for (int i = 0; i < NumPiles; i++)
            {
                if (Piles[i].Cards.Contains(c))
                {
                    return Piles[i];
                }
            }
            return null;
        }

        // clears the tableau
        public void Clear()
        {
            for (int i = 0; i < NumPiles; i++)
            {
                Piles[i].Clear();
            }
        }

        public Pile[] Piles { get => piles; set => piles = value; }

        public static int NumPiles => numPiles;
    }
}
