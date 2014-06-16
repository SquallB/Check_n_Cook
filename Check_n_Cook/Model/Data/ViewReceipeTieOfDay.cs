using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class ViewReceipeTimeOfDay
    {
        public ViewReceipeTimeOfDay(string title, List<string> imgs)
        {
            this.Title = title;
            this.ImagePaths = imgs;
        }

        public ViewReceipeTimeOfDay(Receipe receipe) : this("", new List<string>()) { }

        public string Title { get; private set; }
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public string ImagePath1 { get { if (ImagePaths.Count >= 1 && ImagePaths[0]!= null) { return ImagePaths[0]; } else { return null; } } }
        public string ImagePath2 { get { if (ImagePaths.Count >= 2 && ImagePaths[1]!= null) { return ImagePaths[1]; } else { return null; } } }
        public string ImagePath3 { get { if (ImagePaths.Count >= 3 && ImagePaths[2]!= null) { return ImagePaths[2]; } else { return null; } } }
        public List<string> ImagePaths { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
