using Backend.Contracts;
using Backend.Models;
using Backend.Service;
using ItalianPizza.Validations;
using MaterialDesignThemes.Wpf;
using Notifications.Wpf;
using Server;
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
using System.Windows.Forms;
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
        private readonly NotificationManager notificationManager = new NotificationManager();

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

        //private void exitApp(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //}

        private void doLogin(object sender, RoutedEventArgs e)
        {

        }

        private void LoginEmployee(object sender, RoutedEventArgs e)
        {
            
            EmployeeContract employeeLogin = new EmployeeContract();
            LogOutContract timeLogin = new LogOutContract();

            string passwordEncrypt = EncyptPassword.SHA256(EmployeePasswordPasswordBox.Password);
            string login = DateTime.Now.ToString("T");

            EmployeePasswordTextBox.Text = EmployeePasswordPasswordBox.Password;

            employeeLogin.Username = EmployeeUsernameTextBox.Text;
            employeeLogin.Password = passwordEncrypt;

            timeLogin.DepartureTime = login;

            if (EmployeePasswordPasswordBox.Password == "" && EmployeeUsernameTextBox.Text == "")
            {
                System.Windows.MessageBox.Show("Campos inválidos", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (EmployeePasswordPasswordBox.Password == "")
            {
                System.Windows.MessageBox.Show("Campo password inválido", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (EmployeeUsernameTextBox.Text == "")
            {
                System.Windows.MessageBox.Show("Campo username inválido", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                service.LoginEmployeeEvent += ConfirmLogin;
                serverProxy = new ServerItalianPizzaProxy(service);
                channel = serverProxy.ChannelFactory.CreateChannel();
                channel.LoginEmployee(employeeLogin, timeLogin);
            }
             
        }

        private void ConfirmLogin(EmployeeContract employee, bool confirmLogin)
        {
            if (confirmLogin)
            {
                string fullName = employee.Name + " " + employee.LastName;
                string role = employee.Role;
                string username = EmployeeUsernameTextBox.Text;
                MainWindow mainWindow = new MainWindow(fullName, role, username);
                this.Close();
                mainWindow.Show();
            } 
            else
            {
                System.Windows.MessageBox.Show("Usuario no encontrado", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

       

        public void PersonalizeToast(NotificationType notificationType, string message)
        {
            NotificationContent notificationContent = new NotificationContent
            {
                Title = "Confirmación",
                Message = message,
                Type = notificationType,
            };
            notificationManager.Show(notificationContent);
        }
    }
}
