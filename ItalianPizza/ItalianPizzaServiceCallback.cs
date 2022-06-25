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

        #region JavierBlas
        public delegate void LoginEmployeeDelegate(EmployeeContract employee, bool confirmLogin);
        public event LoginEmployeeDelegate LoginEmployeeEvent;

        public delegate void UpdateLoginDelegate(int result);
        public event UpdateLoginDelegate UpdateLoginEvent;

        public delegate void RegisterEmployeeDelegate(int result);
        public event RegisterEmployeeDelegate RegisterEmployeeEvent;

        public delegate void DeleteEmployeeByIdDelegate(int result);
        public event DeleteEmployeeByIdDelegate DeleteEmployeeByIdEvent;

        public delegate void GetEmployeeListSortedByNameDelegate(List<EmployeeContract> employeeContracts);
        public event GetEmployeeListSortedByNameDelegate GetEmployeeListSortedByNameEvent;

        public delegate void UpdateEmployeeDelegate(int result);
        public event UpdateEmployeeDelegate UpdateEmployeeEvent;

        public delegate void GetEmployeeWorkshiftDelegate(WorkshiftContract workshiftContracts);
        public event GetEmployeeWorkshiftDelegate GetEmployeeWorkshiftEvent;

        public delegate void GetEmployeeLogOutDelegate(LogOutContract logOutContract);
        public event GetEmployeeLogOutDelegate GetEmployeeLogOutEvent;

        public delegate void GetEmployeeRoleDelegate(string employeeRole);
        public event GetEmployeeRoleDelegate GetEmployeeRoleEvent;

        public void LoginEmployee(EmployeeContract employee, bool confirmLogin)
        {
            if (LoginEmployeeEvent != null)
            {
                LoginEmployeeEvent(employee, confirmLogin);
            }
        }

        public void UpdateLogin(int result)
        {
            if (UpdateLoginEvent != null)
            {
                UpdateLoginEvent(result);
            }
        }

        public void RegisterEmployee(int result)
        {
            if (RegisterEmployeeEvent != null)
            {
                RegisterEmployeeEvent(result);
            }
        }

        public void DeleteEmployeeById(int result)
        {
            if (DeleteEmployeeByIdEvent != null)
            {
                DeleteEmployeeByIdEvent(result);
            }
        }

        public void GetEmployeeListSortedByName(List<EmployeeContract> employeeContracts)
        {
            if (GetEmployeeListSortedByNameEvent != null)
            {
                GetEmployeeListSortedByNameEvent(employeeContracts);
            }
        }

        public void UpdateEmployee(int result)
        {
            if (UpdateEmployeeEvent != null)
            {
                UpdateEmployeeEvent(result);
            }
        }

        public void GetEmployeeWorkshift(WorkshiftContract workshiftContracts)
        {
            if (GetEmployeeWorkshiftEvent != null)
            {
                GetEmployeeWorkshiftEvent(workshiftContracts);
            }
        }

        public void GetEmployeeLogOut(LogOutContract logOutContract)
        {
            if (GetEmployeeLogOutEvent != null)
            {
                GetEmployeeLogOutEvent(logOutContract);
            }
        }

        public void GetEmployeeRole(string employeeRole)
        {
            if (GetEmployeeRoleEvent != null)
            {
                GetEmployeeRoleEvent(employeeRole);
            }
        }

        public void GetFoodRecipeList(List<FoodRecipeContract> foodRecipes)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region RubenI

        public delegate void GetProviderListDelegate(List<ProviderContract> providerContracts);
        public event GetProviderListDelegate GetProviderListEvent;

        public delegate void GetProviderListSortedByNameDelegate(List<ProviderContract> providerContracts);
        public event GetProviderListSortedByNameDelegate GetProviderListSortedByNameEvent;

        public delegate void RegisterProviderDelegate(int result);
        public event RegisterProviderDelegate RegisterProviderEvent;

        public delegate void GetProviderByIdDelegate(ProviderContract providerContract);
        public event GetProviderByIdDelegate GetProviderByIdEvent;

        public delegate void DeleteProviderByIdDelegate(int result);
        public event DeleteProviderByIdDelegate DeleteProviderByIdEvent;

        public delegate void UpdateProviderDelegate(int result);
        public event UpdateProviderDelegate UpdateProviderEvent;

        public void GetProviderList(List<ProviderContract> providers)
        {
            if (GetProviderListEvent != null)
            {
                GetProviderListEvent(providers);
            }
        }

        public void GetProviderListSortedByName(List<ProviderContract> providers)
        {
            if (GetProviderListSortedByNameEvent != null)
            {
                GetProviderListSortedByNameEvent(providers);
            }
        }

        public void RegisterProvider(int result)
        {
            if (RegisterProviderEvent != null)
            {
                RegisterProviderEvent(result);
            }
        }

        public void GetProviderById(ProviderContract provider)
        {
            if (GetProviderByIdEvent != null)
            {
                GetProviderByIdEvent(provider);
            }
        }

        public void DeleteProviderById(int result)
        {
            if (DeleteProviderByIdEvent != null)
            {
                DeleteProviderByIdEvent(result);
            }
        }

        public void UpdateProvider(int result)
        {
            if (UpdateProviderEvent != null)
            {
                UpdateProviderEvent(result);
            }
        }


        #endregion


        #region Antonio

        #region Delegates

        public delegate void GetFoodRecipeListDelegate(List<FoodRecipeContract> foodRecipes);
        public event GetFoodRecipeListDelegate GetFoodRecipeListEvent;

        #region Items Delegates

        public delegate void DeleteItemDelegate(int result);
        public event DeleteItemDelegate DeleteItemEvent;

        public delegate void GetItemListDelegate(List<ItemContract> Items);
        public event GetItemListDelegate GetItemListEvent;

        public delegate void GetItemSpecificationDelegate(ItemContract item);
        public event GetItemSpecificationDelegate GetItemSpecificationEvent;

        public delegate void RegisterItemDelegate(int result);
        public event RegisterItemDelegate RegisterItemEvent;

        public delegate void UpdateItemDelegate(int result);
        public event UpdateItemDelegate UpdateItemEvent;

        #endregion

        #region stocktaking delegates

        public delegate void GetStockTakingDelegate(List<StockTakingContract> stockTakings);
        public event GetStockTakingDelegate GetStockTakingEvent;

        public delegate void RegisterStockTakingDelegate(int Result);
        public event RegisterStockTakingDelegate RegisterStockTakingEvent;

        public delegate void GetItemsForStocktakingDelegate(List<StockTakingContract> items);
        public event GetItemsForStocktakingDelegate GetItemsForStocktakingEvent;

        #endregion

        #region monetary expediture delegates
        public delegate void GetMonetaryExpeditureDelegate(List<MonetaryExpeditureContract> monetaryExpeditures);
        public event GetMonetaryExpeditureDelegate GetMonetaryExpeditureEvent;

        public delegate void RegisterMonetaryExpeditureDelegate(int Result);
        public event RegisterMonetaryExpeditureDelegate RegisterMonetaryExpeditureEvent;
        #endregion

        #region daily balance delegates
        public delegate void GetDailyBalanceDelegate(List<DailyBalanceContract> dailyBalances);
        public event GetDailyBalanceDelegate GetDailyBalanceEvent;

        public delegate void RegisterDailyBalanceDelegate(int Result);
        public event RegisterDailyBalanceDelegate RegisterDailyBalanceEvent;

        public delegate void GetAmountsDelegate(decimal dialyEntrys, decimal dialyExits, decimal cashBalance);
        public event GetAmountsDelegate GetAmountsEvent;
        #endregion

        #endregion


        #region Items

        public void DeleteItem(int result)
        {
            if (DeleteItemEvent != null)
            {
                DeleteItemEvent(result);
            }
        }

        public void GetItemList(List<ItemContract> Items)
        {
            if (GetItemListEvent != null)
            {
                GetItemListEvent(Items);
            }
        }

        public void GetItemSpecification(ItemContract item)
        {
            if (GetItemSpecificationEvent != null)
            {
                GetItemSpecificationEvent(item);
            }
        }

        public void RegisterItem(int result)
        {
            if (RegisterItemEvent != null)
            {
                RegisterItemEvent(result);
            }
        }

        public void UpdateItem(int result)
        {
            if (UpdateItemEvent != null)
            {
                UpdateItemEvent(result);
            }
        }

        #endregion

        #region StockTakings

        public void GetStockTaking(List<StockTakingContract> stockTakings)
        {
            if (GetStockTakingEvent != null)
            {
                GetStockTakingEvent(stockTakings);
            }
        }

        public void RegisterStockTaking(int Result)
        {
            if (RegisterStockTakingEvent != null)
            {
                RegisterStockTakingEvent(Result);
            }
        }

        public void GetItemsForStocktaking(List<StockTakingContract> items)
        {
            if (GetItemsForStocktakingEvent != null)
            {
                GetItemsForStocktakingEvent(items);
            }
        }

        #endregion

        #region monetary expediture


        public void GetMonetaryExpediture(List<MonetaryExpeditureContract> monetaryExpeditures)
        {
            if (GetMonetaryExpeditureEvent != null)
            {
                GetMonetaryExpeditureEvent(monetaryExpeditures);
            }
        }

        public void RegisterMonetaryExpediture(int Result)
        {
            if (RegisterMonetaryExpeditureEvent != null)
            {
                RegisterMonetaryExpeditureEvent(Result);
            }
        }

        #endregion

        #region Daily balance

        public void GetDailyBalance(List<DailyBalanceContract> dailyBalances)
        {
            if (GetDailyBalanceEvent != null)
            {
                GetDailyBalanceEvent(dailyBalances);
            }
        }

        public void RegisterDailyBalance(int Result)
        {
            if (RegisterDailyBalanceEvent != null)
            {
                RegisterDailyBalanceEvent(Result);
            }
        }

        public void GetAmounts(decimal dialyEntrys, decimal dialyExits, decimal cashBalance)
        {
            if (GetAmountsEvent != null)
            {
                GetAmountsEvent(dialyEntrys, dialyExits, cashBalance);
            }
        }


        #endregion

        #endregion


        #region CamarilloVilla
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
                MarkRecipeAsDoneEvent(result, foodRecipeMade, information);
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

        #endregion
    }
}