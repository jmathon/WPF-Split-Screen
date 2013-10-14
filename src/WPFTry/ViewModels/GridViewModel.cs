using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFTry.ViewModels
{
    public class GridViewModel
    {
        readonly DispatcherTimer _timer = new DispatcherTimer();
        Stack<PanelViewModel> _panels = new Stack<PanelViewModel>();

        public PanelViewModel Current
        {
            get
            {
                return _panels.Peek();
            }
        }

        public int MaxDeep { get { return Int32.Parse( ConfigurationManager.AppSettings["MaxDeep"] ); } }

        public GridViewModel()
        {
            var m = new PanelViewModel();
            _panels.Push( m );
        }

        public void SwitchCommand()
        {
            Debug.Assert( Current != null );
            _timer.Tick += delegate( object s, EventArgs args )
            {
                Current.Switch();
            };
            _timer.Interval = new TimeSpan( 0, 0, 0, 0, Int32.Parse( ConfigurationManager.AppSettings["TimeToSwitch"] ) );
            _timer.Start();

        }

        public void EnterCommand()
        {
            Debug.Assert( Current != null );
            Debug.Assert( _panels.Count > 0 );

            if( (_panels.Count - 1) < MaxDeep )
            {
                _timer.Stop();
                var newPanel = Current.Enter();
                _panels.Push( newPanel );
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                MessageBox.Show( "You have reached the max deep !", "Warning...", MessageBoxButton.OK, MessageBoxImage.Exclamation );
            }
        }

        public delegate void ExitNodeHandler( PanelViewModel p );
        public event ExitNodeHandler ExitNode;

        public void ExitCommand()
        {
            Debug.Assert( Current != null );
            Debug.Assert( _panels.Count > 0 );

            var panelToRemove = _panels.Pop();
            panelToRemove.Exit( _panels.Count );
        }
    }
}
