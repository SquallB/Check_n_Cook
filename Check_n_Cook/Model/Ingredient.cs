using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class Ingredient
    {
        public String name { get; set; }
        public String quantity { get; set; }

        public Unit unity { get; set; }

        public Ingredient()
        {

        }
        public Ingredient(String name, String qty, Unit unity)
        {
            this.name = name;
            this.quantity = qty;
            this.unity = unity;

        }

    }
}
