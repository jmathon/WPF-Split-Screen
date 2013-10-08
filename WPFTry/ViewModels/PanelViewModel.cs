using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFTry.Views;

namespace WPFTry.ViewModels
{
    public class PanelViewModel : INotifyPropertyChanged
    {
        public event EventHandler<EnterNowEventArgs> EnterNow;

        private void OnEnterNow( EnterNowEventArgs e )
        {
            if( EnterNow != null ) EnterNow( this, e );
        }

        GridZone _visualElement = null;
        public GridZone VisualElement
        {
            get
            {
                if( _visualElement == null ) _visualElement = new GridZone( this );
                return _visualElement;
            }
        }

        #region INotifyPropertyChanged Members

        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public PanelViewModel Current
        {
            get
            {
                return Pan1.IsActive ? Pan1 : Pan2.IsActive ? Pan2 : Pan3.IsActive ? Pan3 : Pan4.IsActive ? Pan4 : Pan1;
            }
        }

        public PanelViewModel Next
        {
            get
            {
                return Pan1.IsActive ? Pan2 : Pan2.IsActive ? Pan3 : Pan3.IsActive ? Pan4 : Pan4.IsActive ? Pan1 : Pan1;
            }
        }

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

        public void Switch()
        {
            var n = Next;

            if( n != Pan1 ) Pan1.IsActive = false;
            if( n != Pan2 ) Pan2.IsActive = false;
            if( n != Pan3 ) Pan3.IsActive = false;
            if( n != Pan4 ) Pan4.IsActive = false;

            n.IsActive = true;
        }

        public PanelViewModel Enter()
        {
            var newPanel = new PanelViewModel();
            OnEnterNow( new EnterNowEventArgs( newPanel ) );
            return newPanel;
        }

        protected virtual void OnPropertyChanged( string name )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if( handler != null )
            {
                handler( this, new PropertyChangedEventArgs( name ) );
            }
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
    }

    public class EnterNowEventArgs : EventArgs
    {
        readonly PanelViewModel _panel;

        public EnterNowEventArgs( PanelViewModel panel )
        {
            _panel = panel;
        }

        public PanelViewModel Panel
        {
            get
            {
                return _panel;
            }
        }
    }
}
