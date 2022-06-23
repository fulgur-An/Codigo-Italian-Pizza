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
        public delegate void RegisterCustomerDelegate(int result);
        public event RegisterCustomerDelegate RegisterCustomerEvent;

        public delegate void DeleteCustomerByIdDelegate(int result);
        public event DeleteCustomerByIdDelegate DeleteCustomerByIdEvent;

        public delegate void GetCustomerListSortedByNameDelegate(List<CustomerContract> customerContracts, List<AddressContract> addressContracts);
        public event GetCustomerListSortedByNameDelegate GetCustomerListSortedByNameEvent;

        public delegate void UpdateCustomerDelegate(int result);
        public event UpdateCustomerDelegate UpdateCustomerEvent;

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

    }
}
