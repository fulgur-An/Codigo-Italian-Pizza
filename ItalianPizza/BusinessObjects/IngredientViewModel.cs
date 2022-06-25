using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianPizza.BusinessObjects
{
    public class IngredientViewModel
    {
        public int IdIngredient { get; set; }
        public int IdFoodRecipe { get; set; }
        public int IdItem { get; set; }
        public string IngredientName { get; set; }
        public string IngredientQuantity { get; set; }
        public byte[] IngredientPhoto { get; set; }
    }
}