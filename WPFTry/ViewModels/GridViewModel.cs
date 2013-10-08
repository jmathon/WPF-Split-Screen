using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFTry.ViewModels
{
    public class SwitchCommand : ICommand
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
            g.SwitchCommand();
        }

        #endregion
    }

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
            m.Pan1.IsActive = true;
            m.Pan2.IsActive = false;
            m.Pan3.IsActive = false;
            m.Pan4.IsActive = false;

            Switch = new SwitchCommand();
            Enter = new EnterCommand();
        }

        public ICommand Switch { get; private set; }

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

            var newPanel = _current.Enter();
            _panels.Add( newPanel );
            _current = newPanel;
        }
    }
}
