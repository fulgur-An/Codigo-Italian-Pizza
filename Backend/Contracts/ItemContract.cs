using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class ItemContract
    {
        [DataMember]
        public int IdItem { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Sku { get; set; }
        [DataMember]
        public byte[] Photo { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public string QuantityWhithUnit { get; set; }
        [DataMember]
        public string Restrictions { get; set; }
        [DataMember]
        public string UnitOfMeasurement { get; set; }
        [DataMember]
        public bool NeedsFoodRecipe { get; set; }
        [DataMember]
        public bool IsIngredient { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public string LastStockTaking { get; set; }


    }
}