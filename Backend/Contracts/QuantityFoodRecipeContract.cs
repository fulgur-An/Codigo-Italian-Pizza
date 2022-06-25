
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class QuantityFoodRecipeContract
    {
        [DataMember]
        public int IdOrder { get; set; }
        [DataMember]
        public int IdFoodRecipe { get; set; }
        [DataMember]
        public int QuantityOfFoodRecipes { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int IdQuantityFoodRecipe { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}