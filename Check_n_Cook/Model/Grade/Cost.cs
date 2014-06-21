using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Grade
{
    public class Cost : Grade
    {
        public Cost(int value = 1) : base(1, 3, value) { }

        public Cost(String jsonString) : base(jsonString) { }

        public override string ToString()
        {
            return "Coût : " + GetGrade().ToString() + "/" + GetMax().ToString();
        }
    }
}
