using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents a time for a receipe.
    /// </summary>
    public abstract class ReceipeTime
    {
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public Time Time { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeTime"/> class.
        /// </summary>
        public ReceipeTime()
        {
            this.Time = new Time();
        }
    }
}
