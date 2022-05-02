using ItalianPizza.BusinessObjects;
using MaterialDesignThemes.Wpf;
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
    /// Lógica de interacción para ListEmployees.xaml
    /// </summary>
    public partial class ListEmployeesPage : Page
    {
        private EmployeeViewModel employee = new EmployeeViewModel();

        private WorkShiftViewModel workshift = new WorkShiftViewModel();

        private string usernameLoggedIn;

        private readonly NotificationManager notificationManager = new NotificationManager();

        public ListEmployeesPage(string usernameLoggedIn)
        {
            InitializeComponent();
            DataContext = employee;
            //DataContext = workshift;
            this.usernameLoggedIn = usernameLoggedIn;
        }

        #region GUI Methods
        public void ShowEmployeeDialogs(bool isEnabled)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            EmployeeInformationGrid.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Hidden;
            EmployeeTypeComboBox.IsEnabled = isEnabled;
        }

        public void ShowEmployeeRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            ShowEmployeeDialogs(true);
            EmployeeHeaderTextBlock.Text = "Registro de empleado";

            AcceptButtonTextBlock.Text = "Registrar";
            AcceptButton.Visibility = Visibility.Visible;
            AdmissionDateDatePicker.SelectedDate = DateTime.Now;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            EmployeeTypeComboBox.SelectedIndex = -1;
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";

            HintAssist.SetHelperText(EmployeeNameTextBox, "Ingrese nombre del empleado");
            HintAssist.SetHelperText(EmployeeLastNameTextBox, "Ingrese apellidos del empleado");
            HintAssist.SetHelperText(EmployeeEmailTextBox, "Ingrese correo eléctronico");
            HintAssist.SetHelperText(EmployeePhoneTextBox, "Ingrese teléfono del empleado");
            HintAssist.SetHelperText(EmployeeTypeComboBox, "Selecciona un rol de empleado");
            HintAssist.SetHelperText(AdmissionDateDatePicker, "Fecha actual establecida");
            HintAssist.SetHelperText(EmployeeSalaryTextBox, "Ingrese salario del empleado");
            HintAssist.SetHelperText(EmployeeUsernameTextBox, "Ingrese usuario del empleado");
            HintAssist.SetHelperText(EmployeePasswordTextBox, "Ingrese contraseña del empleado");
            HintAssist.SetHelperText(EmployeeTimeOfEntryTextBox, "Ingrese hora de entrada");
            HintAssist.SetHelperText(EmployeeDepartureTimeTextBox, "Ingrese hora de salida");
            HintAssist.SetHelperText(EmployeeTimeTextBox, "Total de tiempo");

            EmployeeTimeTextBox.IsEnabled = false;
            UsenarmeBorder.Visibility = Visibility.Visible;
            PasswordBorder.Visibility = Visibility.Visible;
            UsernameTextBlock.Visibility = Visibility.Visible;
            PasswordTextBlock.Visibility = Visibility.Visible;
            EmployeeUsernameTextBox.Visibility = Visibility.Visible;
        }

        public void ChangeEmployeeType(object sender, EventArgs e)
        {
            List<string> employeeTypeList = new List<string>()
            {
                "Mesero", "Contador", "Cocinero", "Administrador", "Gerente"
            };     
        }

        public void ShowSpecificEmployeeInformation(object sender, RoutedEventArgs e)
        {
            ShowEmployeeDialogs(false);
            EmployeeHeaderTextBlock.Text = "Empleado";
            WorkshiftsHeaderTextBlock.Text = "Turno";
            AcceptButton.Visibility = Visibility.Hidden;

            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Información detallada";

            HintAssist.SetHelperText(EmployeeNameTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeLastNameTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeEmailTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeePhoneTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeTypeComboBox, string.Empty);
            HintAssist.SetHelperText(AdmissionDateDatePicker, string.Empty);
            HintAssist.SetHelperText(EmployeeSalaryTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeTimeOfEntryTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeDepartureTimeTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeTimeTextBox, string.Empty);

            UsenarmeBorder.Visibility = Visibility.Hidden;
            PasswordBorder.Visibility = Visibility.Hidden;
            UsernameTextBlock.Visibility = Visibility.Hidden;
            PasswordTextBlock.Visibility = Visibility.Hidden;
            EmployeeUsernameTextBox.Visibility = Visibility.Hidden;
            EmployeePasswordTextBox.Visibility = Visibility.Hidden;
        }

        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InitialMessageBorder.Visibility = Visibility.Hidden;
                EmployeeTableGrid.Visibility = Visibility.Visible;
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
            if (EmployeeFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Empleado");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el nombre del empleado");
            }
            else if (RoleFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Rol");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el rol de empleado");
            }
            else if (WorkshiftFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Turno");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa una hora en formato de 24 hrs");
            }
        }

        public void ResetSearchFilters(object sender, RoutedEventArgs e)
        {
            EmployeeFilterRadioButton.IsChecked = false;
            EmployeeFilterRadioButton.IsChecked = false;
            WorkshiftFilterRadioButton.IsChecked = false;
            SearchTextBox.Text = "";
            InitialMessageBorder.Visibility = Visibility.Visible;
            EmployeeTableGrid.Visibility = Visibility.Hidden;
            SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
            HintAssist.SetHint(SearchTextBox, "Buscar");
            HintAssist.SetHelperText(SearchTextBox, "Selecciona un filtro de búsqueda");
        }

        public void BackToEmployeeRegistration(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            EmployeeInformationGrid.Visibility = Visibility.Visible;

            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
        }

        private void ExitSpecificEmployeeInformation(object sender, RoutedEventArgs e)
        {
            if (EmployeeHeaderTextBlock.Text == "Registro de empleado")
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            }
            else if (EmployeeHeaderTextBlock.Text == "Actualización de empleado")
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            } else
            {
                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                EmployeeInformationGrid.Visibility = Visibility.Hidden;
            }
        }

        private void HideSpecificEmployeeInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            EmployeeInformationGrid.Visibility = Visibility.Hidden;

            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;
        }

        public void ShowEmployeeUpdateDialog(object sender, RoutedEventArgs e)
        {
            ShowEmployeeDialogs(true);
            EmployeeHeaderTextBlock.Text = "Actualización de empleado";

            AcceptButtonTextBlock.Text = "Guardar";
            AcceptButton.Visibility = Visibility.Visible;
            AdmissionDateDatePicker.SelectedDate = DateTime.Now;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            EmployeeTypeComboBox.SelectedIndex = -1;
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";

            HintAssist.SetHelperText(EmployeeNameTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeLastNameTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeEmailTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeePhoneTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeTypeComboBox, string.Empty);
            HintAssist.SetHelperText(AdmissionDateDatePicker, "Este campo no puede modificarse");
            HintAssist.SetHelperText(EmployeeSalaryTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeTimeOfEntryTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeDepartureTimeTextBox, string.Empty);
            HintAssist.SetHelperText(EmployeeTimeTextBox, string.Empty);

            UsenarmeBorder.Visibility = Visibility.Hidden;
            PasswordBorder.Visibility = Visibility.Hidden;
            UsernameTextBlock.Visibility = Visibility.Hidden;
            PasswordTextBlock.Visibility = Visibility.Hidden;
            EmployeeUsernameTextBox.Visibility = Visibility.Hidden;
            EmployeePasswordTextBox.Visibility = Visibility.Hidden;
            AdmissionDateDatePicker.IsEnabled = false;
        }

        public void ShowEmployeDeleteDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void ShowConfirmationToast(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            EmployeeInformationGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;

            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;

            //BackToEmployeeRegistration(sender, e);

            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = "Proceso Realizado",
                    Type = NotificationType.Success,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );

        }
        #endregion


    }
}
