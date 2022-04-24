using MaterialDesignThemes.Wpf;
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
using System.Windows.Threading;

namespace ItalianPizza.Views
{
    /// <summary>
    /// Lógica de interacción para CheckFoodRecipesPage.xaml
    /// </summary>
    public partial class CheckFoodRecipesPage : Page
    {
        private string usernameLoggedIn;

        public CheckFoodRecipesPage(string usernameLoggedIn)
        {
            InitializeComponent();
            this.usernameLoggedIn = usernameLoggedIn;
        }
                                                        
        #region GUI Methods

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
            IngredientesComboBox.IsEnabled = isEnabled;
            IngredientQuantityTextBox.IsEnabled = isEnabled;   
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
            HintAssist.SetHelperText(FoodRecipePortionsTextBox, "Número de porciones");
            HintAssist.SetHelperText(FoodRecipeDescriptionTextBox, "Decripción de un paso de la Receta de platillo");
            HintAssist.SetHelperText(IngredientesComboBox, "Nombre del ingrediente");
            HintAssist.SetHelperText(IngredientQuantityTextBox, "Cantidad necesaria del ingrediente");
            HintAssist.SetHelperText(FoodRecipePriceTextBox, "Precio asociado a la receta");
        }

        public void ShowSpecificFoodRecipeInformation(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(false);
            FoodRecipeHeaderTextBlock.Text = "Receta de Platillo";
            FieldsEnabledMessageTextBlock.Text = "Información detallada";
            AcceptFoodRecipeRegistrationButtton.Visibility = Visibility.Collapsed;
            UpdateFoodRecipeButton.Visibility = Visibility.Visible;
            DeleteFoodRecipeButton.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Hidden;
            MinusButton.Visibility = Visibility.Hidden;
            AddStepButton.Visibility = Visibility.Collapsed;
            DeleteStepButton.Visibility = Visibility.Collapsed;
            IngredientesStackPanel.Visibility = Visibility.Collapsed;
            FoodRecipeDescriptionStackPanel.Visibility = Visibility.Collapsed;
            FoodRecipeStepsBorder.Height = 310;
            FoodRecipeStepsListBox.Height = 310;

            HintAssist.SetHelperText(FoodRecipeNameTextBox, string.Empty);
            HintAssist.SetHelperText(FoodRecipePortionsTextBox, string.Empty);
            HintAssist.SetHelperText(FoodRecipeDescriptionTextBox, string.Empty);
            HintAssist.SetHelperText(IngredientesComboBox, string.Empty);
            HintAssist.SetHelperText(IngredientQuantityTextBox, string.Empty);
            HintAssist.SetHelperText(FoodRecipePriceTextBox, string.Empty);

            IngredientHeaderTextBlock.VerticalAlignment = VerticalAlignment.Top;
            IngredientHeaderTextBlock.Margin = new Thickness(0);
            SixthLayerBorder.VerticalAlignment = VerticalAlignment.Top;
            SixthLayerBorder.Margin = new Thickness(0, 25, 0, 0);
            SixthLayerBorder.Height = 470;
            IngredientListBox.Margin = new Thickness(0, 25, 0, 0);
            IngredientListBox.Height = 470;
            // IngredientListBox.VerticalAlignment = VerticalAlignment.Bottom;
        }

        public void ShowFoodRecipeRegistrarionDialogue(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(true);
            FoodRecipeHeaderTextBlock.Text = "Registro de Receta de Platillo";
            FieldsEnabledMessageTextBlock.Text = "Formulario de registro";
            AcceptFoodRecipeRegistrationButtton.Visibility = Visibility.Visible;
            UpdateFoodRecipeButton.Visibility = Visibility.Collapsed;            
            DeleteFoodRecipeButton.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Visible;
            MinusButton.Visibility = Visibility.Visible;
            IngredientesComboBox.SelectedIndex = -1;
            AddStepButton.Visibility = Visibility.Visible;
            DeleteStepButton.Visibility = Visibility.Visible;     
            IngredientesStackPanel.Visibility = Visibility.Visible;
            FoodRecipePortionsTextBox.Text = "1";
            FoodRecipeDescriptionStackPanel.Visibility = Visibility.Visible;
            FoodRecipeStepsBorder.Height = 190;
            FoodRecipeStepsListBox.Height = 190;
                       
            AccommodateListOfIngredients();
        }

