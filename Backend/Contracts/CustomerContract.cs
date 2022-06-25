
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class CustomerContract
    {
        [DataMember]
        public int IdUserCustomer { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public System.DateTime DateOfBirth { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public int IdEmployee { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }
}