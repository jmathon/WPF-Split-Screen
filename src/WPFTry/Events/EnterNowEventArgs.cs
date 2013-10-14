using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFTry.ViewModels;

namespace WPFTry.Events
{
    public class EnterNowEventArgs : EventArgs
    {
        readonly PanelViewModel _panel;
        readonly int _currentPos;

        public EnterNowEventArgs( PanelViewModel panel, int currentPos )
        {
            _panel = panel;
            _currentPos = currentPos;
        }

        public PanelViewModel Panel { get { return _panel; } }

        public int CurrentPosition { get { return _currentPos; } }
    }
}
