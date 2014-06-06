using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class ReceipeEvent : Event
    {
        public Receipe Receipe { get; set; }

        public ReceipeEvent() : this(null, null) { }

        public ReceipeEvent(AbstractModel model, Receipe receipe) : base(model)
        {
            this.Receipe = receipe;
        }
    }
}
