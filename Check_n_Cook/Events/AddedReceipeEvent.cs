using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class AddedReceipeEvent : ReceipeEvent
    {
        public AddedReceipeEvent() { }

        public AddedReceipeEvent(AbstractModel model, Receipe receipe) : base(model, receipe) { }
    }
}
