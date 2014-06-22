using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class ItemReceipe : Item
    {
        public Receipe Receipe { get; set; }

        public string Author { get { if (Receipe.Author == String.Empty || Receipe.Author == null) { return "-"; } else { return Receipe.Author; } } }

        public ItemReceipe()
        {
                
        }

        public ItemReceipe(Receipe receipe)
        {
            this.Receipe = receipe;
        }

    }
}
