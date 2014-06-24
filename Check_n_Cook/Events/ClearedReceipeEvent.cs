using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a receipe is cleared
    /// </summary>
    public class ClearedReceipeEvent : ReceipeEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearedReceipeEvent"/> class.
        /// </summary>
        public ClearedReceipeEvent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearedReceipeEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ClearedReceipeEvent(AbstractModel model) : base(model, null) { }
    }
}
