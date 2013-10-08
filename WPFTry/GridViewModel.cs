﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFTry.Views;

namespace WPFTry
{
    public class PanelViewModel : INotifyPropertyChanged
    {
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
            SetNextActive();
        }

        void SetNextActive()
        {
            Next.IsActive = true;

            if( Next != Pan1 ) Pan1.IsActive = false;
            if( Next != Pan2 ) Pan2.IsActive = false;
            if( Next != Pan3 ) Pan3.IsActive = false;
            if( Next != Pan4 ) Pan4.IsActive = false;

        }

        public PanelViewModel Enter( DockPanel dock )
        {
            return EnterInternal( dock );
        }

        PanelViewModel EnterInternal( DockPanel dock )
        {
            var newPanel = new PanelViewModel();

            if( dock == null ) throw new ArgumentNullException( "dock" );
            if( dock.Children.Count == 0 ) dock.Children.Add( newPanel.VisualElement );

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
            m.Pan2.IsActive = true;
            m.Pan3.IsActive = true;
            m.Pan4.IsActive = true;
        }

        void Switch()
        {
            Debug.Assert( _current != null );
            _current.Switch();
        }

        void Enter( DockPanel dock )
        {
            Debug.Assert( _current != null );
            Debug.Assert( _panels.Count > 0 );

            var newPanel = _current.Enter( dock );
            _panels.Add( newPanel );
            _current = newPanel;
        }
    }
}
