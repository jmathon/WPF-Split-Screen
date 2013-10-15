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
using WPFTry.Events;

namespace WPFTry.ViewModels
{
    public class GridViewModel
    {
        DispatcherTimer _timer = new DispatcherTimer();
        Stack<PanelViewModel> _panels = new Stack<PanelViewModel>();

        /// <summary>
        /// This property getting the current panel that is highlighted
        /// </summary>
        public PanelViewModel Current
        {
            get
            {
                return _panels.Count != 0 ? _panels.Peek() : null;
            }
        }

        /// <summary>
        /// This property getting the max deep
        /// </summary>
        public int MaxDeep { get { return Int32.Parse( ConfigurationManager.AppSettings["MaxDeep"] ); } }

        public GridViewModel()
        {
            var m = new PanelViewModel( this );
            _panels.Push( m );

            _timer.Tick += delegate( object s, EventArgs args )
            {
                if( Current != null )
                    Current.Switch();
                else
                    _timer.Stop();
            };
            _timer.Interval = new TimeSpan( 0, 0, 0, 0, Int32.Parse( ConfigurationManager.AppSettings["TimeToSwitch"] ) );
        }

        /// <summary>
        /// This method drives all panel switching
        /// </summary>
        public void Switch()
        {
            Debug.Assert( Current != null );
            _timer.Start();
        }

        /// <summary>
        /// This method drives all panel entering
        /// </summary>
        public void Enter()
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

        #region Events

        public event EventHandler<ExitGridEventArgs> ExitNode;

        public void ExitCommand()
        {
            if( _panels.Count <= 1 )
            {
                _timer.Stop();
                if( ExitNode != null ) ExitNode( this, new ExitGridEventArgs() );
            }
            else
            {
                var panelToRemove = _panels.Pop();
                if( panelToRemove != null ) panelToRemove.Exit();
                _timer.Start();
            }
        }

        #endregion
    }
}
