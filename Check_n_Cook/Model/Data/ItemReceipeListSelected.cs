using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents an item for the receipelist selected
    /// </summary>
    public class ItemReceipeListSelected
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets the image path1.
        /// </summary>
        /// <value>
        /// The image path1.
        /// </value>
        public string ImagePath1 { get { if (ImagePaths.Count >= 1 && ImagePaths[0] != null) { return ImagePaths[0]; } else { return null; } } }
        /// <summary>
        /// Gets the image path2.
        /// </summary>
        /// <value>
        /// The image path2.
        /// </value>
        public string ImagePath2 { get { if (ImagePaths.Count >= 2 && ImagePaths[1] != null) { return ImagePaths[1]; } else { return null; } } }
        /// <summary>
        /// Gets the image path3.
        /// </summary>
        /// <value>
        /// The image path3.
        /// </value>
        public string ImagePath3 { get { if (ImagePaths.Count >= 3 && ImagePaths[2] != null) { return ImagePaths[2]; } else { return null; } } }
        /// <summary>
        /// Gets the image path4.
        /// </summary>
        /// <value>
        /// The image path4.
        /// </value>
        public string ImagePath4 { get { if (ImagePaths.Count >= 4 && ImagePaths[3] != null) { return ImagePaths[3]; } else { return null; } } }
        /// <summary>
        /// Gets or sets the image paths.
        /// </summary>
        /// <value>
        /// The image paths.
        /// </value>
        public List<string> ImagePaths { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemReceipeListSelected"/> class.
        /// </summary>
        public ItemReceipeListSelected()
        {
            this.ImagePaths = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemReceipeListSelected"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="list">The list.</param>
        public ItemReceipeListSelected(string title, List<string> list)
        {
            this.Title = title;
            this.ImagePaths = list;
        }
    }
}
