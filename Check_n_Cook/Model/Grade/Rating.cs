using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Grade
{
    public class Rating : Grade
    {
        public Rating(int value = 1) : base(1,5)
        {
            this.Value = value;
        }
    }
}
