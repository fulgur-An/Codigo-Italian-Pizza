
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class AddressContract
    {
        [DataMember]
        public string Colony { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string InsideNumber { get; set; }
        [DataMember]
        public string OutsideNumber { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public int IdCustomer { get; set; }
        [DataMember]
        public string StreetName { get; set; }
        [DataMember]
        public int IdAddresses { get; set; }
    }
}