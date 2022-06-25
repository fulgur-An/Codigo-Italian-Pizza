using Backend.Contracts;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ItalianPizzaService : IItalianPizzaService
    {
        private ItalianPizzaContext context = new ItalianPizzaContext();

        #region JavierBlas

        public void LoginEmployee(EmployeeContract employee, LogOutContract logOutContract)
        {

            //Employee employeeLogin = context.Employees.FirstOrDefault(u => u.Username.Equals(employee.Username));
            Employee employeeLogin = context.Employees.Where(u => u.Username.Equals(employee.Username)).FirstOrDefault();
            //EmployeeContract employeeContract = new EmployeeContract();
            bool confirmLogin = false;

            if (employeeLogin != null)
            {
                if (employeeLogin.Username.Equals(employee.Username) && employeeLogin.Password.Equals(employee.Password))
                {
                    confirmLogin = true;
                }
                employee.IdUserEmployee = employeeLogin.IdUserEmployee;
                employee.Name = employeeLogin.Name;
                employee.LastName = employeeLogin.LastName;
                employee.Role = employeeLogin.Role;
                employee.Username = employeeLogin.Username;
                employee.Password = employeeLogin.Password;
                employee.Email = employeeLogin.Email;

                context.LogOuts.Add(new LogOut
                {
                    DepartureTime = "0",
                    TimeOfEntry = logOutContract.DepartureTime,
                    IdUserEmployee = employeeLogin.IdUserEmployee
                });

                context.SaveChanges();

            }
            else
            {
                Console.WriteLine("Fallo :u");
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().LoginEmployee(employee, confirmLogin);

        }

        public void UpdateLogin(string usernameEmployee, LogOutContract logOutContract)
        {
            Employee employee = context.Employees.Where(x => x.Username.Equals(usernameEmployee)).FirstOrDefault();
            int employeeID = employee.IdUserEmployee;
            string departureTime = "0";
            LogOut logOut = context.LogOuts.Where(x => x.IdUserEmployee.Equals(employeeID) && x.DepartureTime.Equals(departureTime)).FirstOrDefault();
            string timeExit = DateTime.Now.ToString("T");

            //logOutContract.DepartureTime = timeExit;

            //logOut.DepartureTime = logOutContract.DepartureTime;

            //si jala pero no registra el departure time
            if (logOut != null)
            {
                logOut.DepartureTime = timeExit;
            }

            int result = context.SaveChanges();

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().UpdateLogin(result);
        }

        public int RegisterEmployeeWorkshifts(WorkshiftContract workshiftsContracts, int employeeId)
        {
            context.WorkShifts.Add(new WorkShift
            {
                DepartureTime = workshiftsContracts.DepartureTime,
                Time = workshiftsContracts.Time,
                TimeOfEntry = workshiftsContracts.TimeOfEntry,
                IdUserEmployee = employeeId
            });

            int result = context.SaveChanges();

            return result;
        }

        public void RegisterEmployee(EmployeeContract employeeContract, WorkshiftContract workshiftContract)
        {
            int result = 0;

            bool isExisting = context.Employees.Where(x => x.Name.Equals(employeeContract.Name) &&
            x.LastName.Equals(employeeContract.LastName)).Count() > 0;

            Employee employee = new Employee
            {
                Email = employeeContract.Email,
                Name = employeeContract.Name,
                LastName = employeeContract.LastName,
                Phone = employeeContract.Phone,
                Status = "Available",
                AdmissionDate = employeeContract.AdmissionDate,
                Password = employeeContract.Password,
                Role = employeeContract.Role,
                Salary = employeeContract.Salary,
                Username = employeeContract.Username
            };

            try
            {
                if (isExisting)
                {
                    result = -1;
                }
                else
                {
                    context.Employees.Add(employee);
                    result = context.SaveChanges();

                    if (result > 0)
                    {
                        int employeeId = context.Employees.Max(x => x.IdUserEmployee);
                        result = RegisterEmployeeWorkshifts(workshiftContract, employeeId);
                    }
                    else
                    {
                        context.Employees.Remove(employee);
                        context.SaveChanges();
                        result = 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().RegisterEmployee(result);
        }

        public void DeleteEmployeeById(int idEmployee)
        {
            Employee employee = context.Employees.Where(x => x.IdUserEmployee == idEmployee).FirstOrDefault();
            int result = 0;

            if (employee != null)
            {
                employee.Status = "Leave";
                result = context.SaveChanges();
            }
            else
            {
                result = -1;
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().DeleteEmployeeById(result);
        }

        public void GetEmployeeListSortedByName()
        {
            List<Employee> employees = context.Employees.
            Where(x => x.Status.Equals("Available")).
            OrderBy(x => x.Name).ToList();
            List<EmployeeContract> employeeContracts = new List<EmployeeContract>();

            foreach (Employee employee in employees)
            {

                WorkShift workshift = context.WorkShifts.Where(w => w.IdUserEmployee.Equals(employee.IdUserEmployee)).FirstOrDefault();

                employeeContracts.Add(new EmployeeContract
                {
                    IdUserEmployee = employee.IdUserEmployee,
                    Email = employee.Email,
                    Name = employee.Name,
                    LastName = employee.LastName,
                    Phone = employee.Phone,
                    Status = employee.Status,
                    AdmissionDate = employee.AdmissionDate,
                    Password = employee.Password,
                    Role = employee.Role,
                    Salary = employee.Salary,
                    Username = employee.Username,
                    Workshift = workshift.TimeOfEntry + "-" + workshift.DepartureTime
                });
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetEmployeeListSortedByName(employeeContracts);
        }

        public int UpdateEmployeeWorkshift(int idEmployee, WorkshiftContract workshiftContracts)
        {
            int result = 1;

            WorkShift workShift = context.WorkShifts.Where(x => x.IdUserEmployee.Equals(idEmployee)).FirstOrDefault();

            workShift.DepartureTime = workshiftContracts.DepartureTime;
            workShift.Time = workshiftContracts.Time;
            workShift.TimeOfEntry = workshiftContracts.TimeOfEntry;

            context.SaveChanges();

            return result;
        }

        public void UpdateEmployee(EmployeeContract employeeContract, WorkshiftContract workshiftContracts)
        {
            Employee employee = context.Employees.Where(x => x.IdUserEmployee.Equals(employeeContract.IdUserEmployee)).FirstOrDefault();
            //WorkShift workShift = context.WorkShifts.Where(x => x.IdUserEmployee.Equals(employeeContract.IdUserEmployee)).FirstOrDefault();

            int result = 0;
            bool isExisting = context.Employees.Where(x => x.Name.Equals(employeeContract.Name) &&
            x.LastName.Equals(employeeContract.LastName) && x.IdUserEmployee != employee.IdUserEmployee).Count() > 0;

            if (isExisting)
            {
                result = -1;
            }
            else
            {
                if (employee != null)
                {
                    employee.Email = employeeContract.Email;
                    employee.Name = employeeContract.Name;
                    employee.LastName = employeeContract.LastName;
                    employee.Phone = employeeContract.Phone;
                    employee.Status = employeeContract.Status;
                    employee.AdmissionDate = employeeContract.AdmissionDate;
                    employee.Password = employeeContract.Password;
                    employee.Role = employeeContract.Role;
                    employee.Salary = employeeContract.Salary;
                    UpdateEmployeeWorkshift(employeeContract.IdUserEmployee, workshiftContracts);

                    result = context.SaveChanges();
                }
                if ((result >= 0))
                {
                    result = UpdateEmployeeWorkshift(employeeContract.IdUserEmployee, workshiftContracts);
                }
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().UpdateEmployee(result);
        }

        public void GetEmployeeWorkshift(int idEmployee)
        {
            WorkshiftContract workshiftContracts = new WorkshiftContract();
            WorkShift workShift = context.WorkShifts.Where(x => x.IdUserEmployee.Equals(idEmployee)).FirstOrDefault();

            workshiftContracts.DepartureTime = workShift.DepartureTime;
            workshiftContracts.TimeOfEntry = workShift.TimeOfEntry;
            workshiftContracts.Time = workShift.Time;

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetEmployeeWorkshift(workshiftContracts);
        }

        public void GetEmployeeLogOut(string usernameEmployee)
        {
            EmployeeContract employeeContract = new EmployeeContract();
            Employee employee = context.Employees.Where(x => x.Username.Equals(usernameEmployee)).FirstOrDefault();
            LogOutContract logOutContract = new LogOutContract();
            LogOut logOut = context.LogOuts.Where(x => x.IdUserEmployee.Equals(employee.IdUserEmployee)).FirstOrDefault();

            logOutContract.IdUserEmployee = logOut.IdUserEmployee;
            logOutContract.TimeOfEntry = logOut.TimeOfEntry;
            logOutContract.DepartureTime = logOut.DepartureTime;
            logOutContract.IdLogOut = logOut.IdLogOut;

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetEmployeeLogOut(logOutContract);
        }

        public void GetEmployeeRole(string usernameEmployee)
        {
            Employee employee = context.Employees.Where(x => x.Username.Equals(usernameEmployee)).FirstOrDefault();

            string employeeRole = employee.Role;

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetEmployeeRole(employeeRole);
        }

        #endregion


        #region RubenI

        public void GetProviderList()
        {
            List<Provider> providers = context.Providers.Where(x => x.IsEnabled).ToList(); //LISTA DE MODELOS QUE SE CREAN AUTOMATICAMENTE DE BD
            List<ProviderContract> providerContracts = new List<ProviderContract>();

            foreach (Provider provider in providers)
            {
                ProviderContract providerContract = new ProviderContract
                {
                    IdProvider = provider.IdProvider,
                    Category = provider.Category,
                    Phone = provider.Phone,
                    Email = provider.Email,
                    Name = provider.Name,
                    RFC = provider.RFC,
                };
                providerContracts.Add(providerContract);
            }
            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetProviderList(providerContracts);
        }

        public void GetProviderById(int idProvider)
        {
            List<ProviderContract> providerContracts = new List<ProviderContract>();
            Provider provider = context.Providers.FirstOrDefault(pro => pro.IdProvider.Equals(idProvider));


            ProviderContract providerContract = new ProviderContract();

            providerContract.IdProvider = provider.IdProvider;
            providerContract.Category = provider.Category;
            providerContract.Email = provider.Email;
            providerContract.Name = provider.Name;
            providerContract.RFC = provider.RFC;

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetProviderById(providerContract);
        }

        public void GetProviderListSortedByName()
        {
            List<Provider> providers = context.Providers.Where(provider => provider.IsEnabled.Equals(true)).OrderBy(provider => provider.Name).ToList();
            List<ProviderContract> providerContracts = new List<ProviderContract>();

            foreach (Provider provider in providers)
            {
                providerContracts.Add(new ProviderContract
                {
                    IdProvider = provider.IdProvider,
                    Category = provider.Category,
                    Phone = provider.Phone,
                    Email = provider.Email,
                    Name = provider.Name,

                });
            }
            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetProviderListSortedByName(providerContracts);
        }

        public void RegisterProvider(ProviderContract providerContract)
        {
            int result = 0;

            bool isExisting = context.Providers.Where(x => x.Name.Equals(providerContract.Name) && x.RFC.Equals(providerContract.RFC) && x.IsEnabled).Count() > 0;

            Provider provider = new Provider
            {
                Category = providerContract.Category,
                Phone = providerContract.Phone,
                Email = providerContract.Email,
                Name = providerContract.Name,
                RFC = providerContract.RFC,
                Status = "available",
                IsEnabled = true
            };
            try
            {
                if (isExisting)
                {
                    result = -1;
                }
                else
                {
                    context.Providers.Add(provider);
                    result = context.SaveChanges();

                    //if (result > 0)
                    //{
                    //    int providerId = context.Providers.Max(recipe => recipe.IdProvider);
                    //}
                    //else
                    //{
                    //    context.Providers.Remove(provider);
                    //    context.SaveChanges();
                    //    result = 0;
                    //}
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().RegisterProvider(result);
        }

        public void DeleteProviderById(int idProvider)
        {
            Provider provider = context.Providers.Where(vider => vider.IdProvider == idProvider).FirstOrDefault();
            int result = 0;

            if (provider != null)
            {
                provider.IsEnabled = false;
                provider.Status = "Delete";
                result = context.SaveChanges();

            }
            else
            {
                result = -1;
            }
            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().DeleteProviderById(result);

        }

        public void UpdateProvider(ProviderContract providerContract)
        {
            Provider provider = context.Providers.Where(x => x.IdProvider == providerContract.IdProvider).FirstOrDefault();

            int result = 0;
            bool isExisting = context.Providers.Where(x => x.Name.Equals(providerContract.Name) && x.IsEnabled && x.IdProvider != provider.IdProvider).Count() > 0;


            if (isExisting)
            {
                result = -1;
            }
            else
            {
                if (provider != null)
                {
                    provider.Category = providerContract.Category;
                    provider.Phone = providerContract.Phone;
                    provider.Email = providerContract.Email;
                    provider.Name = providerContract.Name;
                    provider.RFC = providerContract.RFC;

                    result = context.SaveChanges();
                    context.SaveChanges();
                }

            }
            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().UpdateProvider(result);
        }
        #endregion


        #region Antonio

        #region Items service

        private List<Item> filterItems(string filter, int option)
        {
            List<Item> items = new List<Item>();
            switch (option)
            {
                case 1:

                    items = context.Items.Where(i => i.Name.Contains(filter)).ToList();

                    break;
                case 2:
                    items = context.Items.Where(i => i.Sku.ToString().Contains(filter)).ToList();
                    break;
                case 3:
                    items = context.Items.Where(i => i.Price.ToString().Contains(filter)).ToList();
                    break;
                case 4:
                    items = context.Items.Where(i => i.Quantity.ToString().Contains(filter)).ToList();
                    break;
            }
            return items;
        }

        private decimal CalculateTotalByItem(int quantity, decimal price)
        {
            decimal result = 0;
            result = quantity * price;
            return result;
        }

        private DateTime GetLastStockTakingByItem(int id)
        {
            DateTime date;
            try
            {
                Stocktaking stockTaking = context.Stocktakings.FirstOrDefault(s => s.IdItem.Equals(id));
                if (stockTaking == null)
                {
                    date = DateTime.MinValue;
                }
                else
                {
                    date = stockTaking.Date;
                }
            }
            catch (Exception ex)
            {
                date = DateTime.MinValue;
                Console.WriteLine(ex.Message);
            }
            return date;
        }

        public void GetItemList(string filterString, int option)
        {

            //List<Item> items = context.Items.Where(i => i.IsEnabled.Equals(true)).ToList();
            //Item item = items.Where(i => i.IdItem.Equals(stockTaking.IdItem)).FirstOrDefault();
            //item.last
            List<Item> items = new List<Item>();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                items = filterItems(filterString, option);
            }

            List<ItemContract> itemsContract = new List<ItemContract>();

            foreach (Item item in items)
            {
                if (item.IsEnabled)
                {
                    Stocktaking stockTaking = context.Stocktakings.Where(st => st.IdItem.Equals(item.IdItem)).FirstOrDefault();

                    itemsContract.Add(new ItemContract
                    {
                        IdItem = item.IdItem,
                        Name = item.Name,
                        Description = item.Description,
                        Sku = item.Sku,
                        Photo = item.Photo,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Total = CalculateTotalByItem(item.Quantity, item.Price),
                        LastStockTaking = GetLastStockTakingByItem(item.IdItem).ToString("dd/MM/yyyy"),
                        Restrictions = item.Restrictions,
                        UnitOfMeasurement = item.UnitOfMeasurement,
                        QuantityWhithUnit = item.Quantity + " " + item.UnitOfMeasurement,
                        NeedsFoodRecipe = item.NeedsFoodRecipe,
                        IsIngredient = item.IsIngredient,
                        IsEnabled = item.IsEnabled,
                        //LastStockTaking = stockTaking.Date
                    });
                }
            }
            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetItemList(
                itemsContract
            );
        }

        public void GetItemSpecification(int idItem)
        {
            Item item = context.Items.FirstOrDefault(i => i.IdItem.Equals(idItem));

            ItemContract itemContract = new ItemContract
            {
                IdItem = item.IdItem,
                Name = item.Name,
                Description = item.Description,
                Sku = item.Sku,
                Photo = item.Photo,
                Price = item.Price,
                Quantity = item.Quantity,
                Restrictions = item.Restrictions,
                UnitOfMeasurement = item.UnitOfMeasurement,
                NeedsFoodRecipe = item.NeedsFoodRecipe,
                IsIngredient = item.IsIngredient,
                IsEnabled = item.IsEnabled
            };

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetItemSpecification(
                itemContract
            );
        }

        public void RegisterItem(ItemContract itemContract)
        {
            context.Items.Add(new Item
            {
                Name = itemContract.Name,
                Description = itemContract.Description,
                Sku = itemContract.Sku,
                Photo = itemContract.Photo,
                Price = itemContract.Price,
                Quantity = itemContract.Quantity,
                Restrictions = itemContract.Restrictions,
                UnitOfMeasurement = itemContract.UnitOfMeasurement,
                NeedsFoodRecipe = itemContract.NeedsFoodRecipe,
                IsIngredient = itemContract.IsIngredient,
                IsEnabled = itemContract.IsEnabled
            });

            int result = context.SaveChanges();
            OperationContext.Current.GetCallbackChannel
                <
                    IItalianPizzaServiceCallback
                >
                ().RegisterItem(result);
        }

        public void UpdateItem(ItemContract itemContract)
        {
            Item item = context.Items.FirstOrDefault(i => i.IdItem.Equals(itemContract.IdItem));
            //Console.WriteLine(itemContract.IdItem + " " +item + " "+ item.IdItem);
            item.Name = itemContract.Name;
            item.Description = itemContract.Description;
            item.Sku = itemContract.Sku;
            item.Photo = itemContract.Photo;
            item.Price = itemContract.Price;
            item.Quantity = itemContract.Quantity;
            item.Restrictions = itemContract.Restrictions;
            item.UnitOfMeasurement = itemContract.UnitOfMeasurement;
            item.NeedsFoodRecipe = itemContract.NeedsFoodRecipe;
            item.IsIngredient = itemContract.IsIngredient;

            int result = context.SaveChanges();
            OperationContext.Current.GetCallbackChannel
                <
                    IItalianPizzaServiceCallback
                >
                ().UpdateItem(result);
        }

        public void DeleteItem(int idItem)
        {
            Item item = context.Items.Where(i => i.IdItem.Equals(idItem)).FirstOrDefault();
            if (item != null)
            {
                item.IsEnabled = false;
                int result = context.SaveChanges();
                OperationContext.Current.GetCallbackChannel
                    <
                        IItalianPizzaServiceCallback
                    >
                    ().DeleteItem(result);
            }

        }

        #endregion


        #region stocktaking service

        public void GetStockTaking(DateTime date)
        {
            List<Stocktaking> stockTakings = new List<Stocktaking>();
            try
            {
                stockTakings = context.Stocktakings.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }

            Console.WriteLine(date);

            List<StockTakingContract> stockTakingsContract = new List<StockTakingContract>();

            foreach (Stocktaking stockTaking in stockTakings)
            {

                //Console.WriteLine(stockTaking);
                Item item = context.Items.Where(i => i.IdItem.Equals(stockTaking.IdItem)).SingleOrDefault();
                stockTakingsContract.Add(new StockTakingContract
                {
                    Name = item.Name,
                    Sku = item.Sku,
                    CurrentAmount = stockTaking.CurrentAmount,
                    PhysicalAmount = stockTaking.PhysicalAmount,
                    Description = stockTaking.Description,
                    Date = stockTaking.Date.ToString("dd/MM/yyyy"),
                    IdItem = stockTaking.IdItem,
                });
                Console.WriteLine(stockTaking.Date.ToString("dd/MM/yyyy"));
            }
            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetStockTaking(
                stockTakingsContract
            );
        }

        public void RegisterStockTaking(List<StockTakingContract> stockTakings)
        {

            foreach (StockTakingContract stockTaking in stockTakings)
            {
                context.Stocktakings.Add(new Stocktaking
                {
                    CurrentAmount = stockTaking.CurrentAmount,
                    PhysicalAmount = stockTaking.PhysicalAmount,
                    Description = stockTaking.Description,
                    Date = Convert.ToDateTime(stockTaking.Date),
                    IdItem = stockTaking.IdItem,
                });

            }


            int result = context.SaveChanges();
            OperationContext.Current.GetCallbackChannel
                <
                    IItalianPizzaServiceCallback
                >
                ().RegisterStockTaking(result);
        }


        public void GetItemsForStocktaking()
        {
            List<Item> items = context.Items.Where(i => i.IsEnabled.Equals(true)).ToList();
            List<StockTakingContract> itemsContract = new List<StockTakingContract>();
            int iden = 0;
            foreach (Item item in items)
            {
                if (item.IsEnabled)
                {
                    itemsContract.Add(new StockTakingContract
                    {
                        Name = item.Name,
                        Sku = item.Sku,
                        Date = DateTime.Now.ToString("dd/MM/yyyy"),
                        CurrentAmount = item.Quantity,
                        IdItem = item.IdItem,
                    });
                }

            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetItemsForStocktaking(
                itemsContract
            );
        }

        #endregion


        private string GetEmployee(int id)
        {
            Employee employee = context.Employees.FirstOrDefault(e => e.IdUserEmployee.Equals(id));
            string employeeFullName = employee.Name + " " + employee.LastName;
            return employeeFullName;
        }

        #region monetary expediture

        private List<MonetaryExpeditureContract> filterMonetaryExpediture(string filter, int option, List<MonetaryExpeditureContract> monetaryExpensContract)
        {
            List<MonetaryExpeditureContract> monetaryExpens = new List<MonetaryExpeditureContract>();
            switch (option)
            {
                case 1:

                    monetaryExpens = monetaryExpensContract.Where(m => m.EmployeeName.Contains(filter)).ToList();

                    break;
                case 2:
                    monetaryExpens = monetaryExpensContract.Where(m => m.Amount.ToString().Contains(filter)).ToList();
                    break;
                case 3:
                    monetaryExpens = monetaryExpensContract.Where(m => m.Date.ToShortDateString().Contains(filter)).ToList();
                    break;
            }
            return monetaryExpens;
        }

        public void GetMonetaryExpediture(string filter, int option)
        {
            List<MonetaryExpens> monetaryExpens = new List<MonetaryExpens>();
            monetaryExpens = context.MonetaryExpenses.ToList();

            List<MonetaryExpeditureContract> monetaryExpensContract = new List<MonetaryExpeditureContract>();

            foreach (MonetaryExpens monetaryExpen in monetaryExpens)
            {
                monetaryExpensContract.Add(new MonetaryExpeditureContract
                {
                    IdMonetaryExpenditure = monetaryExpen.IdMonetaryExpenditure,
                    EmployeeName = GetEmployee(monetaryExpen.IdEmployee),
                    Amount = monetaryExpen.Amount,
                    Description = monetaryExpen.Description,
                    DateString = monetaryExpen.Date.ToString("dd/MM/yyyy"),
                    IdEmployee = monetaryExpen.IdEmployee,
                });
            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                monetaryExpensContract = filterMonetaryExpediture(filter, option, monetaryExpensContract);
                OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetMonetaryExpediture(
                    monetaryExpensContract
                );
            }

        }

        public void RegisterMonetaryExpediture(MonetaryExpeditureContract monetaryExpediture)
        {
            context.MonetaryExpenses.Add(new MonetaryExpens
            {
                //date1.ToString("d",
                //  CultureInfo.CreateSpecificCulture("en-NZ")
                Amount = monetaryExpediture.Amount,
                Description = monetaryExpediture.Description,
                Date = Convert.ToDateTime(monetaryExpediture.Date.ToString("d", CultureInfo.CreateSpecificCulture("en-NZ"))),
                IdEmployee = monetaryExpediture.IdEmployee
            });

            int result = context.SaveChanges();
            OperationContext.Current.GetCallbackChannel
                <
                    IItalianPizzaServiceCallback
                >
                ().RegisterMonetaryExpediture(result);
        }

        #endregion

        #region Daily balance

        private List<DailyBalanceContract> filterDailyBalance(string filter, int option, List<DailyBalanceContract> dailyBalancesContract)
        {
            List<DailyBalanceContract> dailyBalances = new List<DailyBalanceContract>();
            switch (option)
            {
                case 1:

                    dailyBalances = dailyBalancesContract.Where(d => d.EmployeeName.Contains(filter)).ToList();

                    break;
                case 2:
                    dailyBalances = dailyBalancesContract.Where(d => d.PhsycalBalance.ToString().Contains(filter)).ToList();
                    break;
                case 3:
                    dailyBalances = dailyBalancesContract.Where(d => d.CurrentDate.ToShortDateString().Contains(filter)).ToList();
                    break;
            }
            return dailyBalances;
        }
        public void GetDailyBalance(string filter, int option)
        {
            List<DailyBalance> dailyBalances = new List<DailyBalance>();
            dailyBalances = context.DailyBalances.ToList();

            List<DailyBalanceContract> dailyBalancesContract = new List<DailyBalanceContract>();

            foreach (DailyBalance dailyBalance in dailyBalances)
            {
                dailyBalancesContract.Add(new DailyBalanceContract
                {
                    idDailyBalance = dailyBalance.idDailyBalance,
                    EntryBalance = dailyBalance.EntryBalance,
                    ExitBalance = dailyBalance.ExitBalance,
                    InitialBalance = dailyBalance.InitialBalance,
                    CashBalance = dailyBalance.CashBalance,
                    PhsycalBalance = dailyBalance.PhsycalBalance,
                    DateString = dailyBalance.CurrentDate.ToString("dd/MM/yyyy"),
                    EmployeeName = GetEmployee(dailyBalance.IdEmployee),
                    IdEmployee = dailyBalance.IdEmployee
                });
            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                dailyBalancesContract = filterDailyBalance(filter, option, dailyBalancesContract);
                OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetDailyBalance(
                    dailyBalancesContract
                );
            }

        }

        public void RegisterDailyBalance(DailyBalanceContract dailyBalance)
        {
            context.DailyBalances.Add(new DailyBalance
            {
                EntryBalance = dailyBalance.EntryBalance,
                ExitBalance = dailyBalance.ExitBalance,
                InitialBalance = dailyBalance.InitialBalance,
                CashBalance = dailyBalance.CashBalance,
                PhsycalBalance = dailyBalance.PhsycalBalance,
                CurrentDate = Convert.ToDateTime(dailyBalance.CurrentDate.ToString("d", CultureInfo.CreateSpecificCulture("en-NZ"))),
                IdEmployee = dailyBalance.IdEmployee
            });

            int result = context.SaveChanges();
            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().RegisterDailyBalance(result);
        }

        private decimal CalculatateDialyEntrys(DateTime date)
        {
            List<Order> orders = context.Orders.Where(o => o.Date.CompareTo(date) < 0 && o.Status.Equals("Realizado")).ToList();
            decimal total = 0;
            foreach (Order order in orders)
            {
                total = total + order.TotalToPay;
            }

            return total;
        }

        private decimal CalculateDialyExits(DateTime date)
        {
            List<ProviderOrder> providerOrders = context.ProviderOrders.Where(po => po.DeliveryDate.CompareTo(date) < 0  && po.Status.Equals("Received")).ToList();
            List<MonetaryExpens> monetaryExpenses = context.MonetaryExpenses.Where(me => me.Date.CompareTo(date) < 0).ToList();
            decimal total = 0;
            foreach (ProviderOrder providerOrder in providerOrders)
            {
                total = total + providerOrder.TotalToPay;
            }
            foreach (MonetaryExpens monetaryExpense in monetaryExpenses)
            {
                total = total + monetaryExpense.Amount;
            }
            return total;
        }

        public void GetAmounts()
        {
            DateTime date = DateTime.Now;

            decimal dialyEntrys = CalculatateDialyEntrys(date);

            decimal dialyExits = CalculateDialyExits(date);

            decimal cashBalance = 1000 + dialyEntrys - dialyExits;


            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetAmounts(dialyEntrys, dialyExits, cashBalance);
        }

        #endregion

        #endregion


        #region CamarilloVilla


        public void GetFoodRecipeById(int idFoodRecipe)
        {
            FoodRecipe foodRecipe = context.FoodRecipes.FirstOrDefault(food => food.IdFoodRecipe.Equals(idFoodRecipe));
            FoodRecipeContract foodRecipeContract = new FoodRecipeContract();

            foodRecipeContract.IdFoodRecipe = foodRecipe.IdFoodRecipe;
            foodRecipeContract.Price = foodRecipe.Price;
            foodRecipeContract.Name = foodRecipe.Name;
            foodRecipeContract.Description = foodRecipe.Description;
            foodRecipeContract.NumberOfServings = foodRecipe.NumberOfServings;
            foodRecipeContract.Status = foodRecipe.Status;
            foodRecipeContract.IsEnabled = foodRecipe.IsEnabled;

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetFoodRecipeById(
                foodRecipeContract
            );
        }


        public void GetIngredientsById(int idFoodRecipe)
        {
            List<Item> items = context.Items.ToList();
            List<Ingredient> ingredients = context.Ingredients.Where(ingredient => ingredient.IdFoodRecipe == idFoodRecipe).ToList();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            foreach (Ingredient ingredient in ingredients)
            {
                byte[] ingredientPhoto = null;
                foreach (Item item in items)
                {
                    if (ingredient.IdItem == item.IdItem)
                    {
                        ingredientPhoto = item.Photo;
                    }
                }

                ingredientContracts.Add(new IngredientContract
                {
                    IdIngredient = ingredient.IdIngredient,
                    IdFoodRecipe = ingredient.IdFoodRecipe,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName,
                    IngredientPhoto = ingredientPhoto
                });
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetIngredientsById(
                ingredientContracts
            );
        }

        public void GetItemIngredientList()
        {
            List<Item> items = context.Items.Where(item => item.IsIngredient).ToList();

            List<ItemContract> itemContracts = new List<ItemContract>();

            foreach (Item item in items)
            {
                itemContracts.Add(new ItemContract
                {
                    IdItem = item.IdItem,
                    Photo = item.Photo,
                    Name = item.Name
                });
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetItemIngredientList(
                itemContracts
            );
        }

        public void RegisterFoodRecipe(FoodRecipeContract foodRecipeContract, List<IngredientContract> ingredients)
        {
            int result = 0;

            bool isExisting = context.FoodRecipes.Where(recipe => recipe.Name.Equals(foodRecipeContract.Name) &&
                recipe.IsEnabled == true).Count() > 0;

            FoodRecipe foodRecipe = new FoodRecipe
            {
                Price = foodRecipeContract.Price,
                Name = foodRecipeContract.Name,
                Description = foodRecipeContract.Description,
                NumberOfServings = foodRecipeContract.NumberOfServings,
                Status = foodRecipeContract.Status,
                IsEnabled = foodRecipeContract.IsEnabled
            };

            try
            {
                if (isExisting)
                {
                    result = -1;
                }
                else
                {
                    context.FoodRecipes.Add(foodRecipe);
                    result = context.SaveChanges();

                    if (result > 0)
                    {
                        int foodRecipeId = context.FoodRecipes.Max(recipe => recipe.IdFoodRecipe);
                        result = RegisterFoodRecipeIngredients(ingredients, foodRecipeId);
                    }
                    else
                    {
                        context.FoodRecipes.Remove(foodRecipe);
                        context.SaveChanges();
                        result = 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().RegisterFoodRecipe(
               result
            );
        }

        public int RegisterFoodRecipeIngredients(List<IngredientContract> ingredientsContract, int foodRecipeId)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            foreach (IngredientContract ingredient in ingredientsContract)
            {
                context.Ingredients.Add(new Ingredient
                {
                    IdFoodRecipe = foodRecipeId,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }
            int result = context.SaveChanges();

            return result;
        }

        public bool IdentifyChangesInIngredients(List<IngredientContract> ingredientContracts, int idFoodRecipe)
        {
            List<Ingredient> ingredients = context.Ingredients.ToList();
            int foodRecipeIngredientsNumber = context.Ingredients.Where(x => x.IdFoodRecipe == idFoodRecipe).Count();
            int newFoodRecipeIngredients = ingredientContracts.Count;
            bool newIngredient = foodRecipeIngredientsNumber == ingredientContracts.Count ? false : true;

            foreach (IngredientContract ingredientContract in ingredientContracts)
            {
                if (ingredientContract.IdIngredient == 0)
                {
                    newIngredient = true;
                }
            }

            return newIngredient;
        }

        public void GetFoodRecipeListSortedByName()
        {
            List<FoodRecipe> foodRecipes = context.FoodRecipes.
                Where(foodRecipe => foodRecipe.IsEnabled && foodRecipe.Status.Equals("Available")).
                OrderBy(foodRecipe => foodRecipe.Name).ToList();

            List<FoodRecipeContract> foodRecipeContracts = new List<FoodRecipeContract>();

            List<Ingredient> ingredients = context.Ingredients.ToList();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            foreach (FoodRecipe foodRecipe in foodRecipes)
            {
                foodRecipeContracts.Add(new FoodRecipeContract
                {
                    IdFoodRecipe = foodRecipe.IdFoodRecipe,
                    Price = foodRecipe.Price,
                    Name = foodRecipe.Name,
                    Description = foodRecipe.Description,
                    NumberOfServings = foodRecipe.NumberOfServings,
                    Status = foodRecipe.Status,
                    IsEnabled = foodRecipe.IsEnabled
                });
            }

            foreach (Ingredient ingredient in ingredients)
            {
                ingredientContracts.Add(new IngredientContract
                {
                    IdFoodRecipe = ingredient.IdFoodRecipe,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetFoodRecipeListSortedByName(
                foodRecipeContracts, ingredientContracts
            );
        }

        public void GetRecipesAvailable()
        {
            List<FoodRecipe> foodRecipes = context.FoodRecipes.
                Where(foodRecipe => foodRecipe.IsEnabled && foodRecipe.Status.Equals("Available")).
                OrderBy(foodRecipe => foodRecipe.Name).ToList();

            List<FoodRecipeContract> foodRecipesAvailable = new List<FoodRecipeContract>();
            List<Item> items = context.Items.ToList();

            foreach (FoodRecipe foodRecipe in foodRecipes)
            {
                List<Ingredient> ingredients = context.Ingredients.Where(x => x.IdFoodRecipe == foodRecipe.IdFoodRecipe).ToList();
                int foodRecipeIngredients = ingredients.Count;
                int count = 0;

                foreach (Ingredient ingredient in ingredients)
                {
                    foreach (Item item in items)
                    {
                        int ingredientQuantity = int.Parse(ingredient.IngredientQuantity);
                        if ((item.IdItem == ingredient.IdItem) && (item.Quantity >= ingredientQuantity))
                        {
                            count++;
                        }
                    }
                }

                if (count == foodRecipeIngredients)
                {
                    foodRecipesAvailable.Add(new FoodRecipeContract
                    {
                        IdFoodRecipe = foodRecipe.IdFoodRecipe,
                        Price = foodRecipe.Price,
                        Name = foodRecipe.Name,
                        Description = foodRecipe.Description,
                        NumberOfServings = foodRecipe.NumberOfServings,
                        Status = foodRecipe.Status,
                        IsEnabled = foodRecipe.IsEnabled
                    });
                }
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetRecipesAvailable(
                foodRecipesAvailable
            );
        }

        public void DeleteFoodRecipeById(int idFoodRecipe)
        {
            FoodRecipe foodRecipe = context.FoodRecipes.Where(recipe => recipe.IdFoodRecipe == idFoodRecipe).FirstOrDefault();

            if (foodRecipe != null)
            {
                foodRecipe.IsEnabled = false;
                foodRecipe.Status = "Deleted";
                int result = context.SaveChanges();
                OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().DeleteFoodRecipeById(
                    result
                );
            }
        }

        public int UpdateFoodRecipeIngredients(List<IngredientContract> ingredientContracts, int idFoodRecipe)
        {
            int result = 0;

            List<Ingredient> ingredients = context.Ingredients.ToList();

            foreach (Ingredient ingredient in ingredients)
            {
                if (ingredient.IdFoodRecipe == idFoodRecipe)
                {
                    context.Ingredients.Remove(ingredient);
                    result = context.SaveChanges();
                }
            }

            return result;
        }

        public void UpdateFoodRecipe(FoodRecipeContract foodRecipeContract, List<IngredientContract> ingredients)
        {
            FoodRecipe foodRecipe = context.FoodRecipes.Where(recipe => recipe.IdFoodRecipe == foodRecipeContract.IdFoodRecipe).FirstOrDefault();

            int result = 0;
            bool isExisting = context.FoodRecipes.Where(recipe => recipe.Name.Equals(foodRecipeContract.Name) &&
                recipe.IsEnabled && recipe.IdFoodRecipe != foodRecipe.IdFoodRecipe).Count() > 0;
            bool newIngredients = IdentifyChangesInIngredients(ingredients, foodRecipe.IdFoodRecipe);

            if (isExisting)
            {
                result = -1;
            }
            else
            {
                if (foodRecipe != null)
                {
                    foodRecipe.Price = foodRecipeContract.Price;
                    foodRecipe.Name = foodRecipeContract.Name;
                    foodRecipe.Description = foodRecipeContract.Description;
                    foodRecipe.NumberOfServings = foodRecipeContract.NumberOfServings;
                    foodRecipe.IsEnabled = foodRecipeContract.IsEnabled;
                    foodRecipe.Status = foodRecipeContract.Status;

                    result = context.SaveChanges();
                    context.SaveChanges();
                }
                if ((result > 0) || newIngredients)
                {
                    result = UpdateFoodRecipeIngredients(ingredients, foodRecipeContract.IdFoodRecipe);
                    if (result > 0)
                    {
                        result = RegisterFoodRecipeIngredients(ingredients, foodRecipeContract.IdFoodRecipe);
                    }
                }
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().UpdateFoodRecipe(
                result
            );
        }

        public void GetItemsSortedByName()
        {
            List<Item> items = context.Items.Where(item => !item.IsIngredient && item.IsEnabled).ToList();
            List<ItemContract> itemContracts = new List<ItemContract>();

            foreach (Item item in items)
            {
                itemContracts.Add(new ItemContract
                {
                    IdItem = item.IdItem,
                    Name = item.Name,
                    Description = item.Description,
                    Sku = item.Sku,
                    Photo = item.Photo,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Restrictions = item.Restrictions,
                    UnitOfMeasurement = item.UnitOfMeasurement,
                    NeedsFoodRecipe = item.NeedsFoodRecipe,
                    IsIngredient = item.IsIngredient,
                    IsEnabled = item.IsEnabled
                });
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetItemsSortedByName(
                itemContracts
            );
        }

        public List<string> DecreaseNumberOfIngredients(List<QuantityFoodRecipeContract> quantityFoodRecipeContracts)
        {
            List<Item> items = context.Items.ToList();
            List<string> errors = new List<string>();

            foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
            {
                FoodRecipe foodRecipe = context.FoodRecipes.Find(quantityFoodRecipeContract.IdFoodRecipe);
                List<Ingredient> ingredients = context.Ingredients.Where(x => x.IdFoodRecipe == foodRecipe.IdFoodRecipe).ToList();
                foreach (Ingredient ingredient in ingredients)
                {
                    if (ingredient.IdFoodRecipe == quantityFoodRecipeContract.IdFoodRecipe)
                    {
                        Item item = context.Items.Find(ingredient.IdItem);
                        int auxiliary = item.Quantity;
                        auxiliary = auxiliary - quantityFoodRecipeContract.QuantityOfFoodRecipes;
                        if (!(auxiliary >= 0))
                        {
                            errors.Add("Disminuye la cantidad de " + foodRecipe.Name + ". No hay suficientes ingredientes en inventario");
                        }
                    }
                }
            }

            return errors;
        }

        public void ApplyDecreaseNumberOfIngredients(List<QuantityFoodRecipeContract> quantityFoodRecipeContracts)
        {
            foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
            {
                FoodRecipe foodRecipe = context.FoodRecipes.Find(quantityFoodRecipeContract.IdFoodRecipe);
                List<Ingredient> ingredients = context.Ingredients.Where(x => x.IdFoodRecipe == foodRecipe.IdFoodRecipe).ToList();
                foreach (Ingredient ingredient in ingredients)
                {
                    if (ingredient.IdFoodRecipe == quantityFoodRecipeContract.IdFoodRecipe)
                    {
                        Item item = context.Items.Find(ingredient.IdItem);
                        item.Quantity = item.Quantity - quantityFoodRecipeContract.QuantityOfFoodRecipes;
                        context.SaveChanges();
                    }
                }
            }
        }

        public List<string> DecreaseNumberOfItems(List<QuantityItemContract> quantityItemContracts)
        {
            List<string> errors = new List<string>();

            foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
            {
                Item item = context.Items.Find(quantityItemContract.IdItem);
                int auxiliary = item.Quantity;
                auxiliary = auxiliary - quantityItemContract.QuantityOfItems;
                if (!(auxiliary >= 0))
                {
                    errors.Add("Disminuye la cantidad de " + item.Name + ". No hay suficientes ingredientes en inventario");
                }
            }

            return errors;
        }

        public void ApplyDecreaseNumberOfItems(List<QuantityItemContract> quantityItemContracts)
        {
            foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
            {
                Item item = context.Items.Find(quantityItemContract.IdItem);
                item.Quantity = item.Quantity - quantityItemContract.QuantityOfItems;
                context.SaveChanges();
            }
        }

        public void RegisterOrder(OrderContract orderContract, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts,
            List<QuantityItemContract> quantityItemContracts)
        {
            int result = 0;

            Order order = new Order
            {
                Date = orderContract.Date,
                Status = orderContract.Status,
                TotalToPay = orderContract.TotalToPay,
                TableNumber = orderContract.TableNumber,
                TypeOrder = orderContract.TypeOrder,
                IdEmployee = orderContract.IdEmployee,
            };

            if (order.TypeOrder.Equals("Domicilio"))
            {
                order.IdCustomer = orderContract.IdCustomer;
            }

            if (orderContract.Address != null)
            {
                order.IdAddress = orderContract.Address.IdAddresses;
            }

            context.Orders.Add(order);
            result = context.SaveChanges();

            List<string> foodRecipeErrors = DecreaseNumberOfIngredients(quantityFoodRecipeContracts);
            List<string> itemErrors = DecreaseNumberOfItems(quantityItemContracts);

            if ((result > 0) && (foodRecipeErrors.Count == 0) && (itemErrors.Count == 0))
            {
                int orderId = context.Orders.Max(x => x.IdOrder);

                foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
                {
                    context.QuantityFoodRecipes.Add(new QuantityFoodRecipe
                    {
                        IdOrder = orderId,
                        IdFoodRecipe = quantityFoodRecipeContract.IdFoodRecipe,
                        QuantityOfFoodRecipes = quantityFoodRecipeContract.QuantityOfFoodRecipes,
                        Price = quantityFoodRecipeContract.Price,
                        IsDone = false
                    });
                }

                result = context.SaveChanges();

                foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
                {
                    context.QuantityItems.Add(new QuantityItem
                    {
                        IdOrder = orderId,
                        IdItem = quantityItemContract.IdItem,
                        QuantityOfItems = quantityItemContract.QuantityOfItems,
                        Price = quantityItemContract.Price,
                        IsDone = false
                    });
                }

                result = context.SaveChanges();

                ApplyDecreaseNumberOfIngredients(quantityFoodRecipeContracts);
                ApplyDecreaseNumberOfItems(quantityItemContracts);
            }
            else
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().RegisterOrder(
               result, foodRecipeErrors, itemErrors
            );
        }

        public void GetIdEmployeeByName(string fullNameEmployee)
        {
            int idEmployee = context.Employees.FirstOrDefault(x => (x.Name + " " + x.LastName).Equals(fullNameEmployee)).IdUserEmployee;

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetIdEmployeeByName(
               idEmployee
            );
        }

        public void GetOrderList()
        {
            List<Order> orders = context.Orders.OrderBy(x => x.Date).ToList();
            List<OrderContract> orderContracts = new List<OrderContract>();

            List<QuantityFoodRecipe> quantityFoodRecipes = context.QuantityFoodRecipes.ToList();
            List<QuantityFoodRecipeContract> quantityFoodRecipeContracts = new List<QuantityFoodRecipeContract>();

            List<QuantityItem> quantityItems = context.QuantityItems.ToList();
            List<QuantityItemContract> quantityItemContracts = new List<QuantityItemContract>();

            foreach (Order order in orders)
            {
                var customer = context.Customers.Find(order.IdCustomer);
                string customerFullName = "";

                if (customer != null)
                {
                    customerFullName = customer.Name + " " + customer.LastName;
                }
                else
                {
                    customerFullName = "Mesa número " + order.TableNumber;
                }

                Address address = context.Addresses.Find(order.IdAddress);

                OrderContract orderContract = new OrderContract
                {
                    IdOrder = order.IdOrder,
                    Date = order.Date,
                    Status = order.Status,
                    TotalToPay = order.TotalToPay,
                    TableNumber = order.TableNumber,
                    TypeOrder = order.TypeOrder,
                    IdEmployee = order.IdEmployee,
                    CustomerFullName = customerFullName
                };

                if (address != null)
                {
                    orderContract.Address = new AddressContract
                    {
                        Colony = order.Address.Colony,
                        City = order.Address.City,
                        InsideNumber = address.InsideNumber,
                        OutsideNumber = address.OutsideNumber,
                        PostalCode = address.PostalCode,
                        IdCustomer = address.IdCustomer,
                        StreetName = address.StreetName,
                        IdAddresses = address.IdAddresses
                    };
                }

                orderContracts.Add(orderContract);
            }

            foreach (QuantityFoodRecipe quantityFoodRecipe in quantityFoodRecipes)
            {
                string name = (context.FoodRecipes.Find(quantityFoodRecipe.IdFoodRecipe)).Name;
                quantityFoodRecipeContracts.Add(new QuantityFoodRecipeContract
                {
                    IdOrder = quantityFoodRecipe.IdOrder,
                    IdFoodRecipe = quantityFoodRecipe.IdFoodRecipe,
                    QuantityOfFoodRecipes = quantityFoodRecipe.QuantityOfFoodRecipes,
                    Price = quantityFoodRecipe.Price,
                    IdQuantityFoodRecipe = quantityFoodRecipe.IdQuantityFoodRecipe,
                    Name = name
                });
            }

            foreach (QuantityItem quantityItem in quantityItems)
            {
                string name = (context.Items.Find(quantityItem.IdItem)).Name;
                quantityItemContracts.Add(new QuantityItemContract
                {
                    IdItem = quantityItem.IdItem,
                    IdOrder = quantityItem.IdOrder,
                    QuantityOfItems = quantityItem.QuantityOfItems,
                    Price = quantityItem.Price,
                    IdQuantityItem = quantityItem.IdQuantityItem,
                    Name = name
                });
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetOrderList(
                orderContracts, quantityFoodRecipeContracts, quantityItemContracts
            );
        }

        public void DeleteOrderById(int orderId)
        {
            int result = 0;
            Order order = context.Orders.Find(orderId);
            order.Status = "Cancelada";

            List<QuantityFoodRecipe> quantityFoodRecipes = context.QuantityFoodRecipes.Where(x => x.IdOrder == order.IdOrder).ToList();

            List<QuantityItem> quantityItems = context.QuantityItems.Where(x => x.IdOrder == order.IdOrder).ToList();
            List<Item> items = context.Items.ToList();

            foreach (QuantityFoodRecipe quantityFoodRecipe in quantityFoodRecipes)
            {
                FoodRecipe foodRecipe = context.FoodRecipes.Find(quantityFoodRecipe.IdFoodRecipe);
                List<Ingredient> ingredients = context.Ingredients.Where(x => x.IdFoodRecipe == foodRecipe.IdFoodRecipe).ToList();

                foreach (Ingredient ingredient in ingredients)
                {
                    Item item = items.Single(x => x.IdItem == ingredient.IdItem);
                    item.Quantity += int.Parse(ingredient.IngredientQuantity);
                }
            }

            foreach (QuantityItem quantityItem in quantityItems)
            {
                Item item = context.Items.Find(quantityItem.IdItem);
                item.Quantity += quantityItem.QuantityOfItems;
            }

            result = context.SaveChanges();

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().DeleteOrderById(
                result
            );
        }

        public void GetFoodRecipeListToPrepare()
        {
            List<FoodRecipe> foodRecipes = context.FoodRecipes.
                Where(foodRecipe => foodRecipe.IsEnabled && foodRecipe.Status.Equals("Available")).
                OrderBy(foodRecipe => foodRecipe.Name).ToList();
            List<FoodRecipeContract> foodRecipeContracts = new List<FoodRecipeContract>();
            List<Order> ordersToPrepare = context.Orders.Where(x => x.Status.Equals("En proceso")).ToList();
            List<QuantityFoodRecipe> foodRecipesToPrepare = context.QuantityFoodRecipes.Where(x => !x.IsDone).ToList();
            List<int> idFoodRecipesToPrepare = new List<int>();
            List<string> customerFullNames = new List<string>();
            List<int> quantity = new List<int>();

            foreach (Order order in ordersToPrepare)
            {
                foreach (QuantityFoodRecipe foodRecipe in foodRecipesToPrepare)
                {
                    if (foodRecipe.IdOrder == order.IdOrder)
                    {
                        Customer customer = context.Customers.Find(order.IdCustomer);
                        string fullName = "";

                        if (customer != null)
                        {
                            fullName = order.IdOrder + " / " + customer.Name + " " + customer.LastName;
                        }
                        else
                        {
                            fullName = order.IdOrder + " / " + "Número de Mesa" + " " + order.TableNumber;
                        }

                        idFoodRecipesToPrepare.Add(foodRecipe.IdFoodRecipe);
                        customerFullNames.Add(fullName);
                        quantity.Add(foodRecipe.QuantityOfFoodRecipes);
                    }
                }
            }

            List<Ingredient> ingredients = context.Ingredients.ToList();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            int count = 0;

            foreach (int idFoodRecipe in idFoodRecipesToPrepare)
            {
                foreach (FoodRecipe foodRecipe in foodRecipes)
                {
                    if (idFoodRecipe == foodRecipe.IdFoodRecipe)
                    {
                        foodRecipeContracts.Add(new FoodRecipeContract
                        {
                            IdFoodRecipe = foodRecipe.IdFoodRecipe,
                            Price = foodRecipe.Price,
                            Name = quantity[count] + " / " + foodRecipe.Name,
                            Description = foodRecipe.Description,
                            NumberOfServings = foodRecipe.NumberOfServings,
                            Status = foodRecipe.Status,
                            IsEnabled = foodRecipe.IsEnabled,
                            CustomerName = customerFullNames[count]
                        });
                    }
                }
                count++;
            }

            foreach (Ingredient ingredient in ingredients)
            {
                ingredientContracts.Add(new IngredientContract
                {
                    IdFoodRecipe = ingredient.IdFoodRecipe,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetFoodRecipeListToPrepare(
                foodRecipeContracts, ingredientContracts
            );
        }

        public void UpdateOrder(int orderId, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts,
            List<QuantityItemContract> quantityItemContracts)
        {
            List<QuantityFoodRecipe> quantityFoodRecipes = context.QuantityFoodRecipes.ToList();
            List<QuantityItem> quantityItems = context.QuantityItems.ToList();

            int orderQuantityFoodRecipes = context.QuantityFoodRecipes.Where(x => x.IdOrder == orderId).Count();
            int orderQuantityItems = context.QuantityItems.Where(x => x.IdOrder == orderId).Count();

            int newQuantityFoodRecipes = quantityFoodRecipeContracts.Count;
            int newQuantityItems = quantityItemContracts.Count;

            bool newFoodRecipesQuantities = orderQuantityFoodRecipes != newQuantityFoodRecipes;
            bool newItemsQuantities = orderQuantityItems != newQuantityItems;

            foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
            {
                if (quantityFoodRecipeContract.IdOrder == 0)
                {
                    newFoodRecipesQuantities = true;
                }
            }

            foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
            {
                if (quantityItemContract.IdOrder == 0)
                {
                    newItemsQuantities = true;
                }
            }

            int result;

            if (newFoodRecipesQuantities || newItemsQuantities)
            {
                foreach (QuantityFoodRecipe quantityFoodRecipe in quantityFoodRecipes)
                {
                    if (quantityFoodRecipe.IdOrder == orderId)
                    {
                        context.QuantityFoodRecipes.Remove(quantityFoodRecipe);
                    }
                }

                foreach (QuantityItem quantityItem in quantityItems)
                {
                    if (quantityItem.IdOrder == orderId)
                    {
                        context.QuantityItems.Remove(quantityItem);
                    }
                }

                result = context.SaveChanges();

                if (result > 0)
                {
                    Order order = context.Orders.Find(orderId);
                    decimal price = 0;

                    foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
                    {
                        context.QuantityFoodRecipes.Add(new QuantityFoodRecipe
                        {
                            IdOrder = orderId,
                            IdFoodRecipe = quantityFoodRecipeContract.IdFoodRecipe,
                            QuantityOfFoodRecipes = quantityFoodRecipeContract.QuantityOfFoodRecipes,
                            Price = quantityFoodRecipeContract.Price,
                            IsDone = false
                        });

                        price += quantityFoodRecipeContract.Price * quantityFoodRecipeContract.QuantityOfFoodRecipes;
                    }

                    foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
                    {
                        context.QuantityItems.Add(new QuantityItem
                        {
                            IdOrder = orderId,
                            IdItem = quantityItemContract.IdItem,
                            QuantityOfItems = quantityItemContract.QuantityOfItems,
                            Price = quantityItemContract.Price,
                            IsDone = false
                        });

                        price += quantityItemContract.Price * quantityItemContract.QuantityOfItems;
                    }

                    order.TotalToPay = price;

                    result = context.SaveChanges();
                }
            }
            else
            {
                result = -1;
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().UpdateOrder(
               result
            );
        }

        public void MarkRecipeAsDone(int orderId, int foodRecipeId)
        {
            Order order = context.Orders.Find(orderId);
            List<QuantityFoodRecipe> quantityFoodRecipes = context.QuantityFoodRecipes.ToList();

            foreach (QuantityFoodRecipe quantityFoodRecipe in quantityFoodRecipes)
            {
                if ((quantityFoodRecipe.IdFoodRecipe == foodRecipeId) && (order.IdOrder == orderId))
                {
                    quantityFoodRecipe.IsDone = true;
                }
            }

            int result = context.SaveChanges();
            int foodRecipesMade = context.QuantityFoodRecipes.Where(x => x.IdOrder == orderId && x.IsDone).ToList().Count;
            bool foodRecipeMade = foodRecipesMade == context.QuantityFoodRecipes.Where(x => x.IdOrder == orderId).ToList().Count;
            string information = "";

            if (foodRecipeMade)
            {
                order.Status = "Realizado";
                context.SaveChanges();

                information = order.TableNumber != null
                    ? "Pedido ID " + order.IdOrder + " de la Mesa " + order.TableNumber
                    : "Pedido ID " + order.IdOrder + " del Cliente" + context.Customers.Find(order.IdCustomer).Name + " " +
                        context.Customers.Find(order.IdCustomer).LastName;
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().MarkRecipeAsDone(
                result, foodRecipeMade, information
            );
        }






        public bool IdentifyChangesInAddresses(List<AddressContract> addressContracts, int customerId)
        {
            List<Address> addresses = context.Addresses.ToList();
            int customerAddressNumber = context.Addresses.Where(x => x.IdCustomer == customerId).Count();
            int newCustomerAddress = addressContracts.Count;
            bool newAddress = customerAddressNumber == addressContracts.Count ? false : true;

            foreach (AddressContract addressContract in addressContracts)
            {
                if (addressContract.IdAddresses == 0)
                {
                    newAddress = true;
                }
            }

            return newAddress;
        }

        public int RegisterCustomerAddresses(List<AddressContract> addressContracts, int customerId)
        {
            List<Address> addresses = new List<Address>();

            foreach (AddressContract address in addressContracts)
            {
                context.Addresses.Add(new Address
                {
                    Colony = address.Colony,
                    City = address.City,
                    InsideNumber = address.InsideNumber,
                    OutsideNumber = address.OutsideNumber,
                    PostalCode = address.PostalCode,
                    IdCustomer = customerId,
                    StreetName = address.StreetName
                });
            }

            int result = context.SaveChanges();

            return result;
        }

        public void RegisterCustomer(CustomerContract customerContract, List<AddressContract> addresses)
        {
            int result = 0;

            bool isExisting = context.Customers.Where(x => x.Name.Equals(customerContract.Name) &&
                x.LastName.Equals(customerContract.LastName) && x.IsEnabled).Count() > 0;

            Customer customer = new Customer
            {
                Email = customerContract.Email,
                Name = customerContract.Name,
                LastName = customerContract.LastName,
                Phone = customerContract.Phone,
                Status = "Available",
                DateOfBirth = customerContract.DateOfBirth,
                IsEnabled = true,
                IdEmployee = customerContract.IdEmployee
            };

            try
            {
                if (isExisting)
                {
                    result = -1;
                }
                else
                {
                    context.Customers.Add(customer);
                    result = context.SaveChanges();

                    if (result > 0)
                    {
                        int customerId = context.Customers.Max(x => x.IdUserCustomer);
                        result = RegisterCustomerAddresses(addresses, customerId);
                    }
                    else
                    {
                        context.Customers.Remove(customer);
                        context.SaveChanges();
                        result = 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().RegisterCustomer(
               result
            );
        }

        public void DeleteCustomerById(int idCustomer)
        {
            Customer customer = context.Customers.Where(x => x.IdUserCustomer == idCustomer).FirstOrDefault();
            bool haveOrders = context.Orders.Where(order => order.IdCustomer == customer.IdUserCustomer).Count() > 0;
            int result = 0;

            if (customer != null && !haveOrders)
            {
                customer.IsEnabled = false;
                customer.Status = "Deleted";
                result = context.SaveChanges();
            }
            else
            {
                result = -1;
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().DeleteCustomerById(
                    result
            );
        }

        public void GetCustomerListSortedByName()
        {
            List<Customer> customers = context.Customers.
                Where(x => (x.IsEnabled == true) && x.Status.Equals("Available")).
                OrderBy(x => x.Name).ToList();
            List<CustomerContract> customerContracts = new List<CustomerContract>();

            List<Address> addresses = context.Addresses.ToList();
            List<AddressContract> addressContracts = new List<AddressContract>();

            foreach (Customer customer in customers)
            {
                customerContracts.Add(new CustomerContract
                {
                    IdUserCustomer = customer.IdUserCustomer,
                    Email = customer.Email,
                    Name = customer.Name,
                    LastName = customer.LastName,
                    Phone = customer.Phone,
                    Status = customer.Status,
                    DateOfBirth = customer.DateOfBirth,
                    IsEnabled = customer.IsEnabled,
                    IdEmployee = customer.IdEmployee
                });
            }

            foreach (Address address in addresses)
            {
                addressContracts.Add(new AddressContract
                {
                    Colony = address.Colony,
                    City = address.City,
                    InsideNumber = address.InsideNumber,
                    OutsideNumber = address.OutsideNumber,
                    PostalCode = address.PostalCode,
                    IdCustomer = address.IdCustomer,
                    StreetName = address.StreetName,
                    IdAddresses = address.IdAddresses
                });
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetCustomerListSortedByName(
                customerContracts, addressContracts
            );
        }

        public int UpdateCustomerAddresses(int idCustomer)
        {
            int result = 0;

            List<Address> addresses = context.Addresses.ToList();

            foreach (Address address in addresses)
            {
                if (address.IdCustomer == idCustomer)
                {
                    context.Addresses.Remove(address);
                    result = context.SaveChanges();
                }
            }

            return result;
        }

        public void UpdateCustomer(CustomerContract customerContract, List<AddressContract> addressContracts)
        {
            Customer customer = context.Customers.Where(x => x.IdUserCustomer == customerContract.IdUserCustomer).FirstOrDefault();

            int result = 0;
            bool isExisting = context.Customers.Where(x => x.Name.Equals(customerContract.Name) &&
                    x.LastName.Equals(customerContract.LastName) && x.IsEnabled &&
                        x.IdUserCustomer != customer.IdUserCustomer).Count() > 0;
            bool newAddress = IdentifyChangesInAddresses(addressContracts, customer.IdUserCustomer);

            if (isExisting)
            {
                result = -1;
            }
            else
            {
                if (customer != null)
                {
                    customer.Email = customerContract.Email;
                    customer.Name = customerContract.Name;
                    customer.LastName = customerContract.LastName;
                    customer.Phone = customerContract.Phone;
                    customer.Status = customerContract.Status;
                    customer.DateOfBirth = customerContract.DateOfBirth;
                    customer.IsEnabled = customerContract.IsEnabled;

                    result = context.SaveChanges();
                    context.SaveChanges();
                }

                if ((result > 0) || newAddress)
                {
                    result = UpdateCustomerAddresses(customerContract.IdUserCustomer);

                    if (result > 0)
                    {
                        result = RegisterCustomerAddresses(addressContracts, customerContract.IdUserCustomer);
                    }
                }
            }

            OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().UpdateCustomer(
                result
            );
        }

        #endregion

    }
}