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

namespace ItalianPizza.Views
{
    /// <summary>
    /// Lógica de interacción para InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        private string usernameLoggedIn;
        public InventoryPage(string usernameLoggedIn)
        {
            InitializeComponent();
            this.usernameLoggedIn = usernameLoggedIn;
        }


        #region GUI Methods

        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InitialMessageBorder.Visibility = Visibility.Hidden;

                ValidateInventoryTableBodyBorder.Visibility = Visibility.Hidden;
                ValidateInventoryTableBodyListBox.Visibility = Visibility.Hidden;
                ValidateInventoryTableHeaderBorder.Visibility = Visibility.Hidden;
                ValidateInventoryTableHeaderStackPanel.Visibility = Visibility.Hidden;

                InventoryTableGrid.Visibility = Visibility.Visible;
                InventoryTableHeaderBorder.Visibility = Visibility.Visible;
                InventoryTableHeaderStackPanel.Visibility = Visibility.Visible;
                InventoryTableBodyBorder.Visibility = Visibility.Visible;
                InventoryTableBodyListBox.Visibility = Visibility.Visible;
            }
        }

        public void ShowValidateLayout(object sender, RoutedEventArgs e)
        {
            
            InitialMessageBorder.Visibility = Visibility.Hidden;

            ValidateInventoryTableBodyBorder.Visibility = Visibility.Visible;
            ValidateInventoryTableBodyListBox.Visibility = Visibility.Visible;
            ValidateInventoryTableHeaderBorder.Visibility = Visibility.Visible;
            ValidateInventoryTableHeaderStackPanel.Visibility = Visibility.Visible;

            InventoryTableGrid.Visibility = Visibility.Visible;
            InventoryTableHeaderBorder.Visibility = Visibility.Hidden;
            InventoryTableHeaderStackPanel.Visibility = Visibility.Hidden;
            InventoryTableBodyBorder.Visibility = Visibility.Hidden;
            InventoryTableBodyListBox.Visibility = Visibility.Hidden;
        }

        public void ShowFilters(object sender, RoutedEventArgs e)
        {
            if (!FirstLayerFilterBorder.IsVisible)
            {
                FiltersStackPanel.Visibility = Visibility.Visible;
                FirstLayerFilterBorder.Visibility = Visibility.Visible;
                SecondLayerFilterBorder.Visibility = Visibility.Visible;
            }
            else
            {
                FiltersStackPanel.Visibility = Visibility.Hidden;
                FirstLayerFilterBorder.Visibility = Visibility.Hidden;
                SecondLayerFilterBorder.Visibility = Visibility.Hidden;
            }
        }

        public void HideFilters(object sender, RoutedEventArgs e)
        {
            if (SecondLayerFilterBorder.IsVisible)
            {
                FiltersStackPanel.Visibility = Visibility.Hidden;
                FirstLayerFilterBorder.Visibility = Visibility.Hidden;
                SecondLayerFilterBorder.Visibility = Visibility.Hidden;
            }
        }

        public void HideSpecificOrderInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            OrderInformationGrid.Visibility = Visibility.Hidden;
        }
        private void OpenRegistItem(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            OrderInformationGrid.Visibility = Visibility.Visible;
        }

        private void ShowDeleteLayout(object sender, MouseButtonEventArgs e)
        {
            ThirdLayerDeleteBorder.Visibility = Visibility.Visible;
            QuarterLayerDeleteBorder.Visibility = Visibility.Visible;
            DeleteItemGrid.Visibility = Visibility.Visible;
        }

        private void HideCancelGrid(object sender, RoutedEventArgs e)
        {
            ThirdLayerDeleteBorder.Visibility = Visibility.Hidden;
            QuarterLayerDeleteBorder.Visibility = Visibility.Hidden;
            DeleteItemGrid.Visibility = Visibility.Hidden;
        }
        #endregion

    }
}
