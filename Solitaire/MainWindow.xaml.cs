﻿using SolitaireGames;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
    private int seconds = 0;
    private int minutes = 0;
    System.Timers.Timer timer = new System.Timers.Timer();
    private int moves = 0;

    public MainWindow()
    {
        InitializeComponent();
        DisplayTableau();
        DisplayStock();
        DisplayFoundation();
        StartTimer();
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

    private void StartTimer()
    {
        timer.Interval = 1000;
        timer.Elapsed += Timer_Elapsed;
        timer.Start();
    }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (seconds < 59)
        {
            seconds++;
        }
        else
        {
            seconds = 0;
            minutes++;
        }
        timerText.Dispatcher.Invoke(new Action(() =>
        {
            timerText.Text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }));
    }

    private void IncrementMoves()
    {
        moves++;
        movesText.Text = "Moves: " + moves;
    }

    private void ResetMoves()
    {
        moves = 0;
        movesText.Text = "Moves: " + moves;
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
        System.Diagnostics.Debug.WriteLine("dragged card row: " + Grid.GetRow(card.Image));
        System.Diagnostics.Debug.WriteLine("tableau max pile size - 1: " + (solitaire.Tableau.GetMaxPileSize() - 1));
        if (Grid.GetRow(card.Image) >= solitaire.Tableau.GetMaxPileSize() - 1 && Grid.GetRow(card.Image) > 8)
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
        // the target card can be dropped on and is not itself
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
                    TryRemoveTableauRow(draggedCard);
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
            IncrementMoves();
        }
        draggedCard = null;
    }

    private void TableauGrid_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }

    private void TableauGrid_Drop(object sender, DragEventArgs e)
    {
        Grid grid = (Grid)sender;

        // get the column index
        Point droppedPoint = e.GetPosition(grid);
        ColumnDefinition column = grid.ColumnDefinitions[0];
        int columnIndex = (int) Math.Floor(droppedPoint.X / column.ActualWidth);

        if (draggedCard == null)
        {
            return;
        }
        System.Diagnostics.Debug.WriteLine("dragged card not null");

        // return if the dragged card not a king
        if (draggedCard.Value != CardValue.King)
        {
            return;
        }

        // the tableau column is empty
        if (grid.AllowDrop && solitaire.Tableau.Piles[columnIndex].Cards.Count < 1)
        {
            // the dragged card is in the tableau
            if (solitaire.Tableau.Contains(draggedCard))
            {
                Pile pile = solitaire.Tableau.GetPile(draggedCard);
                int draggedCardIndex = pile.GetIndexOfCard(draggedCard);
                int j = 0;
                while (draggedCardIndex < pile.Cards.Count)
                {
                    TryRemoveTableauRow(draggedCard);
                    Grid.SetColumn(draggedCard.Image, columnIndex);
                    Grid.SetRow(draggedCard.Image, j);
                    Grid.SetZIndex(draggedCard.Image, j);

                    if (j == 0)
                    {
                        solitaire.Tableau.MoveCard(draggedCard, columnIndex);
                    } 
                    else
                    {
                        solitaire.Tableau.MoveCard(draggedCard, solitaire.Tableau.Piles[columnIndex].GetCard(j-1));
                    }

                    TryAddTableauRow(draggedCard);
                    draggedCard = pile.GetCard(draggedCardIndex);

                    j++;
                }
            }
            // the dragged card is in the talon
            else if (solitaire.Talon.Cards.Contains(draggedCard))
            {
                if (grid.AllowDrop)
                {
                    Grid.SetColumn(draggedCard.Image, columnIndex);
                    Grid.SetRow(draggedCard.Image, 0);
                    Grid.SetZIndex(draggedCard.Image, 0);
                    talonGrid.Children.Remove(draggedCard.Image);
                    tableauGrid.Children.Add(draggedCard.Image);
                    draggedCard.Image.MouseDown -= OnStockClick;
                    TryAddTableauRow(draggedCard);
                    AdjustTalon();

                    solitaire.MoveFromPileToTableau(solitaire.Talon, draggedCard, columnIndex);
                    
                }
            }
            // the dragged card is in the foundation
            else if (solitaire.Foundation.Contains(draggedCard))
            {
           
                Grid.SetRowSpan(draggedCard.Image, 3);
                Grid.SetColumn(draggedCard.Image, columnIndex);
                Grid.SetRow(draggedCard.Image, 0);
                Grid.SetZIndex(draggedCard.Image, 0);
                foundationGrid.Children.Remove(draggedCard.Image);
                tableauGrid.Children.Add(draggedCard.Image);
                draggedCard.Image.Drop -= FoundationCard_Drop;
                draggedCard.Image.Drop += TableauCard_Drop;
                draggedCard.Image.Margin = new System.Windows.Thickness(0);
                TryAddTableauRow(draggedCard);
                solitaire.MoveFromPileToTableau(solitaire.Foundation.GetPile(draggedCard), draggedCard, columnIndex);
            }
            IncrementMoves();
        }
        draggedCard = null;
    }

    private void FoundationCard_Drop(object sender, DragEventArgs e)
    {
        Image image = (Image)sender;
        Card card = (Card)image.Tag;

        // return if the dragged card is not equal to the target card suit or if the dragged card value is not one more than the target card value
        if (draggedCard.Suit != card.Suit || (draggedCard.Value != CardValue.Ace && draggedCard.Value != card.Value + 1))
        {
            return;
        }

        Pile draggedPile;

        if (solitaire.Tableau.GetPile(draggedCard) != null)
        {
            draggedPile = solitaire.Tableau.GetPile(draggedCard);
        } else
        {
            draggedPile = solitaire.Talon;
        }

        // dragged card is not the target card and the dragged card is either from the talon or if from the tableau it is the bottom card in its pile
        if (image.AllowDrop && card != draggedCard && (draggedPile == solitaire.Talon || draggedPile.GetIndexOfCard(draggedCard) == draggedPile.Cards.Count - 1))
        {
            // the dragged card is from the tableau
            if (solitaire.Tableau.Contains(draggedCard))
            {
                TryRemoveTableauRow(draggedCard);
                Grid.SetRowSpan(draggedCard.Image, 1);
                Grid.SetColumn(draggedCard.Image, Grid.GetColumn(image));
                Grid.SetRow(draggedCard.Image, Grid.GetRow(image));
                Grid.SetZIndex(draggedCard.Image, Grid.GetZIndex(image) + 1);
                tableauGrid.Children.Remove(draggedCard.Image);
                foundationGrid.Children.Add(draggedCard.Image);
                draggedCard.Image.Drop += FoundationCard_Drop;
                draggedCard.Image.Drop -= TableauCard_Drop;
                draggedCard.Image.Margin = new System.Windows.Thickness(5);
                
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
            IncrementMoves();
        }

        if (solitaire.Foundation.Full())
        {
            timer.Stop();
            WinWindow winWindow = new WinWindow(this);
            winWindow.Show();
        }
        draggedCard = null;
    }

    private void OnStockClick(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("stock click");
        if (solitaire.Stock.Cards.Count > 0)
        {
            Card card = solitaire.Stock.Draw();
            card.FaceDown = false;
            Grid.SetColumn(card.Image, 0);
            Grid.SetRow(card.Image, 2);
            Grid.SetZIndex(card.Image, talonGrid.Children.Count);
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
            IncrementMoves();
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
        IncrementMoves();
    }

    private void ResetButton_Click(object sender, EventArgs e)
    {
        ResetGame();
    }

    public void ResetGame()
    {
        System.Diagnostics.Debug.WriteLine("resetting game");
        tableauGrid.Children.Clear();
        foundationGrid.Children.Clear();
        talonGrid.Children.Clear();

        int i = 0;
        while (stockAndTalonGrid.Children.Count > 2)
        {
            if (stockAndTalonGrid.Children[i] is Image)
            {
                stockAndTalonGrid.Children.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        while (tableauGrid.RowDefinitions.Count > 12)
        {
            tableauGrid.RowDefinitions.RemoveAt(tableauGrid.RowDefinitions.Count - 1);
        }

        solitaire.RestartGame();
        DisplayTableau();
        DisplayStock();
        DisplayFoundation();
        seconds = 0;
        minutes = 0;
        ResetMoves();
        timer.Start();
    }
}