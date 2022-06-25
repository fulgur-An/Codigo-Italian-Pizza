
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class ProviderContract
    {
        [DataMember]
        public int IdProvider { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public String Email { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string RFC { get; set; }
        //  [DataMember]
        //  public String Status { get; set; }
    }
}