using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents an item
    /// </summary>
    public abstract class Item
    {
        /// <summary>
        /// The name
        /// </summary>
        private string name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The description
        /// </summary>
        private string description;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// The image
        /// </summary>
        private string image;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

    }
}
