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
using System.Windows.Threading;

namespace ItalianPizza.Views
{
    /// <summary>
    /// Lógica de interacción para CheckOrdersPage.xaml
    /// </summary>
    public partial class CheckOrdersPage : Page
    {
        private string usernameLoggedIn;

        private TimeSpan timeSpan;
        private DispatcherTimer dispatcherTimer;

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
            SearchResultMessageTextBlock.Visibility = Visibility.Visible;
            SaveOrderButton.Visibility = Visibility.Collapsed;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Hidden;
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

            HintAssist.SetHelperText(CustomerNameComboBox, string.Empty);
            HintAssist.SetHelperText(OrderDateDatePicker, string.Empty);
            HintAssist.SetHelperText(OrderDateTimePicker, string.Empty);
            HintAssist.SetHelperText(OrderStatusComboBox, string.Empty);
            HintAssist.SetHelperText(OrderAddressComboBox, string.Empty);
            HintAssist.SetHelperText(OrderTypeComboBox, string.Empty);
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
            SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
        }

        public void HideSpecificOrderInformation(object sender, RoutedEventArgs e)
        {
            bool isRegister = OrderHeaderTextBlock.Text.Equals("Registro de Pedido") ? true : false;

            if ((isRegister && !InvalidFieldsGrid.IsVisible) || (SaveOrderButton.IsVisible && !FifthLayerBorder.IsVisible) || (DeleteConfirmationGrid.IsVisible && !FifthLayerBorder.IsVisible))
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
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;
        }

        public void ShowOrderUpdateDialog(object sender, RoutedEventArgs e)
        {
            GenerateOrderTicketButton.Visibility = Visibility.Collapsed;
            UpdateOrderButton.Visibility = Visibility.Collapsed;
            CancelOrderButton.Visibility = Visibility.Collapsed;
            SaveOrderButton.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            CustomerNameComboBox.IsEnabled = true;
            OrderStatusComboBox.IsEnabled = true;
            OrderAddressComboBox.IsEnabled = true;
            OrderItemsDataGrid.IsEnabled = true;
            // OrderItemsDataGrid.CanUserAddRows = true;

            HintAssist.SetHelperText(CustomerNameComboBox, string.Empty);
            HintAssist.SetHelperText(OrderDateDatePicker, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderDateTimePicker, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderStatusComboBox, string.Empty);
            HintAssist.SetHelperText(OrderAddressComboBox, string.Empty);
            HintAssist.SetHelperText(OrderTypeComboBox, "Este campo no puedo modificarse");
        }

        public void ShowOrderCancellationDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void E(object sender, RoutedEventArgs e)
        {
            StarTimer(1);
        }

        public void StarTimer(int seconds)
        {
            timeSpan = TimeSpan.FromSeconds(seconds);

            dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                ConfirmationGrid.Visibility = Visibility.Visible;

                if (timeSpan == TimeSpan.Zero)
                {
                    dispatcherTimer.Stop();
                    FifthLayerBorder.Visibility = Visibility.Hidden;
                    ConfirmationGrid.Visibility = Visibility.Hidden;
                }

                timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            dispatcherTimer.Start();
        }

        #endregion
    }
}
