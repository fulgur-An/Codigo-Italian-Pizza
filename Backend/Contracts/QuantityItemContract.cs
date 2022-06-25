using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class QuantityItemContract
    {
        [DataMember]
        public int IdItem { get; set; }
        [DataMember]
        public int IdOrder { get; set; }
        [DataMember]
        public int QuantityOfItems { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int IdQuantityItem { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}