using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianPizza.BusinessObjects
{
    public class AddressViewModel
    {
        public string Street { get; set; }
        public string OutsideNumber { get; set; }
        public string InsideNumber { get; set; }
        public string Colony { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}