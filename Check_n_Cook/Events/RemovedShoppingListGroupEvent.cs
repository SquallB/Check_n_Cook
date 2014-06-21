using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class RemovedShoppingListGroupEvent : ShoppingListGroupEvent
    {
        public RemovedShoppingListGroupEvent() : this(null, "") { }

        public RemovedShoppingListGroupEvent(AbstractModel model, String groupName) : base(model, groupName) { }
    }
}
