using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents the receipe time of day item
    /// </summary>
    public class ItemReceipeTimeOfDay
    {
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public Time Time { get; set; }
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
        /// Initializes a new instance of the <see cref="ItemReceipeTimeOfDay"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="imgs">The imgs.</param>
        /// <param name="timeOfDay">The time of day.</param>
        public ItemReceipeTimeOfDay(string date, List<string> imgs, string timeOfDay)
        {
            this.ImagePaths = imgs;
            this.Time = new Time(date, timeOfDay);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemReceipeTimeOfDay"/> class.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public ItemReceipeTimeOfDay(Receipe receipe) : this("", new List<string>(), "") { }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Time.Date;
        }
    }
}
