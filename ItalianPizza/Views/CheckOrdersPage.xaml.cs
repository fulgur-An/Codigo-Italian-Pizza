﻿using Backend.Contracts;
using Backend.Service;
using ItalianPizza.BusinessObjects;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MaterialDesignThemes.Wpf;
using Notifications.Wpf;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private ServerItalianPizzaProxy serverProxy;
        private IItalianPizzaService channel;
        private string usernameLoggedIn;
        private OrderViewModel orderViewModel = new OrderViewModel();

        private List<QuantityFoodRecipeViewModel> foodRecipeList = new List<QuantityFoodRecipeViewModel>();
        private List<QuantityItemViewModel> itemList = new List<QuantityItemViewModel>();

        private readonly NotificationManager notificationManager = new NotificationManager();
        public enum ESearchFilters
        {
            NameFilter = 1,
            DateFilter = 2,
            StatusFilter = 3,
            NoSelection = 4
        }

        public CheckOrdersPage(string usernameLoggedIn)
        {
            InitializeComponent();
            DataContext = orderViewModel;
            this.usernameLoggedIn = usernameLoggedIn;

            OrderFoodRecipeDataGrid.ItemsSource = foodRecipeList;
            OrderItemsDataGrid.ItemsSource = itemList;
            OrderFoodRecipeDataGrid.CellEditEnding += RecalculateOrderTotal;
            OrderItemsDataGrid.CellEditEnding += RecalculateOrderTotal;
            //RequestFoodRecipeList();
            //RequestItemList();
            //RequestCustomerList();
            RequestGetEmployeeId();
            RequestGetOrderList();
        }

        #region Request

        public void RequestFoodRecipeList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetRecipesAvailableEvent += LoadAllFoodRecipes;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetRecipesAvailable();
        }

        public void RequestItemList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();

            service.GetItemsSortedByNameEvent += LoadAllItems;

            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetItemsSortedByName();
        }

        public void RequestCustomerList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetCustomerListSortedByNameEvent += LoadAllCustomers;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetCustomerListSortedByName();
        }

        public void RequestGetEmployeeId()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetIdEmployeeByNameEvent += LoaddEmployeeId;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetIdEmployeeByName(usernameLoggedIn);
        }

        public void RequestRegisterOrder()
        {
            List<QuantityFoodRecipeContract> quantityFoodRecipeContracts = new List<QuantityFoodRecipeContract>();
            List<QuantityItemContract> quantityItemContracts = new List<QuantityItemContract>();
            CustomerContract customerContract = new CustomerContract();

            OrderContract order = new OrderContract
            {
                Date = (DateTime)OrderDateDatePicker.SelectedDate,
                Status = "En proceso",
                TotalToPay = decimal.Parse(OrderTotalTextBlock.Text),
                TypeOrder = OrderTypeComboBox.Text,
                IdEmployee = globalEmployeeId
            };

            if (CustomerNameComboBox.SelectedItem != null)
            {
                customerContract = (CustomerContract)CustomerNameComboBox.SelectedItem;
                order.IdCustomer = customerContract.IdUserCustomer;
            }
            else
            {
                order.TableNumber = int.Parse(OrderTableComboBox.Text);
            }

            if (OrderAddressComboBox.SelectedItem != null)
            {
                AddressContract addressContract = (AddressContract)OrderAddressComboBox.SelectedItem;
                order.Address = addressContract;
                //MessageBox.Show(addressContract.IdAddresses + "");
            }

            foreach (QuantityFoodRecipeViewModel quantityFoodRecipe in foodRecipeList)
            {
                string[] value = quantityFoodRecipe.Precio.Split('$');

                quantityFoodRecipeContracts.Add(new QuantityFoodRecipeContract
                {
                    IdFoodRecipe = quantityFoodRecipe.IdFoodRecipe,
                    QuantityOfFoodRecipes = int.Parse(quantityFoodRecipe.Cantidad),
                    Price = decimal.Parse(value[1]),
                });
            }

            foreach (QuantityItemViewModel quantityItem in itemList)
            {
                string[] value = quantityItem.Precio.Split('$');

                quantityItemContracts.Add(new QuantityItemContract
                {
                    IdItem = quantityItem.IdItem,
                    QuantityOfItems = int.Parse(quantityItem.Cantidad),
                    Price = decimal.Parse(value[1])
                }); ;
            }


            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.RegisterOrderEvent += ConfirmRegistrationOrder;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.RegisterOrder(order, quantityFoodRecipeContracts, quantityItemContracts);
        }

        public void RequestUpdateOrder(int orderId)
        {
            List<QuantityFoodRecipeContract> quantityFoodRecipeContracts = new List<QuantityFoodRecipeContract>();
            List<QuantityItemContract> quantityItemContracts = new List<QuantityItemContract>();

            foreach (QuantityFoodRecipeViewModel quantityFoodRecipe in OrderFoodRecipeDataGrid.Items)
            {
                string[] value = quantityFoodRecipe.Precio.Split('$');
                decimal price = 0;

                QuantityFoodRecipeContract quantityFoodRecipeContract = new QuantityFoodRecipeContract
                {
                    IdFoodRecipe = quantityFoodRecipe.IdFoodRecipe,
                    QuantityOfFoodRecipes = int.Parse(quantityFoodRecipe.Cantidad),
                    IdOrder = quantityFoodRecipe.IdOrder
                };

                if (value.Length > 1)
                {
                    quantityFoodRecipeContract.Price = decimal.TryParse(value[0], out price) ? decimal.Parse(value[0]) : decimal.Parse(value[1]);
                }
                else
                {
                    quantityFoodRecipeContract.Price = decimal.Parse(value[0]);
                }

                quantityFoodRecipeContracts.Add(quantityFoodRecipeContract);
            }

            foreach (QuantityItemViewModel quantityItem in OrderItemsDataGrid.Items)
            {
                string[] value = quantityItem.Precio.Split('$');
                decimal price = 0;

                QuantityItemContract quantityItemContract = new QuantityItemContract
                {
                    IdItem = quantityItem.IdItem,
                    QuantityOfItems = int.Parse(quantityItem.Cantidad),
                    IdOrder = quantityItem.IdOrder
                };

                if (value.Length > 1)
                {
                    quantityItemContract.Price = decimal.TryParse(value[0], out price) ? decimal.Parse(value[0]) : decimal.Parse(value[1]);
                }
                else
                {
                    quantityItemContract.Price = decimal.Parse(value[0]);
                }

                quantityItemContracts.Add(quantityItemContract);
            }

            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.UpdateOrderEvent += ConfirmUpdateOrder;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.UpdateOrder(orderId, quantityFoodRecipeContracts, quantityItemContracts);
            //MessageBox.Show(((QuantityFoodRecipeViewModel)OrderFoodRecipeDataGrid.Items[0]).IdOrder + "");
        }

        public void RequestGetOrderList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();

            service.GetOrderListEvent += LoadAllOrders;

            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetOrderList();
        }

        public void RequestDeleteOrderById(int orderId)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.DeleteOrderByIdEvent += ConfirmDeleteOrder;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.DeleteOrderById(orderId);
        }

        #endregion

        #region Implementation methods

        public void LoadAllFoodRecipes(List<FoodRecipeContract> foodRecipes)
        {
            foreach (FoodRecipeContract foodRecipeContract in foodRecipes)
            {
                FoodRecipeComboBox.Items.Add(foodRecipeContract);
            }
        }

        public void LoadAllItems(List<ItemContract> itemContracts)
        {
            foreach (ItemContract itemContract in itemContracts)
            {
                ItemsComboBox.Items.Add(itemContract);
            }
        }

        List<AddressContract> allAddress = new List<AddressContract>();
        public void LoadAllCustomers(List<CustomerContract> customerContracts, List<AddressContract> addressContracts)
        {
            foreach (CustomerContract customerContract in customerContracts)
            {
                CustomerNameComboBox.Items.Add(customerContract);
            }

            foreach (AddressContract addressContract in addressContracts)
            {
                allAddress.Add(addressContract);
            }
        }

        int globalEmployeeId = 0;

        public void LoaddEmployeeId(int idEmployee)
        {
            globalEmployeeId = idEmployee;

        }

        public void ConfirmRegistrationOrder(int result, List<string> foodRecipeErrors, List<string> itemErrors)
        {
            if ((result > 0) && (foodRecipeErrors.Count == 0) && (itemErrors.Count == 0))
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
                OrderListBox.Items.Clear();
                allOrderContracts.Clear();
                RequestGetOrderList();

            }
            else if ((foodRecipeErrors.Count > 0) || (itemErrors.Count > 0))
            {
                foreach (string error in foodRecipeErrors)
                {
                    NotificationType notificationType = NotificationType.Warning;
                    PersonalizeToast(notificationType, error);
                    ShowOrderDialogs(true);
                }

                foreach (string error in itemErrors)
                {
                    NotificationType notificationType = NotificationType.Warning;
                    PersonalizeToast(notificationType, error);
                    ShowOrderDialogs(true);
                }
            }
        }

        public void ConfirmUpdateOrder(int result)
        {
            if (result == -1)
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Proceso no realizado");
                QuarterLayerInformationBorder.Visibility = Visibility.Visible;
                OrderInformationGrid.Visibility = Visibility.Visible;

            }
            else if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
                OrderListBox.Items.Clear();

                allOrderContracts.Clear();
                RequestGetOrderList();
            }
        }

        private List<QuantityItemContract> allQuantityItemContracts = new List<QuantityItemContract>();
        private List<QuantityFoodRecipeContract> allQuantityFoodRecipeContracts = new List<QuantityFoodRecipeContract>();
        private List<OrderContract> allOrderContracts = new List<OrderContract>();
        private List<QuantityItemContract> auxiliaryItems = new List<QuantityItemContract>();
        private List<QuantityFoodRecipeContract> auxiliaryFoodRecipes = new List<QuantityFoodRecipeContract>();
        public void LoadAllOrders(List<OrderContract> orderContracts, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts, List<QuantityItemContract> quantityItemContracts)
        {
            allQuantityFoodRecipeContracts.Clear();
            allQuantityItemContracts.Clear();
            auxiliaryFoodRecipes.Clear();
            auxiliaryItems.Clear();

            foreach (OrderContract orderContract in orderContracts)
            {
                if (!OrderListBox.Items.Contains(orderContract))
                {
                    OrderListBox.Items.Add(orderContract);
                    allOrderContracts.Add(orderContract);
                }
            }

            foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
            {
                allQuantityFoodRecipeContracts.Add(quantityFoodRecipeContract);
                auxiliaryFoodRecipes.Add(quantityFoodRecipeContract);
            }

            foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
            {
                allQuantityItemContracts.Add(quantityItemContract);
                auxiliaryItems.Add(quantityItemContract);
            }
        }

        public void ConfirmDeleteOrder(int result)
        {
            if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
                allOrderContracts.Clear();
                RequestGetOrderList();
            }
        }

        #endregion

        #region GUI Methods  

        public ESearchFilters GetFilterSelected()
        {
            ESearchFilters filterSelected = new ESearchFilters();

            if (CustomerFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.NameFilter;
            }
            else if (DateFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.DateFilter;
            }
            else if (StatusFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.StatusFilter;
            }
            else
            {
                filterSelected = ESearchFilters.NoSelection;
            }

            return filterSelected;
        }

        public void FilterSelectedCustomerAddresses(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerNameComboBox.SelectedItem != null)
            {
                OrderAddressComboBox.Items.Clear();
                CustomerContract customerContract = (CustomerContract)CustomerNameComboBox.SelectedItem;

                foreach (AddressContract address in allAddress)
                {
                    if (address.IdCustomer == customerContract.IdUserCustomer)
                    {
                        OrderAddressComboBox.Items.Add(address);
                    }
                }
            }
        }

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
            foodRecipeList.Clear();
            itemList.Clear();
            allAddress.Clear();
            FoodRecipeComboBox.Items.Clear();
            ItemsComboBox.Items.Clear();
            CustomerNameComboBox.Items.Clear();
            OrderAddressComboBox.Items.Clear();
            OrderItemsDataGrid.ItemsSource = null;
            OrderFoodRecipeDataGrid.ItemsSource = null;

            RequestCustomerList();
            RequestFoodRecipeList();
            RequestItemList();

            CustomerNameComboBox.Visibility = Visibility.Visible;
            CustomerNameBorder.Visibility = Visibility.Visible;
            OrderAddressComboBox.Visibility = Visibility.Visible;
            CustomerTitleTextBlock.Visibility = Visibility.Visible;
            OrderTableComboBox.IsEnabled = true;
            OrderTableComboBox.Visibility = Visibility.Hidden;

            OrderHeaderTextBlock.Text = "Registro de Pedido";
            HeaderAddressTextblock.Text = "Dirección*";
            AddProductsGrid.Visibility = Visibility.Visible;
            //RegisterCustomerButton.Visibility = Visibility.Visible;
            AcceptOrderRegistrationButtton.Visibility = Visibility.Visible;
            GenerateOrderTicketButton.Visibility = Visibility.Collapsed;
            AddFoodRecipeButton.Visibility = Visibility.Visible;
            AddItemButton.Visibility = Visibility.Visible;
            CustomerNameComboBox.SelectedIndex = -1;
            OrderTypeComboBox.SelectedIndex = -1;
            OrderAddressComboBox.SelectedIndex = -1;
            FoodRecipeComboBox.SelectedIndex = -1;
            OrderAddressComboBox.SelectedIndex = -1;
            ItemsComboBox.SelectedItem = -1;
            OrderDateDatePicker.SelectedDate = DateTime.Now;
            OrderDateTimePicker.SelectedTime = DateTime.Now;
            OrderTotalTextBlock.Text = "0.00";
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
            decimal orderTotal = 0;

            for (int i = 0; i < foodRecipeList.Count; i++)
            {
                int quantity = int.Parse(foodRecipeList[i].Cantidad);
                string[] value = foodRecipeList[i].Precio.Split('$');
                orderTotal += quantity * decimal.Parse(value[1]);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                int quantity = int.Parse(itemList[i].Cantidad);
                string[] value = itemList[i].Precio.Split('$');
                orderTotal += quantity * decimal.Parse(value[1]);
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
            FoodRecipeContract foodRecipeContract = (FoodRecipeContract)FoodRecipeComboBox.SelectedItem;

            string[] value = (foodRecipeContract.Name + " $" + foodRecipeContract.Price).Split('$');
            foodRecipe.IdFoodRecipe = foodRecipeContract.IdFoodRecipe;
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

                OrderContract orderContract = (OrderContract)OrderListBox.SelectedItem;

                if (orderContract != null)
                {
                    foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in allQuantityFoodRecipeContracts)
                    {
                        if (orderContract.IdOrder == quantityFoodRecipeContract.IdOrder)
                        {
                            if (string.Equals(foodRecipe.Nombre.Trim(), quantityFoodRecipeContract.Name.Trim()))
                            {

                                isFound = true;
                            }
                        }
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

                if (!(foodRecipe.IdOrder == 0))
                {
                    QuantityFoodRecipeContract elimination = new QuantityFoodRecipeContract(); ;
                    foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in allQuantityFoodRecipeContracts)
                    {
                        if (foodRecipe.IdOrder == foodRecipe.IdOrder && foodRecipe.Nombre.Equals(quantityFoodRecipeContract.Name))
                        {
                            elimination = quantityFoodRecipeContract;
                        }
                    }

                    allQuantityFoodRecipeContracts.Remove(elimination);
                }

                if (OrderFoodRecipeDataGrid.Items.Count <= 0)
                {
                    DeleteFoodRecipeButton.IsEnabled = false;
                }
            }
            else
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Selecciona un elemento de la lista. Proceso no realizado");
            }
        }

        public void AddItemToOrder(object sender, RoutedEventArgs e)
        {
            QuantityItemViewModel item = new QuantityItemViewModel();
            ItemContract itemContract = (ItemContract)ItemsComboBox.SelectedItem;

            //string[] value = ItemsComboBox.Text.Split('$');
            string[] value = (itemContract.Name + " $" + itemContract.Price).Split('$');
            item.IdItem = itemContract.IdItem;
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

                OrderContract orderContract = (OrderContract)OrderListBox.SelectedItem;

                if (orderContract != null)
                {
                    foreach (QuantityItemContract quantityItemContract in allQuantityItemContracts)
                    {
                        if (orderContract.IdOrder == quantityItemContract.IdOrder)
                        {
                            if (string.Equals(item.Nombre.Trim(), quantityItemContract.Name.Trim()))
                            {
                                isFound = true;
                            }
                        }
                    }
                }

                if (!isFound)
                {
                    itemList.Add(item);
                }
            }

            OrderItemsDataGrid.ItemsSource = itemList;
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

                if (!(item.IdOrder == 0))
                {
                    QuantityItemContract elimination = new QuantityItemContract(); ;
                    foreach (QuantityItemContract quantityItemContract in allQuantityItemContracts)
                    {
                        if (item.IdOrder == item.IdOrder && item.Nombre.Equals(quantityItemContract.Name))
                        {
                            elimination = quantityItemContract;
                        }
                    }

                    allQuantityItemContracts.Remove(elimination);
                }

                if (OrderItemsDataGrid.Items.Count <= 0)
                {
                    DeleteItemButton.IsEnabled = false;
                }
            }
            else
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Selecciona un elemento de la lista. Proceso no realizado");
            }
        }

        public void ChangeOrderType(object sender, EventArgs e)
        {
            List<string> orderTypeList = new List<string>()
            {
                "Domicilio", "Local"
            };

            string orderType = OrderTypeComboBox.Text;

            if (string.Equals(orderType, "Local"))
            {
                HeaderAddressTextblock.Text = "*Mesa";


                for (int i = 1; i <= 10; i++)
                {
                    OrderTableComboBox.Items.Add(i);
                }

                OrderTableComboBox.Visibility = Visibility.Visible;
                OrderAddressComboBox.Visibility = Visibility.Collapsed;
                HintAssist.SetHelperText(OrderTableComboBox, "Selecciona el número de mesa");
                CustomerNameComboBox.IsEnabled = false;
                CustomerNameComboBox.SelectedIndex = -1;

                CustomerNameStackPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                OrderTableComboBox.Visibility = Visibility.Collapsed;
                OrderAddressComboBox.Visibility = Visibility.Visible;
                HeaderAddressTextblock.Text = "*Dirección";
                CustomerNameStackPanel.Visibility = Visibility.Visible;
                CustomerNameComboBox.IsEnabled = true;
                CustomerNameComboBox.SelectedIndex = -1;
                OrderAddressComboBox.SelectedIndex = -1;
            }
        }

        public void ShowSpecificOrderInformation(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(false);
            AdaptOrderGUI(680, 310, 370, 300, 2);
            OrderStatusComboBox.IsEnabled = false;
            AddProductsGrid.Visibility = Visibility.Hidden;
            //RegisterCustomerButton.Visibility = Visibility.Collapsed;
            AcceptOrderRegistrationButtton.Visibility = Visibility.Collapsed;
            GenerateOrderTicketButton.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Información detallada";
            OrderFoodRecipeDataGrid.IsEnabled = false;
            OrderItemsDataGrid.IsEnabled = false;

            if (OrderListBox.SelectedItem != null)
            {
                CustomerNameComboBox.Items.Clear();
                OrderAddressComboBox.Items.Clear();
                OrderStatusComboBox.Items.Clear();
                OrderFoodRecipeDataGrid.ItemsSource = null;
                OrderItemsDataGrid.ItemsSource = null;
                OrderContract orderContract = (OrderContract)OrderListBox.SelectedItem;
                OrderHeaderTextBlock.Text = "Pedido ID " + orderContract.IdOrder;
                OrderTypeComboBox.Text = orderContract.TypeOrder;
                OrderDateDatePicker.SelectedDate = orderContract.Date;
                OrderDateTimePicker.SelectedTime = orderContract.Date;
                OrderTotalTextBlock.Text = orderContract.TotalToPay + "";
                OrderStatusComboBox.Items.Add(orderContract.Status);
                OrderStatusComboBox.SelectedIndex = 0;

                if (orderContract.TypeOrder.Equals("Domicilio"))
                {
                    CustomerNameComboBox.Visibility = Visibility.Visible;
                    CustomerNameBorder.Visibility = Visibility.Visible;
                    OrderAddressComboBox.Visibility = Visibility.Visible;
                    CustomerTitleTextBlock.Visibility = Visibility.Visible;
                    OrderTableComboBox.Visibility = Visibility.Collapsed;
                    HeaderAddressTextblock.Text = "Dirección*";

                    CustomerContract customer = new CustomerContract();
                    string[] fullname = orderContract.CustomerFullName.Split(' ');
                    string name = "";
                    string lastName = fullname[fullname.Length - 2] + " " + fullname[fullname.Length - 1];

                    for (int i = 0; i < fullname.Length - 2; i++)
                    {
                        name += fullname[i] + " ";
                    }

                    customer.Name = name;
                    customer.LastName = lastName;
                    CustomerNameComboBox.Items.Add(customer);
                    CustomerNameComboBox.SelectedIndex = 0;

                    OrderAddressComboBox.Items.Add(orderContract.Address);
                    OrderAddressComboBox.SelectedIndex = 0;
                }
                else
                {
                    CustomerNameComboBox.Visibility = Visibility.Collapsed;
                    CustomerNameBorder.Visibility = Visibility.Collapsed;
                    OrderAddressComboBox.Visibility = Visibility.Collapsed;
                    CustomerTitleTextBlock.Visibility = Visibility.Collapsed;
                    OrderTableComboBox.Visibility = Visibility.Visible;
                    OrderTableComboBox.IsEnabled = false;
                    HeaderAddressTextblock.Text = "Mesa*";
                    OrderTableComboBox.Items.Add(orderContract.TableNumber);
                    //MessageBox.Show(OrderTableComboBox.Items[0] + "");
                    OrderTableComboBox.SelectedIndex = 0;
                }

                List<QuantityFoodRecipeViewModel> quantityFoodRecipeViewModels = new List<QuantityFoodRecipeViewModel>();
                List<QuantityItemViewModel> quantityItemViewModels = new List<QuantityItemViewModel>();

                foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in allQuantityFoodRecipeContracts)
                {
                    if (orderContract.IdOrder == quantityFoodRecipeContract.IdOrder)
                    {
                        quantityFoodRecipeViewModels.Add(new QuantityFoodRecipeViewModel
                        {
                            IdFoodRecipe = quantityFoodRecipeContract.IdFoodRecipe,
                            Cantidad = quantityFoodRecipeContract.QuantityOfFoodRecipes + "",
                            Nombre = quantityFoodRecipeContract.Name,
                            Precio = quantityFoodRecipeContract.Price + ""
                        });
                    }
                }

                foreach (QuantityItemContract quantityItemContract in allQuantityItemContracts)
                {
                    if (orderContract.IdOrder == quantityItemContract.IdOrder)
                    {
                        quantityItemViewModels.Add(new QuantityItemViewModel
                        {
                            IdItem = quantityItemContract.IdItem,
                            Cantidad = quantityItemContract.QuantityOfItems + "",
                            Nombre = quantityItemContract.Name,
                            Precio = quantityItemContract.Price + "",
                        });
                    }
                }

                OrderFoodRecipeDataGrid.ItemsSource = quantityFoodRecipeViewModels;
                OrderFoodRecipeDataGrid.Items.Refresh();
                OrderItemsDataGrid.ItemsSource = quantityItemViewModels;
                OrderItemsDataGrid.Items.Refresh();
            }
        }

        public void ShowSearchResults(object sender, TextChangedEventArgs e)
        {
            string search = SearchTextBox.Text;
            ESearchFilters searchFilters = GetFilterSelected();
            List<OrderContract> orderContracts = new List<OrderContract>();
            OrderListBox.Items.Clear();

            foreach (OrderContract order in allOrderContracts)
            {
                orderContracts.Add(order);
            }

            if ((orderContracts.Count > 0) && (searchFilters != ESearchFilters.NoSelection))
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    InitialMessageBorder.Visibility = Visibility.Collapsed;
                    OrderTableGrid.Visibility = Visibility.Visible;

                    if (CustomerFilterRadioButton.IsChecked == true)
                    {
                        SearchFilterTextBlock.Text = "Consulta: Cliente/" + search;
                        foreach (OrderContract orderContract in orderContracts)
                        {
                            string customerName = orderContract.CustomerFullName;
                            if (customerName.ToLower().StartsWith(search.Trim().ToLower()))
                            {
                                OrderListBox.Items.Add(orderContract);
                            }
                        }
                    }

                    if (DateFilterRadioButton.IsChecked == true)
                    {
                        SearchFilterTextBlock.Text = "Consulta: Fecha/" + search;

                        foreach (OrderContract orderContract in orderContracts)
                        {
                            string orderDate = orderContract.Date.ToShortDateString();

                            if (orderDate.ToLower().StartsWith(search.Trim().ToLower()))
                            {
                                OrderListBox.Items.Add(orderContract);
                            }
                        }
                    }

                    if (StatusFilterRadioButton.IsChecked == true)
                    {
                        SearchFilterTextBlock.Text = "Consulta: Estatus/" + search;

                        foreach (OrderContract orderContract in orderContracts)
                        {
                            string orderStatus = orderContract.Status;

                            if (orderStatus.ToLower().StartsWith(SearchTextBox.Text.Trim().ToLower()))
                            {
                                OrderListBox.Items.Add(orderContract);
                            }
                        }
                    }
                }

                if (OrderListBox.Items.Count == 0)
                {
                    SearchResultMessageTextBlock.Text = "Sin resultados de búsqueda";
                    InitialMessageBorder.Visibility = Visibility.Visible;
                    OrderTableGrid.Visibility = Visibility.Collapsed;
                    OrderListBox.Items.Clear();
                }
            }
            else if (string.IsNullOrWhiteSpace(search))
            {
                SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
                InitialMessageBorder.Visibility = Visibility.Visible;
                OrderTableGrid.Visibility = Visibility.Collapsed;
                OrderListBox.Items.Clear();
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
                OrderListBox.Items.Clear();
                InitialMessageBorder.Visibility = Visibility.Visible;
                OrderTableGrid.Visibility = Visibility.Hidden;
                SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
                SearchFilterTextBlock.Text = "Consulta: Cliente/Búsqueda";
                SearchTextBox.Text = "";
            }
            else if (DateFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Fecha");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa una fecha en el formato: dd/mm/aaaaa");
                OrderListBox.Items.Clear();
                InitialMessageBorder.Visibility = Visibility.Visible;
                OrderTableGrid.Visibility = Visibility.Hidden;
                SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
                SearchFilterTextBlock.Text = "Consulta: Fecha/Búsqueda";
                SearchTextBox.Text = "";
            }
            else if (StatusFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Estatus");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa algún estatus: En Proceso, Realizado, Cancelada");
                OrderListBox.Items.Clear();
                InitialMessageBorder.Visibility = Visibility.Visible;
                OrderTableGrid.Visibility = Visibility.Hidden;
                SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
                SearchFilterTextBlock.Text = "Consulta: Estatus/Búsqueda";
                SearchTextBox.Text = "";
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
            SearchFilterTextBlock.Text = "Consulta: Filtro/Búsqueda";
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
                RemoveValidationAssistant(true);
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                if (DeleteConfirmationGrid.IsVisible)
                {
                    OrderContract orderContract = (OrderContract)OrderListBox.SelectedItem;
                    if (orderContract != null)
                    {
                        if (orderContract.Status.Equals("En proceso"))
                        {
                            RequestDeleteOrderById(orderContract.IdOrder);
                        }
                        else
                        {
                            NotificationType notificationType = NotificationType.Warning;
                            PersonalizeToast(notificationType, "El pedido debe estar 'En Proceso'. Operación no realizada'");
                        }
                    }
                }

                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                OrderInformationGrid.Visibility = Visibility.Hidden;
                BackToOrderRegistration(sender, e);
            }
        }

        public void BackToOrderRegistration(object sender, RoutedEventArgs e)
        {
            RemoveValidationAssistant(false);
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;
        }

        public void ShowOrderUpdateDialog(object sender, RoutedEventArgs e)
        {
            itemList.Clear();
            foodRecipeList.Clear();
            FoodRecipeComboBox.Items.Clear();
            ItemsComboBox.Items.Clear();
            RequestFoodRecipeList();
            RequestItemList();
            //RequestGetOrderList();

            AdaptOrderGUI(430, 200, 230, 160, 1);
            AddProductsGrid.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            OrderInformationGrid.Visibility = Visibility.Visible;
            GenerateOrderTicketButton.Visibility = Visibility.Collapsed;
            SaveOrderButton.Visibility = Visibility.Visible;
            AcceptOrderRegistrationButtton.Visibility = Visibility.Collapsed;
            FieldsEnabledMessageTextBlock.Visibility = Visibility.Visible;
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";
            //RegisterCustomerButton.Visibility = Visibility.Hidden;
            DeleteFoodRecipeButton.IsEnabled = true;
            DeleteItemButton.IsEnabled = true;

            CustomerNameComboBox.IsEnabled = false;
            OrderStatusComboBox.IsEnabled = false;
            OrderAddressComboBox.IsEnabled = false;
            OrderTableComboBox.IsEnabled = false;
            OrderTypeComboBox.IsEnabled = false;
            OrderFoodRecipeDataGrid.IsEnabled = true;
            OrderItemsDataGrid.IsEnabled = true;
            OrderFoodRecipeDataGrid.ItemsSource = null;
            OrderItemsDataGrid.ItemsSource = null;

            HintAssist.SetHelperText(CustomerNameComboBox, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderDateDatePicker, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderDateTimePicker, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderStatusComboBox, string.Empty);
            HintAssist.SetHelperText(OrderAddressComboBox, "Este campo no puedo modificarse");
            HintAssist.SetHelperText(OrderTypeComboBox, "Este campo no puedo modificarse");

            OrderContract orderContract = (OrderContract)OrderListBox.SelectedItem;

            if (orderContract != null)
            {
                OrderHeaderTextBlock.Text = "Pedido ID " + orderContract.IdOrder;
                OrderDateDatePicker.SelectedDate = orderContract.Date;
                OrderDateTimePicker.SelectedTime = orderContract.Date;
                OrderTotalTextBlock.Text = orderContract.TotalToPay + "";
                CustomerNameComboBox.Items.Clear();

                CustomerContract customer = new CustomerContract();
                string[] fullname = orderContract.CustomerFullName.Split(' ');
                string name = "";
                string lastName = fullname[fullname.Length - 2] + " " + fullname[fullname.Length - 1];

                for (int i = 0; i < fullname.Length - 2; i++)
                {
                    name += fullname[i] + " ";
                }

                customer.Name = name;
                customer.LastName = lastName;
                CustomerNameComboBox.Items.Add(customer);
                CustomerNameComboBox.SelectedIndex = 0;


                if (orderContract.TypeOrder.Equals("Local"))
                {
                    OrderTypeComboBox.SelectedIndex = 1;
                    OrderTableComboBox.Items.Clear();
                    OrderTableComboBox.Items.Add(orderContract.TableNumber);
                    OrderTableComboBox.SelectedIndex = 0;
                    OrderAddressComboBox.Visibility = Visibility.Hidden;
                    OrderTableComboBox.Visibility = Visibility.Visible;
                }
                else
                {
                    OrderTypeComboBox.SelectedIndex = 0;
                    OrderAddressComboBox.Items.Clear();
                    OrderAddressComboBox.Items.Add(orderContract.Address);
                    OrderAddressComboBox.SelectedIndex = 0;
                    OrderAddressComboBox.Visibility = Visibility.Visible;
                    OrderTableComboBox.Visibility = Visibility.Hidden;
                }



                foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in auxiliaryFoodRecipes)
                {
                    if (orderContract.IdOrder == quantityFoodRecipeContract.IdOrder)
                    {
                        foodRecipeList.Add(new QuantityFoodRecipeViewModel
                        {
                            IdFoodRecipe = quantityFoodRecipeContract.IdFoodRecipe,
                            Cantidad = quantityFoodRecipeContract.QuantityOfFoodRecipes + "",
                            Nombre = quantityFoodRecipeContract.Name,
                            Precio = "$" + quantityFoodRecipeContract.Price,
                            IdOrder = quantityFoodRecipeContract.IdOrder
                        });
                    }
                }

                foreach (QuantityItemContract quantityItemContract in auxiliaryItems)
                {
                    if (orderContract.IdOrder == quantityItemContract.IdOrder)
                    {
                        itemList.Add(new QuantityItemViewModel
                        {
                            IdItem = quantityItemContract.IdItem,
                            Cantidad = quantityItemContract.QuantityOfItems + "",
                            Nombre = quantityItemContract.Name,
                            Precio = "$" + quantityItemContract.Price,
                            IdOrder = quantityItemContract.IdOrder
                        });
                    }
                }

                OrderFoodRecipeDataGrid.ItemsSource = foodRecipeList;
                OrderItemsDataGrid.ItemsSource = itemList;
            }
        }

        public void ReceiveOrderModification(object sender, RoutedEventArgs e)
        {
            OrderContract order = (OrderContract)OrderListBox.SelectedItem;

            if ((order != null) && order.Status.Equals("En proceso"))
            {
                RequestUpdateOrder(order.IdOrder);
            }
            else
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "El pedido debe estar 'En proceso. Operación no realizada'");
            }

            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            OrderInformationGrid.Visibility = Visibility.Hidden;
            BackToOrderRegistration(sender, e);
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
            OrderHourBorder.Margin = new Thickness(marginExtra, 0, 0, 0);
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
            OrderTableComboBox.Width = largeFieldsWidth;

            if (span == 1)
            {
                CustomerTitleTextBlock.Margin = new Thickness(0, 25, 0, 0);
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
            RequestRegisterOrder();
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            OrderInformationGrid.Visibility = Visibility.Hidden;
            BackToOrderRegistration(sender, e);
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
            ValidationAssist.SetSuppress(OrderTypeComboBox, isNotVisible);
            ValidationAssist.SetSuppress(CustomerNameComboBox, isNotVisible);
            ValidationAssist.SetSuppress(OrderAddressComboBox, isNotVisible);
            ValidationAssist.SetSuppress(FoodRecipeComboBox, isNotVisible);
            ValidationAssist.SetSuppress(ItemsComboBox, isNotVisible);
        }

        #endregion
    }
}