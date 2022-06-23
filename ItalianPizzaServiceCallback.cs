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

        public delegate void GetProviderListDelegate (List<ProviderContract> providerContracts);
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
            if(UpdateProviderEvent != null)
            {
                UpdateProviderEvent(result);
            }
        }
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

        public void GetFoodRecipeList(List<FoodRecipeContract> foodRecipes)
        {
            if (GetFoodRecipeListEvent != null)
            {
                GetFoodRecipeListEvent(foodRecipes);
            }
        }

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
    }
}
