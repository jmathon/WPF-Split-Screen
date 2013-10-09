using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using WPFTry.ViewModels;
using WPFTry.Views;

namespace WPFTry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        IList<MainWindow> _windows = new List<MainWindow>();

        public App()
        {
            InitializeComponent();

            foreach( Screen s in Screen.AllScreens )
                ConfigureScreen( s );

        }

        void ConfigureScreen( Screen screen )
        {
            GridViewModel grid = new GridViewModel();

            MainWindow w = new MainWindow();
            w.Left = screen.WorkingArea.Left;
            w.Top = screen.WorkingArea.Top;

            w.DataContext = grid;
            GridZone g = new GridZone( grid.Current );
            Grid.SetColumn( g, 0 );
            Grid.SetRow( g, 0 );
            Grid myGrid = (Grid)w.Content;
            myGrid.Children.Add( g );
            w.Show();
            w.WindowState = WindowState.Maximized;
            Timer( grid );

            _windows.Add( w );
        }

        private void Timer( GridViewModel grid )
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += delegate( object s, EventArgs args )
            {
                grid.SwitchCommand();
            };
            timer.Interval = new TimeSpan( 0, 0, 0, 0, Int32.Parse( ConfigurationManager.AppSettings["TimeToSwitch"] ) );
            timer.Start();
        }

        [STAThread]
        public static void Main( string[] args )
        {
            App app = new App();
            app.Run();
        }
    }
}
