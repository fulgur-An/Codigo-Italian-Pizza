using ItalianPizza.BusinessObjects;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MaterialDesignThemes.Wpf;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace ItalianPizza.Views
{
    /// <summary>
    /// Lógica de interacción para CheckOrdersPage.xaml
    /// </summary>
    public partial class CheckOrdersPage : Page
    {
        private string usernameLoggedIn;
        
        private OrderViewModel orderViewModel = new OrderViewModel();

        private List<QuantityFoodRecipeViewModel> foodRecipeList = new List<QuantityFoodRecipeViewModel>();
        private List<QuantityItemViewModel> itemList = new List<QuantityItemViewModel>();

        private readonly NotificationManager notificationManager = new NotificationManager();
    
        public CheckOrdersPage(string usernameLoggedIn)
        {
            InitializeComponent();
            DataContext = orderViewModel;
            this.usernameLoggedIn = usernameLoggedIn;

            OrderFoodRecipeDataGrid.ItemsSource = foodRecipeList;
            OrderItemsDataGrid.ItemsSource = itemList;
            OrderFoodRecipeDataGrid.CellEditEnding += RecalculateOrderTotal;
            OrderItemsDataGrid.CellEditEnding += RecalculateOrderTotal;
        }

        #region GUI Methods  

        public void ShowOrderDialogs(bool isEnabled)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            OrderInformationGrid.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Visibility = Visibility.Visible;
            SaveOrderButton.Visibility = Visibility.Collapsed;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Hidden;
            CustomerNameComboBox.IsEnabled = isEnabled;
            OrderTypeComboBox.IsEnabled = isEnabled;
            OrderAddressComboBox.IsEnabled = isEnabled;
        }

        public void ShowOrderRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(true);
            AdaptOrderGUI(430, 200, 230, 160, 1);

            OrderHeaderTextBlock.Text = "Registro de Pedido";
            AddProductsGrid.Visibility = Visibility.Visible;
            RegisterCustomerButton.Visibility = Visibility.Visible;
            AcceptOrderRegistrationButtton.Visibility = Visibility.Visible;
            GenerateOrderTicketButton.Visibility = Visibility.Collapsed;
            AddFoodRecipeButton.Visibility = Visibility.Visible;
            AddItemButton.Visibility = Visibility.Visible;
            CustomerNameComboBox.SelectedIndex = -1;
            OrderTypeComboBox.SelectedIndex = -1;
            OrderAddressComboBox.SelectedIndex = -1;
            FoodRecipeComboBox.SelectedItem = -1;
            ItemsComboBox.SelectedItem = -1;
            OrderDateDatePicker.SelectedDate = DateTime.Now;
            OrderDateTimePicker.SelectedTime = DateTime.Now;
            //OrderFoodRecipeDataGrid.CanUserAddRows = true;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";

            HintAssist.SetHelperText(CustomerNameComboBox, "Selecciona el nombre de un cliente");
            HintAssist.SetHelperText(OrderDateDatePicker, "Fecha actual establecida");
            HintAssist.SetHelperText(OrderDateTimePicker, "Hora actual establecida");
            HintAssist.SetHelperText(OrderStatusComboBox, "Estatus 'En proceso' establecido");
            HintAssist.SetHelperText(OrderAddressComboBox, "Selecciona una dirección del cliente");
            HintAssist.SetHelperText(OrderTypeComboBox, "Selecciona el tipo de entrega");
        }

        public void UpdateOrderTotal()
        {
            // int orderTotal = int.Parse(OrderTotalTextBlock.Text);
            int orderTotal = 0;

            for (int i = 0; i < foodRecipeList.Count; i++)
            {
                int quantity = int.Parse(foodRecipeList[i].Cantidad);
                string[] value = foodRecipeList[i].Precio.Split('$');
                orderTotal += quantity * int.Parse(value[1]);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                int quantity = int.Parse(itemList[i].Cantidad);
                string[] value = itemList[i].Precio.Split('$');
                orderTotal += quantity * int.Parse(value[1]);
            }

            OrderTotalTextBlock.Text = orderTotal + "";
        }

        public void RecalculateOrderTotal(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    string quantityEdited = e.EditingElement.ToString();
                    string[] value = quantityEdited.Split(':');
                    int quantity = 0;
                    bool isNumber = int.TryParse(value[1], out quantity);

                    if (isNumber)
                    {
                        UpdateOrderTotal();
                    }
                }
            }
        }

        public void AddRecipeToOrder(object sender, RoutedEventArgs e)
        {
            QuantityFoodRecipeViewModel foodRecipe = new QuantityFoodRecipeViewModel();
            string[] value = FoodRecipeComboBox.Text.Split('$');
            foodRecipe.Cantidad = "1";
            foodRecipe.Nombre = value[0];
            foodRecipe.Precio = "$" + value[1];

            bool isFound = false;

            if (foodRecipeList.Count == 0)
            {
                foodRecipeList.Add(foodRecipe);
            }
            else
            {
                for (int i = 0; i < foodRecipeList.Count; i++)
                {
                    if (string.Equals(foodRecipe.Nombre, foodRecipeList[i].Nombre))
                    {
                        isFound = true;
                    }
                }

                if (!isFound)
                {
                    foodRecipeList.Add(foodRecipe);
                }
            }

            OrderFoodRecipeDataGrid.ItemsSource = foodRecipeList;
            OrderFoodRecipeDataGrid.Items.Refresh();
            UpdateOrderTotal();
            FoodRecipeComboBox.SelectedIndex = -1;
            DeleteFoodRecipeButton.IsEnabled = true;

        }

        public void DeleteRecipeToOrder(object sender, RoutedEventArgs e)
        {
            if (OrderFoodRecipeDataGrid.SelectedItem != null)
            {
                QuantityFoodRecipeViewModel foodRecipe = (QuantityFoodRecipeViewModel)OrderFoodRecipeDataGrid.SelectedItem;
                foodRecipeList.Remove(foodRecipe);
                OrderFoodRecipeDataGrid.Items.Refresh();
                UpdateOrderTotal();

                if (OrderFoodRecipeDataGrid.Items.Count <= 0)
                {
                    DeleteFoodRecipeButton.IsEnabled = false;
                }
            }
            else
            {
                ShowUnselectedItemToast();
            }
        }

        public void AddItemToOrder(object sender, RoutedEventArgs e)
        {
            QuantityItemViewModel item = new QuantityItemViewModel();
            string[] value = ItemsComboBox.Text.Split('$');
            item.Cantidad = "1";
            item.Nombre = value[0];
            item.Precio = "$" + value[1];

            bool isFound = false;

            if (itemList.Count == 0)
            {
                itemList.Add(item);
            }
            else
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (string.Equals(item.Nombre, itemList[i].Nombre))
                    {
                        isFound = true;
                    }
                }

                if (!isFound)
                {
                    itemList.Add(item);
                }
            }

            //OrderItemsDataGrid.ItemsSource = foodRecipeList;
            OrderItemsDataGrid.Items.Refresh();
            UpdateOrderTotal();
            ItemsComboBox.SelectedIndex = -1;
            DeleteItemButton.IsEnabled = true;
        }

        public void DeleteItemToOrder(object sender, RoutedEventArgs e)
        {
            if (OrderItemsDataGrid.SelectedItem != null)
            {
                QuantityItemViewModel item = (QuantityItemViewModel)OrderItemsDataGrid.SelectedItem;
                itemList.Remove(item);
                OrderItemsDataGrid.Items.Refresh();
                UpdateOrderTotal();

                if (OrderItemsDataGrid.Items.Count <= 0)
                {
                    DeleteItemButton.IsEnabled = false;
                }
            }
            else
            {
                ShowUnselectedItemToast();
            }
        }

        public void ChangeOrderType(object sender, EventArgs e)
        {
            List<int> tableList = new List<int>();
            List<string> orderTypeList = new List<string>()
            {
                "Domicilio", "Local"
            };
            for (int i = 1; i < 10; i++)
            {
                tableList.Add(i);
            }

            string orderType = OrderTypeComboBox.Text;

            if (string.Equals(orderType, "Local"))
            {
                HeaderAddressTextblock.Text = "*Mesa";
                OrderAddressComboBox.ItemsSource = tableList;
                HintAssist.SetHelperText(OrderAddressComboBox, "Selecciona el número de mesa");
                CustomerNameComboBox.IsEnabled = false;
                CustomerNameComboBox.SelectedIndex = -1;

                CustomerNameStackPanel.Visibility = Visibility.Hidden;
                //HintAssist.SetHelperText(CustomerNameComboBox, "Campo deshabilitado");
                //HintAssist.SetHint(CustomerNameComboBox, "Campo deshabilitado");
            }
            else
            {
                HeaderAddressTextblock.Text = "*Dirección";
                CustomerNameStackPanel.Visibility = Visibility.Visible;
                OrderAddressComboBox.ItemsSource = new List<string>() { "Fidencio Ocaña #64 Col. Francisco Ferrer Guardia" };
                //HintAssist.SetHelperText(OrderAddressComboBox, "Selecciona una dirección del cliente");
                //HintAssist.SetHelperText(CustomerNameComboBox, "Selecciona un nombre");
                CustomerNameComboBox.IsEnabled = true;
                CustomerNameComboBox.SelectedIndex = -1;
                OrderAddressComboBox.SelectedIndex = -1;
            }
        }

        public void ShowSpecificOrderInformation(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(false);
            AdaptOrderGUI(680, 310, 370, 300, 2);

            OrderHeaderTextBlock.Text = "Pedido";
            AddProductsGrid.Visibility = Visibility.Hidden;
            RegisterCustomerButton.Visibility = Visibility.Collapsed;
            AcceptOrderRegistrationButtton.Visibility = Visibility.Collapsed;
            GenerateOrderTicketButton.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Información detallada";

            HintAssist.SetHelperText(CustomerNameComboBox, string.Empty);
            HintAssist.SetHelperText(OrderDateDatePicker, string.Empty);
            HintAssist.SetHelperText(OrderDateTimePicker, string.Empty);
            HintAssist.SetHelperText(OrderStatusComboBox, string.Empty);
            HintAssist.SetHelperText(OrderAddressComboBox, string.Empty);
            HintAssist.SetHelperText(OrderTypeComboBox, string.Empty);
        }

        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InitialMessageBorder.Visibility = Visibility.Hidden;
                OrderTableGrid.Visibility = Visibility.Visible;
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
            if (CustomerFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Cliente");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el nombre del cliente");
            }
            else if (DateFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Fecha");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa una fecha en el formato: dd/mm/aaaaa");
            }
            else if (StatusFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Estatus");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa algún estatus: En Proceso, Entregado, Cancelado");
            }
        }

        public void ResetSearchFilters(object sender, RoutedEventArgs e)
        {
            CustomerFilterRadioButton.IsChecked = false;
            DateFilterRadioButton.IsChecked = false;
            StatusFilterRadioButton.IsChecked = false;
            SearchTextBox.Text = "";
            InitialMessageBorder.Visibility = Visibility.Visible;
            OrderTableGrid.Visibility = Visibility.Hidden;
            SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
            HintAssist.SetHint(SearchTextBox, "Buscar");
            HintAssist.SetHelperText(SearchTextBox, "Selecciona un filtro de búsqueda");
        }

        public void HideSpecificOrderInformation(object sender, RoutedEventArgs e)
        {
            bool isRegister = OrderHeaderTextBlock.Text.Equals("Registro de Pedido");

            if ((isRegister && !InvalidFieldsGrid.IsVisible) ||
                (SaveOrderButton.IsVisible && !FifthLayerBorder.IsVisible) ||
                (DeleteConfirmationGrid.IsVisible && !FifthLayerBorder.IsVisible))
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                if (DeleteConfirmationGrid.IsVisible)
                {
                    ShowConfirmationToast(sender, e);
                }

                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                OrderInformationGrid.Visibility = Visibility.Hidden;
                BackToOrderRegistration(sender, e);
            }
        }

        public void BackToOrderRegistration(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;
        }

        public void ShowOrderUpdateDialog(object sender, RoutedEventArgs e)
        {
            AdaptOrderGUI(430, 200, 230, 160, 1);
            AddProductsGrid.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            OrderInformationGrid.Visibility = Visibility.Visible;
            GenerateOrderTicketButton.Visibility = Visibility.Collapsed;
            SaveOrderButton.Visibility = Visibility.Visible;
            AcceptOrderRegistrationButtton.Visibility = Visibility.Collapsed;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";
            CustomerNameComboBox.IsEnabled = false;
            OrderStatusComboBox.IsEnabled = true;
            OrderAddressComboBox.IsEnabled = false;
            OrderFoodRecipeDataGrid.IsEnabled = true;
            OrderFoodRecipeDataGrid.CanUserAddRows = true;

            HintAssist.SetHelperText(CustomerNameComboBox, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderDateDatePicker, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderDateTimePicker, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderStatusComboBox, string.Empty);
            HintAssist.SetHelperText(OrderAddressComboBox, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderTypeComboBox, "Este campo no puedo modificarse");
        }

        public void AdaptOrderGUI(int largeFieldsWidth, int smallFieldsWidth, int marginExtra, int titleMargin, int span)
        {
            MainOrderFieldsGrid.SetValue(Grid.ColumnSpanProperty, span);
            OrderTypeBorder.Width = largeFieldsWidth;
            OrderTypeComboBox.Width = largeFieldsWidth;

            OrderDateBorder.Width = smallFieldsWidth;
            OrderDateDatePicker.Width = smallFieldsWidth;
            HourTitleTextBlock.Margin = new Thickness(titleMargin, 25, 0, 0);
            OrderHourBorder.Width = smallFieldsWidth;
            OrderHourBorder.Margin = new Thickness(marginExtra, 0, 0 ,0);
            OrderDateTimePicker.Width = smallFieldsWidth;
            OrderDateTimePicker.Margin = new Thickness(marginExtra - 310, 0, 0, 0);

            OrderStatusBorder.Width = smallFieldsWidth;
            OrderStatusComboBox.Width = smallFieldsWidth;
            CustomerTitleTextBlock.Margin = new Thickness(titleMargin - 270, 25, 0, 0);
            CustomerNameBorder.Width = smallFieldsWidth;
            CustomerNameBorder.Margin = new Thickness(marginExtra - 340, 0, 0, 0);
            CustomerNameComboBox.Width = smallFieldsWidth;
            CustomerNameComboBox.Margin = new Thickness(marginExtra - 340, 0, 0, 0);

            OrderAddressBorder.Width = largeFieldsWidth;
            OrderAddressComboBox.Width = largeFieldsWidth;

            if (span == 1)
            {
                CustomerTitleTextBlock.Margin = new Thickness(0, 25, 0 , 0);
                CustomerNameBorder.Margin = new Thickness(0);
                CustomerNameComboBox.Margin = new Thickness(0);
                OrderDateTimePicker.Margin = new Thickness(30, 0, 0, 0);
            }
        }

        public void ShowOrderCancellationDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void ShowConfirmationToast(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            OrderInformationGrid.Visibility = Visibility.Hidden;
            BackToOrderRegistration(sender, e);

            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = "Proceso Realizado",
                    Type = NotificationType.Success,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }

        public void ShowUnrealizedChangesToast(object sender, RoutedEventArgs e)
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Sin cambios en la actualización",
                    Message = "Proceso no realizado",
                    Type = NotificationType.Warning,
              }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }

        public void ShowUnselectedItemToast()
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Selecciona un elemento de la lista",
                    Message = "Proceso no realizado",
                    Type = NotificationType.Warning,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }

        #endregion
    }
}
