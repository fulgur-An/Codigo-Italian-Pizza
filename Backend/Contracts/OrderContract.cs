using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class OrderContract
    {
        [DataMember]
        public int IdOrder { get; set; }
        [DataMember]
        public System.DateTime Date { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public decimal TotalToPay { get; set; }
        [DataMember]
        public Nullable<int> TableNumber { get; set; }
        [DataMember]
        public int IdCustomer { get; set; }
        [DataMember]
        public string TypeOrder { get; set; }
        [DataMember]
        public int IdEmployee { get; set; }

        [DataMember]
        public string CustomerFullName { get; set; }

        [DataMember]
        public AddressContract Address { get; set; }
    }
}