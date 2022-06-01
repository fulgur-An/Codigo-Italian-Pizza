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
