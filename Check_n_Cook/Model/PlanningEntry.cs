using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents an entry in the planning of receipes.
    /// </summary>
    public class PlanningEntry
    {
        /// <summary>
        /// Gets or sets the planning date.
        /// </summary>
        /// <value>
        /// The planning date.
        /// </value>
        public DateTime PlanningDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the planning.
        /// </summary>
        /// <value>
        /// The type of the planning.
        /// </value>
        public PlanningType PlanningType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningEntry"/> class.
        /// </summary>
        public PlanningEntry()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningEntry"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="plantype">The type of the planning.</param>
        public PlanningEntry(DateTime date, PlanningType plantype)
        {
            this.PlanningDate = date;
            this.PlanningType = plantype;
        }
    }
}
