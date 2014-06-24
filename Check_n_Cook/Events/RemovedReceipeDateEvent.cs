using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a ReceipeDate is removed
    /// </summary>
    public class RemovedReceipeDateEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedReceipeDateEvent"/> class.
        /// </summary>
        public RemovedReceipeDateEvent()
        {
                
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedReceipeDateEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public RemovedReceipeDateEvent(AppModel model) 
            : base(model)
        {
                
        }
    }
}
