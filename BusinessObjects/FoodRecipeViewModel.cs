using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianPizza.BusinessObjects
{
   public class FoodRecipeViewModel
   {
        public string Name { get; set; }
        public string Portions { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string IngredientName { get; set; }
        public string QuantityItem { get; set; }
    }
}
