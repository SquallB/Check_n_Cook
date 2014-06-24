using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when the user go to the page that displays a receipe list
    /// </summary>
    public class GoToReceipeListEvent : GoToReceipeEvent
    {
        /// <summary>
        /// Gets or sets the receipe time.
        /// </summary>
        /// <value>
        /// The receipe time.
        /// </value>
        public ReceipeTime ReceipeTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoToReceipeListEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="time">The time.</param>
        /// <param name="receipeTime">The receipe time.</param>
        public GoToReceipeListEvent(AppModel model, Time time, ReceipeTime receipeTime)
            : base(model, time)
        {
            this.ReceipeTime = receipeTime;
        }
    }
}
