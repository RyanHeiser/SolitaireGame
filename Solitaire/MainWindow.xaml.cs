using SolitaireGames;
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

namespace Solitaire;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    SolitaireGame solitaire = new SolitaireGame();
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
                System.Diagnostics.Debug.WriteLine(image.Source);
                Grid.SetColumn(image, i);
                Grid.SetRow(image, j);
                Grid.SetRowSpan(image, 2);
                image.Visibility = Visibility.Visible;
                tableauGrid.Children.Add(image);

            }
        }
    }
   
}