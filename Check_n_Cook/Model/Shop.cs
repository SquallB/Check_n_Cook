using Bing.Maps.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class Shop
    {
        public String Name { get; set; }

        public String Address { get; set; }

        public Shop() : this("", "") { }

        public Shop(String name, String address)
        {
            this.Name = name;
            this.Address = address;
        }
    }
}
