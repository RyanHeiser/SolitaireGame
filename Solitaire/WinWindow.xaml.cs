using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Solitaire
{
    /// <summary>
    /// This window displays if the player wins
    /// </summary>
    public partial class WinWindow : Window
    {
        MainWindow MainWindow;
        public WinWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            InitializeComponent();
        }

        // listener for if the play again button is clicked
        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ResetGame();
            this.Close();
        }

        // listener for if the exit button is clicked
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
