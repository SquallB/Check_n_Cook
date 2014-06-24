using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a receipe list is modified
    /// </summary>
    public class ModifyReceipeListPrint : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyReceipeListPrint"/> class.
        /// </summary>
        public ModifyReceipeListPrint()
        {
                
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyReceipeListPrint"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ModifyReceipeListPrint(ReceipeListSelected model) : base(model)
        {
                
        }
    }
}
