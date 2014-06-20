using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class ModifyReceipeListPrint : Event
    {
        public ModifyReceipeListPrint()
        {
                
        }
        public ModifyReceipeListPrint(PrintReceipe model) : base(model)
        {
                
        }
    }
}
