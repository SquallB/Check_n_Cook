using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a ShoppingListGroup is added
    /// </summary>
    public class AddedShoppingListGroupEvent : ShoppingListGroupEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddedShoppingListGroupEvent"/> class.
        /// </summary>
        public AddedShoppingListGroupEvent() : this(null, "") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddedShoppingListGroupEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="groupName">Name of the group.</param>
        public AddedShoppingListGroupEvent(AbstractModel model, String groupName) : base(model, groupName) { }
    }
}
