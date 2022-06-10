using Backend.Contracts;
using Backend.Service;
using ItalianPizza.BusinessObjects;
using MaterialDesignThemes.Wpf;
using Notifications.Wpf;
using Server;
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
        private ServerItalianPizzaProxy serverProxy;
        private IItalianPizzaService channel;
        private CustomerViewModel customer = new CustomerViewModel();
        private List<AddressContract> allAddresses = new List<AddressContract>();
        private List<int> addressIdRemoved = new List<int>();
        private string usernameLoggedIn;
        private readonly NotificationManager notificationManager = new NotificationManager();

        
        private enum statusOfCustomers
        {
            Available
        }

        public ListCustomersPage(string usernameLoggedIn)
        {
            InitializeComponent();
            DataContext = customer;
            this.usernameLoggedIn = usernameLoggedIn;
            RequestCustomerList();
        }

        #region Request

        public void RequestRegisterCustomer()
        {
            DateTime dateOfBirtCustomer = (DateTime)DateOfBirthDatePicker.SelectedDate;
            bool isOfLegalAge = (DateTime.Now.Year - dateOfBirtCustomer.Year) >= 18;

            CustomerContract customer = new CustomerContract
            {
                Email = CustomerEmailTextBox.Text,
                Name = CustomerNameTextBox.Text,
                LastName = CustomerLastNameTextBox.Text,
                Phone = CustomerPhoneTextBox.Text,
                Status = statusOfCustomers.Available.ToString(),
                DateOfBirth = dateOfBirtCustomer,
                IsEnabled = true,
                IdEmployee = 1
            };

            List<AddressContract> addresses = new List<AddressContract>();

            foreach (var  address in AddressListBox.Items)
            {
                addresses.Add((AddressContract)address);
            }

            if (isOfLegalAge)
            {
                string header = CustomerHeaderTextBlock.Text;

                if (string.Equals(header, "Registro de cliente"))
                {
                    ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                    service.RegisterCustomerEvent += ConfirmRegistrationCustomer;
                    serverProxy = new ServerItalianPizzaProxy(service);
                    channel = serverProxy.ChannelFactory.CreateChannel();
                    channel.RegisterCustomer(customer, addresses);
                }
                else if (string.Equals(header, "Actualización de cliente"))
                {
                    int customerId = ((CustomerContract)CustomerListBox.SelectedItem).IdUserCustomer;
                    customer.IdUserCustomer = customerId;
                    ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                    service.UpdateCustomerEvent += ConfirmRegistrationCustomer;
                    serverProxy = new ServerItalianPizzaProxy(service);
                    channel = serverProxy.ChannelFactory.CreateChannel();
                    channel.UpdateCustomer(customer, addresses);
                }
            }
            else
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "El Cliente debe tener al menos 18 años");
                ShowCustomerDialogs();
            }
        }

        public void RequestCustomerList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetCustomerListSortedByNameEvent += LoadAllCustomers;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetCustomerListSortedByName();
        }

        public void RequestDeleteCustomer(int idCustomer)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.DeleteCustomerByIdEvent += ConfirmDeleteCustomer;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.DeleteCustomerById(idCustomer);
        }

        #endregion

        #region Implementation methods

        public void ConfirmRegistrationCustomer(int result)
        {
            if (result > 0)
            {
                CustomerListBox.Items.Clear();
                RequestCustomerList();
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
            }
            else if (result == -1)
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Nombre de Cliente ya registrado");
                ShowCustomerDialogs();
            }
            else
            {
                NotificationType notificationType = NotificationType.Error;
                PersonalizeToast(notificationType, "Proceso no realizado. Intentelo de nuevo");
                ShowCustomerDialogs();
            }
        }

        public void LoadAllCustomers(List<CustomerContract> customerContracts, List<AddressContract> addressContracts)
        {
            allAddresses.Clear();

            foreach (AddressContract addressContract in addressContracts)
            {
                allAddresses.Add(addressContract);
            }

            foreach (CustomerContract customer in customerContracts)
            {
                foreach (var addreess in addressContracts)
                {
                    string fullAddres = "\n• " + addreess.StreetName + " #" + addreess.OutsideNumber + " Ext#" + addreess.InsideNumber +
                        " Col. " + addreess.Colony + ", CP " + addreess.PostalCode + ", " + addreess.City + "\n";

                    if (addreess.IdCustomer == customer.IdUserCustomer)
                    {
                        customer.FullAddress += fullAddres;
                    }

                }
                CustomerListBox.Items.Add(customer);
            }
        }

        public void ConfirmDeleteCustomer(int result)
        {
            if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");                
                CustomerListBox.Items.Clear();
                RequestCustomerList();
            }
        }

        #endregion

        #region GUI Methods

        public void ShowCustomerDialogs()
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            CustomerInformationGrid.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Hidden;
        }

        public void ShowCustomerRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            ShowCustomerDialogs();
            RemoveValidationAssistant(false);
            CustomerHeaderTextBlock.Text = "Registro de cliente";

            //AddAddressButton.Visibility = Visibility.Visible;
            //DeleteAddressButton.Visibility = Visibility.Visible;
            AcceptButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Collapsed;
            //DateOfBirthDatePicker.SelectedDate = DateTime.Now;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";

            CustomerNameTextBox.Text = "";
            CustomerLastNameTextBox.Text = "";
            CustomerEmailTextBox.Text = "";
            CustomerPhoneTextBox.Text = "";
            //DateOfBirthDatePicker.SelectedDate = null;
            DateOfBirthDatePicker.Text = "";
            OutsideNumberTextBox.Text = "";
            InsideNumberTextBox.Text = "";
            ColonyTextBox.Text = "";
            CityTextBox.Text = "";
            PostalCodeTextBox.Text = "";
            AddressListBox.Items.Clear();

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
            AdressesViewListBox.Items.Clear();
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            SeventhLayerBorder.Visibility = Visibility.Visible;
            CustomerViewGrid.Visibility = Visibility.Visible;
            CustomerContract customerContract = (CustomerContract)CustomerListBox.SelectedItem;
            CustomerNameViewTextBox.Text = customerContract.Name;
            CustomerLastNameViewTextBox.Text = customerContract.LastName;
            CustomerEmailViewTextBox.Text = customerContract.Email;
            CustomerPhoneViewTextBox.Text = customerContract.Phone;
            CustomerViewDatePicker.SelectedDate = customerContract.DateOfBirth;

            List<string> addresses = customerContract.FullAddress.Split('•').ToList();

            for (int i = 1; i < addresses.Count; i++)
            {
                AdressesViewListBox.Items.Add(addresses[i].Replace("\r\n", "").Replace("\n", "").Replace("\r", ""));
            }
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
            RemoveValidationAssistant(false);
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            CustomerInformationGrid.Visibility = Visibility.Visible;
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
        }

        private void ExitSpecificCustomerInformation(object sender, RoutedEventArgs e)
        {
            string header = CustomerHeaderTextBlock.Text;

            if (string.Equals(header, "Registro de cliente") || string.Equals(header, "Actualización de cliente"))
            {
                RemoveValidationAssistant(true);
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

        public void ExitDetailedInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            CustomerViewGrid.Visibility = Visibility.Hidden;
            SeventhLayerBorder.Visibility = Visibility.Hidden;
        }

        public void ShowCustomerUpdateDialog(object sender, RoutedEventArgs e)
        {
            AddressListBox.Items.Clear();
            ShowCustomerDialogs();
            RemoveValidationAssistant(false);
            CustomerHeaderTextBlock.Text = "Actualización de cliente";
            AddressListBox.Items.Clear();
            DeleteAddressButton.IsEnabled = true;
            //AddAddressButton.Visibility = Visibility.Visible;
            //DeleteAddressButton.Visibility = Visibility.Visible;
            AcceptButton.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";

            CustomerContract customer = (CustomerContract)CustomerListBox.SelectedItem;
            if (customer != null)
            {
                CustomerNameTextBox.Text = customer.Name;
                CustomerLastNameTextBox.Text = customer.LastName;
                DateOfBirthDatePicker.SelectedDate = customer.DateOfBirth;
                CustomerPhoneTextBox.Text = customer.Phone;
                CustomerEmailTextBox.Text = customer.Email;

                foreach (AddressContract address in allAddresses)
                {
                    if (addressIdRemoved.Count > 0)
                    {
                        foreach (int addressId in addressIdRemoved)
                        {
                            if ((address.IdCustomer == customer.IdUserCustomer) &&
                                (address.IdAddresses != addressId))
                            {
                                AddressListBox.Items.Add(address);
                            }
                        }
                    }
                    else
                    {
                        if (address.IdCustomer == customer.IdUserCustomer)
                        {
                            AddressListBox.Items.Add(address);
                        }
                    }
                }
            }
        }

        public void AddAddress(object sender, RoutedEventArgs e)
        {
            AddressContract address = new AddressContract
            {
                Colony = ColonyTextBox.Text,
                City = CityTextBox.Text,
                InsideNumber = InsideNumberTextBox.Text,
                OutsideNumber = OutsideNumberTextBox.Text,
                PostalCode = PostalCodeTextBox.Text,
                StreetName = StreetNameTextBox.Text
            };
            
            AddressListBox.Items.Add(address);

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
                if (SaveButton.IsVisible)
                {
                    addressIdRemoved.Add(((AddressContract)AddressListBox.SelectedItem).IdAddresses);
                }

                AddressListBox.Items.RemoveAt
                (
                    AddressListBox.Items.IndexOf(AddressListBox.SelectedItem)
                );

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

        public void AcceptDeleteConfirmationButton(object sender, RoutedEventArgs e)
        {
            CustomerContract customerContract = (CustomerContract)CustomerListBox.SelectedItem;

            if (customerContract != null)
            {
                RequestDeleteCustomer(customerContract.IdUserCustomer);
                DeleteConfirmationGrid.Visibility = Visibility.Hidden;
                FifthLayerBorder.Visibility = Visibility.Hidden;
            } 
            else
            {
                MessageBox.Show("sin selección");
            }
        }   

        public void SaveRegisterOrUpdateCustomer(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            CustomerInformationGrid.Visibility = Visibility.Hidden;
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;

            RequestRegisterCustomer();
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

        public void PersonalizeToast(NotificationType notificationType, string message)
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = message,
                    Type = notificationType,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }

        public void RemoveValidationAssistant(bool isNotVisible)
        {
            ValidationAssist.SetSuppress(CustomerNameTextBox, isNotVisible);
            ValidationAssist.SetSuppress(CustomerLastNameTextBox, isNotVisible);
            ValidationAssist.SetSuppress(DateOfBirthDatePicker, isNotVisible);
            ValidationAssist.SetSuppress(CustomerPhoneTextBox, isNotVisible);
            ValidationAssist.SetSuppress(CustomerEmailTextBox, isNotVisible);
            ValidationAssist.SetSuppress(StreetNameTextBox, isNotVisible);
            ValidationAssist.SetSuppress(OutsideNumberTextBox, isNotVisible);
            ValidationAssist.SetSuppress(InsideNumberTextBox, isNotVisible);
            ValidationAssist.SetSuppress(ColonyTextBox, isNotVisible);
            ValidationAssist.SetSuppress(CityTextBox, isNotVisible);
            ValidationAssist.SetSuppress(PostalCodeTextBox, isNotVisible);
        }

        #endregion
    }
}
