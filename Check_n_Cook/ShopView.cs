using Bing.Maps;
using Bing.Maps.Search;
using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Check_n_Cook
{
    public class ShopView
    {
        public Shop Shop { get; set; }

        public GeocodeLocation Location { get; set; }

        public CustomPushPin Pin { get; set; }

        public ShopView() {

        }

        public ShopView(Shop shop, GeocodeLocation location, Style style)
        {
            this.Shop = shop;
            this.Pin = new CustomPushPin(location.Location);
            Pin.Pin.Style = style;
            ToolTipService.SetToolTip(Pin.Pin, String.Format("{0}\r\n{1}", shop.Name, shop.Address));
        }
    }
}
