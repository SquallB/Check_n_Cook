
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class ItemFavorite
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public ItemFavorite(string name, string image)
        {
            this.Name = name;
            this.Image = image;
        }
    }
}
