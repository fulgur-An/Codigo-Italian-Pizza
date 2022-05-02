using ItalianPizza.BusinessObjects;
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

namespace ItalianPizza.Views
{
    /// <summary>
    /// Lógica de interacción para ListCustomers.xaml
    /// </summary>
    public partial class ListCustomersPage : Page
    {
        private CustomerViewModel customer = new CustomerViewModel();

        private string usernameLoggedIn;

        private readonly NotificationManager notificationManager = new NotificationManager();

        private List<string> addressList = new List<string>();
        public ListCustomersPage(string usernameLoggedIn)
        {
            InitializeComponent();
            DataContext = customer;
            this.usernameLoggedIn = usernameLoggedIn;
        }

        #region GUI Methods
        public void ShowCustomerDialogs(bool isEnabled)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            CustomerInformationGrid.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Hidden;
        }

        public void ShowCustomerRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            ShowCustomerDialogs(true);
            CustomerHeaderTextBlock.Text = "Registro de cliente";

            AddAddressButton.Visibility = Visibility.Visible;
            DeleteAddressButton.Visibility = Visibility.Visible;
            AcceptButtonTextBlock.Text = "Registrar";
            AcceptButton.Visibility = Visibility.Visible;
            DateOfBirthDatePicker.SelectedDate = DateTime.Now;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";

