using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a ShoppingListGroup is removed
    /// </summary>
    public class RemovedShoppingListGroupEvent : ShoppingListGroupEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedShoppingListGroupEvent"/> class.
        /// </summary>
        public RemovedShoppingListGroupEvent() : this(null, "") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedShoppingListGroupEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="groupName">Name of the group.</param>
        public RemovedShoppingListGroupEvent(AbstractModel model, String groupName) : base(model, groupName) { }
    }
}
