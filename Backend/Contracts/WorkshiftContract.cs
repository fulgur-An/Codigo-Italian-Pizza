using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class WorkshiftContract
    {
        [DataMember]
        public int IdUserEmployee { get; set; }
        [DataMember]
        public string TimeOfEntry { get; set; }
        [DataMember]
        public string DepartureTime { get; set; }
        [DataMember]
        public string Time { get; set; }
    }
}