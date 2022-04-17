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
            ClearMenuSelections();
        }

        public void ClearMenuSelections()
        {
            OrdersStackPanel.Opacity = 0.5;
            OrdersGreendBorder.Visibility = Visibility.Hidden;
            FoodRecipeStackPanel.Opacity = 0.5;
            FoodRecipesGreendBorder.Visibility = Visibility.Hidden;

        }

        public void OpenCustomerModule(object sender, RoutedEventArgs e) {

        }

        public  void OpenOrderModule(object sender, RoutedEventArgs e)
        {
            ClearMenuSelections();
            CheckOrdersPage checkOrdersPage = new CheckOrdersPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(checkOrdersPage);
            OrdersStackPanel.Opacity = 1;
            OrdersGreendBorder.Visibility = Visibility;
        }

        public void OpeenFoodRecipeModule(object sender, RoutedEventArgs e)
        {
            ClearMenuSelections();
            CheckFoodRecipesPage checkFoodRecipesPage = new CheckFoodRecipesPage(usernameLoggedIn);
            NavigationFrame.NavigationService.Navigate(checkFoodRecipesPage);
            FoodRecipeStackPanel.Opacity = 1;
            FoodRecipesGreendBorder.Visibility = Visibility;
        }

        #endregion
    }
}
