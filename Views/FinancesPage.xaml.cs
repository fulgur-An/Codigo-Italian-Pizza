using Backend.Contracts;
using Backend.Service;
using Notifications.Wpf;
using Server;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private ServerItalianPizzaProxy serverItalianPizzaProxy;
        private IItalianPizzaService serviceChannel;

        private string usernameLoggedIn;

        private readonly NotificationManager notificationManager = new NotificationManager();

        public FinancesPage(string usernameLoggedIn)
        {
            InitializeComponent();
            this.usernameLoggedIn = usernameLoggedIn;
        }

        #region service connection

        #region Monetary expediture
        public void SaveMonetaryExpediture()
        {
            var service = new ItalianPizzaServiceCallback();
            service.RegisterMonetaryExpeditureEvent += ConfirmMonetaryExpeditureSaved;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            MonetaryExpeditureContract monetaryExpediture = new MonetaryExpeditureContract();
            monetaryExpediture.Amount = int.Parse(MonetaryExpeditureImportField.Text);
            monetaryExpediture.Description = MonetaryExpeditureDescriptionField.Text;
            monetaryExpediture.Date = DateTime.Now;
            monetaryExpediture.IdEmployee = 1;
            serviceChannel.RegisterMonetaryExpediture(monetaryExpediture);
        }

        public void ConsultMonetaryExpediture()
        {

            var service = new ItalianPizzaServiceCallback();
            service.GetMonetaryExpeditureEvent += ShowMonetaryExpeditureList;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.GetMonetaryExpediture(DateTime.Now);

        }
        #endregion

        #region Daily balance
        public void SaveDailyBalance()
        {
            var service = new ItalianPizzaServiceCallback();


            service.RegisterDailyBalanceEvent += ConfirmDailyBalanceSaved;


            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            DailyBalanceContract dailyBalance = new DailyBalanceContract();

            try
            {
                
                dailyBalance.EntryBalance = decimal.Parse(DailyBalanceEntryBalanceField.Text);
                dailyBalance.ExitBalance = decimal.Parse(DailyBalanceExitBalanceField.Text);
                dailyBalance.InitialBalance = decimal.Parse(DailyBalanceInitialBalanceField.Text);
                dailyBalance.CashBalance = decimal.Parse(DailyBalanceCashBalanceField.Text);
                dailyBalance.PhsycalBalance = decimal.Parse(DailyBalancePhysicBalanceField.Text);
                dailyBalance.CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("en-NZ")));
                dailyBalance.IdEmployee = 1;
            }
            catch (Exception ex)
            {

            }

            serviceChannel.RegisterDailyBalance(dailyBalance);
        }

        public void ConsultDailyBalance()
        {

            var service = new ItalianPizzaServiceCallback();
            service.GetDailyBalanceEvent += ShowDailyBalanceList;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.GetDailyBalance(DateTime.Now);

        }

        public void FillAmountsFields()
        {

            var service = new ItalianPizzaServiceCallback();
            service.GetAmountsEvent += ShowAmountsFields;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.GetAmounts();

        }
        #endregion

        #endregion

        #region Callbacks connection

        #region Monetary expediture
        public void ConfirmMonetaryExpeditureSaved(int result)
        {
            if (!result.Equals(0))
            {
                ShowConfirmationFileToast();
            }
        }

        public void ShowMonetaryExpeditureList(List<MonetaryExpeditureContract> monetaryExpeditures)
        {
            if (!monetaryExpeditures.Count.Equals(0))
            {
                monetaryExpeditureTableBodyListBox.ItemsSource = monetaryExpeditures;
            }
        }

        #endregion

        #region Daily Balance
        public void ConfirmDailyBalanceSaved(int result)
        {
            if (!result.Equals(0))
            {
                ShowConfirmationFileToast();
            }
        }

        public void ShowDailyBalanceList(List<DailyBalanceContract> dailyBalances)
        {
            if (!dailyBalances.Count.Equals(0))
            {
                dailyBalanceTableBodyListBox.ItemsSource = dailyBalances;
            }
        }

        public void ShowAmountsFields(decimal dialyEntrys, decimal dialyExits, decimal cashBalance)
        {
            DailyBalanceInitialBalanceField.Text = "1000";
            DailyBalanceEntryBalanceField.Text = dialyEntrys.ToString();
            DailyBalanceExitBalanceField.Text = dialyExits.ToString();
            DailyBalanceCashBalanceField.Text = cashBalance.ToString();

        }

        #endregion

        #endregion


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
            ConsultMonetaryExpediture();
            ConsultDailyBalance();
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

        public void SaveRegistMonetaryExpediture(object sender, RoutedEventArgs e)
        {
            SaveMonetaryExpediture();
        }

        public void SaveRegistDailyBalance(object sender, RoutedEventArgs e)
        {
            SaveDailyBalance();
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
            FillAmountsFields();
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
            DailyBalancePhysicBalanceField.Text = totalDouble.ToString();
        }

        #endregion



    }
}
