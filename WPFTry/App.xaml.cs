using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WPFTry.ViewModels;
using WPFTry.Views;

namespace WPFTry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly GridViewModel _grid = new GridViewModel();

        public App()
        {
            InitializeComponent();
            Window w = new MainWindow();
            w.DataContext = _grid;
            GridZone g = new GridZone( _grid.Current );
            Grid.SetColumn( g, 0 );
            Grid.SetRow( g, 0 );
            Grid myGrid = (Grid)w.Content;
            myGrid.Children.Add( g );
            w.Show();

            Timer();
        }

        private void Timer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += delegate( object s, EventArgs args )
            {
                _grid.SwitchCommand();
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
