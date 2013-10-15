using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFTry.Events;
using WPFTry.Views;

namespace WPFTry.ViewModels
{
    public class PanelViewModel : INotifyPropertyChanged
    {
        int loop = 0;
        PanelViewModel _parent = null;
        GridViewModel _grid = null;

        public int MaxColumnByRowProperty { get { return Int32.Parse( ConfigurationManager.AppSettings["MaxColumnByRow"] ); } }

        public int MaxRowProperty { get { return Int32.Parse( ConfigurationManager.AppSettings["MaxRow"] ); } }

        int SwitchLoop { get { return MaxColumnByRowProperty * MaxRowProperty * Int32.Parse( ConfigurationManager.AppSettings["Loop"] ); } }

        IList<PanelViewModel> _panels = new List<PanelViewModel>();

        public PanelViewModel( GridViewModel owner, PanelViewModel parent )
            : this( owner )
        {
            _parent = parent;
        }

        public PanelViewModel( GridViewModel owner )
        {
            _grid = owner;
        }

        public void CreatePanels()
        {
            _panels.Clear();
            int nbPanels = MaxColumnByRowProperty * MaxRowProperty;
            for( int i = 0; i < nbPanels; i++ )
            {
                PanelViewModel p = new PanelViewModel( _grid, this );
                if( i == 0 ) p.IsActive = true;
                _panels.Add( p );
            }
        }

        public event EventHandler<EnterNowEventArgs> EnterNow;

        private void OnEnterNow( EnterNowEventArgs e )
        {
            if( EnterNow != null ) EnterNow( this, e );
        }

        #region INotifyPropertyChanged Members

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged( string name )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if( handler != null )
            {
                handler( this, new PropertyChangedEventArgs( name ) );
            }
        }

        #endregion

        public PanelViewModel Parent { get { return _parent; } }

        public PanelViewModel Current { get { return _panels.Where( a => a.IsActive ).SingleOrDefault() ?? _panels[0]; } }

        public PanelViewModel Next
        {
            get
            {
                int currentIndex = _panels.IndexOf( Current );
                int nextIndex = (currentIndex + 1) < _panels.Count ? currentIndex + 1 : 0;
                return _panels[nextIndex];
            }
        }

        public IList<PanelViewModel> Panels { get { return _panels; } }

        public void Switch()
        {
            if( loop++ < SwitchLoop )
            {
                var n = Next;

                Current.IsActive = false;
                n.IsActive = true;
            }
            else
            {
                loop = 0;
                _grid.ExitCommand();
            }
        }

        public PanelViewModel Enter()
        {
            loop = 0;
            var newPanel = new PanelViewModel( _grid, this );
            OnEnterNow( new EnterNowEventArgs( newPanel, _panels.IndexOf( Current ) ) );
            return newPanel;
        }

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

        public event EventHandler<ExitPanelEventArgs> ExitNode;

        public void Exit()
        {
            if( ExitNode != null )
                ExitNode( this, new ExitPanelEventArgs { Position = 0 } );
        }
    }
}
