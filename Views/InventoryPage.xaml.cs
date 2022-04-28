using Notifications.Wpf;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        private string usernameLoggedIn;

        private readonly NotificationManager notificationManager = new NotificationManager();

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

                InventoryTableGrid.Visibility = Visibility.Visible;
                RegionSearchGrid.Visibility = Visibility.Visible;
                MainElementsInventoryGrid.Visibility = Visibility.Visible;
                ValidateInventoryTableGrid.Visibility = Visibility.Hidden;


            }
        }

        public void ShowValidateLayout(object sender, RoutedEventArgs e)
        {
            
            InitialMessageBorder.Visibility = Visibility.Hidden;

            InventoryTableGrid.Visibility = Visibility.Hidden;
            RegionSearchGrid.Visibility = Visibility.Hidden;
            MainElementsInventoryGrid.Visibility = Visibility.Hidden;
            ValidateInventoryTableGrid.Visibility = Visibility.Visible;
        }

        public void CancelValidationLayout(object sender, RoutedEventArgs e)
        {
            ShowConfirmationDeparture();
            
            
        }

        public void HideValidateLayout()
        {
            InitialMessageBorder.Visibility = Visibility.Visible;

            InventoryTableGrid.Visibility = Visibility.Hidden;

            RegionSearchGrid.Visibility = Visibility.Visible;
            MainElementsInventoryGrid.Visibility = Visibility.Visible;
            ValidateInventoryTableGrid.Visibility = Visibility.Hidden;
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

        private void ShowSpecificItemInformation()
        {

            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            OrderInformationGrid.Visibility = Visibility.Visible;
            FieldsAreEditableTextBlock.Visibility = Visibility.Hidden;
            ChageEnableProperty(true);
        }
        private void PackIcon_DragEnter(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(500);

            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            OrderInformationGrid.Visibility = Visibility.Visible;
            FieldsAreEditableTextBlock.Visibility = Visibility.Hidden;
            GetOutStackPanel.Visibility = Visibility.Hidden;
            CancelRegisterItemButton.Visibility = Visibility.Hidden;
            RegisterItemButton.Visibility = Visibility.Hidden;
            UpdateItemDataButton.Visibility = Visibility.Hidden;
            ChageEnableProperty(false);
        }

        private void PackIcon_DragLeave(object sender, RoutedEventArgs e)
        {
            HideSpecificItemInformation(sender, e);
            
        }

        public void HideSpecificItemInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            OrderInformationGrid.Visibility = Visibility.Hidden;
        }
        private void OpenRegistItem(object sender, RoutedEventArgs e)
        {
            ShowSpecificItemInformation();
            GetOutStackPanel.Visibility = Visibility.Hidden;
            RegisterItemButton.Visibility = Visibility.Visible;
            CancelRegisterItemButton.Visibility = Visibility.Visible;
            UpdateItemDataButton.Visibility = Visibility.Collapsed;
            FieldsAreEditableTextBlock.Visibility = Visibility.Visible;
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

        public void ShowEspecificDataItem(object sender, RoutedEventArgs e)
        {
            ShowSpecificItemInformation();
            GetOutStackPanel.Visibility = Visibility.Visible;
            RegisterItemButton.Visibility = Visibility.Collapsed;
            CancelRegisterItemButton.Visibility = Visibility.Hidden;
            UpdateItemDataButton.Visibility = Visibility.Collapsed;
            ChageEnableProperty(false);
        }


        public void ShowUpdateFields(object sender, RoutedEventArgs e)
        {
            ShowEspecificDataItem(sender, e);
            GetOutStackPanel.Visibility = Visibility.Hidden;
            RegisterItemButton.Visibility = Visibility.Collapsed;
            CancelRegisterItemButton.Visibility = Visibility.Visible;
            UpdateItemDataButton.Visibility = Visibility.Visible;
            FieldsAreEditableTextBlock.Visibility = Visibility.Visible;
            ChageEnableProperty(true);
            if(DeleteItemGrid.Visibility == Visibility.Visible)
            {
                HideSpecificItemInformation(sender, e);
            }
        }

        public void ChageEnableProperty(bool enableProperty)
        {
            ItemNameField.IsEnabled = enableProperty;
            ItemCodeField.IsEnabled = enableProperty;
            ItemDescriptionField.IsEnabled = enableProperty;
            ItemValueField.IsEnabled = enableProperty;
            ItemQuantityField.IsEnabled = enableProperty;
            ItemRestrictionsField.IsEnabled = enableProperty;
        }

        public void UpdateItem(object sender, RoutedEventArgs e)
        {
            HideSpecificItemInformation(sender, e);
            ShowConfirmationToast();
        }

        public void RegistItem(object sender, RoutedEventArgs e)
        {
            HideSpecificItemInformation(sender, e);
            ShowConfirmationToast();
        }
        public void CancelProdecure(object sender, RoutedEventArgs e)
        {
            ShowConfirmationDeparture();
        }

        public void ShowConfirmationDeparture()
        {
            ConfirmationBackBorder.Visibility = Visibility.Visible;
            DeparureConfirmationBorder.Visibility = Visibility.Visible;
            
        }

        public void HideDepartureConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmationBackBorder.Visibility = Visibility.Hidden;
            DeparureConfirmationBorder.Visibility = Visibility.Hidden;
        }

        private void AcceptDepartureConfirmation(object sender, RoutedEventArgs e)
        {
            ShowWarningToast();
            HideSpecificItemInformation(sender, e);
            HideValidateLayout();
            HideDepartureConfirmation(sender, e);
        }

        public void PrintValidationInventoryResult(object sender, RoutedEventArgs e)
        {
            HideValidateLayout();
            ShowConfirmationFileToast();
        }

        public void GenerateReport(object sender, RoutedEventArgs e)
        {
            ShowConfirmationFileToast();
        }


        public void ShowConfirmationToast()
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = "Proceso Realizado",
                    Type = NotificationType.Success,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }
        public void ShowConfirmationFileToast()
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = "Archivo creaado",
                    Type = NotificationType.Success,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }

        public void ShowWarningToast()
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Warning",
                    Message = "Proceso cancelado",
                    Type = NotificationType.Warning,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }




        #endregion

        
    }
}
