using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class Time
    {
        public string Date { get; set; }
        public string TimeOfDay { get; set; }

        public Time()
        {
                
        }

        public Time(string date, string timeOfDay)
        {
            this.Date = date;
            this.TimeOfDay = timeOfDay;
        }
    }
}
