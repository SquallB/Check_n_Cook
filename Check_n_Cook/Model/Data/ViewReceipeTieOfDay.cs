﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class ViewReceipeTimeOfDay
    {
        public Time Time { get; set; }
        public string ImagePath1 { get { if (ImagePaths.Count >= 1 && ImagePaths[0] != null) { return ImagePaths[0]; } else { return null; } } }
        public string ImagePath2 { get { if (ImagePaths.Count >= 2 && ImagePaths[1] != null) { return ImagePaths[1]; } else { return null; } } }
        public string ImagePath3 { get { if (ImagePaths.Count >= 3 && ImagePaths[2] != null) { return ImagePaths[2]; } else { return null; } } }
        public string ImagePath4 { get { if (ImagePaths.Count >= 4 && ImagePaths[3] != null) { return ImagePaths[3]; } else { return null; } } }
        public List<string> ImagePaths { get; set; }

        public ViewReceipeTimeOfDay(string date, List<string> imgs, string timeOfDay)
        {
            this.ImagePaths = imgs;
            this.Time = new Time(date, timeOfDay);
        }

        public ViewReceipeTimeOfDay(Receipe receipe) : this("", new List<string>(), "") { }


        public override string ToString()
        {
            return this.Time.Date;
        }
    }
}
