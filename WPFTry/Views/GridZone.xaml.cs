using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFTry.ViewModels;

namespace WPFTry.Views
{
    /// <summary>
    /// Interaction logic for GridZone.xaml
    /// </summary>
    public partial class GridZone : UserControl
    {
        public GridZone( PanelViewModel panel )
        {
            DataContext = panel;
            InitializeComponent();

            panel.EnterNow += OnEnterNow;
        }

        private void OnEnterNow( object sender, EnterNowEventArgs e )
        {
            PanelViewModel p = (PanelViewModel)sender;
            DockPanel dp = null;

            if( p.Pan1.IsActive ) dp = p.VisualElement.Pan1;
            if( p.Pan2.IsActive ) dp = p.VisualElement.Pan2;
            if( p.Pan3.IsActive ) dp = p.VisualElement.Pan3;
            if( p.Pan4.IsActive ) dp = p.VisualElement.Pan4;

            if( dp != null )
            {
                e.Panel.Pan1.IsActive = true;
                dp.Children.Add( e.Panel.VisualElement );
            }
        }
    }
}
