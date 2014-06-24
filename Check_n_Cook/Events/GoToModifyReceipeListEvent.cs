using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when the user go to the page for modify a receipe list
    /// </summary>
    public class GoToModifyReceipeListEvent : GoToReceipeEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoToModifyReceipeListEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="time">The time.</param>
        public GoToModifyReceipeListEvent(AppModel model, Time time) : base(model, time)
        {
        }
    }
}
