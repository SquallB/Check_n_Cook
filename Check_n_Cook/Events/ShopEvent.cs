using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    /// <summary>
    /// This is a class that represents an shop event
    /// </summary>
    public class ShopEvent : Event
    {
        /// <summary>
        /// Gets or sets the shop.
        /// </summary>
        /// <value>
        /// The shop.
        /// </value>
        public Shop Shop { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopEvent"/> class.
        /// </summary>
        public ShopEvent() : this(null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopEvent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="shop">The shop.</param>
        public ShopEvent(AbstractModel model, Shop shop) : base(model)
        {
            this.Shop = shop;
        }
    }
}
