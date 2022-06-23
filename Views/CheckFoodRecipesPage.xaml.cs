using Backend.Contracts;
using Backend.Service;
using ItalianPizza.BusinessObjects;
using MaterialDesignThemes.Wpf;
using Notifications.Wpf;
using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para CheckFoodRecipesPage.xaml
    /// </summary>
    public partial class CheckFoodRecipesPage : Page
    {
        private ServerItalianPizzaProxy serverProxy;
        private IItalianPizzaService channel;
        private string usernameLoggedIn;
        private List<IngredientViewModel> foodRecipeIngredients = new List<IngredientViewModel>();
        private FoodRecipeViewModel foodRecipe = new FoodRecipeViewModel();
        private List<string> recipeStepList = new List<string>();
        public enum ESearchFilters
        {
            CookFilter = 1,
            NameFilter = 2,
            IngredientFilter = 3,
            PortionsFilter = 4,
            NoSelection = 5
        }
        private enum statusOfRecipes
        {
            Available
        }
        private readonly NotificationManager notificationManager = new NotificationManager();

        public CheckFoodRecipesPage(string usernameLoggedIn)
        {
            InitializeComponent();
            this.DataContext = foodRecipe;
            this.usernameLoggedIn = usernameLoggedIn;
            RequestFoodRecipeList();
            Thread.Sleep(500);
            RequestGetFoodRecipesToPrepare();
        }

        #region Request

        public void RequestFoodRecipeList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();

            service.GetFoodRecipeListSortedByNameEvent += LoadAllFoodRecipes;

            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetFoodRecipeListSortedByName();
        }

        public void RequestIngredientList()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();

            service.GetItemIngredientListEvent += LoadAllIngredients;

            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetItemIngredientList();
        }

        public void RequestRegisterFoodRecipe()
        {
            FoodRecipeContract foodRecipeContract = new FoodRecipeContract();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            foodRecipeContract.Price = decimal.Parse(FoodRecipePriceTextBox.Text);
            foodRecipeContract.Name = FoodRecipeNameTextBox.Text;

            string steps = "";
            foreach (string step in recipeStepList)
            {
                steps += step;
            }

            foodRecipeContract.Description = steps;
            foodRecipeContract.NumberOfServings = int.Parse(FoodRecipePortionsComboBox.Text);
            foodRecipeContract.Status = statusOfRecipes.Available.ToString();
            foodRecipeContract.IsEnabled = true;


            foreach (IngredientViewModel ingredient in foodRecipeIngredients)
            {
                ingredientContracts.Add(new IngredientContract
                {
                    IdFoodRecipe = ingredient.IdItem,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }

            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.RegisterFoodRecipeEvent += ConfirmRegistrationFoodRecipe;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.RegisterFoodRecipe(foodRecipeContract, ingredientContracts);
        }

        public void RequestGetIngredientsByFoodRecipe(int idFoodRecipe)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();

            service.GetIngredientsByIdEvent += LoaddAllIngredientsById;

            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetIngredientsById(idFoodRecipe);
        }

        public void RequestGetFoodRecipeById(object sender, RoutedEventArgs e)
        {
            EmptyUIElements();

            if (FoodRecipeListBox.SelectedItem != null)
            {
                FoodRecipeViewModel foodRecipe = (FoodRecipeViewModel)FoodRecipeListBox.SelectedItem;
                ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
                service.GetFoodRecipeByIdEvent += LoadFoodRecipe;

                serverProxy = new ServerItalianPizzaProxy(service);
                channel = serverProxy.ChannelFactory.CreateChannel();
                channel.GetFoodRecipeById(foodRecipe.IdFoodRecipe);
                RequestGetIngredientsByFoodRecipe(foodRecipe.IdFoodRecipe);
                ShowSpecificFoodRecipeInformation();
            }
        }

        public void RequestDeleteFoodRecipe(int idFoodRecipe)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.DeleteFoodRecipeByIdEvent += ConfirmDeleteFoodRecipe;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.DeleteFoodRecipeById(idFoodRecipe);
        }

        public void RequestUpdateFoodRecipe()
        {
            FoodRecipeContract foodRecipeContract = new FoodRecipeContract();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            if (FoodRecipeListBox.SelectedItem != null)
            {
                foodRecipeContract.IdFoodRecipe = ((FoodRecipeViewModel)FoodRecipeListBox.SelectedItem).IdFoodRecipe;
            }

            foodRecipeContract.Price = decimal.Parse(FoodRecipePriceTextBox.Text);
            foodRecipeContract.Name = FoodRecipeNameTextBox.Text;

            string steps = "";
            foreach (string step in recipeStepList)
            {
                steps += step;
            }

            foodRecipeContract.Description = steps;
            foodRecipeContract.NumberOfServings = int.Parse(FoodRecipePortionsComboBox.Text);
            foodRecipeContract.Status = statusOfRecipes.Available.ToString();
            foodRecipeContract.IsEnabled = true;

            foreach (IngredientContract ingredient in recoveredIngredients)
            {
                ingredientContracts.Add(new IngredientContract
                {
                    IdIngredient = ingredient.IdIngredient,
                    IdFoodRecipe = ingredient.IdItem,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }

            foreach (IngredientViewModel ingredient in foodRecipeIngredients)
            {
                ingredientContracts.Add(new IngredientContract
                {
                    IdIngredient = ingredient.IdIngredient,
                    IdFoodRecipe = ingredient.IdItem,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }

            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.UpdateFoodRecipeEvent += ConfirmUpdateFoodRecipe;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.UpdateFoodRecipe(foodRecipeContract, ingredientContracts);
        }

        public void RequestGetFoodRecipesToPrepare()
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.GetFoodRecipeListToPrepareEvent += LoadFoodRecipeToPrepare;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.GetFoodRecipeListToPrepare();
        }

        public void RequestMarkRecipeAsDone(int orderId, int foodRecipeId)
        {
            ItalianPizzaServiceCallback service = new ItalianPizzaServiceCallback();
            service.MarkRecipeAsDoneEvent += ConfirmFoodRecipeMade;
            serverProxy = new ServerItalianPizzaProxy(service);
            channel = serverProxy.ChannelFactory.CreateChannel();
            channel.MarkRecipeAsDone(orderId, foodRecipeId);
        }

        #endregion

        #region Implementation methods

        List<FoodRecipeViewModel> allFoodRecipes = new List<FoodRecipeViewModel>();
        public void LoadAllFoodRecipes(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients)
        {
            foreach (FoodRecipeContract foodRecipe in foodRecipes)
            {
                string recipeIngredients = "";

                for (int i = 0; i < ingredients.Count; i++)
                {
                    if (ingredients[i].IdFoodRecipe == foodRecipe.IdFoodRecipe)
                    {
                        if (i == (ingredients.Count - 1))
                        {
                            recipeIngredients += ingredients[i].IngredientQuantity + " " + ingredients[i].IngredientName;
                        }
                        else
                        {
                            recipeIngredients += ingredients[i].IngredientQuantity + " " + ingredients[i].IngredientName + ", ";
                        }
                    }
                }

                allFoodRecipes.Add(new FoodRecipeViewModel
                {
                    IdFoodRecipe = foodRecipe.IdFoodRecipe,
                    Name = foodRecipe.Name,
                    Portions = foodRecipe.NumberOfServings + "",
                    Price = foodRecipe.Price + "",
                    Description = foodRecipe.Description,
                    Ingredients = recipeIngredients
                });
            }

            foreach (FoodRecipeViewModel foodRecipe in allFoodRecipes)
            {
                FoodRecipeListBox.Items.Add(foodRecipe);
            }
        }

        public void LoadAllIngredients(List<ItemContract> items)
        {
            foreach (ItemContract item in items)
            {
                IngredientsComboBox.Items.Add(item);
            }

            //IngredientsComboBox.ItemsSource = items;
            IngredientsComboBox.Items.Refresh();
        }

        public void ConfirmRegistrationFoodRecipe(int result)
        {
            if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
                allFoodRecipes.Clear();
                FoodRecipeListBox.Items.Clear();
                RequestFoodRecipeList();
            }
            if (result == -1)
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Nombre de receta ya registrado");
                ShowOrderDialogs(true);
            }
        }

        private List<IngredientContract> recoveredIngredients = new List<IngredientContract>();
      
        public void LoaddAllIngredientsById(List<IngredientContract> ingredients)
        {
            recoveredIngredients = ingredients;
            List<IngredientViewModel> ingredientViewModels = new List<IngredientViewModel>();
            foreach (IngredientContract ingredient in ingredients)
            {
                ingredientViewModels.Add(new IngredientViewModel 
                {                    
                    IdFoodRecipe = ingredient.IdFoodRecipe,
                    IdItem = ingredient.IdItem,
                    IngredientName = ingredient.IngredientName,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientPhoto = ingredient.IngredientPhoto,
                    IdIngredient = ingredient.IdIngredient
                });
            }

            foreach (IngredientViewModel ingredientViewModel in ingredientViewModels)
            {
                IngredientListBox.Items.Add(ingredientViewModel);
            }

            IngredientListBox.Items.Refresh();
        }

        public void LoadFoodRecipe(FoodRecipeContract foodRecipe)
        {
            FoodRecipeStepsListBox.Items.Clear();
            recipeStepList.Clear();

            List<string> stepList = foodRecipe.Description.Split(';').ToList();

            FoodRecipeNameTextBox.Text = foodRecipe.Name;
            FoodRecipePriceTextBox.Text = foodRecipe.Price + "";
            FoodRecipePortionsComboBox.Text = foodRecipe.NumberOfServings + "";
            

            for (int i = 0; i < stepList.Count; i++)
            {
                if (i != (stepList.Count - 1))
                {
                    FoodRecipeStepsListBox.Items.Add(stepList[i] + ";");
                    recipeStepList.Add(stepList[i] + ";");
                }
            }
        }

        public void ConfirmDeleteFoodRecipe(int result)
        {
            if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
                allFoodRecipes.Clear();
                FoodRecipeListBox.Items.Clear();
                RequestFoodRecipeList();
            }
        }

        public void ConfirmUpdateFoodRecipe(int result)
        {
            if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");
                allFoodRecipes.Clear();
                FoodRecipeListBox.Items.Clear();
                RequestFoodRecipeList();
            }
            else if (result == -1)
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Nombre de receta ya registrado");
                ShowOrderDialogs(true);
                SaveFoodRecipeButton.Visibility = Visibility.Visible;
            }
            else
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Modificación no realizada");
                ShowOrderDialogs(true);
                SaveFoodRecipeButton.Visibility = Visibility.Visible;
            }
        }

        public void LoadFoodRecipeToPrepare(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients)
        {
            List<FoodRecipeViewModel> foodRecipesToPrepare = new List<FoodRecipeViewModel>();
            FoodRecipeToPrepareListBox.Items.Clear();

            foreach (FoodRecipeContract foodRecipe in foodRecipes)
            {
                string recipeIngredients = "";

                for (int i = 0; i < ingredients.Count; i++)
                {
                    if (ingredients[i].IdFoodRecipe == foodRecipe.IdFoodRecipe)
                    {
                        if (i == (ingredients.Count - 1))
                        {
                            recipeIngredients += ingredients[i].IngredientQuantity + " " + ingredients[i].IngredientName;
                        }
                        else
                        {
                            recipeIngredients += ingredients[i].IngredientQuantity + " " + ingredients[i].IngredientName + ", ";
                        }
                    }
                }

                string[] value = foodRecipe.CustomerName.Split('/');
                foodRecipesToPrepare.Add(new FoodRecipeViewModel
                {
                    IdFoodRecipe = foodRecipe.IdFoodRecipe,
                    Name = foodRecipe.Name,
                    Portions = foodRecipe.NumberOfServings + "",
                    Price = foodRecipe.Price + "",
                    Description = foodRecipe.Description,
                    Ingredients = recipeIngredients,
                    CustomerName = foodRecipe.CustomerName,
                    IdOrder = int.Parse(value[0])
                });
            }

            foreach (FoodRecipeViewModel foodRecipe in foodRecipesToPrepare)
            {
                FoodRecipeToPrepareListBox.Items.Add(foodRecipe);
            }
        }

        public void ConfirmFoodRecipeMade(int result, bool foodRecipeMade, string information)
        {
            if (result > 0)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, "Proceso Realizado");

                //FoodRecipeToPrepareListBox.Items.Clear();
                //FoodRecipeToPrepareListBox.ItemsSource = null;
                //RequestGetFoodRecipesToPrepare();
                FoodRecipeToPrepareListBox.Items.Refresh();
            }

            if (foodRecipeMade)
            {
                NotificationType notificationType = NotificationType.Success;
                PersonalizeToast(notificationType, information + ", ha sido realizado. Estatus: Realizado");
            }
        }

        #endregion

        #region GUI Methods

        public void MarkFoodRecipeAsDone(object sender, RoutedEventArgs e)
        {
            FoodRecipeViewModel foodRecipeViewModel = (FoodRecipeViewModel)FoodRecipeToPrepareListBox.SelectedItem;
            
            if (foodRecipeViewModel != null)
            {
                RequestMarkRecipeAsDone(foodRecipeViewModel.IdOrder, foodRecipeViewModel.IdFoodRecipe);

                FoodRecipeToPrepareListBox.Items.Remove(foodRecipeViewModel);

                if (FoodRecipeToPrepareListBox.Items.Count == 0)
                {
                    SearchResultMessageTextBlock.Text = "No hay Platillos que preparar";
                    FoodRecipeTableGrid.Visibility = Visibility.Hidden;
                    FoodRecipeToPrepareListBox.Visibility = Visibility.Hidden;
                    InitialMessageBorder.Visibility = Visibility.Visible;
                }                
            }
        }

        public ESearchFilters GetFitlerSelected()
        {
            ESearchFilters filterSelected = new ESearchFilters();

            if (CookFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.CookFilter;
            }
            else if (NameFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.NameFilter;
            }
            else if (IngredientFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.IngredientFilter;
            }
            else if (PortionsFilterRadioButton.IsChecked == true)
            {
                filterSelected = ESearchFilters.IngredientFilter;
            } else
            {
                filterSelected = ESearchFilters.NoSelection;
            }

            return filterSelected;
        }

        public void RefreshRecipeList(object sender, RoutedEventArgs e)
        {
            FoodRecipeListBox.Items.Clear();
            allFoodRecipes.Clear();
            FoodRecipeListBox.Items.Clear();
            RequestFoodRecipeList();
        }

        private bool isOrderedAscending = true;
        public void SortRecipesAscending(object sender, RoutedEventArgs e)
        {
            List<FoodRecipeViewModel> foodRecipes = new List<FoodRecipeViewModel>();
            foreach (FoodRecipeViewModel foodRecipe in FoodRecipeListBox.Items)
            {
                foodRecipes.Add(foodRecipe);
            }

            List<FoodRecipeViewModel> auxiliaryOrderByDescending = foodRecipes.OrderByDescending(foodRecipe => foodRecipe.Name).ToList();
            List<FoodRecipeViewModel> auxiliaryOrderByAscending = foodRecipes.OrderBy(foodRecipe => foodRecipe.Name).ToList();
            FoodRecipeListBox.Items.Clear();

            if (allFoodRecipes != null)
            {
                if (isOrderedAscending)
                {
                    isOrderedAscending = false;

                    foreach (FoodRecipeViewModel foodRecipe in auxiliaryOrderByDescending)
                    {
                        FoodRecipeListBox.Items.Add(foodRecipe);
                    }
                }
                else
                {
                    isOrderedAscending = true;

                    foreach (FoodRecipeViewModel foodRecipe in auxiliaryOrderByAscending)
                    {
                        FoodRecipeListBox.Items.Add(foodRecipe);
                    }
                }
            }
        }

        public void ShowOrderDialogs(bool isEnabled)
        {
            ThirdLayerInformationBorder.Visibility = Visibility.Visible;
            QuarterLayerInformationBorder.Visibility = Visibility.Visible;
            FoodRecipeGrid.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Visibility = Visibility.Visible;
            SaveFoodRecipeButton.Visibility = Visibility.Collapsed;
            FoodRecipeNameTextBox.IsEnabled = isEnabled;
            PortionsAndPriceGrid.IsEnabled = isEnabled;
            FoodRecipeDescriptionTextBox.IsEnabled = isEnabled;
            IngredientsComboBox.IsEnabled = isEnabled;
            IngredientQuantityTextBox.IsEnabled = isEnabled;
            FoodRecipePortionsComboBox.IsEnabled = isEnabled;

            RequestIngredientList();
        }

        public void AccommodateListOfIngredients()
        {
            IngredientHeaderTextBlock.VerticalAlignment = VerticalAlignment.Center;
            IngredientHeaderTextBlock.Margin = new Thickness(0, 15, 0, 0);
            SixthLayerBorder.VerticalAlignment = VerticalAlignment.Bottom;
            SixthLayerBorder.Margin = new Thickness(0, 0, 0, 50);
            SixthLayerBorder.Height = 200;
            IngredientListBox.Margin = new Thickness(0, 0, 0, 50);
            IngredientListBox.Height = 200;
            IngredientListBox.VerticalAlignment = VerticalAlignment.Bottom;

            HintAssist.SetHelperText(FoodRecipeNameTextBox, "Nombre de la receta de platillo");
            HintAssist.SetHelperText(FoodRecipePortionsComboBox, "Número de porciones");
            HintAssist.SetHelperText(FoodRecipeDescriptionTextBox, "Decripción de un paso de la Receta de platillo");
            HintAssist.SetHelperText(IngredientsComboBox, "Nombre del ingrediente");
            HintAssist.SetHelperText(IngredientQuantityTextBox, "Cantidad necesaria del ingrediente");
            HintAssist.SetHelperText(FoodRecipePriceTextBox, "Precio asociado a la receta");
        }

        public void ShowSpecificFoodRecipeInformation()
        {            
            ShowOrderDialogs(false);
            FoodRecipeHeaderTextBlock.Text = "Receta de Platillo";
            FieldsEnabledMessageTextBlock.Text = "Información detallada";
            AcceptFoodRecipeRegistrationButtton.Visibility = Visibility.Collapsed;
            UpdateFoodRecipeButton.Visibility = Visibility.Visible;
            DeleteFoodRecipeButton.Visibility = Visibility.Visible;
            AddStepButton.Visibility = Visibility.Collapsed;
            DeleteStepButton.Visibility = Visibility.Collapsed;
            IngredientesStackPanel.Visibility = Visibility.Collapsed;
            FoodRecipeDescriptionStackPanel.Visibility = Visibility.Collapsed;
            FoodRecipeStepsBorder.Height = 310;
            FoodRecipeStepsListBox.Height = 310;  

            HintAssist.SetHelperText(FoodRecipeNameTextBox, string.Empty);
            HintAssist.SetHelperText(FoodRecipePortionsComboBox, string.Empty);
            HintAssist.SetHelperText(FoodRecipeDescriptionTextBox, string.Empty);
            HintAssist.SetHelperText(IngredientsComboBox, string.Empty);
            HintAssist.SetHelperText(IngredientQuantityTextBox, string.Empty);
            HintAssist.SetHelperText(FoodRecipePriceTextBox, string.Empty);

            IngredientHeaderTextBlock.VerticalAlignment = VerticalAlignment.Top;
            IngredientHeaderTextBlock.Margin = new Thickness(0);
            SixthLayerBorder.VerticalAlignment = VerticalAlignment.Top;
            SixthLayerBorder.Margin = new Thickness(0, 25, 0, 0);
            SixthLayerBorder.Height = 470;
            IngredientListBox.Margin = new Thickness(0, 25, 0, 70);
            IngredientListBox.Height = 470;
        }

        public void ShowFoodRecipeRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            EmptyUIElements();
            ShowOrderDialogs(true);
            FoodRecipeNameTextBox.Text = "";
            FoodRecipePortionsComboBox.Text = "";
            FoodRecipePriceTextBox.Text = "";
            FoodRecipeHeaderTextBlock.Text = "Registro de Receta de Platillo";
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";
            AcceptFoodRecipeRegistrationButtton.Visibility = Visibility.Visible;
            UpdateFoodRecipeButton.Visibility = Visibility.Collapsed;            
            DeleteFoodRecipeButton.Visibility = Visibility.Collapsed;
            IngredientsComboBox.SelectedIndex = -1;
            AddStepButton.Visibility = Visibility.Visible;
            DeleteStepButton.Visibility = Visibility.Visible;     
            IngredientesStackPanel.Visibility = Visibility.Visible;
            FoodRecipeDescriptionStackPanel.Visibility = Visibility.Visible;
            DeleteStepButton.IsEnabled = false;
            DeleteIngredientButton.IsEnabled = false;
            FoodRecipeStepsBorder.Height = 190;
            FoodRecipeStepsListBox.Height = 190;
                       
            AccommodateListOfIngredients();
        }

        public void ShowFoodRecipeUpdateDialog(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(true);
            IngredientsComboBox.Items.Clear();
            FoodRecipeHeaderTextBlock.Text = "Actualización de Receta de Platillo";
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";
            AcceptFoodRecipeRegistrationButtton.Visibility = Visibility.Collapsed;
            UpdateFoodRecipeButton.Visibility = Visibility.Collapsed;
            DeleteFoodRecipeButton.Visibility = Visibility.Collapsed;
            SaveFoodRecipeButton.Visibility = Visibility.Visible;
            AddStepButton.Visibility = Visibility.Visible;
            DeleteStepButton.Visibility = Visibility.Visible;
            DeleteStepButton.IsEnabled = true;
            DeleteIngredientButton.IsEnabled = true;
            IngredientesStackPanel.Visibility = Visibility.Visible;
            FoodRecipeDescriptionStackPanel.Visibility = Visibility.Visible;
            FoodRecipeStepsBorder.Height = 190;
            FoodRecipeStepsListBox.Height = 190;

            AccommodateListOfIngredients();
        }

        public void ShowSearchResults(object sender, TextChangedEventArgs e)
        {
            string search = SearchTextBox.Text;
            int count = 0;
            ESearchFilters searchFilters = GetFitlerSelected();
            if ((allFoodRecipes.Count() > 0) && (searchFilters != ESearchFilters.NoSelection))
            {
                if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
                {
                    InitialMessageBorder.Visibility = Visibility.Hidden;
                    FoodRecipeTableGrid.Visibility = Visibility.Visible;

                    if (NameFilterRadioButton.IsChecked == true)
                    {
                        count = 0;
                        FoodRecipeListBox.Items.Clear();
                        SearchFilterTextBlock.Text = "Consulta: " + "Nombre" + "/" + search;

                        foreach (FoodRecipeViewModel foodRecipe in allFoodRecipes)
                        {
                            string foodRecipeNmae = foodRecipe.Name;
                            if (foodRecipeNmae.ToLower().StartsWith(SearchTextBox.Text.Trim().ToLower()))
                            {
                                FoodRecipeListBox.Items.Add(foodRecipe);
                                count++;
                            }
                        }
                    }

                    if (PortionsFilterRadioButton.IsChecked == true)
                    {
                        count = 0;
                        FoodRecipeListBox.Items.Clear();
                        SearchFilterTextBlock.Text = "Consulta: " + "N° Porciones" + "/" + search;

                        foreach (FoodRecipeViewModel foodRecipe in allFoodRecipes)
                        {
                            string numberOfServings = foodRecipe.Portions;
                            if (numberOfServings.ToLower().StartsWith(SearchTextBox.Text.Trim().ToLower()))
                            {
                                FoodRecipeListBox.Items.Add(foodRecipe);
                                count++;
                            }
                        }
                    }

                    if (IngredientFilterRadioButton.IsChecked == true)
                    {
                        count = 0;
                        FoodRecipeListBox.Items.Clear();
                        SearchFilterTextBlock.Text = "Consulta: " + "Ingrediente" + "/" + search;

                        foreach (FoodRecipeViewModel foodRecipe in allFoodRecipes)
                        {
                            string ingredients = foodRecipe.Ingredients.ToLower();
                            bool isIngredient = ingredients.Contains(SearchTextBox.Text.Trim().ToLower().Trim());
                            if (isIngredient)
                            {
                                FoodRecipeListBox.Items.Add(foodRecipe);
                                count++;
                            }
                        }
                    }
                }

                if (count == 0 && !string.IsNullOrWhiteSpace(SearchTextBox.Text))
                {
                    SearchResultMessageTextBlock.Text = "Sin resultados de búsqueda";
                    InitialMessageBorder.Visibility = Visibility.Visible;
                    FoodRecipeTableGrid.Visibility = Visibility.Hidden;
                    FoodRecipeListBox.Items.Clear();
                }
            }
            else if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
                InitialMessageBorder.Visibility = Visibility.Visible;
                FoodRecipeTableGrid.Visibility = Visibility.Hidden;
                SearchFilterTextBlock.Text = "Consulta: " + "Filtro" + "/" + "Búsqueda";
                FoodRecipeListBox.Items.Clear();
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
            if (CookFilterRadioButton.IsChecked == true)
            {
                SearchTextBox.Text = "";
                SearchTextBox.IsEnabled = false;
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Por Preparar");
                HintAssist.SetHelperText(SearchTextBox, "Campo de búsqueda desactivado. Lista de recetas por preparar");
                SearchFilterTextBlock.Text = "Consulta: " + "Recetas de platillos por preparar";
                SearchTextBox.Text = "";
                                
                if (!(FoodRecipeToPrepareListBox.Items.Count > 0))
                {
                    SearchResultMessageTextBlock.Text = "No hay Platillos que preparar";
                    FoodRecipeTableGrid.Visibility = Visibility.Hidden;
                    FoodRecipeToPrepareListBox.Visibility = Visibility.Hidden;
                    InitialMessageBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    FoodRecipeTableGrid.Visibility = Visibility.Visible;
                    FoodRecipeToPrepareListBox.Visibility = Visibility.Visible;
                    FoodRecipeListBox.Visibility = Visibility.Collapsed;
                    InitialMessageBorder.Visibility = Visibility.Hidden;
                }
            }
            else if (NameFilterRadioButton.IsChecked == true)
            {
                SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Nombre");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el nombre de alguna receta de platillo");
                SearchFilterTextBlock.Text = "Consulta: " + "Nombre" + "/" + "Búsqueda";
                SearchTextBox.Text = "";
                SearchTextBox.IsEnabled = true;
                FoodRecipeListBox.Visibility = Visibility.Visible;
                FoodRecipeToPrepareListBox.Visibility = Visibility.Collapsed;
                FoodRecipeTableGrid.Visibility = Visibility.Hidden;
                InitialMessageBorder.Visibility = Visibility.Visible;
            }
            else if (IngredientFilterRadioButton.IsChecked == true)
            {
                SearchResultMessageTextBlock.Text = "No hay Platillos que preparar";
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Ingrediente");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa algún el nombre de algún ingrediente");
                SearchFilterTextBlock.Text = "Consulta: " + "Ingrediente" + "/" + "Búsqueda";
                SearchTextBox.Text = "";
                SearchTextBox.IsEnabled = true;
                FoodRecipeListBox.Visibility = Visibility.Visible;
                FoodRecipeToPrepareListBox.Visibility = Visibility.Collapsed;
                FoodRecipeTableGrid.Visibility = Visibility.Hidden;
                InitialMessageBorder.Visibility = Visibility.Visible;
            }
            else if (PortionsFilterRadioButton.IsChecked == true)
            {
                SearchResultMessageTextBlock.Text = "No hay Platillos que preparar";
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: N° Porciones");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el número de porciones del platillo");
                SearchFilterTextBlock.Text = "Consulta: " + "N° Porciones" + "/" + "Búsqueda";
                SearchTextBox.Text = "";
                SearchTextBox.IsEnabled = true;
                FoodRecipeListBox.Visibility = Visibility.Visible;
                FoodRecipeToPrepareListBox.Visibility = Visibility.Collapsed;
                FoodRecipeTableGrid.Visibility = Visibility.Hidden;
                InitialMessageBorder.Visibility = Visibility.Visible;
            }
        }

        public void ResetSearchFilters(object sender, RoutedEventArgs e)
        {
            CookFilterRadioButton.IsChecked = false;
            NameFilterRadioButton.IsChecked = false;
            IngredientFilterRadioButton.IsChecked = false;
            PortionsFilterRadioButton.IsChecked = false;
            SearchTextBox.Text = "";
            FoodRecipeToPrepareListBox.Visibility = Visibility.Collapsed;
            FoodRecipeTableGrid.Visibility = Visibility.Hidden;
            InitialMessageBorder.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
            HintAssist.SetHint(SearchTextBox, "Buscar");
            HintAssist.SetHelperText(SearchTextBox, "Selecciona un filtro de búsqueda");
            SearchFilterTextBlock.Text = "Consulta: Filtro/Búsqueda";
        }

        public void HideSpecificFoodRecipeInformation(object sender, RoutedEventArgs e)
        {
            bool isRegister = FoodRecipeHeaderTextBlock.Text.Equals("Registro de Receta de Platillo");
            
            if ((isRegister && !InvalidFieldsGrid.IsVisible) ||
                (SaveFoodRecipeButton.IsVisible && !FifthLayerBorder.IsVisible) ||
                (DeleteConfirmationGrid.IsVisible && !FifthLayerBorder.IsVisible))
            {
                RemoveValidationAssistant(true);
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
                EmptyUIElements();
            }
            else
            {
                if (DeleteConfirmationGrid.IsVisible)
                {
                    FoodRecipeViewModel foodRecipeViewModel = (FoodRecipeViewModel) FoodRecipeListBox.SelectedItem;
                    if (foodRecipeViewModel != null)
                    {
                        RequestDeleteFoodRecipe(foodRecipeViewModel.IdFoodRecipe);
                        //ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                        //QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                        //FoodRecipeGrid.Visibility = Visibility.Hidden;
                        //BackToFoodRecipeRegistration(sender, e);
                    }
                }

                recoveredIngredients.Clear();
                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                FoodRecipeGrid.Visibility = Visibility.Hidden;
                BackToFoodRecipeRegistration(sender, e);
            }
            EmptyUIElements();
        }

        public void ReceiveFoodRecipeModification(object sender, RoutedEventArgs e)
        {
            bool isUpdate = FoodRecipeHeaderTextBlock.Text.Equals("Actualización de Receta de Platillo");

            if (isUpdate)
            {
                RequestUpdateFoodRecipe();
                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                FoodRecipeGrid.Visibility = Visibility.Hidden;
                BackToFoodRecipeRegistration(sender, e);
            }
        }

        public void BackToFoodRecipeRegistration(object sender, RoutedEventArgs e)
        {
            RemoveValidationAssistant(false);
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;
        }

        public void ShowDeleteConfirmationDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        public void AddStepToRecipe(object sender, RoutedEventArgs e)
        {
            int stepsNumber = FoodRecipeStepsListBox.Items.Count + 1;          

            string step = FoodRecipeDescriptionTextBox.Text;
            FoodRecipeStepsListBox.Items.Add(stepsNumber + ".\n" + step + ";");
            recipeStepList.Add(stepsNumber + ".\n" + step + ";");
            FoodRecipeDescriptionTextBox.Text = "";
            DeleteStepButton.IsEnabled = true;
        }

        public void DeleteStepToRecipe(object sender, RoutedEventArgs e)
        {
            if (FoodRecipeStepsListBox.SelectedItem != null)
            {
                FoodRecipeStepsListBox.Items.RemoveAt
                (
                    FoodRecipeStepsListBox.Items.IndexOf(FoodRecipeStepsListBox.SelectedItem)
                );

                recipeStepList.Clear();

                foreach (var step in FoodRecipeStepsListBox.Items)
                {
                    string[] value = step.ToString().Split('\n');
                    recipeStepList.Add(value[1]);
                }

                FoodRecipeStepsListBox.Items.Clear();

                for (int i = 0; i < recipeStepList.Count; i++)
                {
                    FoodRecipeStepsListBox.Items.Add((i + 1) + ".\n" + recipeStepList[i]);
                }

                List<string> auxiliary = new List<string>();

                foreach (string value in recipeStepList)
                {
                    auxiliary.Add(value);
                }

                recipeStepList.Clear();

                for (int i = 0; i < auxiliary.Count; i++)
                {
                    recipeStepList.Add(i + 1 + ".\n" + auxiliary[i]);
                }

                DeleteStepButton.IsEnabled = FoodRecipeStepsListBox.Items.Count >= 1;
            } 
            else
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Selecciona un elemento de la lista. Proceso no realizado");
            }
        }

        public void AddIngredientToRecipe(object sender, RoutedEventArgs e)
        {
            ItemContract selectedIngredient = (ItemContract) IngredientsComboBox.SelectedItem;
            IngredientViewModel ingredient = new IngredientViewModel();
            ingredient.IngredientName = selectedIngredient.Name;
            ingredient.IdItem = selectedIngredient.IdItem;
            ingredient.IngredientQuantity = IngredientQuantityTextBox.Text;
            ingredient.IngredientPhoto = selectedIngredient.Photo;

            bool isIngredient = false;

            foreach (IngredientViewModel value in foodRecipeIngredients)
            {
                if (string.Equals(value.IngredientName, ingredient.IngredientName))
                {
                    isIngredient = true;
                }
            }

            if (recoveredIngredients.Count > 0)
            {
                foreach (IngredientContract value in recoveredIngredients)
                {
                    if (string.Equals(value.IngredientName, ingredient.IngredientName))
                    {
                        isIngredient = true;
                    }
                }
            }

            if (!isIngredient)
            {
                IngredientListBox.Items.Add(ingredient);
                foodRecipeIngredients.Add(ingredient);
            }
            IngredientListBox.Items.Refresh();
            IngredientsComboBox.SelectedIndex = -1;
            IngredientQuantityTextBox.Text = "";
            DeleteIngredientButton.IsEnabled = true;
        }

        public void DeleteIngredientToRecipe(object sender, RoutedEventArgs e)
        {
            if (IngredientListBox.SelectedItem != null)
            {
                IngredientViewModel ingredientViewModel = (IngredientViewModel)IngredientListBox.SelectedItem;
                foodRecipeIngredients.Remove((IngredientViewModel)IngredientListBox.SelectedItem);
                IngredientListBox.Items.RemoveAt
                (
                    IngredientListBox.Items.IndexOf(IngredientListBox.SelectedItem)
                );

                if (IngredientListBox.Items.Count <= 0)
                {
                    DeleteIngredientButton.IsEnabled = false;
                }

                if (recoveredIngredients.Count > 0)
                {
                    IngredientContract ingredientToRemove = new IngredientContract();
                    foreach (IngredientContract ingredient in recoveredIngredients)
                    {
                        if (string.Equals(ingredient.IngredientName, ingredientViewModel.IngredientName))
                        {
                            ingredientToRemove = ingredient;
                        }
                    }
                    recoveredIngredients.Remove(ingredientToRemove);
                }
            }
            else
            {
                NotificationType notificationType = NotificationType.Warning;
                PersonalizeToast(notificationType, "Selecciona un elemento de la lista. Proceso no realizado");
            }
        }

        public void ShowConfirmationToast(object sender, RoutedEventArgs e)
        {
            RequestRegisterFoodRecipe();
            ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
            QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
            FoodRecipeGrid.Visibility = Visibility.Hidden;
            BackToFoodRecipeRegistration(sender, e);
        }

        public void PersonalizeToast(NotificationType notificationType, string message)
        {
            //notificationManager.Show(
            //    new NotificationContent
            //    {
            //        Title = "Confirmación",
            //        Message = message,
            //        Type = notificationType,
            //    }, areaName: "ConfirmationToast", expirationTime: TimeSpan.FromSeconds(2)
            //);

            NotificationContent notificationContent = new NotificationContent
            {
                Title = "Confirmación",
                Message = message,
                Type = notificationType,
            };

            notificationManager.Show(notificationContent);
        }

        public void EmptyUIElements()
        {
            foodRecipeIngredients.Clear();
            IngredientListBox.Items.Clear();
            IngredientsComboBox.Items.Clear();
            FoodRecipeStepsListBox.Items.Clear();
            recipeStepList.Clear();
        }

        public void RemoveValidationAssistant(bool isNotVisible)
        {
            ValidationAssist.SetSuppress(FoodRecipeNameTextBox, isNotVisible);
            ValidationAssist.SetSuppress(FoodRecipePortionsComboBox, isNotVisible);
            ValidationAssist.SetSuppress(FoodRecipePriceTextBox, isNotVisible);
            ValidationAssist.SetSuppress(FoodRecipeDescriptionTextBox, isNotVisible);
            ValidationAssist.SetSuppress(IngredientsComboBox, isNotVisible);
            ValidationAssist.SetSuppress(IngredientQuantityTextBox, isNotVisible);
        }

        #endregion
    }
}