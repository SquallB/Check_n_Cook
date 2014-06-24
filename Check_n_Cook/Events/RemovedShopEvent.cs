using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a shop is removed
    /// </summary>
    public class RemovedShopEvent : ShopEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedShopEvent"/> class.
        /// </summary>
        public RemovedShopEvent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedShopEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="shop">The shop.</param>
        public RemovedShopEvent(AbstractModel model, Shop shop) : base(model, shop) { }
    }
}
