using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class Event
    {
        public AbstractModel Model { get; set; }

        public Event() : this(null) { }

        public Event(AbstractModel model)
        {
            this.Model = model;
        }
    }
}
