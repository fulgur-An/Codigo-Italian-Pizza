using Backend.Contracts;
using Backend.Service;
using ItalianPizza.BusinessObjects;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
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

namespace ItalianPizza.Views
{
    /// <summary>
    /// Lógica de interacción para InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {

        private ServerItalianPizzaProxy serverItalianPizzaProxy;
        private IItalianPizzaService serviceChannel;
        private string saveFileDialogName;
        private string usernameLoggedIn;
        private int idItemToUpdate;
        private ItemViewModel itemViewMode = new ItemViewModel();
        private StockTakingViewModel stockTakingViewModel = new StockTakingViewModel();
        private List<StockTakingContract> stocktakingToPrint = new List<StockTakingContract>();
        private readonly NotificationManager notificationManager = new NotificationManager();

        public InventoryPage(string usernameLoggedIn)
        {
            InitializeComponent();
            DataContext = itemViewMode;

            this.usernameLoggedIn = usernameLoggedIn;
        }

        #region service connection

        public void RegisterItem()
        {
            var service = new ItalianPizzaServiceCallback();
            service.RegisterItemEvent += RegistItem;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();

            ItemContract item = new ItemContract();
            item.Name = ItemNameField.Text;
            item.Description = ItemDescriptionField.Text;
            int sku = int.Parse(ItemCodeField.Text);
            item.Sku = sku;
            //item.Photo = itemContract.Text;
            item.Price = decimal.Parse(ItemValueField.Text);
            item.Quantity = int.Parse(ItemQuantityField.Text);
            if (itemImage.Source != null)
            {
                var bitmapImage = itemImage.Source as BitmapImage;
                item.Photo = getJPGFromImageControl(bitmapImage);
            }

            item.Restrictions = ItemRestrictionsField.Text;
            item.UnitOfMeasurement = ItemUnitOfMeasurementComboBox.Text;
            item.NeedsFoodRecipe = !isIngredientButton.IsChecked.Value;
            item.IsIngredient = isIngredientButton.IsChecked.Value;
            item.IsEnabled = true;
            serviceChannel.RegisterItem(item);
            ClearItemFields();
        }

        public void UpdateItem()
        {
            var service = new ItalianPizzaServiceCallback();
            service.UpdateItemEvent += UpdatedItem;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();

            ItemContract item = new ItemContract();
            item.IdItem = idItemToUpdate;
            item.Name = ItemNameField.Text;
            item.Description = ItemDescriptionField.Text;
            item.Sku = int.Parse(ItemCodeField.Text);
            if (!itemImage.Source.Equals(null))
            {
                var bitmapImage = itemImage.Source as BitmapImage;
                item.Photo = getJPGFromImageControl(bitmapImage);
            }
            item.Price = decimal.Parse(ItemValueField.Text);
            item.Quantity = int.Parse(ItemQuantityField.Text);
            item.Restrictions = ItemRestrictionsField.Text;
            item.UnitOfMeasurement = ItemUnitOfMeasurementComboBox.Text;
            item.NeedsFoodRecipe = !isIngredientButton.IsChecked.Value;
            item.IsIngredient = isIngredientButton.IsChecked.Value;
            serviceChannel.UpdateItem(item);
            ClearItemFields();
        }

        public void GetInventory(string filterString, int option)
        {
            var service = new ItalianPizzaServiceCallback();
            service.GetItemListEvent += LoadInventory;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.GetItemList(filterString, option);
        }

        public void DeleteItem(int itemId)
        {
            var service = new ItalianPizzaServiceCallback();
            service.DeleteItemEvent += ReciveResultDeleteItetm;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.DeleteItem(itemId);
        }

        private void ConfirmDeleteItem(object sender, RoutedEventArgs e)
        {
            int itemId = 0;

            List<ItemContract> items = (List<ItemContract>)InventoryTableBodyListBox.ItemsSource;
            for (int i = 0; i < items.Count(); i++)
            {
                if (!items[i].IsEnabled)
                {
                    itemId = items[i].IdItem;
                }
            }
            HideCancelGrid();
            DeleteItem(itemId);
        }

