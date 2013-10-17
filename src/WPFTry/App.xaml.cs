using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
        int selectedScreen = -1;
        int _loop = 0;

        public int MaxLoop { get { return _windows.Count * Int32.Parse( ConfigurationManager.AppSettings["ScrollingBeforeStop"] ); } }

        IList<MainWindow> _windows = new List<MainWindow>();

        public App()
        {
            InitializeComponent();

            foreach( Screen s in Screen.AllScreens.Reverse() )
                ConfigureScreen( s );

            if( _windows.Count == 1 )
            {
                MainWindow w = _windows[0];
                WindowViewModel wdc = (WindowViewModel)w.DataContext;
                CreateFirstGrid( w, wdc, true );
            }
            else
            {
                _timer.Tick += delegate( object s, EventArgs args )
                {
                    SwitchWindow();
                };
                _timer.Interval = new TimeSpan( 0, 0, 0, 0, Int32.Parse( ConfigurationManager.AppSettings["TimeToSwitch"] ) );
                _timer.Start();
            }
        }

        public MainWindow CurrentWindow { get { return _windows[selectedScreen]; } }

        #region Screens configuration

        /// <summary>
        /// Configure a MainWindow in function of a Screen object
        /// </summary>
        /// <param name="screen"></param>
        void ConfigureScreen( Screen screen )
        {
            MainWindow w = new MainWindow();
            w.Closed += ( o, e ) =>
            {
                ((MainWindow)o).IsClosed = true;
            };

            w.Left = screen.WorkingArea.Left;
            w.Top = screen.WorkingArea.Top;

            w.Show();
            w.WindowState = WindowState.Maximized;
            w.KeyUp += EnterKeyUp;
            w.Closed += WindowClosed;

            _windows.Add( w );
        }

        /// <summary>
        /// This method switch the current used window
        /// </summary>
        void SwitchWindow()
        {
            if( _loop++ < MaxLoop )
            {
                if( selectedScreen < _windows.Count - 1 ) selectedScreen++;
                else selectedScreen = 0;

                if( selectedScreen > 0 ) ((WindowViewModel)_windows[selectedScreen - 1].DataContext).IsActive = false;
                else ((WindowViewModel)_windows[_windows.Count - 1].DataContext).IsActive = false;

                ((WindowViewModel)_windows[selectedScreen].DataContext).IsActive = true;
                _windows[selectedScreen].Focus();
            }
            else
            {
                _loop = 0;
                _timer.Stop();

                PauseAllWindows();
            }
        }

        public bool ArePaused { get { return _windows.All( a => ((WindowViewModel)a.DataContext).IsPause ); } }

        private void PauseAllWindows()
        {
            foreach( var w in _windows )
                ((WindowViewModel)w.DataContext).Pause();
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
                    if( _timer.IsEnabled )
                    {
                        _timer.Stop();
                        _windows.Where( a => a != w ).All( ( a ) =>
                            {
                                a.Hide();
                                return true;
                            }
                        );

                        CreateFirstGrid( w, wdc );
                        _loop = 0;
                    }
                    else
                    {
                        PauseAllWindows();
                        _timer.Start();
                    }
                }
                else
                {
                    _timer.Stop();
                    wdc.Enter();
                }
            }
            else if( args.Key == System.Windows.Input.Key.F12 )
            {
                WindowViewModel wdc = (WindowViewModel)w.DataContext;
                if( wdc.IsEnter )
                {
                    wdc.Exit();
                }
                else
                {
                    if( !ArePaused )
                    {
                        _loop = 0;
                        _timer.Stop();
                        PauseAllWindows();
                    }
                }
            }
        }

        private void CreateFirstGrid( MainWindow w, WindowViewModel wdc, bool onlyOneMode = false )
        {
            Grid myGrid = w.MainWindowGrid;
            myGrid.Children.Add( wdc.Enter( onlyOneMode ) );
            wdc.GridOwned.ExitNode += ExitGridNode;
        }

        /// <summary>
        /// This method will be executed when a grid exited event is sent by a main grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExitGridNode( object sender, Events.ExitGridEventArgs e )
        {
            if( _windows.Count > 1 )
            {
                foreach( var w in _windows )
                {
                    if( !w.IsClosed )
                    {
                        w.Show();
                        Grid myGrid = w.MainWindowGrid;
                        myGrid.Children.Clear();
                    }
                }
                _windows[selectedScreen].Focus();
                Dispatcher.Invoke( new Action( () => _timer.Start() ) );
            }
            else
            {
                Debug.Assert( _windows.Count > 0 );
                WindowViewModel wdc = (WindowViewModel)_windows[0].DataContext;
                wdc.GridOwned.RestartSwitch();
            }
        }

        void WindowClosed( object sender, EventArgs e )
        {
            bool canCloseApp = true;
            foreach( var w in _windows )
            {
                if( w.Visibility != Visibility.Hidden && !w.IsClosed )
                    canCloseApp = false;
            }

            if( canCloseApp ) System.Windows.Application.Current.Shutdown();
        }

        [STAThread]
        public static void Main( string[] args )
        {
            App app = new App();
            app.Run();
        }
    }
}
