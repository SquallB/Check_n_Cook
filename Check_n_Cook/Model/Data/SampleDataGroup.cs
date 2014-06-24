using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents a group of ItemReceipeTimeOfDay
    /// </summary>
    public class SampleDataGroup
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<ItemReceipeTimeOfDay> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDataGroup"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public SampleDataGroup(String title)
        {
            this.Title = title;
            this.Items = new List<ItemReceipeTimeOfDay>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Title;
        }
    }
}
