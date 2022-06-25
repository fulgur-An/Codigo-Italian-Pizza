using Backend.Contracts;
using Backend.Service;
using ItalianPizza.BusinessObjects;
using ItalianPizza.Validations;
using MaterialDesignThemes.Wpf;
using Notifications.Wpf;
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
    /// Lógica de interacción para ListEmployees.xaml
    /// </summary>
    public partial class ListEmployeesPage : Page
    {
        private ServerItalianPizzaProxy serverProxy;
        private IItalianPizzaService channel;
        private EmployeeViewModel employee = new EmployeeViewModel();
        private WorkShiftViewModel workShift = new WorkShiftViewModel();
        private WorkshiftContract workShift1 = new WorkshiftContract();
        private List<EmployeeContract> allEmployeeContracts = new List<EmployeeContract>();
        private string usernameLoggedIn;
        private readonly NotificationManager notificationManager = new NotificationManager();

        public enum ESearchFilters
        {
            NameFilter = 1,
            RoleFilter = 2,
            WorkshiftFilter = 3,
            NoSelection = 4
        }
        private enum statusOfEmployees
        {
            Available
        }

        public ListEmployeesPage(string usernameLoggedIn)
        {
            InitializeComponent();
            DataContext = employee;
            this.usernameLoggedIn = usernameLoggedIn;
            RequestEmployeeList();

        }

        #region Request

        public void RequestRegisterEmployee()
        {
            DateTime admissionDatePicker = (DateTime)EmployeeAdmissionDateDatePicker.SelectedDate;
            string passwordEncrypt = EncyptPassword.SHA256(EmployeePasswordTextBox.Text);
            int timeOfentry = int.Parse(EmployeeTimeOfEntryTextBox.Text);
            int departureTime = int.Parse(EmployeeDepartureTimeTextBox.Text);
            int time = departureTime - timeOfentry;
            string timeTextBox = time.ToString();

            EmployeeContract employee = new EmployeeContract
            {
                Email = EmployeeEmailTextBox.Text,
                Name = EmployeeNameTextBox.Text,
                LastName = EmployeeLastNameTextBox.Text,
                Phone = EmployeePhoneTextBox.Text,
                Status = statusOfEmployees.Available.ToString(),
                AdmissionDate = admissionDatePicker,
                Password = passwordEncrypt,
                Role = EmployeeTypeComboBox.Text,
                Salary = decimal.Parse(EmployeeSalaryTextBox.Text),
                Username = EmployeeUsernameTextBox.Text,
            };

            WorkshiftContract workshift = new WorkshiftContract
            {
                DepartureTime = EmployeeDepartureTimeTextBox.Text,
                Time = timeTextBox,
                TimeOfEntry = EmployeeTimeOfEntryTextBox.Text,
            };

            string header = EmployeeHeaderTextBlock.Text;

            if (departureTime > timeOfentry)
            {
                if (string.Equals(header, "Registro de empleado"))
                {
                    ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                    service.RegisterEmployeeEvent += ConfirmRegistrationEmployee;
                    serverProxy = new ServerItalianPizzaProxy(service);
                    channel = serverProxy.ChannelFactory.CreateChannel();
                    channel.RegisterEmployee(employee, workshift);
                }
                else if (string.Equals(header, "Actualización de empleado"))
                {
                    int employeeId = ((EmployeeContract)EmployeeListBox.SelectedItem).IdUserEmployee;
                    employee.IdUserEmployee = employeeId;
                    ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                    service.UpdateEmployeeEvent += ConfirmRegistrationEmployee;
                    serverProxy = new ServerItalianPizzaProxy(service);
                    channel = serverProxy.ChannelFactory.CreateChannel();
                    channel.UpdateEmployee(employee, workshift);
                }
            }
            else
            {
                int result = -2;
                ConfirmRegistrationEmployee(result);
            }
        }

        public void RequestEmployeeList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetEmployeeListSortedByNameEvent += LoadAllEmployees;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetEmployeeListSortedByName();
        }

        public void RequestDeleteEmployee(int idEmployee)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.DeleteEmployeeByIdEvent += ConfirmDeleteEmployee;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.DeleteEmployeeById(idEmployee);
        }

        public void RequestGetWorkshift(int idEmployee)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetEmployeeWorkshiftEvent += LoadEmployeesWorkshift;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetEmployeeWorkshift(idEmployee);
        }

        #endregion

        #region Implementation methods

        public void ConfirmRegistrationEmployee(int result)
        {
            if (result > 0)
            {
                //EmployeeListBox.Items.Clear();
                RequestEmployeeList();
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
            }
            else if (result == -1)
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Nombre de empleado ya registrado");
                ShowEmployeeDialogs();
            }
            else if (result == -2)
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Hora de salida menor a la hora de entrada");
                ShowEmployeeDialogs();
            }
            else
            {
                NotificationType notificationType = NotificationType.Error;
                PersonalizeToast(notificationType, "Proceso no realizado. Intentelo de nuevo");
                ShowEmployeeDialogs();
            }
        }

        public void LoadAllEmployees(List<EmployeeContract> employeeContracts)
        {
            if (employeeContracts.Count() > 0)
            {
                EmployeeListBox.ItemsSource = employeeContracts;
            }
        }

        public void ConfirmDeleteEmployee(int result)
        {
            if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
                //EmployeeListBox.Items.Clear();
                RequestEmployeeList();
            }
        }

        #endregion

        #region GUI Methods

        public ESearchFilters GetFilterSelected()
        {
            ESearchFilters filterSelected = new ESearchFilters();

            if (EmployeeFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.NameFilter;
            }
            else if (RoleFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.RoleFilter;
            }
            else if (WorkshiftFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.WorkshiftFilter;
            }
            else
            {
                filterSelected = ESearchFilters.NoSelection;
            }

            return filterSelected;
        }

        public void ShowEmployeeDialogs()
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            EmployeeInformationGrid.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Hidden;

        }

        public void ShowEmployeeRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            ShowEmployeeDialogs();
            RemoveValidationAssistant(false);
            EmployeeHeaderTextBlock.Text = "Registro de empleado";

            EmployeeNameTextBox.Text = "";
            EmployeeLastNameTextBox.Text = "";
            EmployeeEmailTextBox.Text = "";
            EmployeePhoneTextBox.Text = "";
            EmployeeTypeComboBox.Text = "";
            EmployeeSalaryTextBox.Text = "";
            EmployeeUsernameTextBox.Text = "";
            EmployeePasswordTextBox.Text = "";
            EmployeeTimeOfEntryTextBox.Text = "";
            EmployeeDepartureTimeTextBox.Text = "";
            EmployeeTimeTextBox.Text = "";

            AcceptButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Collapsed;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";
            EmployeeAdmissionDateDatePicker.SelectedDate = DateTime.Now;
            EmployeePasswordGrid.Visibility = Visibility.Visible;
            EmployeePasswordGrid2.Visibility = Visibility.Visible;
            //timeAllStackPanel.Visibility = Visibility.Hidden;
            //EmployeeTimeViewTextBox.Visibility = Visibility.Hidden;

            HintAssist.SetHelperText(EmployeeNameTextBox, "Ingrese nombre del empleado");
            HintAssist.SetHelperText(EmployeeLastNameTextBox, "Ingrese apellidos del empleado");
            HintAssist.SetHelperText(EmployeeEmailTextBox, "Ingrese correo eléctronico");
            HintAssist.SetHelperText(EmployeePhoneTextBox, "Ingrese teléfono del empleado");
            HintAssist.SetHelperText(EmployeeTypeComboBox, "Selecciona un rol de empleado");
            HintAssist.SetHelperText(EmployeeAdmissionDateDatePicker, "Fecha actual establecida");
            HintAssist.SetHelperText(EmployeeSalaryTextBox, "Ingrese salario del empleado");
            HintAssist.SetHelperText(EmployeeUsernameTextBox, "Ingrese usuario del empleado");
            HintAssist.SetHelperText(EmployeePasswordTextBox, "Ingrese contraseña del empleado");

            EmployeeTimeTextBox.IsEnabled = false;
        }

        private void ShowSpecificEmployeeInformation(object sender, MouseButtonEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            SeventhLayerBorder.Visibility = Visibility.Visible;
            EmployeeViewGrid.Visibility = Visibility.Visible;
            EmployeeContract employeeContract = (EmployeeContract)EmployeeListBox.SelectedItem;
            EmployeeNameViewTextBox.Text = employeeContract.Name;
            EmployeeLastNameViewTextBox.Text = employeeContract.LastName;
            EmployeeEmailViewTextBox.Text = employeeContract.Email;
            EmployeePhoneViewTextBox.Text = employeeContract.Phone;
            EmployeeViewDatePicker.SelectedDate = employeeContract.AdmissionDate;
            EmployeePasswordViewTextBox.Text = employeeContract.Password;
            EmployeeRoleViewTextBox.Text = employeeContract.Role;
            EmployeeSalaryViewTextBox.Text = employeeContract.Salary.ToString();
            EmployeeUsernameViewTextBox.Text = employeeContract.Username;
            RequestGetWorkshift(employeeContract.IdUserEmployee);

            EmployeeGrid.Visibility = Visibility.Hidden;
            EmployeeGrid2.Visibility = Visibility.Hidden;
        }

        private void LoadEmployeesWorkshift(WorkshiftContract workshiftContracts)
        {

            EmployeeDepartureTimeViewTextBox.Text = workshiftContracts.DepartureTime;
            EmployeeTimeViewTextBox.Text = workshiftContracts.Time;
            EmployeeTimeOfEntryViewTextBox.Text = workshiftContracts.TimeOfEntry;

            EmployeeDepartureTimeTextBox.Text = workshiftContracts.DepartureTime;
            EmployeeTimeTextBox.Text = workshiftContracts.Time;
            EmployeeTimeOfEntryTextBox.Text = workshiftContracts.TimeOfEntry;
        }

        public void ChangeEmployeeType(object sender, EventArgs e)
        {
            List<string> employeeTypeList = new List<string>()
            {
                "Mesero", "Contador", "Cocinero", "Administrador", "Gerente"
            };
        }

        public void ShowSearchResults(object sender, KeyEventArgs e)
        {

            InitialMessageBorder.Visibility = Visibility.Hidden;
            EmployeeTableGrid.Visibility = Visibility.Visible;
            RequestEmployeeList();

        }

        //public void ShowSearchResults(object sender, TextChangedEventArgs e)
        //{
        //    string search = SearchTextBox.Text;
        //    ESearchFilters searchFilters = GetFilterSelected();
        //    List<EmployeeContract> employeeContracts = new List<EmployeeContract>();
        //    //EmployeeListBox.ItemsSource = null;
        //    EmployeeListBox.Items.Clear();

        //    foreach (EmployeeContract employee in allEmployeeContracts)
        //    {
        //        employeeContracts.Add(employee);
        //    }

        //    if ((employeeContracts.Count > 0) && (searchFilters != ESearchFilters.NoSelection))
        //    {
        //        if (!string.IsNullOrWhiteSpace(search))
        //        {
        //            InitialMessageBorder.Visibility = Visibility.Collapsed;
        //            EmployeeTableGrid.Visibility = Visibility.Visible;

        //            if (EmployeeFilterRadioButton.IsChecked == true)
        //            {
        //                SearchFilterTextBlock.Text = "Consulta: Empleado/" + search;
        //                foreach (EmployeeContract employeeContract in employeeContracts)
        //                {
        //                    string employeeFullName = employeeContract.Name + " " + employeeContract.LastName;
        //                    if (employeeFullName.ToLower().StartsWith(search.Trim().ToLower()))
        //                    {
        //                        EmployeeListBox.Items.Add(employeeContract);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (EmployeeListBox.Items.Count > 0)
        //    {
        //        if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
        //        {
        //            InitialMessageBorder.Visibility = Visibility.Hidden;
        //            EmployeeTableGrid.Visibility = Visibility.Visible;

        //            if (EmployeeFilterRadioButton.IsChecked == true)
        //            {
        //                //EmployeeListBox.Items.Clear();
        //                count = 0;
        //                EmployeeListBox.ItemsSource = null;
        //                SearchFilterTextBlock.Text = "Consulta :" + "Nombre" + "/" + search;

        //                List<EmployeeContract> employeeContracts = new List<EmployeeContract>();

        //                foreach (EmployeeContract employee in EmployeeListBox.Items)
        //                {
        //                    employeeContracts.Add(employee);
        //                }

        //                //EmployeeListBox.Items.Clear();
        //                EmployeeListBox.ItemsSource = null;

        //                foreach (EmployeeContract employeeContract in employeeContracts)
        //                {
        //                    string employeeFullName = employeeContract.Name + " " + employeeContract.LastName;

        //                    if (employeeFullName.ToLower().StartsWith(search.Trim().ToLower()))
        //                    {
        //                        EmployeeListBox.Items.Add(employeeContract);
        //                        count++;
        //                    }
        //                }
        //            }

        //            if (RoleFilterRadioButton.IsChecked == true)
        //            {
        //                //EmployeeListBox.Items.Clear();
        //            }

        //            if (WorkshiftFilterRadioButton.IsChecked == true)
        //            {
        //                //List<WorkshiftContract> workshiftContracts = new List<WorkshiftContract>();

        //            }

        //            if (EmployeeListBox.Items.Count == 0)
        //            {
        //                SearchResultMessageTextBlock.Text = "Sin resultados de búsqueda";
        //                InitialMessageBorder.Visibility = Visibility.Visible;
        //                EmployeeTableGrid.Visibility = Visibility.Hidden;
        //                EmployeeListBox.ItemsSource = null;
        //            }

        //        }
        //    }
        //}

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
            RemoveValidationAssistant(false);
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            EmployeeInformationGrid.Visibility = Visibility.Visible;
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
        }

        private void ExitSpecificEmployeeInformation(object sender, RoutedEventArgs e)
        {
            string header = EmployeeHeaderTextBlock.Text;

            if (string.Equals(header, "Registro de empleado") || string.Equals(header, "Actualización de empleado"))
            {
                RemoveValidationAssistant(true);
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            }
            else
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

        public void ExitDetailedInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            EmployeeViewGrid.Visibility = Visibility.Hidden;
            SeventhLayerBorder.Visibility = Visibility.Hidden;
        }

        public void ShowEmployeeUpdateDialog(object sender, RoutedEventArgs e)
        {
            ShowEmployeeDialogs();
            RemoveValidationAssistant(false);
            EmployeeHeaderTextBlock.Text = "Actualización de empleado";
            AcceptButton.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";
            EmployeeTimeTextBox.IsEnabled = false;
            EmployeePasswordGrid.Visibility = Visibility.Hidden;
            EmployeePasswordGrid2.Visibility = Visibility.Hidden;

            EmployeeContract employee = (EmployeeContract)EmployeeListBox.SelectedItem;

            if (employee != null)
            {
                EmployeeNameTextBox.Text = employee.Name;
                EmployeeLastNameTextBox.Text = employee.LastName;
                EmployeeEmailTextBox.Text = employee.Email;
                EmployeePhoneTextBox.Text = employee.Phone;
                EmployeeAdmissionDateDatePicker.SelectedDate = employee.AdmissionDate;
                EmployeePasswordTextBox.Text = employee.Password;
                EmployeeTypeComboBox.Text = employee.Role;
                EmployeeSalaryTextBox.Text = employee.Salary.ToString();
                EmployeeUsernameTextBox.Text = employee.Username;
                RequestGetWorkshift(employee.IdUserEmployee);
            }
        }

        public void ShowEmployeeDeleteDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void AcceptDeleteConfirmationButton(object sender, RoutedEventArgs e)
        {
            EmployeeContract employeeContract = (EmployeeContract)EmployeeListBox.SelectedItem;

            if (employeeContract != null)
            {
                RequestDeleteEmployee(employeeContract.IdUserEmployee);
                DeleteConfirmationGrid.Visibility = Visibility.Hidden;
                FifthLayerBorder.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("sin selección");
            }
        }

        private void SaveRegisterOrUpdateEmployee(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            EmployeeInformationGrid.Visibility = Visibility.Hidden;
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;

            RequestRegisterEmployee();
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

        public void RemoveValidationAssistant(bool isNotVisible)
        {
            ValidationAssist.SetSuppress(EmployeeNameTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeeLastNameTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeePhoneTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeeEmailTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeePasswordTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeeTypeComboBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeeSalaryTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeeUsernameTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeeDepartureTimeTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeeTimeTextBox, isNotVisible);
            ValidationAssist.SetSuppress(EmployeeTimeOfEntryTextBox, isNotVisible);
        }

        #endregion





    }
}