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
    }

    private void DisplayTableau()
    {
        for (int i = 0; i < Tableau.NumPiles; i++)
        {
            Pile pile = solitaire.Tableau.Piles[i];
            for (int j = 0; j < pile.Cards.Count; j++)
            {
                Image image = pile.Cards[j].Image;
                //image.MouseDown += Card_MouseDown;
                image.MouseMove += Card_MouseMove;
                //image.MouseUp += Card_MouseUp;
                image.Drop += Card_Drop;
                Grid.SetColumn(image, i);
                Grid.SetRow(image, j);
                Grid.SetRowSpan(image, 2);
                Grid.SetZIndex(image, j);
                tableauGrid.Children.Add(image);

            }
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

        if (e.LeftButton == MouseButtonState.Pressed && image.AllowDrop)
        {
            System.Diagnostics.Debug.WriteLine("drop allowed");
            draggedCard = card;
            DragDrop.DoDragDrop(image, image, DragDropEffects.Move);
        }
        
    }

    private void Card_Drop(object sender, DragEventArgs e)
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