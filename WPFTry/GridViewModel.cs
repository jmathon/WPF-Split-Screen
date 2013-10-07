using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFTry.Views;

namespace WPFTry
{
    public class PanelViewModel : PanAbstract
    {
        bool _isActive = false;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
                OnPropertyChanged( "IsActive" );
            }
        }
    }

    public class GridViewModel
    {
        IList<PanelViewModel> _panels = new List<PanelViewModel>();
        PanelViewModel _current = null;

        public GridViewModel()
        {
        }
    }

    public abstract class PanAbstract : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        PanelViewModel _pan1 = null;
        public PanelViewModel Pan1
        {
            get
            {
                SetPanels();
                return _pan1;
            }
        }

        PanelViewModel _pan2 = null;
        public PanelViewModel Pan2
        {
            get
            {
                SetPanels();
                return _pan2;
            }
        }

        PanelViewModel _pan3 = null;
        public PanelViewModel Pan3
        {
            get
            {
                SetPanels();
                return _pan3;
            }
        }

        PanelViewModel _pan4 = null;
        public PanelViewModel Pan4
        {
            get
            {
                SetPanels();
                return _pan4;
            }
        }

        void SetPanels()
        {
            if( _pan1 == null ) _pan1 = new PanelViewModel();
            if( _pan2 == null ) _pan2 = new PanelViewModel();
            if( _pan3 == null ) _pan3 = new PanelViewModel();
            if( _pan4 == null ) _pan4 = new PanelViewModel();
        }

        public void SetActive( PanelViewModel panel )
        {
            if( panel == null ) throw new ArgumentNullException( "panel" );
            panel.IsActive = true;

            if( panel != Pan1 ) Pan1.IsActive = false;
            if( panel != Pan2 ) Pan2.IsActive = false;
            if( panel != Pan3 ) Pan3.IsActive = false;
            if( panel != Pan4 ) Pan4.IsActive = false;

        }

        public void Enter( PanAbstract currentPanel, DockPanel dock )
        {
            if( dock == null ) throw new ArgumentNullException( "dock" );
            if( dock.Children.Count == 0 ) dock.Children.Add( new GridZone() );

            currentPanel.Pan1.IsActive = true;
            currentPanel.Pan2.IsActive = false;
            currentPanel.Pan3.IsActive = false;
            currentPanel.Pan4.IsActive = false;
        }

        protected virtual void OnPropertyChanged( string name )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if( handler != null )
            {
                handler( this, new PropertyChangedEventArgs( name ) );
            }
        }
    }
}
