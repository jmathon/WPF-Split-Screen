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
using System.Windows.Media;
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
        int selectedScreen = 0;
        IList<MainWindow> _windows = new List<MainWindow>();

        public App()
        {
            InitializeComponent();

            foreach( Screen s in Screen.AllScreens )
                ConfigureScreen( s );

            StartTimer( delegate( object s, EventArgs args )
                {
                    if( selectedScreen > 0 ) _windows[selectedScreen - 1].Background = new SolidColorBrush( System.Windows.Media.Color.FromRgb( 204, 204, 204 ) );
                    else _windows[_windows.Count - 1].Background = new SolidColorBrush( System.Windows.Media.Color.FromRgb( 204, 204, 204 ) );

                    _windows[selectedScreen].Background = new SolidColorBrush( System.Windows.Media.Color.FromRgb( 255, 153, 0 ) );

                    if( selectedScreen < _windows.Count - 1 ) selectedScreen++;
                    else selectedScreen = 0;
                }
            );
        }

        void ConfigureScreen( Screen screen )
        {
            MainWindow w = new MainWindow();
            w.Left = screen.WorkingArea.Left;
            w.Top = screen.WorkingArea.Top;

            w.Show();
            w.WindowState = WindowState.Maximized;

            _windows.Add( w );
        }

        void UseScreen( MainWindow w )
        {
            GridViewModel grid = new GridViewModel();

            GridZone g = new GridZone( grid.Current );
            Grid.SetColumn( g, 0 );
            Grid.SetRow( g, 0 );
            Grid myGrid = (Grid)w.Content;
            myGrid.Children.Add( g );

            StartTimer( delegate( object s, EventArgs args )
                {
                    grid.SwitchCommand();
                }
            );
        }

        private void StartTimer( EventHandler handler )
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += handler;
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
