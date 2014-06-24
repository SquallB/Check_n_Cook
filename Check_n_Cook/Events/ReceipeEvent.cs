using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an receipe event
    /// </summary>
    public class ReceipeEvent : Event
    {
        /// <summary>
        /// Gets or sets the receipe.
        /// </summary>
        /// <value>
        /// The receipe.
        /// </value>
        public Receipe Receipe { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeEvent"/> class.
        /// </summary>
        public ReceipeEvent() : this(null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="receipe">The receipe.</param>
        public ReceipeEvent(AbstractModel model, Receipe receipe) : base(model)
        {
            this.Receipe = receipe;
        }
    }
}