        public void IncreaseNumberOfPortions(object sender, RoutedEventArgs e)
        {       
            int ingredientQuantity = Int32.Parse(FoodRecipePortionsTextBox.Text);

            if (ingredientQuantity < 20)
            {
                ingredientQuantity++;
                FoodRecipePortionsTextBox.Text = ingredientQuantity + "";
            }
            else
            {
                HintAssist.SetHelperText(FoodRecipePortionsTextBox, "Rango superior alcanzado");
            }
        }

        public void DecreaseNumberOfPortions(object sender, RoutedEventArgs e)
        {
            int ingredientQuantity = Int32.Parse(FoodRecipePortionsTextBox.Text);

            if (ingredientQuantity > 1)
            {
                ingredientQuantity--;
                FoodRecipePortionsTextBox.Text = ingredientQuantity + "";
            } else
            {
                HintAssist.SetHelperText(FoodRecipePortionsTextBox, "Rango inferior alcanzado");
            }
        }

        public void ShowFoodRecipeUpdateDialog(object sender, RoutedEventArgs e)
        {
            ShowOrderDialogs(true);
            FoodRecipeHeaderTextBlock.Text = "Actualización de Receta de Platillo";
            FieldsEnabledMessageTextBlock.Text = "Formulario de actualización";
            AcceptFoodRecipeRegistrationButtton.Visibility = Visibility.Collapsed;
            UpdateFoodRecipeButton.Visibility = Visibility.Collapsed;
            DeleteFoodRecipeButton.Visibility = Visibility.Collapsed;
            SaveFoodRecipeButton.Visibility = Visibility.Visible;
            AddStepButton.Visibility = Visibility.Visible;
            DeleteStepButton.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Visible;
            MinusButton.Visibility = Visibility.Visible;
            IngredientesStackPanel.Visibility = Visibility.Visible;
            FoodRecipeDescriptionStackPanel.Visibility = Visibility.Visible;
            FoodRecipeStepsBorder.Height = 190;
            FoodRecipeStepsListBox.Height = 190;

            AccommodateListOfIngredients();
        }

        public void ShowSearchResults(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InitialMessageBorder.Visibility = Visibility.Hidden;
                FoodRecipeTableGrid.Visibility = Visibility.Visible;
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
            }
            else if (NameFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Nombre");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el nombre de alguna receta de platillo");
            }
            else if (IngredientFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: Ingrediente");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa algún el nombre de algún ingrediente");
            } else if (PortionsFilterRadioButton.IsChecked == true)
            {
                HintAssist.SetHint(SearchTextBox, "Filtro seleccionado: N° Porciones");
                HintAssist.SetHelperText(SearchTextBox, "Ingresa el número de porciones del platillo");
            }
        }

        public void ResetSearchFilters(object sender, RoutedEventArgs e)
        {
            CookFilterRadioButton.IsChecked = false;
            NameFilterRadioButton.IsChecked = false;
            IngredientFilterRadioButton.IsChecked = false;
            PortionsFilterRadioButton.IsChecked = false;
            SearchTextBox.Text = "";
            FoodRecipeTableGrid.Visibility = Visibility.Hidden;
            InitialMessageBorder.Visibility = Visibility.Visible;
            SearchResultMessageTextBlock.Text = "Realiza una búsqueda";
            HintAssist.SetHint(SearchTextBox, "Buscar");
            HintAssist.SetHelperText(SearchTextBox, "Selecciona un filtro de búsqueda");

        }

        public void HideSpecificFoodRecipeInformation(object sender, RoutedEventArgs e)
        {
            bool isRegister = FoodRecipeHeaderTextBlock.Text.Equals("Registro de Receta de Platillo");

            if ((isRegister && !InvalidFieldsGrid.IsVisible) || (SaveFoodRecipeButton.IsVisible && !FifthLayerBorder.IsVisible) || (DeleteConfirmationGrid.IsVisible && !FifthLayerBorder.IsVisible))
            {
                FifthLayerBorder.Visibility = Visibility.Visible;
                InvalidFieldsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ThirdLayerInformationBorder.Visibility = Visibility.Hidden;
                QuarterLayerInformationBorder.Visibility = Visibility.Hidden;
                FoodRecipeGrid.Visibility = Visibility.Hidden;
                BackToFoodRecipeRegistration(sender, e);
            }
        }

        public void BackToFoodRecipeRegistration(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Hidden;
            InvalidFieldsGrid.Visibility = Visibility.Hidden;
            DeleteConfirmationGrid.Visibility = Visibility.Hidden;
        }

        public void ShowDeleteConfirmationDialog(object sender, RoutedEventArgs e)
        {
            FifthLayerBorder.Visibility = Visibility.Visible;
            DeleteConfirmationGrid.Visibility = Visibility.Visible;
        }

        #endregion
    }
}
