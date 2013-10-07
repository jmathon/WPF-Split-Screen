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

namespace WPFTry.Views
{
    /// <summary>
    /// Interaction logic for GridZone.xaml
    /// </summary>
    public partial class GridZone : UserControl
    {
        public GridZone()
        {
            InitializeComponent();
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            PanAbstract viewModel = (PanAbstract)base.DataContext;

            var panToShow = viewModel.Pan1.IsActive ? viewModel.Pan2 : viewModel.Pan2.IsActive ? viewModel.Pan3 : viewModel.Pan3.IsActive ? viewModel.Pan4 : viewModel.Pan4.IsActive ? viewModel.Pan1 : viewModel.Pan1;
            viewModel.SetActive( panToShow );
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            PanAbstract viewModel = (PanAbstract)base.DataContext;
            var panToModify = viewModel.Pan1.IsActive ? Pan1 : viewModel.Pan2.IsActive ? Pan2 : viewModel.Pan3.IsActive ? Pan3 : viewModel.Pan4.IsActive ? Pan4 : Pan1;
            var currentPan = viewModel.Pan1.IsActive ? viewModel.Pan1 : viewModel.Pan2.IsActive ? viewModel.Pan2 : viewModel.Pan3.IsActive ? viewModel.Pan3 : viewModel.Pan4.IsActive ? viewModel.Pan4 : viewModel.Pan1;
            viewModel.Enter( currentPan, panToModify );
        }
    }
}
