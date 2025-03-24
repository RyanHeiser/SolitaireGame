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
    private Boolean mouseDownOverEmpty = false;

    public MainWindow()
    {
        InitializeComponent();
        DisplayTableau();
        DisplayStock();
        DisplayFoundation();
    }

    private void DisplayTableau()
    {
        for (int i = 0; i < Tableau.NumPiles; i++)
        {
            Pile pile = solitaire.Tableau.Piles[i];
            for (int j = 0; j < pile.Cards.Count; j++)
            {
                Image image = pile.Cards[j].Image;
                image.MouseDown += Card_MouseDown;
                image.Drop += TableauCard_Drop;
                Grid.SetColumn(image, i);
                Grid.SetRow(image, j);
                Grid.SetRowSpan(image, 3);
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
            image.MouseDown += Card_MouseDown;
            image.Drop += TableauCard_Drop;
            image.MouseDown += OnStockClick;
            Grid.SetColumn(image, 0);
            Grid.SetRow(image, 0);
            Grid.SetZIndex(image, i);
            stockAndTalonGrid.Children.Add(image);
        }
    }

    private void DisplayFoundation()
    {
        for (int i = 0; i < 4; i++)
        {
            Card card = solitaire.Foundation.GetPiles()[i].GetCard(0);
            Grid.SetRow(card.Image, i);
            foundationGrid.Children.Add(card.Image);
            card.FaceDown = false;
            card.Image.Margin = new System.Windows.Thickness(5);
            card.Image.AllowDrop = true;
            card.Image.Drop += FoundationCard_Drop;
            card.Image.MouseDown += Card_MouseDown;
            System.Diagnostics.Debug.WriteLine(card.Image.Source);
        }
    }

    private void TryAddTableauRow(Card card)
    {
        if (Grid.GetRow(card.Image) >= tableauGrid.RowDefinitions.Count - 3)
        {
            System.Diagnostics.Debug.WriteLine("adding row");
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            tableauGrid.RowDefinitions.Add(row);
        }
        
    }

    private void TryRemoveTableauRow(Card card)
    {
        if (Grid.GetRow(card.Image) >= solitaire.Tableau.GetMaxPileSize() - 1)
        {
            System.Diagnostics.Debug.WriteLine("removing row");
            tableauGrid.RowDefinitions.RemoveAt(tableauGrid.RowDefinitions.Count - 1);
        }
    }

    private void AdjustTalon()
    {
        for (int i = 1; i < 3; i++)
        {
            if (solitaire.Talon.GetCard(i) == null)
            {
                break;
            }
            if (i == 1)
            {
                solitaire.Talon.GetCard(i).Draggable = true;
            }
            Grid.SetRow(solitaire.Talon.GetCard(i).Image, 3 - i);
        }
    }

    private void Card_MouseDown(object sender, MouseEventArgs e)
    {
        Image image = (Image)sender;
        Card card = (Card)image.Tag;

        if (e.LeftButton == MouseButtonState.Pressed && card.Draggable)
        {
            draggedCard = card;
            DragDrop.DoDragDrop(image, image, DragDropEffects.Move);
        }
        
    }

    private void TableauCard_Drop(object sender, DragEventArgs e)
    {
        Image image = (Image)sender;
        Card card = (Card)image.Tag;

        // return if the dragged card is of the same color as the target card or if the dragged card is not one less than the target card
        if (draggedCard.Color == card.Color || draggedCard.Value != card.Value - 1)
        {
            return;
        }

        if (image.AllowDrop && card != draggedCard)
        {
            // the dragged card and target card are in the tableau
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
                    TryAddTableauRow(draggedCard);
                   

                    // sets target card to the dragged card and dragged card to the next card in its former pile
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
            // the dragged card is in the talon
            else if (solitaire.Talon.Cards.Contains(draggedCard))
            {
                if (image.AllowDrop && card != draggedCard)
                {
                    if (solitaire.Tableau.Contains(card))
                    {
                        Grid.SetColumn(draggedCard.Image, Grid.GetColumn(image));
                        Grid.SetRow(draggedCard.Image, Grid.GetRow(image) + 1);
                        Grid.SetZIndex(draggedCard.Image, Grid.GetZIndex(image) + 1);
                        talonGrid.Children.Remove(draggedCard.Image);
                        tableauGrid.Children.Add(draggedCard.Image);
                        draggedCard.Image.MouseDown -= OnStockClick;
                        TryAddTableauRow(draggedCard);
                        AdjustTalon();
                        
                        solitaire.MoveFromPileToTableau(solitaire.Talon, draggedCard, card);
                    }
                }
            }
            // the dragged card is in the foundation
            else if (solitaire.Foundation.Contains(draggedCard))
            {
                if (solitaire.Tableau.Contains(card))
                {
                    Grid.SetRowSpan(draggedCard.Image, 3);
                    Grid.SetColumn(draggedCard.Image, Grid.GetColumn(image));
                    Grid.SetRow(draggedCard.Image, Grid.GetRow(image) + 1);
                    Grid.SetZIndex(draggedCard.Image, Grid.GetZIndex(image) + 1);
                    foundationGrid.Children.Remove(draggedCard.Image);
                    tableauGrid.Children.Add(draggedCard.Image);
                    draggedCard.Image.Drop -= FoundationCard_Drop;
                    draggedCard.Image.Drop += TableauCard_Drop;
                    draggedCard.Image.Margin = new System.Windows.Thickness(0);
                    TryAddTableauRow(draggedCard);
                    solitaire.MoveFromPileToTableau(solitaire.Foundation.GetPile(draggedCard), draggedCard, card);
                }
            }
        }
        draggedCard = null;
    }

    private void FoundationCard_Drop(object sender, DragEventArgs e)
    {
        Image image = (Image)sender;
        Card card = (Card)image.Tag;

        Pile draggedPile;

        if (solitaire.Tableau.GetPile(draggedCard) != null)
        {
            draggedPile = solitaire.Tableau.GetPile(draggedCard);
        } else
        {
            draggedPile = solitaire.Talon;
        }

            System.Diagnostics.Debug.WriteLine("foundation drop");

        if (image.AllowDrop && card != draggedCard && (draggedPile == solitaire.Talon || draggedPile.GetIndexOfCard(draggedCard) == draggedPile.Cards.Count - 1))
        {
            System.Diagnostics.Debug.WriteLine("drop allowed");
            // the dragged card is from the tableau
            if (solitaire.Tableau.Contains(draggedCard))
            {
                Grid.SetRowSpan(draggedCard.Image, 1);
                Grid.SetColumn(draggedCard.Image, Grid.GetColumn(image));
                Grid.SetRow(draggedCard.Image, Grid.GetRow(image));
                Grid.SetZIndex(draggedCard.Image, Grid.GetZIndex(image) + 1);
                tableauGrid.Children.Remove(draggedCard.Image);
                foundationGrid.Children.Add(draggedCard.Image);
                draggedCard.Image.Drop += FoundationCard_Drop;
                draggedCard.Image.Drop -= TableauCard_Drop;
                draggedCard.Image.Margin = new System.Windows.Thickness(5);
                TryRemoveTableauRow(draggedCard);
                
                solitaire.MoveFromTableauToPile(solitaire.Foundation.GetPile(card), draggedCard, card);
            } 
            // the dragged card is from the talon
            else if (solitaire.Talon.Contains(draggedCard))
            {
                Grid.SetRowSpan(draggedCard.Image, 1);
                Grid.SetColumn(draggedCard.Image, Grid.GetColumn(image));
                Grid.SetRow(draggedCard.Image, Grid.GetRow(image));
                Grid.SetZIndex(draggedCard.Image, Grid.GetZIndex(image) + 1);
                talonGrid.Children.Remove(draggedCard.Image);
                foundationGrid.Children.Add(draggedCard.Image);
                draggedCard.Image.Drop += FoundationCard_Drop;
                draggedCard.Image.Drop -= TableauCard_Drop;
                draggedCard.Image.AllowDrop = true;
                draggedCard.Image.Margin = new System.Windows.Thickness(5);
                AdjustTalon();

                solitaire.Foundation.GetPile(card).AddCard(draggedCard);
                solitaire.Talon.RemoveCard(draggedCard);
            }
        }
        draggedCard = null;
    }

    private void OnStockClick(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("stock click");
        if (solitaire.Stock.Cards.Count > 0)
        {
            System.Diagnostics.Debug.WriteLine("more than 0");
            Card card = solitaire.Stock.Draw();
            card.FaceDown = false;
            Grid.SetColumn(card.Image, 0);
            Grid.SetRow(card.Image, 2);
            Grid.SetZIndex(card.Image, tableauGrid.Children.Count);
            Grid.SetRowSpan(card.Image, 3);
            stockAndTalonGrid.Children.Remove(card.Image);
            talonGrid.Children.Add(card.Image);
            solitaire.Talon.AddCard(card);
            card.Image.MouseDown -= OnStockClick;
            card.Draggable = true;

            for (int i = 1; i <= 2; i++)
            {
                if (solitaire.Talon.Cards.Count > i)
                {
                    Grid.SetRow(solitaire.Talon.GetCard(i).Image, 2 - i);
                    solitaire.Talon.GetCard(i).Image.AllowDrop = false;
                    solitaire.Talon.GetCard(i).Draggable = false;
                }
            }

            if (solitaire.Stock.Cards.Count == 0)
            {
                resetStockButton.Visibility = Visibility.Visible;
            }

        }
    }

    private void ResetStock_Click(object sender, EventArgs e)
    {
        if (solitaire.Stock.Cards.Count != 0)
        {
            return;
        }

        for (int i = 0; i < solitaire.Talon.Cards.Count; i++)
        {
            Card card = solitaire.Talon.Cards[i];
            card.FaceDown = true;
            card.Draggable = false;
            card.Image.MouseDown += OnStockClick;
            talonGrid.Children.Remove(card.Image);
            stockAndTalonGrid.Children.Add(card.Image);
            Grid.SetColumn(card.Image, 0);
            Grid.SetRow(card.Image, 0);
            Grid.SetRowSpan(card.Image, 1);
            Grid.SetZIndex(card.Image, i);
            solitaire.Stock.AddCard(card);
        }
        solitaire.Talon.Cards.Clear();
        resetStockButton.Visibility = Visibility.Hidden;
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