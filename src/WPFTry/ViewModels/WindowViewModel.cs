using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using WPFTry.Views;

namespace WPFTry.ViewModels
{
    public class WindowViewModel
    {
        bool _isOnlyOne = false;
        bool _isEnter = false;

        public WindowViewModel()
        {
            IsActive = false;
            GridOwned = new GridViewModel();
        }

        public bool IsActive { get; set; }

        public bool IsEnter { get { return _isEnter; } }

        public bool IsOnlyOne { get { return _isOnlyOne; } }

        public GridViewModel GridOwned { get; set; }

        public GridZone Enter( bool isOnlyOne = false )
        {
            if( !_isEnter )
            {
                _isOnlyOne = isOnlyOne;
                GridZone g = new GridZone( GridOwned.Current );
                Grid.SetColumn( g, 0 );
                Grid.SetRow( g, 0 );

                GridOwned.Switch();
                GridOwned.ExitNode += ExitGridNode;

                _isEnter = true;
                return g;
            }
            GridOwned.Enter();
            return null;
        }

        public void Exit()
        {
            GridOwned.ExitCommand();
        }

        void ExitGridNode( object sender, Events.ExitGridEventArgs e )
        {
            if( !IsOnlyOne )
                _isEnter = false;
        }
    }
}
