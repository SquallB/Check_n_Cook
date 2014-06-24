using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents a group of the shopping list, contaning ingrediens.
    /// </summary>
    public class ShoppingListGroup
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the items (ingredients).
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<ItemIngredient> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingListGroup"/> class.
        /// </summary>
        public ShoppingListGroup() : this("") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingListGroup"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ShoppingListGroup(String name)
        {
            this.Name = name;
            this.Items = new List<ItemIngredient>();
        }
    }
}
