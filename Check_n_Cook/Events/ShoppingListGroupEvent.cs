using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an ShoppingListGroup event
    /// </summary>
    public class ShoppingListGroupEvent : Event
    {
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public String GroupName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingListGroupEvent"/> class.
        /// </summary>
        public ShoppingListGroupEvent() : this(null, "") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingListGroupEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="groupName">Name of the group.</param>
        public ShoppingListGroupEvent(AbstractModel model, String groupName) : base(model)
        {
            this.GroupName = groupName;
        }
    }
}
