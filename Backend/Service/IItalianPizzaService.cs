using Backend.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service
{
    [ServiceContract(CallbackContract = typeof(IItalianPizzaServiceCallback))]
    public interface IItalianPizzaService
    {

        [OperationContract(IsOneWay = true)]
        void LoginEmployee(EmployeeContract employee, LogOutContract logOutContract);

        [OperationContract(IsOneWay = true)]
        void UpdateLogin(string usernameEmploye, LogOutContract logOutContract);

        [OperationContract(IsOneWay = true)]
        void RegisterEmployee(EmployeeContract employeeContract, WorkshiftContract workshiftContract);

        [OperationContract(IsOneWay = true)]
        void DeleteEmployeeById(int idEmployee);

        [OperationContract(IsOneWay = true)]
        void GetEmployeeListSortedByName();

        [OperationContract(IsOneWay = true)]
        void UpdateEmployee(EmployeeContract employeeContract, WorkshiftContract workshiftContracts);

        [OperationContract(IsOneWay = true)]
        void GetEmployeeWorkshift(int idEmployee);

        [OperationContract(IsOneWay = true)]
        void GetEmployeeLogOut(string usernameEmployee);

        [OperationContract(IsOneWay = true)]
        void GetEmployeeRole(string usernameEmployee);

        [OperationContract(IsOneWay = true)]
        void GetProviderList();

        [OperationContract(IsOneWay = true)]
        void GetProviderListSortedByName();

        [OperationContract(IsOneWay = true)]
        void RegisterProvider(ProviderContract provider);

        [OperationContract(IsOneWay = true)]
        void GetProviderById(int idProvider);

        [OperationContract(IsOneWay = true)]
        void DeleteProviderById(int idProvider);

        [OperationContract(IsOneWay = true)]
        void UpdateProvider(ProviderContract providerContract);

        #region ItemServices

        [OperationContract(IsOneWay = true)]
        void GetItemList(string filterString, int option);

        [OperationContract(IsOneWay = true)]
        void GetItemSpecification(int idItem);

        [OperationContract(IsOneWay = true)]
        void RegisterItem(ItemContract itemContract);

        [OperationContract(IsOneWay = true)]
        void UpdateItem(ItemContract itemContract);

        [OperationContract(IsOneWay = true)]
        void DeleteItem(int idItem);

        #endregion

        #region StockTakingServices

        [OperationContract(IsOneWay = true)]
        void GetStockTaking(DateTime date);

        [OperationContract(IsOneWay = true)]
        void RegisterStockTaking(List<StockTakingContract> stockTaking);

        [OperationContract(IsOneWay = true)]
        void GetItemsForStocktaking();

        #endregion

        #region MonetaryExpediture

        [OperationContract(IsOneWay = true)]
        void GetMonetaryExpediture(string filter, int option);

        [OperationContract(IsOneWay = true)]
        void RegisterMonetaryExpediture(MonetaryExpeditureContract monetaryExpediture);

        #endregion

        #region DailyBalance

        [OperationContract(IsOneWay = true)]
        void GetDailyBalance(string filter, int option);

        [OperationContract(IsOneWay = true)]
        void RegisterDailyBalance(DailyBalanceContract dailyBalance);

        [OperationContract(IsOneWay = true)]
        void GetAmounts();

        #endregion

        [OperationContract(IsOneWay = true)]
        void GetItemIngredientList();

        [OperationContract(IsOneWay = true)]
        void GetFoodRecipeListSortedByName();

        [OperationContract(IsOneWay = true)]
        void RegisterFoodRecipe(FoodRecipeContract foodRecipes, List<IngredientContract> ingredients);

        [OperationContract(IsOneWay = true)]
        void GetFoodRecipeById(int idFoodRecipe);

        [OperationContract(IsOneWay = true)]
        void GetIngredientsById(int idFoodRecipe);

        [OperationContract(IsOneWay = true)]
        void DeleteFoodRecipeById(int idFoodRecipe);

        [OperationContract(IsOneWay = true)]
        void UpdateFoodRecipe(FoodRecipeContract foodRecipes, List<IngredientContract> ingredients);

        [OperationContract(IsOneWay = true)]
        void GetItemsSortedByName();

        [OperationContract(IsOneWay = true)]
        void RegisterOrder(OrderContract orderContract, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts,
            List<QuantityItemContract> quantityItemContracts);

        [OperationContract(IsOneWay = true)]
        void GetIdEmployeeByName(string fullNameEmployee);

        [OperationContract(IsOneWay = true)]
        void GetRecipesAvailable();

        [OperationContract(IsOneWay = true)]
        void GetOrderList();

        [OperationContract(IsOneWay = true)]
        void DeleteOrderById(int orderId);

        [OperationContract(IsOneWay = true)]
        void GetFoodRecipeListToPrepare();

        [OperationContract(IsOneWay = true)]
        void UpdateOrder(int orderId, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts,
            List<QuantityItemContract> quantityItemContracts);

        [OperationContract(IsOneWay = true)]
        void MarkRecipeAsDone(int orderId, int foodRecipeId);



        [OperationContract(IsOneWay = true)]
        void RegisterCustomer(CustomerContract customer, List<AddressContract> addresses);

        [OperationContract(IsOneWay = true)]
        void DeleteCustomerById(int idCustomer);

        [OperationContract(IsOneWay = true)]
        void GetCustomerListSortedByName();

        [OperationContract(IsOneWay = true)]
        void UpdateCustomer(CustomerContract customerContract, List<AddressContract> addressContracts);

    }

    [ServiceContract]
    public interface IItalianPizzaServiceCallback
    {


        [OperationContract(IsOneWay = true)]
        void LoginEmployee(EmployeeContract employee, bool confirmLogin);

        [OperationContract(IsOneWay = true)]
        void UpdateLogin(int result);

        [OperationContract(IsOneWay = true)]
        void RegisterEmployee(int result);

        [OperationContract(IsOneWay = true)]
        void DeleteEmployeeById(int result);

        [OperationContract(IsOneWay = true)]
        void GetEmployeeListSortedByName(List<EmployeeContract> employeeContracts);

        [OperationContract(IsOneWay = true)]
        void UpdateEmployee(int result);

        [OperationContract(IsOneWay = true)]
        void GetEmployeeWorkshift(WorkshiftContract workshiftContracts);

        [OperationContract(IsOneWay = true)]
        void GetEmployeeLogOut(LogOutContract logOutContract);

        [OperationContract(IsOneWay = true)]
        void GetEmployeeRole(string employeeRole);

        [OperationContract(IsOneWay = true)]
        void GetProviderList(List<ProviderContract> providerContracts);

        [OperationContract(IsOneWay = true)]
        void GetProviderListSortedByName(List<ProviderContract> providerContracts);

        [OperationContract(IsOneWay = true)]
        void RegisterProvider(int result);

        [OperationContract(IsOneWay = true)]
        void GetProviderById(ProviderContract providerContract);

        [OperationContract(IsOneWay = true)]
        void DeleteProviderById(int result);

        [OperationContract(IsOneWay = true)]
        void UpdateProvider(int result);

        #region Items callbacks

        [OperationContract(IsOneWay = true)]
        void GetItemList(List<ItemContract> Items);

        [OperationContract(IsOneWay = true)]
        void GetItemSpecification(ItemContract item);

        [OperationContract(IsOneWay = true)]
        void RegisterItem(int result);

        [OperationContract(IsOneWay = true)]
        void UpdateItem(int result);

        [OperationContract(IsOneWay = true)]
        void DeleteItem(int result);

        #endregion

        #region StockTaking  Callbacks

        [OperationContract(IsOneWay = true)]
        void GetStockTaking(List<StockTakingContract> stockTakings);

        [OperationContract(IsOneWay = true)]
        void RegisterStockTaking(int Result);

        [OperationContract(IsOneWay = true)]
        void GetItemsForStocktaking(List<StockTakingContract> items);

        #endregion

        #region MonetaryExpediture

        [OperationContract(IsOneWay = true)]
        void GetMonetaryExpediture(List<MonetaryExpeditureContract> monetaryExpeditures);

        [OperationContract(IsOneWay = true)]
        void RegisterMonetaryExpediture(int Result);

        #endregion

        #region DailyBalance

        [OperationContract(IsOneWay = true)]
        void GetDailyBalance(List<DailyBalanceContract> dailyBalances);

        [OperationContract(IsOneWay = true)]
        void RegisterDailyBalance(int Result);

        [OperationContract(IsOneWay = true)]
        void GetAmounts(decimal dialyEntrys, decimal dialyExits, decimal cashBalance);

        #endregion


        [OperationContract(IsOneWay = true)]
        void GetFoodRecipeListSortedByName(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients);

        [OperationContract(IsOneWay = true)]
        void GetItemIngredientList(List<ItemContract> items);

        [OperationContract(IsOneWay = true)]
        void RegisterFoodRecipe(int result);

        [OperationContract(IsOneWay = true)]
        void GetFoodRecipeById(FoodRecipeContract foodRecipeContract);

        [OperationContract(IsOneWay = true)]
        void GetIngredientsById(List<IngredientContract> ingredientContracts);

        [OperationContract(IsOneWay = true)]
        void DeleteFoodRecipeById(int result);

        [OperationContract(IsOneWay = true)]
        void UpdateFoodRecipe(int result);

        [OperationContract(IsOneWay = true)]
        void GetItemsSortedByName(List<ItemContract> items);

        [OperationContract(IsOneWay = true)]
        void RegisterOrder(int result, List<string> foodRecipeErrors, List<string> itemsErrors);

        [OperationContract(IsOneWay = true)]
        void GetIdEmployeeByName(int intEmployee);

        [OperationContract(IsOneWay = true)]
        void GetRecipesAvailable(List<FoodRecipeContract> foodRecipeContracts);

        [OperationContract(IsOneWay = true)]
        void GetOrderList(List<OrderContract> orderContracts, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts, List<QuantityItemContract> quantityItemContracts);

        [OperationContract(IsOneWay = true)]
        void DeleteOrderById(int result);

        [OperationContract(IsOneWay = true)]
        void GetFoodRecipeListToPrepare(List<FoodRecipeContract> foodRecipes, List<IngredientContract> ingredients);

        [OperationContract(IsOneWay = true)]
        void UpdateOrder(int result);

        [OperationContract(IsOneWay = true)]
        void MarkRecipeAsDone(int result, bool foodRecipeMade, string information);







        [OperationContract(IsOneWay = true)]
        void RegisterCustomer(int result);

        [OperationContract(IsOneWay = true)]
        void DeleteCustomerById(int result);

        [OperationContract(IsOneWay = true)]
        void GetCustomerListSortedByName(List<CustomerContract> customerContracts, List<AddressContract> addressContracts);

        [OperationContract(IsOneWay = true)]
        void UpdateCustomer(int result);
    }
}
