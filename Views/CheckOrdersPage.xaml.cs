using MaterialDesignThemes.Wpf;
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
    /// Lógica de interacción para CheckOrdersPage.xaml
    /// </summary>
    public partial class CheckOrdersPage : Page
    {
        private string usernameLoggedIn;

        public CheckOrdersPage(string usernameLoggedIn)
        {
            InitializeComponent();
            this.usernameLoggedIn = usernameLoggedIn;
        }

        #region GUI Methods  

        public void ShowOrderDialogs(bool isEnabled)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            OrderInformationGrid.Visibility = Visibility.Visible;
            CustomerNameComboBox.IsEnabled = isEnabled;
            OrderTypeComboBox.IsEnabled = isEnabled;
            OrderAddressComboBox.IsEnabled = isEnabled;
        }

        public void ShowOrderRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(true);
            OrderHeaderTextBlock.Text = "Registro de Pedido";

            RegisterCustomerButton.Visibility = Visibility.Visible;
            AcceptOrderRegistrationButtton.Visibility = Visibility.Visible;
            UpdateOrderButton.Visibility = Visibility.Collapsed;
            CancelOrderButton.Visibility = Visibility.Collapsed;
            GenerateOrderTicketButton.Visibility = Visibility.Collapsed;
            CustomerNameComboBox.SelectedIndex = -1;
            OrderTypeComboBox.SelectedIndex = -1;
            OrderAddressComboBox.SelectedIndex = -1;
            OrderDateDatePicker.SelectedDate = DateTime.Now;
            OrderDateTimePicker.SelectedTime = DateTime.Now;
        }

        public void ShowSpecificOrderInformation(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(false);
            OrderHeaderTextBlock.Text = "Pedido";
            RegisterCustomerButton.Visibility = Visibility.Collapsed;
            AcceptOrderRegistrationButtton.Visibility = Visibility.Collapsed;
            UpdateOrderButton.Visibility = Visibility.Visible;
            CancelOrderButton.Visibility = Visibility.Visible;
            GenerateOrderTicketButton.Visibility = Visibility.Visible;
        }

        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InitialMessageBorder.Visibility = Visibility.Hidden;
                OrderTableGrid.Visibility = Visibility.Visible;
            }
        }

        public void ShowFilters(object sender, RoutedEventArgs e)
        {
            if (!FirstLayerFilterBorder.IsVisible)
            {
                FiltersStackPanel.Visibility = Visibility.Visible;
                FirstLayerFilterBorder.Visibility = Visibility.Visible;
                SecondLayerFilterBorder.Visibility = Visibility.Visible;
            } else
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

        public void ResetSearchFilters(object sender, RoutedEventArgs e)
        {
            CustomerFilterRadioButton.IsChecked = false;
            DateFilterRadioButton.IsChecked = false;
            StatusFilterRadioButton.IsChecked = false;
            SearchTextBox.Text = "";
            InitialMessageBorder.Visibility = Visibility.Visible;
            OrderTableGrid.Visibility = Visibility.Hidden;
        }

        public void HideSpecificOrderInformation(object sender, RoutedEventArgs e)
        {
            bool isRegister = OrderHeaderTextBlock.Text.Equals("Registro de Pedido") ? true : false;

            if  (isRegister && !InvalidFieldsGrid.IsVisible)
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            } else
            {
                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                OrderInformationGrid.Visibility = Visibility.Hidden;
                BackToOrderRegistration(sender, e);
            }
        }

        public void BackToOrderRegistration(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
