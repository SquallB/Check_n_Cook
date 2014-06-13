using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class ShopEvent : Event
    {
        public Shop Shop { get; set; }

        public ShopEvent() : this(null, null) { }

        public ShopEvent(AbstractModel model, Shop shop) : base(model)
        {
            this.Shop = shop;
        }
    }
}
