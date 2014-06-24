using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when an ingredient is removed
    /// </summary>
    public class RemovedIngredientEvent : IngredientEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedIngredientEvent"/> class.
        /// </summary>
        public RemovedIngredientEvent() : this(null, null, "") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedIngredientEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="ingredient">The ingredient.</param>
        /// <param name="groupName">Name of the group.</param>
        public RemovedIngredientEvent(AbstractModel model, Ingredient ingredient, String groupName) : base(model, ingredient, groupName) { }
    }
}
