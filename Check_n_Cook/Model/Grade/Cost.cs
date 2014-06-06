using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Grade
{
    public class Cost : Grade
    {
        public Cost(int value) : base(1, 3)
        {
            this.Value = value;
        }
    }
}
