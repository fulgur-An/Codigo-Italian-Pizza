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


    }
}
