using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class GoToReceipeListEvent : GoToReceipeEvent
    {
        public ReceipeTime ReceipeTime { get; set; }

        public GoToReceipeListEvent(AppModel model, Time time, ReceipeTime receipeTime)
            : base(model, time)
        {
            this.ReceipeTime = receipeTime;
        }
    }
}
