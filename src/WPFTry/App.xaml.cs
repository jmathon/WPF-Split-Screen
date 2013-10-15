using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
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
        DispatcherTimer _timer = new DispatcherTimer();
        int selectedScreen = 0;
        IList<MainWindow> _windows = new List<MainWindow>();

        public App()
        {
            InitializeComponent();

            foreach( Screen s in Screen.AllScreens )
                ConfigureScreen( s );

            StartScreenScrolling();
        }

        public MainWindow CurrentWindow { get { return _windows[selectedScreen]; } }

        #region Screens configuration

        void StartScreenScrolling()
        {
            selectedScreen = 0;
            _timer.Tick += delegate( object s, EventArgs args )
            {
                SwitchWindow();
            };
            _timer.Interval = new TimeSpan( 0, 0, 0, 0, Int32.Parse( ConfigurationManager.AppSettings["TimeToSwitch"] ) );
            _timer.Start();
        }

        /// <summary>
        /// Configure a MainWindow in function of a Screen object
        /// </summary>
        /// <param name="screen"></param>
        void ConfigureScreen( Screen screen )
        {
            MainWindow w = new MainWindow( new WindowViewModel() );
            w.Left = screen.WorkingArea.Left;
            w.Top = screen.WorkingArea.Top;

            w.Show();
            w.WindowState = WindowState.Maximized;
            w.KeyUp += EnterKeyUp;

            _windows.Add( w );
        }

        /// <summary>
        /// This method switch the current used window
        /// </summary>
        void SwitchWindow()
        {
            if( selectedScreen > 0 ) ((WindowViewModel)_windows[selectedScreen - 1].DataContext).IsActive = false;
            else ((WindowViewModel)_windows[_windows.Count - 1].DataContext).IsActive = false;

            ((WindowViewModel)_windows[selectedScreen].DataContext).IsActive = true;
            _windows[selectedScreen].Focus();

            if( selectedScreen < _windows.Count - 1 ) selectedScreen++;
            else selectedScreen = 0;
        }

        #endregion

        /// <summary>
        /// This method will be executed when a keyup event is sent by the window
        /// </summary>
        /// <param name="sender">MainWindow</param>
        /// <param name="args">KeyEventArgs</param>
        void EnterKeyUp( object sender, System.Windows.Input.KeyEventArgs args )
        {
            MainWindow w = (MainWindow)sender;
            if( args.Key == System.Windows.Input.Key.F11 )
            {
                WindowViewModel wdc = (WindowViewModel)w.DataContext;
                if( !wdc.IsEnter )
                {
                    _windows.Where( a => a != w ).All( ( a ) =>
                        {
                            a.Hide();
                            return true;
                        }
                    );

                    Grid myGrid = w.MainWindowGrid;
                    myGrid.Children.Add( wdc.Enter() );
                    wdc.GridOwned.ExitNode += ExitGridNode;
                    _timer.Stop();
                }
                else
                {
                    wdc.Enter();
                }
            }
            else if( args.Key == System.Windows.Input.Key.F12 )
            {
                WindowViewModel wdc = (WindowViewModel)w.DataContext;
                wdc.Exit();
            }
        }

        void ExitGridNode( object sender, Events.ExitGridEventArgs e )
        {
            foreach( var w in _windows )
            {
                w.Show();
                Grid myGrid = w.MainWindowGrid;
                myGrid.Children.Clear();
            }

            _timer.Start();
        }

        [STAThread]
        public static void Main( string[] args )
        {
            App app = new App();
            app.Run();
        }
    }
}
