using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents a time, with a date and time of day.
    /// </summary>
    public class Time
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the time of day.
        /// </summary>
        /// <value>
        /// The time of day.
        /// </value>
        public string TimeOfDay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        public Time()
        {
                
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="timeOfDay">The time of day.</param>
        public Time(string date, string timeOfDay)
        {
            this.Date = date;
            this.TimeOfDay = timeOfDay;
        }
    }
}
