using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an event when a shop is added
    /// </summary>
    public class AddedShopEvent : ShopEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddedShopEvent"/> class.
        /// </summary>
        public AddedShopEvent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddedShopEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="shop">The shop.</param>
        public AddedShopEvent(AbstractModel model, Shop shop) : base(model, shop) { }
    }
}
