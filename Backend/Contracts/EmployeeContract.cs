
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class EmployeeContract
    {
        [DataMember]
        public int IdUserEmployee { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime AdmissionDate { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Role { get; set; }
        [DataMember]
        public decimal Salary { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string TimeOfEntry { get; set; }
        [DataMember]
        public string DepartureTime { get; set; }
        [DataMember]
        public string Workshift { get; set; }
    }
}