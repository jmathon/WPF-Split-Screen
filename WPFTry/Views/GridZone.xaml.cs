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
using WPFTry.Events;
using WPFTry.ViewModels;

namespace WPFTry.Views
{
    /// <summary>
    /// Interaction logic for GridZone.xaml
    /// </summary>
    public partial class GridZone : UserControl
    {
        readonly PanelViewModel _panel;
        IList<DockPanel> _dockPanels = new List<DockPanel>();

        public GridZone( PanelViewModel panel )
        {
            _panel = panel;
            DataContext = panel;
            InitializeComponent();

            panel.EnterNow += OnEnterNow;

            _panel.CreatePanels();
            CreateDefinitions();
            CreatePanels();
        }

        void CreateDefinitions()
        {
            for( int i = 0; i < _panel.MaxColumnByRowProperty; i++ )
            {
                SplitGrid.ColumnDefinitions.Add( new ColumnDefinition() );
            }

            for( int i = 0; i < _panel.MaxRowProperty; i++ )
            {
                SplitGrid.RowDefinitions.Add( new RowDefinition() );
            }
        }

        void CreatePanels()
        {
            int nbPanels = _panel.MaxColumnByRowProperty * _panel.MaxRowProperty;

            int column = 0;
            int row = 0;
            bool rightDirection = true;
            for( int i = 0; i < nbPanels; i++ )
            {
                DockPanel dp = new DockPanel();
                dp.DataContext = _panel.Panels[i];
                dp.SetBinding( DockPanel.BackgroundProperty, new Binding( "IsActive" ) { Converter = new BooleanToColor() } );

                Grid.SetColumn( dp, column );
                Grid.SetRow( dp, row );
                SplitGrid.Children.Add( dp );

                if( rightDirection ) column++;
                else column--;

                if( column >= _panel.MaxColumnByRowProperty && rightDirection )
                {
                    row++;
                    column--;
                    rightDirection = false;
                }
                else if( column == -1 && !rightDirection )
                {
                    row++;
                    rightDirection = true;
                    column++;
                }
                _dockPanels.Add( dp );
            }
        }

        private void OnEnterNow( object sender, EnterNowEventArgs e )
        {
            PanelViewModel p = (PanelViewModel)sender;
            DockPanel dp = _dockPanels[e.CurrentPosition];

            if( dp != null )
            {
                dp.Children.Add( new GridZone( e.Panel ) );
            }
        }
    }
}
