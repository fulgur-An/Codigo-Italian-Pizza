using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class IngredientContract
    {
        [DataMember]
        public int IdIngredient { get; set; }
        [DataMember]
        public int IdFoodRecipe { get; set; }
        [DataMember]
        public int IdItem { get; set; }
        [DataMember]
        public string IngredientName { get; set; }
        [DataMember]
        public string IngredientQuantity { get; set; }
        [DataMember]
        public byte[] IngredientPhoto { get; set; }
    }
}