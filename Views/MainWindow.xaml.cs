using Backend.Contracts;
using Backend.Service;
using ItalianPizza.Views;
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

namespace ItalianPizza
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string usernameLoggedIn;
        private string role;
        private string username;
        private ServerItalianPizzaProxy serverProxy;
        private IItalianPizzaService channel;

        public MainWindow(string usernameLoggedIn, string role, string username)
        {
            InitializeComponent();
            this.usernameLoggedIn = usernameLoggedIn;
            this.role = role;
            this.username = username;
            MainPage mainPage = new MainPage();
            NavigationFrame.NavigationService.Navigate(mainPage);
            UserLoggedInTextBlock.Text = this.usernameLoggedIn;
            EmployeeRoleTextBlock.Text = this.role;
            UsernameTextBlock.Text = this.username;
        }

        #region Request

        public void RequestGetLogOut(string usernameEmployee)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetEmployeeLogOutEvent += LoadEmployeesLogOut;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetEmployeeLogOut(usernameEmployee);
        }

        public void RequestUpdate(string usernameEmployee, LogOutContract logOutContract)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.UpdateLoginEvent += ConfirmLogOut;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.UpdateLogin(usernameEmployee, logOutContract);
        }

        #endregion


        #region GUI Methods


        public void ClearMenuSelections()
        {
            OrdersStackPanel.Opacity = 0.5;
            OrdersGreendBorder.Visibility = Visibility.Hidden;
            EmployeesStackPanel.Opacity = 0.5;
            EmployeesGreendBorder.Visibility = Visibility.Hidden;
            CustomersStackPanel.Opacity = 0.5;
            CustomersGreendBorder.Visibility = Visibility.Hidden;
        }

        public void OpenStartupModule(object sender, RoutedEventArgs e)
        {
            MainPage mainPage = new MainPage();
            NavigationFrame.NavigationService.Navigate(mainPage);
            ClearMenuSelections();
        }

        public void OpenCustomerModule(object sender, RoutedEventArgs e) {
            ClearMenuSelections();
            ListCustomersPage listCustomersPage = new ListCustomersPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(listCustomersPage);
            CustomersStackPanel.Opacity = 1;
            CustomersGreendBorder.Visibility = Visibility;
        }

        public void OpenEmployeeModule(object sender, RoutedEventArgs e)
        {
            ClearMenuSelections();
            ListEmployeesPage listEmployeesPage = new ListEmployeesPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(listEmployeesPage);
            EmployeesStackPanel.Opacity = 1;
            EmployeesGreendBorder.Visibility = Visibility;
        }

        public  void OpenOrderModule(object sender, RoutedEventArgs e)
        {
            ClearMenuSelections();
            CheckOrdersPage checkOrdersPage = new CheckOrdersPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(checkOrdersPage);
            OrdersStackPanel.Opacity = 1;
            OrdersGreendBorder.Visibility = Visibility;
        }
        
        public void OpenProviderModule(object sender, RoutedEventArgs e)
        {
            ClearMenuSelections();
            CheckOrdersPage checkOrdersPage = new CheckOrdersPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(checkOrdersPage);
            HiddeMenuBorders();
            OrdersStackPanel.Opacity = 1;
            OrdersGreendBorder.Visibility = Visibility;
        }

        public void OpenInventoryModule(object sender, RoutedEventArgs e)
        {
            InventoryPage inventoryPage = new InventoryPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(inventoryPage);
            HiddeMenuBorders();
            InventorysStackPanel.Opacity = 1;
            IventorysGreenBorder.Visibility = Visibility;
        }

        public void OpenFinancesModule(object sender, RoutedEventArgs e)
        {
            FinancesPage financesPage = new FinancesPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(financesPage);
            HiddeMenuBorders();
            FinanceStackPanel.Opacity = 1;
            FinanceGreenBorder.Visibility = Visibility;
        }

        public void HiddeMenuBorders()
        {
            OrdersStackPanel.Opacity = 0.5;
            OrdersGreendBorder.Visibility = Visibility.Hidden;
            InventorysStackPanel.Opacity = 0.5;
            IventorysGreenBorder.Visibility = Visibility.Hidden;
            FinanceStackPanel.Opacity = 0.5;
            FinanceGreenBorder.Visibility = Visibility.Hidden;
        }
        
        public void OpeenFoodRecipeModule(object sender, RoutedEventArgs e)
        {
            ClearMenuSelections();
            CheckFoodRecipesPage checkFoodRecipesPage = new CheckFoodRecipesPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(checkFoodRecipesPage);
            FoodRecipeStackPanel.Opacity = 1;
            FoodRecipesGreendBorder.Visibility = Visibility;
        }

        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            string usernameEmployee = UsernameTextBlock.Text;

            RequestGetLogOut(usernameEmployee);
        }

        private void LoadEmployeesLogOut(LogOutContract logOutContract)
        {
            string usernameEmployee = UsernameTextBlock.Text;

            RequestUpdate(usernameEmployee, logOutContract);
        }

        private void ConfirmLogOut(int result)
        {
            if (result == 1)
            {
                Login login = new Login();
                this.Close();
                login.Show();
            }
        }


        public void OpenProviderModule(object sender, RoutedEventArgs e)
        {
            CheckProviderPage checkProviderPage = new CheckProviderPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(checkProviderPage);
            ProviderStackPanel.Opacity = 1;
            ProviderGreenBorder.Visibility = Visibility;
        }

        #endregion


    }
}
