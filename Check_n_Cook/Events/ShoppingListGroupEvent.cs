using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class ShoppingListGroupEvent : Event
    {
        public String GroupName { get; set; }

        public ShoppingListGroupEvent() : this(null, "") { }

        public ShoppingListGroupEvent(AbstractModel model, String groupName) : base(model)
        {
            this.GroupName = groupName;
        }
    }
}
