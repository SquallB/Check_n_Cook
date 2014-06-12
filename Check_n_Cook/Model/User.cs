using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class User
    {
        public String Name { get; set; }

        public User() : this("") { }

        public User(String name)
        {
            this.Name = name;
        }
    }
}
