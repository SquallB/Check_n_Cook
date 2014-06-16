using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Grade
{
    public class Difficulty : Grade
    {
        public Difficulty(int value = 1) : base(1, 4, value) { }

        public Difficulty(String jsonString) : base(jsonString) { }
    }
}
