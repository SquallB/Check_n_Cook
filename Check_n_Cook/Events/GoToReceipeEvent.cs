using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an abstract event when the user go to a page that contains receipes
    /// </summary>
    public abstract class GoToReceipeEvent
    {
        /// <summary>
        /// Gets or sets the application model.
        /// </summary>
        /// <value>
        /// The application model.
        /// </value>
        public AppModel AppModel { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public Time Time { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoToReceipeEvent"/> class.
        /// </summary>
        public GoToReceipeEvent()
        {
            this.AppModel = new AppModel();
            this.Time = new Time();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoToReceipeEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="time">The time.</param>
        public GoToReceipeEvent(AppModel model, Time time)
        {
            this.Time = time;
            this.AppModel = model;
        }
    }
}
