using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class RemovedIngredientEvent : IngredientEvent
    {
        public RemovedIngredientEvent() : this(null, null, "") { }

        public RemovedIngredientEvent(AbstractModel model, Ingredient ingredient, String groupName) : base(model, ingredient, groupName) { }
    }
}
