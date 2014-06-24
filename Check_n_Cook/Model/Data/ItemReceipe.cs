using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents an receipe item
    /// </summary>
    public class ItemReceipe : Item
    {
        /// <summary>
        /// Gets or sets the receipe.
        /// </summary>
        /// <value>
        /// The receipe.
        /// </value>
        public Receipe Receipe { get; set; }

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author { get { if (Receipe.Author == String.Empty || Receipe.Author == null) { return "-"; } else { return Receipe.Author; } } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemReceipe"/> class.
        /// </summary>
        public ItemReceipe()
        {
                
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemReceipe"/> class.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public ItemReceipe(Receipe receipe)
        {
            this.Receipe = receipe;
        }

    }
}
