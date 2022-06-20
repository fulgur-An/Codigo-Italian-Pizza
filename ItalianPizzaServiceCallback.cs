using Backend.Contracts;
using Backend.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianPizza
{
    public class ItalianPizzaServiceCallback : IItalianPizzaServiceCallback
    {
        public delegate void GetFoodRecipeListDelegate(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients);
        public event GetFoodRecipeListDelegate GetFoodRecipeListEvent;

        public delegate void GetFoodRecipeListSortedByNameDelegate(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients);
        public event GetFoodRecipeListSortedByNameDelegate GetFoodRecipeListSortedByNameEvent;

        public delegate void GetItemIngredientListDelegate(List<ItemContract> items);
        public event GetItemIngredientListDelegate GetItemIngredientListEvent;

        public delegate void RegisterFoodRecipeDelegate(int result);
        public event RegisterFoodRecipeDelegate RegisterFoodRecipeEvent;

        public delegate void GetFoodRecipeByIdDelegate(FoodRecipeContract foodRecipe);
        public event GetFoodRecipeByIdDelegate GetFoodRecipeByIdEvent;

        public delegate void GetIngredientsByIdDelegate(List<IngredientContract> ingredientContracts);
        public event GetIngredientsByIdDelegate GetIngredientsByIdEvent;

        public delegate void DeleteFoodRecipeByIdDelegate(int result);
        public event DeleteFoodRecipeByIdDelegate DeleteFoodRecipeByIdEvent;

        public delegate void UpdateFoodRecipeDelegate(int result);
        public event UpdateFoodRecipeDelegate UpdateFoodRecipeEvent;

        public delegate void GetItemsSortedByNameDelegate(List<ItemContract> items);
        public event GetItemsSortedByNameDelegate GetItemsSortedByNameEvent;

        public delegate void RegisterOrderDelegate(int result, List<string> foodRecipeErrors, List<string> itemsErrors);
        public event RegisterOrderDelegate RegisterOrderEvent;

        public delegate void GetIdEmployeeByNameDelegate(int idEmployee);
        public event GetIdEmployeeByNameDelegate GetIdEmployeeByNameEvent;

        public delegate void GetRecipesAvailableDelegate(List<FoodRecipeContract> foodRecipeContracts);
        public event GetRecipesAvailableDelegate GetRecipesAvailableEvent;

        public delegate void GetOrderListDelegate(List<OrderContract> orderContracts, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts, List<QuantityItemContract> quantityItemContracts);
        public event GetOrderListDelegate GetOrderListEvent;

        public delegate void DeleteOrderByIdDelegate(int result);
        public event DeleteOrderByIdDelegate DeleteOrderByIdEvent;

        public delegate void GetFoodRecipeListToPrepareDelegate(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients);
        public event GetFoodRecipeListToPrepareDelegate GetFoodRecipeListToPrepareEvent;

        public delegate void UpdateOrderDelegate(int result);
        public event UpdateOrderDelegate UpdateOrderEvent;

        public delegate void MarkRecipeAsDoneDelegate(int result, bool foodRecipeMade, string information);
        public event MarkRecipeAsDoneDelegate MarkRecipeAsDoneEvent;



        public delegate void RegisterCustomerDelegate(int result);
        public event RegisterCustomerDelegate RegisterCustomerEvent;

        public delegate void DeleteCustomerByIdDelegate(int result);
        public event DeleteCustomerByIdDelegate DeleteCustomerByIdEvent;

        public delegate void GetCustomerListSortedByNameDelegate(List<CustomerContract> customerContracts, List<AddressContract> addressContracts);
        public event GetCustomerListSortedByNameDelegate GetCustomerListSortedByNameEvent;

        public delegate void UpdateCustomerDelegate(int result);
        public event UpdateCustomerDelegate UpdateCustomerEvent;

        public void GetFoodRecipeList(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients)
        {
            if (GetFoodRecipeListEvent  != null)
            {
                GetFoodRecipeListEvent(foodRecipes, ingredients);
            }
        }

        public void GetItemIngredientList(List<ItemContract> items)
        {
            if (GetItemIngredientListEvent != null)
            {
                GetItemIngredientListEvent(items);
            }
        }

        public void RegisterFoodRecipe(int result)
        {
            if (RegisterFoodRecipeEvent != null)
            {
                RegisterFoodRecipeEvent(result);
            }
        }

        public void GetFoodRecipeById(FoodRecipeContract foodRecipe)
        {
            if (GetFoodRecipeByIdEvent != null)
            {
                GetFoodRecipeByIdEvent(foodRecipe);
            }
        }

        public void GetFoodRecipeListSortedByName(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients)
        {
            if (GetFoodRecipeListSortedByNameEvent != null)
            {
                GetFoodRecipeListSortedByNameEvent(foodRecipes, ingredients);
            }
        }

        public void GetIngredientsById(List<IngredientContract> ingredientContracts)
        {
            if (GetIngredientsByIdEvent != null)
            {
                GetIngredientsByIdEvent(ingredientContracts);
            }
        }

        public void DeleteFoodRecipeById(int result)
        {
            if (DeleteFoodRecipeByIdEvent != null)
            {
                DeleteFoodRecipeByIdEvent(result);
            }
        }

        public void UpdateFoodRecipe(int result)
        {
            if (UpdateFoodRecipeEvent != null)
            {
                UpdateFoodRecipeEvent(result);
            }
        }

        public void GetItemsSortedByName(List<ItemContract> items)
        {
            if (GetItemsSortedByNameEvent != null)
            {
                GetItemsSortedByNameEvent(items);
            }
        }

        public void RegisterOrder(int result, List<string> foodRecipeErrors, List<string> itemErrors)
        {
            if (RegisterOrderEvent != null)
            {
                RegisterOrderEvent(result, foodRecipeErrors, itemErrors);
            }
        }

        public void GetIdEmployeeByName(int idEmployee)
        {
            if (GetIdEmployeeByNameEvent != null)
            {
                GetIdEmployeeByNameEvent(idEmployee);
            }
        }

        public void GetRecipesAvailable(List<FoodRecipeContract> foodRecipeContracts)
        {
            if (GetRecipesAvailableEvent != null)
            {
                GetRecipesAvailableEvent(foodRecipeContracts);
            }
        }

        public void GetOrderList(List<OrderContract> orderContracts, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts, List<QuantityItemContract> quantityItemContracts)
        {
            if (GetOrderListEvent != null)
            {
                GetOrderListEvent(orderContracts, quantityFoodRecipeContracts, quantityItemContracts);
            }
        }

        public void DeleteOrderById(int result)
        {
            if (DeleteOrderByIdEvent != null)
            {
                DeleteOrderByIdEvent(result);
            }
        }

        public void GetFoodRecipeListToPrepare(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients)
        {
            if (GetFoodRecipeListToPrepareEvent != null)
            {
                GetFoodRecipeListToPrepareEvent(foodRecipes, ingredients);
            }
        }

        public void UpdateOrder(int result)
        {
            if (UpdateOrderEvent != null)
            {
                UpdateOrderEvent(result);
            }
        }

        public void MarkRecipeAsDone(int result, bool foodRecipeMade, string information)
        {
            if (MarkRecipeAsDoneEvent != null)
            {
                MarkRecipeAsDoneEvent(result,  foodRecipeMade, information);
            }
        }









        public void RegisterCustomer(int result)
        {
            if (RegisterCustomerEvent != null)
            {
                RegisterCustomerEvent(result);
            }
        }

        public void DeleteCustomerById(int result)
        {
            if (DeleteCustomerByIdEvent != null)
            {
                DeleteCustomerByIdEvent(result);
            }
        }

        public void GetCustomerListSortedByName(List<CustomerContract> customerContracts, List<AddressContract> addressContracts)
        {
            if (GetCustomerListSortedByNameEvent != null)
            {
                GetCustomerListSortedByNameEvent(customerContracts, addressContracts);
            }
        }

        public void UpdateCustomer(int result)
        {
            if (UpdateCustomerEvent != null)
            {
                UpdateCustomerEvent(result);
            }
        }
    }
}
