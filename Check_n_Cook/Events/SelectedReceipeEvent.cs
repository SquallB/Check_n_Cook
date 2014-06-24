using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a receipe is selected
    /// </summary>
    public class SelectedReceipeEvent : ReceipeEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedReceipeEvent"/> class.
        /// </summary>
        public SelectedReceipeEvent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedReceipeEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="receipe">The receipe.</param>
        public SelectedReceipeEvent(AbstractModel model, Receipe receipe) : base(model, receipe) { }
    }
}
