
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents an item for the AboutCrew page
    /// </summary>
    public class ItemAbout
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>
        /// The subtitle.
        /// </value>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemAbout"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="title">The title.</param>
        /// <param name="subTitle">The sub title.</param>
        /// <param name="content">The content.</param>
        public ItemAbout(string image, string title, string subTitle, string content)
        {
            this.Image = image;
            this.Title = title;
            this.Subtitle = subTitle;
            this.Content = content;
        }
    }
}