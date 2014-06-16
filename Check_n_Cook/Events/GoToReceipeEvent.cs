using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public abstract class GoToReceipeEvent
    {
        public AppModel AppModel { get; set; }
        public Time Time { get; set; }

        public GoToReceipeEvent()
        {
            this.AppModel = new AppModel();
            this.Time = new Time();
        }

        public GoToReceipeEvent(AppModel model, Time time)
        {
            this.Time = time;
            this.AppModel = model;
        }
    }
}
