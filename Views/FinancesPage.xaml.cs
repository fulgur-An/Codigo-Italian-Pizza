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
    /// Lógica de interacción para FinancesPage.xaml
    /// </summary>
    public partial class FinancesPage : Page
    {
        private string usernameLoggedIn;

        private readonly NotificationManager notificationManager = new NotificationManager();

        public FinancesPage(string usernameLoggedIn)
        {
            InitializeComponent();
            this.usernameLoggedIn = usernameLoggedIn;
        }


        #region GUI Methods
        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                //InitialMessageBorder.Visibility = Visibility.Hidden;

                //InventoryTableGrid.Visibility = Visibility.Visible;
                //RegionSearchGrid.Visibility = Visibility.Visible;
                //MainElementsInventoryGrid.Visibility = Visibility.Visible;
                //ValidateInventoryTableGrid.Visibility = Visibility.Hidden;
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

        private void AcceptDepartureConfirmation(object sender, RoutedEventArgs e)
        {
            ShowWarningToast();
            HideRegisterFinanceLayout();
            HideDepartureConfirmation(sender, e);
        }

        public void HideRegisterFinanceLayout()
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            MonetaryExpeditureInformationGrid.Visibility = Visibility.Hidden;
            ChoiceRegisterStackPanel.Visibility = Visibility.Hidden;
            DailyBalanceInformationGrid.Visibility = Visibility.Hidden;
        }

        public void HideDepartureConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmationBackBorder.Visibility = Visibility.Hidden;
            DeparureConfirmationBorder.Visibility = Visibility.Hidden;
        }

        public void OpenMonetaryExpeditureDrawer(object sender, RoutedEventArgs e)
        {
            HideElementsToOpenDrawer();
        }

        public void OpenDiaryBalanceDrawer(object sender, RoutedEventArgs e)
        {
            HideElementsToOpenDrawer();
        }

        private void CloseElementsDrawer(object sender, RoutedEventArgs e)
        {
            MainElementsInventoryGrid.Visibility = Visibility.Visible;
            InitialMessageBorder.Visibility = Visibility.Visible;
            MonetaryExpeditureButton.Visibility = Visibility.Visible;
            DiaryBalanceButton.Visibility = Visibility.Visible;
        }

        public void HideElementsToOpenDrawer()
        {
            MainElementsInventoryGrid.Visibility = Visibility.Collapsed;
            InitialMessageBorder.Visibility = Visibility.Collapsed;
            MonetaryExpeditureButton.Visibility = Visibility.Collapsed;
            DiaryBalanceButton.Visibility = Visibility.Collapsed;
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

        private void UnCheckDiaryBalance(object sender, RoutedEventArgs e)
        {
            DiaryBalanceCheckbox.IsChecked = false;
            MonetaryExpeditureCheckbox.IsChecked = true;
            MonetaryExpeditureInformationGrid.Visibility = Visibility.Visible;
            DailyBalanceInformationGrid.Visibility = Visibility.Hidden;
        }

        private void UnCheckMonetaryExpediture(object sender, RoutedEventArgs e)
        {
            DiaryBalanceCheckbox.IsChecked = true;
            MonetaryExpeditureCheckbox.IsChecked = false;
            MonetaryExpeditureInformationGrid.Visibility = Visibility.Hidden;
            DailyBalanceInformationGrid.Visibility = Visibility.Visible;
        }

        private void OpenRegistLayout(object sender, MouseButtonEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            MonetaryExpeditureInformationGrid.Visibility = Visibility.Visible;
            ChoiceRegisterStackPanel.Visibility = Visibility.Visible;
        }

        private void MilPesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = milPesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 1000);
        }

        private void QuinientosPesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = quinientosPesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 500);
        }

        private void DoscientosPesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = doscientosPesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 200);
        }

        private void CienPesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = cienPesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 100);
        }

        private void CincuentaPesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = cincuentaPesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 50);
        }

        private void VeintePesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = veintePesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 20);
        }

        private void DiezPesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = diezPesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 10);
        }

        private void CincoPesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = cincoPesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 5);
        }

        private void DosPesosKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = dosPesos.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 2);
        }

        private void PesoKeyUp(object sender, KeyEventArgs e)
        {

            string amountstring = peso.Text;
            double amount = double.Parse(amountstring);
            Calculate(amount * 1);
        }

        private void Calculate(double amount)
        {
            string totalString = total.Text;

            double totalDouble = double.Parse(totalString);

            totalDouble += amount;

            total.Text = totalDouble.ToString();
        }

        #endregion



    }
}
