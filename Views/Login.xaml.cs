using Backend.Contracts;
using Backend.Models;
using Backend.Service;
using MaterialDesignThemes.Wpf;
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
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private ServerItalianPizzaProxy serverProxy;
        private IItalianPizzaService channel;

        public Login()
        {
            InitializeComponent();
            DataContext = this;
        }
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        public string dUsername { get; set; }
        protected string dPassword { private get; set; }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).dPassword = ((PasswordBox)sender).Password; }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }

            paletteHelper.SetTheme(theme);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void doLogin(object sender, RoutedEventArgs e)
        {

        }

        private void LoginEmployee(object sender, RoutedEventArgs e)
        {
            
                EmployeeContract employeeLogin = new EmployeeContract();

                employeeLogin.Username = Username.Text;
                employeeLogin.Password = Password.Password;

              if (Password.Password == employeeLogin.Password)
                {
                    
                        ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                        service.LoginEmployeeEvent += ConfirmLogin;
                        serverProxy = new ServerItalianPizzaProxy(service);
                        channel = serverProxy.ChannelFactory.CreateChannel();
                        channel.LoginEmployee(employeeLogin);
                   
                }
                else
                {
                    MessageBox.Show("No existe el usuario", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            
        }

        private void ConfirmLogin(EmployeeContract employee, bool confirmLogin)
        {
            if (confirmLogin)
            {
                string fullName = employee.Name + " " + employee.LastName;
                string role = employee.Role;
                MainWindow mainWindow = new MainWindow(fullName, role);
                this.Close();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("No es posible acceder al sistema");
            }
        }

    }
}
