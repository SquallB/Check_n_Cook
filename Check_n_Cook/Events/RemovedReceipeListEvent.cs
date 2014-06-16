using Check_n_Cook.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Check_n_Cook.Model
{
    public class RemovedReceipeListEvent : ReceipeEvent
    {
        public Time Time { get; set; }
        public RemovedReceipeListEvent()
        {
            this.Time = new Time();
        }

        public RemovedReceipeListEvent(AbstractModel model, Receipe receipe, Time time)
            : base(model, receipe)
        {
            this.Time = time;
        }
    }
}
