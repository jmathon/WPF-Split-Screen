using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFTry.ViewModels;
using WPFTry.Views;

namespace WPFTry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            GridViewModel grid = new GridViewModel();
            Window w = new MainWindow();
            w.DataContext = grid;
            GridZone g = grid.Current.VisualElement;
            Grid.SetColumn( g, 0 );
            Grid.SetRow( g, 0 );
            Grid myGrid = (Grid)w.Content;
            myGrid.Children.Add( g );
            w.Show();
        }

        [STAThread]
        public static void Main( string[] args )
        {
            App app = new App();
            app.Run();
        }
    }
}
