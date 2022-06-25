using Backend.Contracts;
using Backend.Service;
using ItalianPizza.BusinessObjects;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Notifications.Wpf;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para CheckProviderPage.xaml
    /// </summary>
    public partial class CheckProviderPage : Page
    {
        private ServerItalianPizzaProxy serverProxy;
        private IItalianPizzaService channel;
        private ProviderViewModel provider = new ProviderViewModel();
        private List<ProviderContract> allProviders = new List<ProviderContract>();
        private List<int> providerIdRemoved = new List<int>();
        private string usernameLoggedIn;
        private readonly NotificationManager notificationManager = new NotificationManager();

        private enum statusOfProviders
        {
            Avaible
        }

        public enum ESearchFilters
        {
            NameFilter = 1,
            RFCFilter = 2,
            NoSelection = 3,
        }

        public CheckProviderPage(string usernameLoggedIn)
        {
            InitializeComponent();
            DataContext = provider;
            this.usernameLoggedIn = usernameLoggedIn;
            RequestProviderList();
        }

        public void RequestDeleteProviderById(int idProvider)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.DeleteProviderByIdEvent += ConfirmDeleteProvider;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.DeleteProviderById(idProvider);
        }

        int globalEmployeeId = 0;

        public void RequestGetProviderList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetProviderListEvent += LoadAllProviders;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetProviderList();
        }

        public ESearchFilters GetFiltersSelected()
        {
            ESearchFilters filterSelected = new ESearchFilters();
            if (ProviderFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.NameFilter;
            }
            else if (RFCFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.RFCFilter;
            }
            else
            {
                filterSelected = ESearchFilters.NoSelection;
            }
            return filterSelected;
        }

        public void ReceiveProviderModification(object sender, RoutedEventArgs e)
        {
            ProviderContract provider = (ProviderContract)ProviderListBox.SelectedItem;

            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            ProviderInformationGrid.Visibility = Visibility.Hidden;
            BackToProviderRegistration(sender, e);
        }

        public void ShowProviderCancellationDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void ShowConfirmationToast(object sender, RoutedEventArgs e)
        {
            RequestRegisterProvider();
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            ProviderInformationGrid.Visibility = Visibility.Hidden;
            BackToProviderRegistration(sender, e);
        }




        public void RequestRegisterProvider()
        {
            ProviderContract provider = new ProviderContract
            {
                Category = ProviderCategoryComboBox.Text,
                Phone = ProviderPhoneTextBox.Text,
                Email = ProviderEmailTextBox.Text,
                Name = ProviderNameTextBox.Text,
                RFC = ProviderRFCTextBox.Text,
            };

            string header = ProviderHeaderTextBlock.Text;

            if (string.Equals(header, "Registro de Proveedor"))
            {
                ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                service.RegisterProviderEvent += ConfirmRegistrationProvider;
                serverProxy = new ServerItalianPizzaProxy(service);
                channel = serverProxy.ChannelFactory.CreateChannel();
                channel.RegisterProvider(provider);
            }
            else if (string.Equals(header, "Actualización de proveedor"))
            {
                int providerId = ((ProviderContract)ProviderListBox.SelectedItem).IdProvider;
                provider.IdProvider = providerId;
                ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                service.UpdateProviderEvent += ConfirmRegistrationProvider;
                serverProxy = new ServerItalianPizzaProxy(service);
                channel = serverProxy.ChannelFactory.CreateChannel();
                channel.UpdateProvider(provider);
            }

        }

        public void RequestProviderList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetProviderListSortedByNameEvent += LoadAllProviders;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetProviderListSortedByName();
        }

        public void RequestDeleteProvider(int idProvider)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.DeleteProviderByIdEvent += ConfirmDeleteProvider;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.DeleteProviderById(idProvider);
        }



        #region Implementation methods
        public void ConfirmRegistrationProvider(int result)
        {
            if (result > 0)
            {
                ProviderListBox.Items.Clear();
                RequestProviderList();
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
            }
            else if (result == -1)
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Nombre de proveedor ya registrado");
                ShowProviderDialogs();
            }
            else
            {
                NotificationType notificationType = NotificationType.Error;
                PersonalizeToast(notificationType, "Proceso no realizado. Intente de nuevo");
                ShowProviderDialogs();
            }
        }

        public void LoadAllProviders(List<ProviderContract> providerContracts)
        {
            foreach (ProviderContract provider in providerContracts)
            {
                ProviderListBox.Items.Add(provider);
            }
        }

        public void ConfirmDeleteProvider(int result)
        {
            if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
                ProviderListBox.Items.Clear();
                RequestProviderList();
            }
        }
        #endregion

        #region GUI Methods
        public void ShowProviderDialogs()
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            ProviderInformationGrid.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Hidden;
        }

        public void ShowProviderRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            ShowProviderDialogs();
            RemoveValidationAssistant(false);
            ProviderHeaderTextBlock.Text = "Registro de Proveedor";

            AcceptButtton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Collapsed;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";

            ProviderCategoryComboBox.SelectedIndex = -1;
            ProviderPhoneTextBox.Text = "";
            ProviderEmailTextBox.Text = "";
            ProviderNameTextBox.Text = "";
            ProviderRFCTextBox.Text = "";
            ProviderListBox.Items.Clear();

            ProviderCategoryComboBox.Visibility = Visibility.Visible;
            ProviderPhoneStackPanel.Visibility = Visibility.Visible;
            ProviderEmailStackPanel.Visibility = Visibility.Visible;
            ProviderNameStackPanel.Visibility = Visibility.Visible;
            ProviderRFCStackPanel.Visibility = Visibility.Visible;


            HintAssist.SetHelperText(ProviderCategoryComboBox, "Selecciona la categoría");
            HintAssist.SetHelperText(ProviderPhoneTextBox, "Ingrese el número de teléfono");
            HintAssist.SetHelperText(ProviderEmailTextBox, "Ingrese el Email");
            HintAssist.SetHelperText(ProviderNameTextBox, "Ingrese el nombre");
            HintAssist.SetHelperText(ProviderRFCTextBox, "Ingrese el RFC");
        }

        public void ShowSpecificProviderInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            SeventhLayerBorder.Visibility = Visibility.Visible;
            ProviderViewGrid.Visibility = Visibility.Visible;
            ProviderContract providerContract = (ProviderContract)ProviderListBox.SelectedItem;
            ProviderCategoryViewComboBox.Text = providerContract.Category;
            ProviderPhoneViewTextBox.Text = providerContract.Phone;
            ProviderEmailViewTextBox.Text = providerContract.Email;
            ProviderNombreViewTextBox.Text = providerContract.Name;
            ProviderRFCViewTextBox.Text = providerContract.RFC;
        }

        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InitialMessageBorder.Visibility = Visibility.Hidden;
                ProviderTableGrid.Visibility = Visibility.Visible;
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
            if (ProviderFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Proveedor");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el nombre del proveedor");

            }

            else if (RFCFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: RFC");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa algún RFC");

            }
        }

        public void ResetSearchFilters(object sender, RoutedEventArgs e)
        {
            ProviderFilterRadioButton.IsChecked = false;
            RFCFilterRadioButton.IsChecked = false;
            SearchTextBox.Text = "";
            InitialMessageBorder.Visibility = Visibility.Visible;
            ProviderTableGrid.Visibility = Visibility.Hidden;
            SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
            HintAssist.SetHint(SearchTextBox, "Buscar");
            HintAssist.SetHelperText(SearchTextBox, "Selecciona un filtro de búsqueda");
        }

        public void BackToProviderRegistration(object sender, RoutedEventArgs e)
        {
            RemoveValidationAssistant(false);
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            ProviderInformationGrid.Visibility = Visibility.Visible;
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
        }

        private void ExitSpecificProviderInformation(object sender, RoutedEventArgs e)
        {
            string header = ProviderHeaderTextBlock.Text;

            if (string.Equals(header, "Registro de proveedor") || string.Equals(header, "Actualización de proveedor"))
            {
                RemoveValidationAssistant(true);
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                ProviderInformationGrid.Visibility = Visibility.Hidden;
            }
        }

        public void HideSpecificProviderInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            ProviderInformationGrid.Visibility = Visibility.Hidden;

            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;
        }

        public void ExitDetailedInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            ProviderViewGrid.Visibility = Visibility.Hidden;
            SeventhLayerBorder.Visibility = Visibility.Hidden;
        }

        public void ShowProviderUpdateDialog(object sender, RoutedEventArgs e)
        {
            AdaptProviderGUI(430, 200, 230, 160, 1);
            ProviderListBox.Items.Clear();
            ShowProviderDialogs();
            RemoveValidationAssistant(false);
            ProviderHeaderTextBlock.Text = "Actualización de proveedor";
            AcceptButtton.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";

            ProviderContract provider = (ProviderContract)ProviderListBox.SelectedItem;
            if (provider != null)
            {
                ProviderHeaderTextBlock.Text = "Proveedor ID" + provider.IdProvider;
                ProviderCategoryComboBox.Text = provider.Category;
                ProviderPhoneTextBox.Text = provider.Phone;
                ProviderEmailTextBox.Text = provider.Email;

                ProviderListBox.Items.Add(provider);

            }
        }

        public void ShowProviderDeleteDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void AcceptDeleteConfirmationButton(object sender, RoutedEventArgs e)
        {
            ProviderContract providerContract = (ProviderContract)ProviderListBox.SelectedItem;

            if (providerContract != null)
            {
                RequestDeleteProvider(providerContract.IdProvider);
                DeleteConfirmationGrid.Visibility = Visibility.Hidden;
                FifthLayerBorder.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("sin selección");
            }
        }

        public void SaveRegisterOrUpdateProvider(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            ProviderInformationGrid.Visibility = Visibility.Hidden;
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;

            RequestRegisterProvider();
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
            ValidationAssist.SetSuppress(ProviderCategoryComboBox, isNotVisible);
            ValidationAssist.SetSuppress(ProviderPhoneTextBox, isNotVisible);
            ValidationAssist.SetSuppress(ProviderEmailTextBox, isNotVisible);
            ValidationAssist.SetSuppress(ProviderNameTextBox, isNotVisible);
            ValidationAssist.SetSuppress(ProviderNameTextBox, isNotVisible);
        }

        public void AdaptProviderGUI(int largeFieldsWidth, int smallFieldsWidth, int marginExtra, int titleMargin, int span)
        {
            MainProviderFieldsGrid.SetValue(Grid.ColumnSpanProperty, span);
            ProviderTypeBorder.Width = largeFieldsWidth;
            ProviderTypeBorder.Width = largeFieldsWidth;

            ProviderTypeBorder.Width = largeFieldsWidth;
            ProviderCategoryComboBox.Width = largeFieldsWidth;

            ProviderPhoneBorder.Width = smallFieldsWidth;
            ProviderPhoneTextBox.Width = smallFieldsWidth;

            ProviderEmailBorder.Width = smallFieldsWidth;
            ProviderEmailTextBox.Width = smallFieldsWidth;

            ProviderNameBorder.Width = smallFieldsWidth;
            ProviderNameTextBox.Width = smallFieldsWidth;

            ProviderRFCBorder.Width = smallFieldsWidth;
            ProviderRFCTextBox.Width = smallFieldsWidth;

            ProviderTitleRFCTextBlock.Margin = new Thickness(titleMargin - 270, 25, 0, 0);

            if (span == 1)
            {
                ProviderTitleRFCTextBlock.Margin = new Thickness(0, 25, 0, 0);
                ProviderNameBorder.Margin = new Thickness(0);
                ProviderNameTextBox.Margin = new Thickness(0);
            }
        }

        public void ChangeProviderType(object sender, EventArgs e)
        {
            List<int> tableList = new List<int>();
            List<string> providerTypeList = new List<string>()
                {
                    "Semillas", "Carnes", "Frutas y verduras"
                };
            for (int i = 1; i < 10; i++)
            {
                tableList.Add(i);
            }

            string orderType = ProviderCategoryComboBox.Text;
        }

        #endregion
    }
}