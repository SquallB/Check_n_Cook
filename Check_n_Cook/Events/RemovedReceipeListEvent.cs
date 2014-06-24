using Check_n_Cook.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// This is a class that represents an event when a receipe list is removed
    /// </summary>
    public class RemovedReceipeListEvent : ReceipeEvent
    {
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public Time Time { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedReceipeListEvent"/> class.
        /// </summary>
        public RemovedReceipeListEvent()
        {
            this.Time = new Time();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedReceipeListEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="receipe">The receipe.</param>
        /// <param name="time">The time.</param>
        public RemovedReceipeListEvent(AbstractModel model, Receipe receipe, Time time)
            : base(model, receipe)
        {
            this.Time = time;
        }
    }
}
