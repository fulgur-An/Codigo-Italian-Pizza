using Backend.Contracts;
using Backend.Service;
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

        private string usernameLoggedIn;

        private readonly NotificationManager notificationManager = new NotificationManager();

        public InventoryPage(string usernameLoggedIn)
        {
            InitializeComponent();
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
            item.Price = int.Parse(ItemValueField.Text);
            item.Quantity = int.Parse(ItemQuantityField.Text);
            var bitmapImage = itemImage.Source as BitmapImage;
            item.Photo = getJPGFromImageControl(bitmapImage);
            item.Restrictions = ItemRestrictionsField.Text;
            item.UnitOfMeasurement = ItemUnitOfMeasurementComboBox.Text;
            item.NeedsFoodRecipe = !isIngredientButton.IsChecked.Value;
            item.IsIngredient = isIngredientButton.IsChecked.Value;
            item.IsEnabled = true;
            serviceChannel.RegisterItem(item);
        }

        public void UpdateItem()
        {
            var service = new ItalianPizzaServiceCallback();
            service.UpdateItemEvent += UpdatedItem;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();

            ItemContract item = new ItemContract();
            item.Name = ItemNameField.Text;
            item.Description = ItemDescriptionField.Text;
            item.Sku = int.Parse(ItemCodeField.Text);
            //item.Photo = itemContract.Text;
            item.Price = int.Parse(ItemValueField.Text);
            item.Quantity = int.Parse(ItemQuantityField.Text);
            item.Restrictions = ItemRestrictionsField.Text;
            item.UnitOfMeasurement = ItemUnitOfMeasurementComboBox.Text;
            item.NeedsFoodRecipe = !isIngredientButton.IsChecked.Value;
            item.IsIngredient = isIngredientButton.IsChecked.Value;
            item.IsEnabled = true;
            serviceChannel.UpdateItem(item);
        }

        public void GetInventory(string filterString, int filterInt, int option)
        {
            var service = new ItalianPizzaServiceCallback();
            service.GetItemListEvent += LoadInventory;
            serverItalianPizzaProxy = new ServerItalianPizzaProxy(service);
            serviceChannel = serverItalianPizzaProxy.ChannelFactory.CreateChannel();
            serviceChannel.GetItemList(filterString, filterInt, option);
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
            DateTime date = new DateTime().Date;
            serviceChannel.GetStockTaking(date);
        }

        #endregion

        #region Callbacks connection

        public void RegistItem(int result)
        {
            if (result > 0)
            {
                ShowConfirmationToast();
            }
            else
            {
                ShowWarningToast();
            }
        }

        public void UpdatedItem(int result)
        {
            if (result > 0)
            {
                ShowConfirmationToast();
            }
            else
            {
                ShowWarningToast();
            }

        }

        public void LoadInventory(List<ItemContract> Items)
        {
            if (!Items.Count.Equals(0) && InventoryTableGrid.IsVisible)
            {
                InventoryTableBodyListBox.ItemsSource = null;
                InventoryTableBodyListBox.ItemsSource = Items;
                InventoryTableBodyListBox.Items.Refresh();
                ShowConfirmationToast();
            }
        }

        public void ReciveResultDeleteItetm(int result)
        {
            if (result != 0)
            {
                ShowConfirmationToast();
            }
            
        }

        public void LoadStockTaking(List<StockTakingContract> stockTakings)
        {
            if (!stockTakings.Count().Equals(0))
            {
                ValidateInventoryTableBodyListBox.ItemsSource = stockTakings;
                ShowConfirmationToast();
            }
            
        }

        #endregion

        #region GUI Methods

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
            int filterInt = 0;
            if (e.Key == Key.Return)
            {
                InitialMessageBorder.Visibility = Visibility.Hidden;

                InventoryTableGrid.Visibility = Visibility.Visible;
                RegionSearchGrid.Visibility = Visibility.Visible;
                MainElementsInventoryGrid.Visibility = Visibility.Visible;
                ValidateInventoryTableGrid.Visibility = Visibility.Hidden;
            }
            if (option != 1)
            {
                filterInt = int.Parse(searchFilter);
            }
            GetInventory(searchFilter, filterInt, option);
        }

        public void ShowValidateLayout(object sender, RoutedEventArgs e)
        {
            
            InitialMessageBorder.Visibility = Visibility.Hidden;

            InventoryTableGrid.Visibility = Visibility.Hidden;
            RegionSearchGrid.Visibility = Visibility.Hidden;
            MainElementsInventoryGrid.Visibility = Visibility.Hidden;
            ValidateInventoryTableGrid.Visibility = Visibility.Visible;
        }

        public void CancelValidationLayout(object sender, RoutedEventArgs e)
        {
            ShowConfirmationDeparture();
            
            
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

        public void FillItemSpecification()
        {
            ItemContract item = InventoryTableBodyListBox.SelectedItem as ItemContract;
            ItemNameField.Text = item.Name;
            ItemCodeField.Text = item.Sku.ToString();
            ItemDescriptionField.Text = item.Description;
            ItemValueField.Text = item.Price.ToString();
            ItemQuantityField.Text = item.Quantity.ToString();
            itemImage.Source = LoadImage(item.Photo);
            itemImage.Visibility = Visibility.Visible;
            uploadImageItemIcom.Visibility = Visibility.Collapsed;
            ItemUnitOfMeasurementComboBox.SelectedValue = item.UnitOfMeasurement;
            ItemRestrictionsField.Text = item.Restrictions;
        }

        public void HideSpecificItemInformation(object sender, RoutedEventArgs e)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            OrderInformationGrid.Visibility = Visibility.Hidden;
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


        //public void ShowUpdateFields(object sender, RoutedEventArgs e)
        //{
        //    ShowEspecificDataItem(sender, e);
        //    GetOutStackPanel.Visibility = Visibility.Hidden;
        //    RegisterItemButton.Visibility = Visibility.Collapsed;
        //    CancelRegisterItemButton.Visibility = Visibility.Visible;
        //    UpdateItemDataButton.Visibility = Visibility.Visible;
        //    FieldsAreEditableTextBlock.Visibility = Visibility.Visible;
        //    ChageEnableProperty(true);
        //    if (DeleteItemGrid.Visibility == Visibility.Visible)
        //    {
        //        HideSpecificItemInformation(sender, e);
        //    }
        //else if (visualizeIcon.ismouseover)
        //{
        //    getoutstackpanel.visibility = visibility.visible;
        //    registeritembutton.visibility = visibility.collapsed;
        //    cancelregisteritembutton.visibility = visibility.hidden;
        //    updateitemdatabutton.visibility = visibility.hidden;
        //    fieldsareeditabletextblock.visibility = visibility.hidden;
        //    chageenableproperty(false);
        //}
        //}

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
            HideSpecificItemInformation(sender, e);
            UpdateItem();
        }

        public void RegistItem(object sender, RoutedEventArgs e)
        {
            HideSpecificItemInformation(sender, e);
            RegisterItem();
           
        }
        public void CancelProdecure(object sender, RoutedEventArgs e)
        {
            ShowConfirmationDeparture();
        }

        public void ShowConfirmationDeparture()
        {
            ConfirmationBackBorder.Visibility = Visibility.Visible;
            DeparureConfirmationBorder.Visibility = Visibility.Visible;
            
        }

        public void HideDepartureConfirmation(object sender, RoutedEventArgs e)
        {
            ConfirmationBackBorder.Visibility = Visibility.Hidden;
            DeparureConfirmationBorder.Visibility = Visibility.Hidden;
        }

        private void AcceptDepartureConfirmation(object sender, RoutedEventArgs e)
        {
            ShowWarningToast();
            HideSpecificItemInformation(sender, e);
            HideValidateLayout();
            HideDepartureConfirmation(sender, e);
        }

        public void PrintValidationInventoryResult(object sender, RoutedEventArgs e)
        {
            HideValidateLayout();
            ShowConfirmationFileToast();
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

        public void ShowConfirmationToast()
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = "Proceso Realizado",
                    Type = NotificationType.Success,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }
        public void ShowConfirmationFileToast()
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Confirmación",
                    Message = "Archivo creaado",
                    Type = NotificationType.Success,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }

        public void ShowWarningToast()
        {
            notificationManager.Show(
                new NotificationContent
                {
                    Title = "Warning",
                    Message = "Proceso cancelado",
                    Type = NotificationType.Warning,
                }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            );
        }





        #endregion

        #region Files methods

        private void uploadImageItem(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de imagen (.jpg)|*.jpg|All Files(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;

            if(ofd.ShowDialog() == true)
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
                    ShowWarningToast();
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

                try
                {
                    List<ItemContract> items = new List<ItemContract>();
                    if (!InventoryTableBodyListBox.Items.Count.Equals(0))
                    {
                        items = (List<ItemContract>)InventoryTableBodyListBox.ItemsSource;
                    }
                    
                    GenerateFile file = new GenerateFile();

                    file.MakeInventoryReport(new Uri(saveFileDialog1.FileName), items);
                }
                catch (Exception ex)
                {
                    ShowWarningToast();
                }
                finally
                {
                    ShowConfirmationFileToast();
                }
            }


            
        }

        #endregion
    }
}
