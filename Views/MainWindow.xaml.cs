using ItalianPizza.Views;
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
        
        public MainWindow()
        {
            InitializeComponent();
            this.usernameLoggedIn = UserLoggedInTextBlock.Text;
            MainPage mainPage = new MainPage();
            NavigationFrame.NavigationService.Navigate(mainPage);
        }

        #region GUI Methods

        public void OpenStartupModule(object sender, RoutedEventArgs e)
        {
            MainPage mainPage = new MainPage();
            NavigationFrame.NavigationService.Navigate(mainPage);
            OrdersStackPanel.Opacity = 0.5;
            OrdersGreendBorder.Visibility = Visibility.Hidden;
        }

        public void OpenCustomerModule(object sender, RoutedEventArgs e) {

        }

        public  void OpenOrderModule(object sender, RoutedEventArgs e)
        {
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

        #endregion
    }
}
