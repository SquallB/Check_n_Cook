using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class ItemIngredient : Item
    {
        public Ingredient Ingredient { get; set; }
        public string Group { get; set; }

        public ItemIngredient()
        {
                
        }

        public ItemIngredient(Ingredient ing)
        {
            this.Ingredient = ing;
        }
    }
}
