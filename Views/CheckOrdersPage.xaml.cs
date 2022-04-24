using MaterialDesignThemes.Wpf;
using Notifications.Wpf;
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

        private readonly NotificationManager notificationManager = new NotificationManager();

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
            OrderItemsDataGrid.CanUserAddRows = true;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";

            HintAssist.SetHelperText(CustomerNameComboBox, "Selecciona el nombre de un cliente");
            HintAssist.SetHelperText(OrderDateDatePicker, "Fecha actual establecida");
            HintAssist.SetHelperText(OrderDateTimePicker, "Hora actual establecida");
            HintAssist.SetHelperText(OrderStatusComboBox, "Estatus 'En proceso' establecido");
            HintAssist.SetHelperText(OrderAddressComboBox, "Selecciona el tipo de entrega");
            HintAssist.SetHelperText(OrderTypeComboBox, "Selecciona una dirección del cliente");
        }

        public void ChangeOrderType(object sender, EventArgs e)
        {
            List<int> tableList = new List<int>();
            List<string> orderTypeList = new List<string>()
            {
                "Domicilio", "Local"
            };
            for (int i = 1; i < 10; i++)
            {
                tableList.Add(i);
            }

            string orderType = OrderTypeComboBox.Text;


            if (string.Equals(orderType, "Local"))
            {
                HeaderAddressTextblock.Text = "Mesa";
                OrderAddressComboBox.ItemsSource = tableList;
                HintAssist.SetHelperText(OrderAddressComboBox, "Selecciona el número de mesa");
            }
            else
            {
                HeaderAddressTextblock.Text = "Dirección";
                OrderAddressComboBox.ItemsSource  = new List<string>() {"Fidencio Ocaña #64 Col. Francisco Ferrer Guardia"};
                HintAssist.SetHelperText(OrderAddressComboBox, "Selecciona una dirección del cliente");
            }
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

            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Información detallada";

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

        public void ShowSelectedFilter(object sender, RoutedEventArgs e)
        {
            if (CustomerFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Cliente");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el nombre del cliente");
            }
            else if (DateFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Fecha");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa una fecha en el formato: dd/mm/aaaaa");
            } 
            else if (StatusFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Estatus");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa algún estatus: En Proceso, Entregado, Cancelado");
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
            HintAssist.SetHint(SearchTextBox, "Buscar");
            HintAssist.SetHelperText(SearchTextBox, "Selecciona un filtro de búsqueda");
        }

        public void HideSpecificOrderInformation(object sender, RoutedEventArgs e)
        {
            bool isRegister = OrderHeaderTextBlock.Text.Equals("Registro de Pedido");

            if ((isRegister && !InvalidFieldsGrid.IsVisible) || 
                (SaveOrderButton.IsVisible && !FifthLayerBorder.IsVisible) || 
                (DeleteConfirmationGrid.IsVisible && !FifthLayerBorder.IsVisible))
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;                
            } 
            else
            {
                if (DeleteConfirmationGrid.IsVisible)
                {
                    ShowConfirmationToast(sender, e);
                }

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
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";
            CustomerNameComboBox.IsEnabled = false;
            OrderStatusComboBox.IsEnabled = true;
            OrderAddressComboBox.IsEnabled = false;
            OrderItemsDataGrid.IsEnabled = true;
            OrderItemsDataGrid.CanUserAddRows = true;

            HintAssist.SetHelperText(CustomerNameComboBox, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderDateDatePicker, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderDateTimePicker, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderStatusComboBox, string.Empty);
            HintAssist.SetHelperText(OrderAddressComboBox, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderTypeComboBox, "Este campo no puedo modificarse");
        }

        public void ShowOrderCancellationDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void ShowConfirmationToast(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            OrderInformationGrid.Visibility = Visibility.Hidden;
            BackToOrderRegistration(sender, e);

            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = "Proceso Realizado",
                    Type =  NotificationType.Success,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );

        }
        
        #endregion
    }
}
