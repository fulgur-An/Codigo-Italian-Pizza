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


    }
}
