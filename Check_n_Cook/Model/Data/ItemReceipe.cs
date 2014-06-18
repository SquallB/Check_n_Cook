using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class ItemReceipe
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public Receipe Receipe { get; set; }

        public ItemReceipe(string image, string name, string description)
        {
            this.Image = image;
            this.Name = name;
            this.Description = description;
        }

        public ItemReceipe( Receipe receipe)
        {
            this.Receipe = receipe;
        }
    }
}
