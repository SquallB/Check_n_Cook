
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class ItemAbout
    {
        public string Image { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public ItemAbout(string image, string title, string subTitle)
        {
            this.Image = image;
            this.Title = title;
            this.Subtitle = subTitle;
        }
    }
}