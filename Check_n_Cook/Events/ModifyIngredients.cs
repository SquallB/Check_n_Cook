using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when an ingredient is modified
    /// </summary>
    public class ModifyIngredients : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyIngredients"/> class.
        /// </summary>
        public ModifyIngredients()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyIngredients"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ModifyIngredients(NewReceipe model) : base(model)
        {
                
        }
    }
}