            HintAssist.SetHelperText(CustomerNameTextBox, "Ingrese nombre del cliente");
            HintAssist.SetHelperText(CustomerLastNameTextBox, "Ingrese apellidos del cliente");
            HintAssist.SetHelperText(CustomerEmailTextBox, "Ingrese correo eléctronico");
            HintAssist.SetHelperText(CustomerPhoneTextBox, "Ingrese teléfono del cliente");
            HintAssist.SetHelperText(DateOfBirthDatePicker, "Ingrese fecha de nacimiento");
            HintAssist.SetHelperText(StreetNameTextBox, "Ingrese nombre de la calle");
            HintAssist.SetHelperText(OutsideNumberTextBox, "Ingrese número exterior");
            HintAssist.SetHelperText(InsideNumberTextBox, "Ingrese npumero interior");
            HintAssist.SetHelperText(ColonyTextBox, "Ingrese colonia");
            HintAssist.SetHelperText(CityTextBox, "Ingrese ciudad");
            HintAssist.SetHelperText(PostalCodeTextBox, "Ingrese código postal");
        }

        public void ShowSpecificCustomerInformation(object sender, RoutedEventArgs e)
        {
            ShowCustomerDialogs(false);
            CustomerHeaderTextBlock.Text = "Cliente";
            AddressHeaderTextBlock.Text = "Direcciones";
            AcceptButton.Visibility = Visibility.Hidden;
            AddAddressButton.Visibility = Visibility.Hidden;
            DeleteAddressButton.Visibility = Visibility.Hidden;

            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Información detallada";

            HintAssist.SetHelperText(CustomerNameTextBox, string.Empty);
            HintAssist.SetHelperText(CustomerLastNameTextBox, string.Empty);
            HintAssist.SetHelperText(CustomerEmailTextBox, string.Empty);
            HintAssist.SetHelperText(CustomerPhoneTextBox, string.Empty);
            HintAssist.SetHelperText(DateOfBirthDatePicker, string.Empty);
            HintAssist.SetHelperText(StreetNameTextBox, string.Empty);
            HintAssist.SetHelperText(OutsideNumberTextBox, string.Empty);
            HintAssist.SetHelperText(InsideNumberTextBox, string.Empty);
            HintAssist.SetHelperText(ColonyTextBox, string.Empty);
            HintAssist.SetHelperText(CityTextBox, string.Empty);
            HintAssist.SetHelperText(PostalCodeTextBox, string.Empty);
        }

        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InitialMessageBorder.Visibility = Visibility.Hidden;
                CustomerTableGrid.Visibility = Visibility.Visible;
            }
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

        public void ShowSelectedFilter(object sender, RoutedEventArgs e)
        {
            if (CustomerFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Cliente");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el nombre del cliente");
            }
            else if (AddressFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Dirección");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa la dirección del cliente");
            }
            else if (PhoneFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Teléfono");
                HintAssist.SetHelperText(SearchTextBox, "Ingrese el teléfono del cliente");
            }
        }

        public void ResetSearchFilters(object sender, RoutedEventArgs e)
        {
            CustomerFilterRadioButton.IsChecked = false;
            AddressFilterRadioButton.IsChecked = false;
            AddressFilterRadioButton.IsChecked = false;
            SearchTextBox.Text = "";
            InitialMessageBorder.Visibility = Visibility.Visible;
            CustomerTableGrid.Visibility = Visibility.Hidden;
            SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
            HintAssist.SetHint(SearchTextBox, "Buscar");
            HintAssist.SetHelperText(SearchTextBox, "Selecciona un filtro de búsqueda");
        }

        public void BackToCustomerRegistration(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            CustomerInformationGrid.Visibility = Visibility.Visible;

            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
        }

        private void ExitSpecificCustomerInformation(object sender, RoutedEventArgs e)
        {
            if (CustomerHeaderTextBlock.Text == "Registro de cliente")
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            }
            else if (CustomerHeaderTextBlock.Text == "Actualización de cliente")
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                CustomerInformationGrid.Visibility = Visibility.Hidden;
            }
        }

        private void HideSpecificCustomerInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            CustomerInformationGrid.Visibility = Visibility.Hidden;

            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;
        }

        public void ShowCustomerUpdateDialog(object sender, RoutedEventArgs e)
        {
            ShowCustomerDialogs(true);
            CustomerHeaderTextBlock.Text = "Actualización de cliente";

            AddAddressButton.Visibility = Visibility.Visible;
            DeleteAddressButton.Visibility = Visibility.Visible;
            AcceptButtonTextBlock.Text = "Guardar";
            AcceptButton.Visibility = Visibility.Visible;
            DateOfBirthDatePicker.SelectedDate = DateTime.Now;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";

            HintAssist.SetHelperText(CustomerNameTextBox, string.Empty);
            HintAssist.SetHelperText(CustomerLastNameTextBox, string.Empty);
            HintAssist.SetHelperText(CustomerEmailTextBox, string.Empty);
            HintAssist.SetHelperText(CustomerPhoneTextBox, string.Empty);
            HintAssist.SetHelperText(DateOfBirthDatePicker, string.Empty);
            HintAssist.SetHelperText(StreetNameTextBox, string.Empty);
            HintAssist.SetHelperText(OutsideNumberTextBox, string.Empty);
            HintAssist.SetHelperText(InsideNumberTextBox, string.Empty);
            HintAssist.SetHelperText(ColonyTextBox, string.Empty);
            HintAssist.SetHelperText(CityTextBox, string.Empty);
            HintAssist.SetHelperText(PostalCodeTextBox, string.Empty);
        }

        public void AddAddress(object sender, RoutedEventArgs e)
        {
            int addressNumber = AddressListBox.Items.Count + 1;
            string addAddress = 
            StreetNameTextBox.Text + " #" + 
            OutsideNumberTextBox.Text + ", " +
            InsideNumberTextBox.Text + ", Col" +
            ColonyTextBox.Text + ", " +
            CityTextBox.Text + ", " +
            PostalCodeTextBox.Text;
            AddressListBox.Items.Add(addressNumber + ".\n" + addAddress);
            StreetNameTextBox.Text = "";
            OutsideNumberTextBox.Text = "";
            InsideNumberTextBox.Text = "";
            ColonyTextBox.Text = "";
            CityTextBox.Text = "";
            PostalCodeTextBox.Text = "";
            DeleteAddressButton.IsEnabled = true;
        }

        public void DeleteAddress(object sender, RoutedEventArgs e)
        {
            if (AddressListBox.SelectedItem != null)
            {
                AddressListBox.Items.RemoveAt
                (
                    AddressListBox.Items.IndexOf(AddressListBox.SelectedItem)
                );

                addressList.Clear();

                foreach (var step in AddressListBox.Items)
                {
                    string[] value = step.ToString().Split('\n');
                    addressList.Add(value[1]);
                }

                AddressListBox.Items.Clear();

                for (int i = 0; i < addressList.Count; i++)
                {
                    AddressListBox.Items.Add((i + 1) + ".\n" + addressList[i]);
                }

                DeleteAddressButton.IsEnabled = AddressListBox.Items.Count >= 1;
            }
            else
            {
                ShowUnselectedItemToast();
            }
        }

        public void ShowEmployeDeleteDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void ShowConfirmationToast(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            CustomerInformationGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;

            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;

            //BackToEmployeeRegistration(sender, e);

            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = "Proceso Realizado",
                    Type = NotificationType.Success,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );

        }

        public void ShowUnselectedItemToast()
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Selecciona un elemento de la lista",
                    Message = "Proceso no realizado",
                    Type = NotificationType.Warning,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }
        #endregion
    }
}
