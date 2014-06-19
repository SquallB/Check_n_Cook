using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class ModifyIngredients : Event
    {
        public ModifyIngredients()
        {
        }

        public ModifyIngredients(NewReceipe model) : base(model)
        {
                
        }
    }
}
