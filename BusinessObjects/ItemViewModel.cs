using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianPizza.BusinessObjects
{
    public class ItemViewModel
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Sku { get; set; }

        public string Price { get; set; }

        public string Quantity { get; set; }

        public string Restrictions { get; set; }

        public string UnitOfMeasurement { get; set; }

    }
}
