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
        private Boolean _draggable;
        private Image _image;
        private BitmapImage _bitmapImage;
        private BitmapImage _faceDownBitmapImage;

        public Card(SolitaireGames.CardSuit suit, SolitaireGames.CardValue value)
        {
            this.Suit = suit;
            this.Value = value;
            Draggable = false;

            UpdateColor();
            SetupImage();
            FaceDown = true;
        }

        private void SetupImage()
        {
            Image = new Image();
            Image.Tag = this;
            Image.AllowDrop = false;

            _bitmapImage = new BitmapImage();
            _faceDownBitmapImage = new BitmapImage();
            String _uri = @"/SolitaireGames;component/Assets/Playing Cards/";

            // convert the suit to path string
            _uri += ValueToString();
            _uri += "_of_";

            // convert the value to path string
            _uri += SuitToString();
            _uri += ".png";

            _bitmapImage.BeginInit();
            _bitmapImage.UriSource = new Uri(_uri, UriKind.Relative);
            _bitmapImage.DecodePixelWidth = 400;
            _bitmapImage.EndInit();
            _faceDownBitmapImage.BeginInit();
            _faceDownBitmapImage.UriSource = new Uri(@"/SolitaireGames;component/Assets/Playing Cards/card back.png", UriKind.Relative);
            _faceDownBitmapImage.DecodePixelWidth = 400;
            _faceDownBitmapImage.EndInit();
            Image.Source = _faceDownBitmapImage;
        }

        private String ValueToString()
        {
            if ((int)Value == 0)
            {
               return "ace";
            }
            else if ((int)Value == 10)
            {
                return "jack";
            }
            else if ((int)Value == 11)
            {
                return "queen";
            }
            else if ((int)Value == 12)
            {
                return "king";
            }
            else
            {
                return (((int)Value) + 1).ToString();
            }
        }

        private String SuitToString()
        {
            if (Suit == SolitaireGames.CardSuit.Hearts)
            {
                return "hearts";
            }
            else if (Suit == SolitaireGames.CardSuit.Diamonds)
            {
                return "diamonds";
            }
            else if (Suit == SolitaireGames.CardSuit.Clubs)
            {
                return "clubs";
            }
            else
            {
                return "spades";
            }
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
        public bool Draggable { get => _draggable; set => _draggable = value; }
    }
}
