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
        IList<PanelViewModel> _panels = new List<PanelViewModel>();
        PanelViewModel _current = null;

        public PanelViewModel Current
        {
            get
            {
                return _current;
            }
        }

        public int MaxDeep { get { return Int32.Parse( ConfigurationManager.AppSettings["MaxDeep"] ); } }

        public GridViewModel()
        {
            var m = new PanelViewModel();
            _panels.Add( m );
            _current = m;
        }

        public void SwitchCommand()
        {
            Debug.Assert( _current != null );
            _timer.Tick += delegate( object s, EventArgs args )
            {
                _current.Switch();
            };
            _timer.Interval = new TimeSpan( 0, 0, 0, 0, Int32.Parse( ConfigurationManager.AppSettings["TimeToSwitch"] ) );
            _timer.Start();

        }

        public void EnterCommand()
        {
            Debug.Assert( _current != null );
            Debug.Assert( _panels.Count > 0 );

            if( (_panels.Count - 1) < MaxDeep )
            {
                _timer.Stop();
                var newPanel = _current.Enter();
                _panels.Add( newPanel );
                _current = newPanel;
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                MessageBox.Show( "You have reached the max deep !", "Warning...", MessageBoxButton.OK, MessageBoxImage.Exclamation );
            }
        }
    }
}
