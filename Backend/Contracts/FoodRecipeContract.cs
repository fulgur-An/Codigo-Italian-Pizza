using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Contracts
{
    [DataContract]
    public class FoodRecipeContract
    {
        [DataMember]
        public int IdFoodRecipe { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int NumberOfServings { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public string CustomerName { get; set; }
    }
}