        private void GetStockTakingList()
        {
            var service = new ItalianPizzaServiceCallback();
            service.GetStockTakingEvent += LoadStockTaking;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            DateTime date = DateTime.Now;
            serviceChannel.GetStockTaking(DateTime.Now);
        }

        private void LoadStockTakingList()
        {
            var service = new ItalianPizzaServiceCallback();
            service.GetItemsForStocktakingEvent += FillStockTaking;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.GetItemsForStocktaking();
        }

        private void SaveStockTaking()
        {
            List<StockTakingContract> items = (List<StockTakingContract>)StocktakingDataGrid.ItemsSource;
            var service = new ItalianPizzaServiceCallback();
            service.RegisterStockTakingEvent += RegistStockTaking;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.RegisterStockTaking(items);
        }

        #endregion

        #region Callbacks connection

        public void RegistItem(int result)
        {
            if (result > 0)
            {
                PersonalizeToast(NotificationType.Success, "Proceso realizado", "Confirmación");
            }
            else
            {
                PersonalizeToast(NotificationType.Success, "Proceso cancelado", "Atención");
            }
        }

        public void UpdatedItem(int result)
        {
            if (result > 0)
            {
                PersonalizeToast(NotificationType.Success, "Proceso realizado", "Confirmación");
            }
            else
            {
                PersonalizeToast(NotificationType.Success, "Proceso cancelado", "Atención");
            }

        }

        public void LoadInventory(List<ItemContract> Items)
        {
            if (!Items.Count.Equals(0) && InventoryTableGrid.IsVisible)
            {

                InventoryTableBodyListBox.ItemsSource = null;
                InventoryTableBodyListBox.ItemsSource = Items;
                InventoryTableBodyListBox.Items.Refresh();

            }
        }

        public void ReciveResultDeleteItetm(int result)
        {
            if (result != 0)
            {
                PersonalizeToast(NotificationType.Success, "Proceso realizado", "Confirmación");
            }

        }

        public void LoadStockTaking(List<StockTakingContract> stockTakings)
        {
            if (!stockTakings.Count().Equals(0))
            {
                try
                {



                    GenerateFile file = new GenerateFile();

                    file.MakeInventoryReport(new Uri(saveFileDialogName), stockTakings);
                }
                catch (Exception ex)
                {
                    PersonalizeToast(NotificationType.Success, "Proceso cancelado", "Atención");
                }
                finally
                {
                    PersonalizeToast(NotificationType.Success, "Archivo creado", "Confirmación");
                }
                //stocktakingToPrint = stockTakings;
                //ValidateInventoryTableBodyListBox.ItemsSource = stockTakings;
                //PersonalizeToast(NotificationType.Success, "Proceso realizado","Confirmación");
            }
        }

        public void FillStockTaking(List<StockTakingContract> Items)
        {
            if (!Items.Count.Equals(0))
            {
                StocktakingDataGrid.ItemsSource = Items;
            }
        }

        public void RegistStockTaking(int result)
        {
            if (result > 0)
            {
                PersonalizeToast(NotificationType.Success, "Proceso realizado", "Confirmación");
            }
            else
            {
                PersonalizeToast(NotificationType.Success, "Proceso cancelado", "Atención");
            }
        }

        #endregion

