using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPFTry.ViewModels
{
    public class EnterCommand : ICommand
    {
        #region ICommand Members

        public bool CanExecute( object parameter )
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute( object gridView )
        {
            GridViewModel g = (GridViewModel)gridView;
            g.EnterCommand();
        }

        #endregion
    }

    public class GridViewModel
    {
        public int MaxDeep { get { return Int32.Parse( ConfigurationManager.AppSettings["MaxDeep"] ); } }

        IList<PanelViewModel> _panels = new List<PanelViewModel>();
        PanelViewModel _current = null;

        public PanelViewModel Current
        {
            get
            {
                return _current;
            }
        }

        public GridViewModel()
        {
            var m = new PanelViewModel();
            _panels.Add( m );
            _current = m;

            Enter = new EnterCommand();
        }

        public ICommand Enter { get; set; }

        public void SwitchCommand()
        {
            Debug.Assert( _current != null );
            _current.Switch();
        }

        public void EnterCommand()
        {
            Debug.Assert( _current != null );
            Debug.Assert( _panels.Count > 0 );

            if( (_panels.Count - 1) < MaxDeep )
            {
                var newPanel = _current.Enter();
                _panels.Add( newPanel );
                _current = newPanel;
            }
            else
            {
                MessageBox.Show( "You have reached the max deep !", "Warning...", MessageBoxButton.OK, MessageBoxImage.Exclamation );
            }
        }
    }
}
