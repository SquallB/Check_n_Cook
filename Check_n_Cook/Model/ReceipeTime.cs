using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public abstract class ReceipeTime
    {
        public Time Time { get; set; }

        public ReceipeTime()
        {
            this.Time = new Time();
        }
    }
}