        #region GUI Methods

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
                        //UpdateOrderTotal();
                    }
                }
            }
        }

        public int GetFilterSelection()
        {
            int option = 1;
            if (itemNameFilter.IsChecked.Value)
            {
                option = 1;
            }
            else if (itemSkuFilter.IsChecked.Value)
            {
                option = 2;
            }
            else if (itemPriceFilter.IsChecked.Value)
            {
                option = 3;
            }
            else if (itemQuantityFilter.IsChecked.Value)
            {
                option = 4;
            }
            return option;
        }


        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            int option = GetFilterSelection();

            string searchFilter = GettInventory.Text;
            InitialMessageBorder.Visibility = Visibility.Hidden;

            InventoryTableGrid.Visibility = Visibility.Visible;
            RegionSearchGrid.Visibility = Visibility.Visible;
            MainElementsInventoryGrid.Visibility = Visibility.Visible;
            ValidateInventoryTableGrid.Visibility = Visibility.Hidden;
            GetInventory(searchFilter, option);
        }

        public void ShowSelectedFilter(object sender, RoutedEventArgs e)
        {
            if (itemNameFilter.IsChecked == true)
            {
                HintAssist.SetHint(GettInventory, "Filtro seleccionado: Nombre");
                HintAssist.SetHelperText(GettInventory, "Ingresa el nombre de algun producto");
                SearchFilterTextBlock.Text = "Consulta: " + "Nombre" + "/" + "Búsqueda";
                GettInventory.Text = "";
            }
            else if (itemSkuFilter.IsChecked == true)
            {
                HintAssist.SetHint(GettInventory, "Filtro seleccionado: Código");
                HintAssist.SetHelperText(GettInventory, "Ingresa el codigo de algun producto");
                SearchFilterTextBlock.Text = "Consulta: " + "Código" + "/" + "Búsqueda";
                GettInventory.Text = "";
            }
            else if (itemPriceFilter.IsChecked == true)
            {
                HintAssist.SetHint(GettInventory, "Filtro seleccionado: Precio");
                HintAssist.SetHelperText(GettInventory, "Ingresa el precio de los productos a buscar");
                SearchFilterTextBlock.Text = "Consulta: " + "precio" + "/" + "Búsqueda";
                GettInventory.Text = "";
            }
            else if (itemQuantityFilter.IsChecked == true)
            {
                HintAssist.SetHint(GettInventory, "Filtro seleccionado: Cantidad");
                HintAssist.SetHelperText(GettInventory, "Ingresa la cantidad que desea buscar");
                SearchFilterTextBlock.Text = "Consulta: " + "cantidad" + "/" + "Búsqueda";
                GettInventory.Text = "";
            }
        }

        public void ShowValidateLayout(object sender, RoutedEventArgs e)
        {

            InitialMessageBorder.Visibility = Visibility.Hidden;

            InventoryTableGrid.Visibility = Visibility.Hidden;
            RegionSearchGrid.Visibility = Visibility.Hidden;
            MainElementsInventoryGrid.Visibility = Visibility.Hidden;
            ValidateInventoryTableGrid.Visibility = Visibility.Visible;
            LoadStockTakingList();
            DataContext = stockTakingViewModel;
        }

        public void CancelValidationLayout(object sender, RoutedEventArgs e)
        {
            ShowConfirmationDeparture();

            DataContext = itemViewMode;
        }

        public void HideValidateLayout()
        {
            InitialMessageBorder.Visibility = Visibility.Visible;

            InventoryTableGrid.Visibility = Visibility.Hidden;

            RegionSearchGrid.Visibility = Visibility.Visible;
            MainElementsInventoryGrid.Visibility = Visibility.Visible;
            ValidateInventoryTableGrid.Visibility = Visibility.Hidden;
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

        private void ShowSpecificItemInformation()
        {

            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            OrderInformationGrid.Visibility = Visibility.Visible;
            FieldsAreEditableTextBlock.Visibility = Visibility.Hidden;
            ChageEnableProperty(true);
        }
        private void ShowItemSpecification(object sender, RoutedEventArgs e)
        {
            ShowSpecificItemInformation();
            GetOutStackPanel.Visibility = Visibility.Visible;
            CancelRegisterItemButton.Visibility = Visibility.Hidden;
            RegisterItemButton.Visibility = Visibility.Hidden;
            UpdateItemDataButton.Visibility = Visibility.Hidden;
            ChageEnableProperty(false);
            FillItemSpecification();
        }

        private void UpdateItemSpecification(object sender, RoutedEventArgs e)
        {
            ShowSpecificItemInformation();
            GetOutStackPanel.Visibility = Visibility.Hidden;
            CancelRegisterItemButton.Visibility = Visibility.Visible;
            RegisterItemButton.Visibility = Visibility.Hidden;
            UpdateItemDataButton.Visibility = Visibility.Visible;
            ChageEnableProperty(true);
            FillItemSpecification();
        }

        public void FillItemSpecification()
        {
            ItemContract item = InventoryTableBodyListBox.SelectedItem as ItemContract;
            idItemToUpdate = item.IdItem;
            ItemNameField.Text = item.Name;
            ItemCodeField.Text = item.Sku.ToString();
            ItemDescriptionField.Text = item.Description;
            ItemValueField.Text = item.Price.ToString();
            ItemQuantityField.Text = item.Quantity.ToString();
            itemImage.Source = LoadImage(item.Photo);
            itemImage.Visibility = Visibility.Visible;
            uploadImageItemIcom.Visibility = Visibility.Collapsed;
            ItemUnitOfMeasurementComboBox.Text = item.UnitOfMeasurement;
            ItemRestrictionsField.Text = item.Restrictions;
        }

        public void HideSpecificItemInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            OrderInformationGrid.Visibility = Visibility.Hidden;
            ClearItemFields();
        }
        private void OpenRegistItem(object sender, RoutedEventArgs e)
        {
            ShowSpecificItemInformation();
            GetOutStackPanel.Visibility = Visibility.Hidden;
            RegisterItemButton.Visibility = Visibility.Visible;
            CancelRegisterItemButton.Visibility = Visibility.Visible;
            UpdateItemDataButton.Visibility = Visibility.Collapsed;
            FieldsAreEditableTextBlock.Visibility = Visibility.Visible;
        }

        private void ShowDeleteLayout(object sender, MouseButtonEventArgs e)
        {
            ThirdLayerDeleteBorder.Visibility = Visibility.Visible;
            QuarterLayerDeleteBorder.Visibility = Visibility.Visible;
            DeleteItemGrid.Visibility = Visibility.Visible;
            ItemContract item = InventoryTableBodyListBox.SelectedItem as ItemContract;
            item.IsEnabled = false;
            InventoryTableBodyListBox.SelectedItem = item;
            InventoryTableBodyListBox.Items.Refresh();
        }

        private void DenyDeleteItem(object sender, RoutedEventArgs e)
        {
            HideCancelGrid();
        }

        public void HideCancelGrid()
        {
            ThirdLayerDeleteBorder.Visibility = Visibility.Hidden;
            QuarterLayerDeleteBorder.Visibility = Visibility.Hidden;
            DeleteItemGrid.Visibility = Visibility.Hidden;
        }

        public void ShowEspecificDataItem(object sender, RoutedEventArgs e)
        {
            ShowSpecificItemInformation();
            GetOutStackPanel.Visibility = Visibility.Visible;
            RegisterItemButton.Visibility = Visibility.Collapsed;
            CancelRegisterItemButton.Visibility = Visibility.Hidden;
            UpdateItemDataButton.Visibility = Visibility.Collapsed;
            ChageEnableProperty(false);
        }

        public void ChageEnableProperty(bool enableProperty)
        {
            ItemNameField.IsEnabled = enableProperty;
            ItemCodeField.IsEnabled = enableProperty;
            ItemDescriptionField.IsEnabled = enableProperty;
            ItemValueField.IsEnabled = enableProperty;
            ItemQuantityField.IsEnabled = enableProperty;
            ItemRestrictionsField.IsEnabled = enableProperty;
            isIngredientButton.IsEnabled = enableProperty;
            ItemUnitOfMeasurementComboBox.IsEnabled = enableProperty;
        }

        public void UpdateItem(object sender, RoutedEventArgs e)
        {

            UpdateItem();
            HideSpecificItemInformation(sender, e);
        }

        public void RegistItem(object sender, RoutedEventArgs e)
        {

            RegisterItem();
            HideSpecificItemInformation(sender, e);

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

        public void HideDepartureConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmationBackBorder.Visibility = Visibility.Hidden;
            DeparureConfirmationBorder.Visibility = Visibility.Hidden;
            RemoveValidationAssistant(false);
        }

        private void AcceptDepartureConfirmation(object sender, RoutedEventArgs e)
        {
            PersonalizeToast(NotificationType.Success, "Proceso cancelado", "Atención");
            HideSpecificItemInformation(sender, e);
            HideValidateLayout();
            HideDepartureConfirmation(sender, e);
        }

        public void PrintValidationInventoryResult(object sender, RoutedEventArgs e)
        {
            SaveStockTaking();
            HideValidateLayout();
        }

        public void GenerateReport(object sender, RoutedEventArgs e)
        {
            MakeInventoryReport();

        }

        public void ClearItemFields()
        {
            ItemNameField.Clear();
            ItemCodeField.Clear();
            ItemDescriptionField.Clear();
            ItemValueField.Clear();
            ItemQuantityField.Clear();
            ItemUnitOfMeasurementComboBox.SelectedItem = null;
            ItemDescriptionField.Clear();
            itemImage.Visibility = Visibility.Collapsed;
            uploadImageItemIcom.Visibility = Visibility.Visible;
        }

        public void PersonalizeToast(NotificationType notificationType, string message, string title)
        {
            NotificationContent notificationContent = new NotificationContent
            {
                Title = title,
                Message = message,
                Type = notificationType
            };
            notificationManager.Show(notificationContent);

        }


        #endregion

        #region Files methods

        private void uploadImageItem(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de imagen (.jpg)|*.jpg|All Files(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == true)
            {
                //tbrurltext.text = "";

                try
                {
                    BitmapImage photo = new BitmapImage();
                    photo.BeginInit();
                    photo.UriSource = new Uri(ofd.FileName);
                    photo.EndInit();
                    photo.Freeze();

                    itemImage.Source = photo;
                    itemImage.Visibility = Visibility.Visible;
                    uploadImageItemIcom.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    PersonalizeToast(NotificationType.Success, "Proceso cancelado", "Atención");
                }
            }
        }

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
        public BitmapImage LoadImage(byte[] imageData)
        {
            BitmapImage image = new BitmapImage();
            try
            {

                if (imageData == null || imageData.Length == 0)
                {
                    image = null;
                }
                using (MemoryStream memoryStream = new MemoryStream(imageData))
                {
                    memoryStream.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = memoryStream;
                    image.EndInit();
                }
                image.Freeze();

            }
            catch (Exception)
            {
                image = null;
            }
            return image;
        }

        public void MakeInventoryReport()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF file|*.pdf";
            saveFileDialog1.Title = "Save an PDF File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {

                saveFileDialogName = saveFileDialog1.FileName;
                GetStockTakingList();
            }



        }

        public void RemoveValidationAssistant(bool isNotVisible)
        {
            ValidationAssist.SetSuppress(ItemNameField, isNotVisible);
            ValidationAssist.SetSuppress(ItemCodeField, isNotVisible);
            ValidationAssist.SetSuppress(ItemDescriptionField, isNotVisible);
            ValidationAssist.SetSuppress(ItemValueField, isNotVisible);
            ValidationAssist.SetSuppress(ItemQuantityField, isNotVisible);
            ValidationAssist.SetSuppress(ItemRestrictionsField, isNotVisible);
            ValidationAssist.SetSuppress(ItemUnitOfMeasurementComboBox, isNotVisible);
        }

        #endregion
    }
}