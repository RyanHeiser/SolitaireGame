using System.Configuration;
using System.Data;
using System.Windows;

namespace Solitaire;

public partial class App : Application
{

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow wnd = new MainWindow();
        wnd.Show();
    }
}

