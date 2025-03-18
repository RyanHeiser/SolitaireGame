using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SolitaireGames
{
   
    public class Card
    {
        private SolitaireGames.CardColor _color;
        private SolitaireGames.CardSuit _suit;
        private SolitaireGames.CardValue _value;
        private Boolean _faceDown;
        private Image _image;
        private BitmapImage _bitmapImage;
        private BitmapImage _faceDownBitmapImage;

        public Card(SolitaireGames.CardSuit suit, SolitaireGames.CardValue value)
        {
            this.Suit = suit;
            this.Value = value;
            FaceDown = true;

            UpdateColor();
            SetupImage();

        }

        private void SetupImage()
        {
            Image = new Image();
            _bitmapImage = new BitmapImage();
            _faceDownBitmapImage = new BitmapImage();
            String _uri = @"./Assets/Playing Cards/";

            // convert the value to path string
            _uri = SuitToString();

            // convert the suit to path string
            _uri = ValueToString();

            _bitmapImage.BeginInit();
            _bitmapImage.UriSource = new Uri(_uri, UriKind.Relative);
            _bitmapImage.DecodePixelWidth = 200;
            _bitmapImage.EndInit();
            _faceDownBitmapImage.BeginInit();
            _faceDownBitmapImage.UriSource = new Uri(@"./Assets/Playing Cards/card back.png");
            _faceDownBitmapImage.DecodePixelWidth = 200;
            _faceDownBitmapImage.EndInit();
            Image.Source = _faceDownBitmapImage;
        }

        private String SuitToString()
        {
            String s = "";
            if ((int)Value == 0)
            {
                s = s + "ace";
            }
            else if ((int)Value == 11)
            {
                s = s + "jack";
            }
            else if ((int)Value == 12)
            {
                s = s + "queen";
            }
            else if ((int)Value == 13)
            {
                s = s + "king";
            }
            else
            {
                s = s + (((int)Value) - 1).ToString();
            }
            return s;
        }

        private String ValueToString()
        {
            String s = "";
            if (Suit == SolitaireGames.CardSuit.Hearts)
            {
                s = s + "_of_hearts.png";
            }
            else if (Suit == SolitaireGames.CardSuit.Diamonds)
            {
                s = s + "_of_diamonds.png";
            }
            else if (Suit == SolitaireGames.CardSuit.Clubs)
            {
                s = s + "_of_clubs.png";
            }
            else
            {
                s = s + "_of_spades.png";
            }
            return s;
        }

        private void UpdateColor()
        {
            if (Suit == CardSuit.Hearts || Suit == CardSuit.Diamonds)
            {
                _color = CardColor.Red;
            } else
            {
                _color = CardColor.Black;
            }
        }
        public CardColor Color { get => _color;}
        public CardSuit Suit { get => _suit; set => _suit = value; }
        public CardValue Value { get => _value; set => this._value = value; }
        public bool FaceDown 
        { 
            get => _faceDown;
            set
            {
                if (value)
                {
                    _faceDown = true;
                    Image.Source = _faceDownBitmapImage;
                } else
                {
                    _faceDown = false;
                    Image.Source = _bitmapImage;
                }
            }
        }
        public Image Image { get => _image; set => _image = value; }
    }
}
