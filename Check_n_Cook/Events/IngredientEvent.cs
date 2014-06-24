using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an ingredient event
    /// </summary>
    public class IngredientEvent : Event
    {
        /// <summary>
        /// Gets or sets the ingredient.
        /// </summary>
        /// <value>
        /// The ingredient.
        /// </value>
        public Ingredient Ingredient { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public String GroupName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngredientEvent"/> class.
        /// </summary>
        public IngredientEvent() : this(null, null, "") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngredientEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="ingredient">The ingredient.</param>
        /// <param name="groupName">Name of the group.</param>
        public IngredientEvent(AbstractModel model, Ingredient ingredient, String groupName) : base(model)
        {
            this.Ingredient = ingredient;
            this.GroupName = groupName;
        }
    }
}
