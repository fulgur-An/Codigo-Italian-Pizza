using Backend.Contracts;
using Backend.Service;
using ItalianPizza.BusinessObjects;
using MaterialDesignThemes.Wpf;
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
        private DailyBalanceViewModel dailyBalanceViewModel = new DailyBalanceViewModel();
        private MonetaryExpeditureDailyBalanceViewModel monetaryExpeditureViewModel = new MonetaryExpeditureDailyBalanceViewModel();
        private string usernameLoggedIn;
        private readonly NotificationManager notificationManager = new NotificationManager();

        public FinancesPage(string usernameLoggedIn)
        {
            DataContext = monetaryExpeditureViewModel;
            InitializeComponent();
            this.usernameLoggedIn = usernameLoggedIn;
            this.MonetaryExpeditureEmployee.Text = usernameLoggedIn;
            this.DailyBalanceEmployeeField.Text = usernameLoggedIn;
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
            int option = GetFilterSelection();
            string filter = SearchTextBox.Text;
            var service = new ItalianPizzaServiceCallback();
            service.GetMonetaryExpeditureEvent += ShowMonetaryExpeditureList;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.GetMonetaryExpediture(filter, option);

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
            int option = GetFilterSelection();
            string filter = SearchTextBox.Text;
            var service = new ItalianPizzaServiceCallback();
            service.GetDailyBalanceEvent += ShowDailyBalanceList;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.GetDailyBalance(filter, option);

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
                PersonalizeToast(NotificationType.Success, "Proceso realizado","Confirmación");
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
                PersonalizeToast(NotificationType.Success, "Proceso realizado","Confirmación");
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
            if (true)
            {

            }
            ConsultMonetaryExpediture();
            ConsultDailyBalance();
        }

        public int GetFilterSelection()
        {
            int option = 1;
            if (employeeNameRadioButton.IsChecked.Value)
            {
                option = 1;
            }
            else if (quantityRadioButton.IsChecked.Value)
            {
                option = 2;
            }
            else if (dateRadioButton.IsChecked.Value)
            {
                option = 3;
            }
            return option;
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
            PersonalizeToast(NotificationType.Success, "Proceso cancelado","Atención");
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
            ClearFields();
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
            RemoveValidationAssistant(false);
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
            RemoveValidationAssistant(true);

        }


        public void PersonalizeToast(NotificationType notificationType, string message, string title)
        {
            NotificationContent notificationContent = new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Success,
            };
            notificationManager.Show(notificationContent);
            
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


        private void ClearFields()
        {
            DailyBalancePhysicBalanceField.Clear();
            MonetaryExpeditureImportField.Clear();
            MonetaryExpeditureDescriptionField.Clear();
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
            string amountstring = "0";
            if (Bill1000Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill1000Textbox.Text;
            }
            else
            {
                amountstring = "0";
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 1000);
        }

        private void QuinientosPesosKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (Bill500Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill500Textbox.Text;
                
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 500);
        }

        private void DoscientosPesosKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (Bill200Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill200Textbox.Text;
                
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 200);
        }

        private void CienPesosKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (Bill100Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill100Textbox.Text;
                
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 100);
        }

        private void CincuentaPesosKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (Bill50Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill50Textbox.Text;
                
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 50);
        }

        private void VeintePesosKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (Bill20Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill20Textbox.Text;
                
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 20);
        }

        private void DiezPesosKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (Bill10Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill10Textbox.Text;
                
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 10);
        }

        private void CincoPesosKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (Bill5Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill5Textbox.Text;
                
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 5);
        }

        private void DosPesosKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (Bill2Textbox.Text.All(Char.IsDigit))
            {
                amountstring = Bill2Textbox.Text;
                
            }
            double amount = double.Parse(amountstring);
            Calculate(amount * 2);
        }

        private void PesoKeyUp(object sender, KeyEventArgs e)
        {
            string amountstring = "0";
            if (BillTextbox.Text.All(Char.IsDigit))
            {
                amountstring = BillTextbox.Text;
                
            }
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

        public void RemoveValidationAssistant(bool isNotVisible)
        {
            ValidationAssist.SetSuppress(DailyBalancePhysicBalanceField, isNotVisible);
            ValidationAssist.SetSuppress(MonetaryExpeditureImportField, isNotVisible);
            ValidationAssist.SetSuppress(MonetaryExpeditureDescriptionField, isNotVisible);
        }

        #endregion

    }
}
