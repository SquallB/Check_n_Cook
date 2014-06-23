using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class ShoppingListGroup
    {
        public String Name { get; set; }

        public List<Ingredient> Items { get; set; }

        public ShoppingListGroup() : this("") { }

        public ShoppingListGroup(String name)
        {
            this.Name = name;
            this.Items = new List<Ingredient>();
        }
    }
}
