using SolitaireGames;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace Solitaire;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    SolitaireGame solitaire = new SolitaireGame();
    Card draggedCard;

    public MainWindow()
    {
        InitializeComponent();
        DisplayTableau();
        DisplayStock();
    }

    private void DisplayTableau()
    {
        for (int i = 0; i < Tableau.NumPiles; i++)
        {
            Pile pile = solitaire.Tableau.Piles[i];
            for (int j = 0; j < pile.Cards.Count; j++)
            {
                Image image = pile.Cards[j].Image;
                image.MouseMove += Card_MouseMove;
                image.Drop += TableauCard_Drop;
                Grid.SetColumn(image, i);
                Grid.SetRow(image, j);
                Grid.SetRowSpan(image, 2);
                Grid.SetZIndex(image, j);
                tableauGrid.Children.Add(image);

            }
        }
    }

    private void DisplayStock()
    {
        for (int i = 0; i < solitaire.Stock.Cards.Count; i++)
        {
            Image image = solitaire.Stock.Cards[i].Image;
            image.MouseMove += Card_MouseMove;
            image.Drop += StockAndTalonCard_Drop;
            image.MouseDown += OnStockClick;
            Grid.SetColumn(image, 0);
            Grid.SetRow(image, 0);
            Grid.SetZIndex(image, i);
            stockAndTalonGrid.Children.Add(image);
        }
    }

    private void AddTableauRow()
    {
        RowDefinition row = new RowDefinition();
        row.Height = new GridLength(1, GridUnitType.Star);
        tableauGrid.RowDefinitions.Add(row);
    }

    private void Card_MouseMove(object sender, MouseEventArgs e)
    {
        Image image = (Image)sender;
        Card card = (Card)image.Tag;

        if (e.LeftButton == MouseButtonState.Pressed && !card.FaceDown)
        {
            System.Diagnostics.Debug.WriteLine("drop allowed");
            draggedCard = card;
            DragDrop.DoDragDrop(image, image, DragDropEffects.Move);
        }
        
    }

    private void TableauCard_Drop(object sender, DragEventArgs e)
    {
        Image image = (Image)sender;
        Card card = (Card)image.Tag;

        if (image.AllowDrop && card != draggedCard)
        {
            if (solitaire.Tableau.Contains(draggedCard) && solitaire.Tableau.Contains(card))
            {
                Pile pile = solitaire.Tableau.GetPile(draggedCard);
                int draggedCardIndex = pile.GetIndexOfCard(draggedCard);
                int j = 0;
                while (draggedCardIndex < pile.Cards.Count)
                {
                    Grid.SetColumn(draggedCard.Image, Grid.GetColumn(image));
                    Grid.SetRow(draggedCard.Image, Grid.GetRow(image) + 1 + j);
                    Grid.SetZIndex(draggedCard.Image, Grid.GetZIndex(image) + 1 + j);
                    solitaire.Tableau.MoveCard(draggedCard, card);
                    if (Grid.GetRow(draggedCard.Image) + 1 >= tableauGrid.RowDefinitions.Count)
                    {
                        System.Diagnostics.Debug.WriteLine("adding row");
                        AddTableauRow();
                    }
                    if (pile.Cards.Count > draggedCardIndex)
                    {
                        card = draggedCard;
                        draggedCard = pile.GetCard(draggedCardIndex);
                    } 
                    else
                    {
                        break;
                    }

                        j++;
                }
            }
           
        }
        draggedCard = null;
    }

    private void StockAndTalonCard_Drop(object sender, EventArgs e)
    {
        Image image = (Image)sender;
        Card card = (Card)image.Tag;

        if (image.AllowDrop && card != draggedCard)
        {
            if (solitaire.Tableau.Contains(card))
            {
                Grid.SetColumn(draggedCard.Image, Grid.GetColumn(image));
                Grid.SetRow(draggedCard.Image, Grid.GetRow(image) + 1);
                Grid.SetZIndex(draggedCard.Image, Grid.GetZIndex(image) + 1);
                draggedCard.Image.Drop -= StockAndTalonCard_Drop;
                draggedCard.Image.MouseDown -= OnStockClick;
                draggedCard.Image.Drop += TableauCard_Drop;
            }
        }
        draggedCard = null;
    }

    private void OnStockClick(object sender, RoutedEventArgs e)
    {
        if (solitaire.Stock.Cards.Count > 0)
        {
            Card card = solitaire.Stock.Draw();
            card.FaceDown = false;
            Grid.SetColumn(card.Image, 0);
            Grid.SetRow(card.Image, 2);
            Grid.SetZIndex(card.Image, tableauGrid.Children.Count);
            Grid.SetRowSpan(card.Image, 3);
            stockAndTalonGrid.Children.Remove(card.Image);
            talonGrid.Children.Add(card.Image);

            for (int i = 0; i < 2; i++)
            {
                if (solitaire.Talon.Cards.Count > i)
                {
                    Grid.SetRow(solitaire.Talon.GetCard(0).Image, 1);
                    Grid.SetRow(solitaire.Talon.GetCard(1).Image, 0);
                }
            }
            
        }
    }

    private void BringToFront(Grid grid, Image image)
    {
        int currIndex = Grid.GetZIndex(image);
        int zIndex = 0;
        int maxZIndex = 0;
        Image otherImage;
        for (int i = 0; i < grid.Children.Count; i++)
        {
            if (grid.Children[i] is Image && grid.Children[i] != image && Grid.GetColumn(grid.Children[i]) == Grid.GetColumn(image))
            {
                otherImage = (Image)grid.Children[i];
                zIndex = Grid.GetZIndex(otherImage);
                maxZIndex = Math.Max(maxZIndex, zIndex);
                if (zIndex >= currIndex)
                {
                    Grid.SetZIndex(otherImage, zIndex - 1);
                }
            }
        }
        Grid.SetZIndex(image, maxZIndex);
    }
}