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
        bool _isEnter = false;

        public WindowViewModel()
        {
            IsActive = false;
            GridOwned = new GridViewModel();
            GridOwned.ExitNode += Exit;
        }

        void Exit( PanelViewModel sender )
        {
        }

        public bool IsActive { get; set; }

        public bool IsEnter { get { return _isEnter; } }

        public GridViewModel GridOwned { get; set; }

        public GridZone Enter()
        {
            if( !_isEnter )
            {
                GridZone g = new GridZone( GridOwned.Current );
                Grid.SetColumn( g, 0 );
                Grid.SetRow( g, 0 );

                GridOwned.SwitchCommand();

                _isEnter = true;
                return g;
            }
            GridOwned.EnterCommand();
            return null;
        }
    }
}
