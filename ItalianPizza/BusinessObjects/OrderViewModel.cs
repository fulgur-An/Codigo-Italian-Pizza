using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianPizza.BusinessObjects
{
    public class OrderViewModel
    {
        public string TypeOfDelivery { get; set; }

        public string Address { get; set; }

        public string CustomerName { get; set; }

        public string FoodRecipeName { get; set; }

        public string ItemName { get; set; }
    }
}