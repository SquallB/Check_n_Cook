using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents an ingredient item
    /// </summary>
    public class ItemIngredient : Item
    {
        /// <summary>
        /// Gets or sets the ingredient.
        /// </summary>
        /// <value>
        /// The ingredient.
        /// </value>
        public Ingredient Ingredient { get; set; }
        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public string Group { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemIngredient"/> class.
        /// </summary>
        public ItemIngredient()
        {
                
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemIngredient"/> class.
        /// </summary>
        /// <param name="ing">The ing.</param>
        public ItemIngredient(Ingredient ing)
        {
            this.Ingredient = ing;
        }
    }
}
