using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a receipe is added
    /// </summary>
    public class AddedReceipeEvent : ReceipeEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddedReceipeEvent"/> class.
        /// </summary>
        public AddedReceipeEvent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddedReceipeEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="receipe">The receipe.</param>
        public AddedReceipeEvent(AbstractModel model, Receipe receipe) : base(model, receipe) { }
    }
}
