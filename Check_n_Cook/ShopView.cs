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
    /// <summary>
    /// 
    /// </summary>
    public class ShopView
    {
        /// <summary>
        /// Gets or sets the shop.
        /// </summary>
        /// <value>
        /// The shop.
        /// </value>
        public Shop Shop { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public GeocodeLocation Location { get; set; }

        /// <summary>
        /// Gets or sets the pin.
        /// </summary>
        /// <value>
        /// The pin.
        /// </value>
        public CustomPushPin Pin { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopView"/> class.
        /// </summary>
        public ShopView() {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopView"/> class.
        /// </summary>
        /// <param name="shop">The shop.</param>
        /// <param name="location">The location.</param>
        /// <param name="style">The style.</param>
        public ShopView(Shop shop, GeocodeLocation location, Style style)
        {
            this.Shop = shop;
            this.Pin = new CustomPushPin(location.Location);
            Pin.Pin.Style = style;
            ToolTipService.SetToolTip(Pin.Pin, String.Format("{0}\r\n{1}", shop.Name, shop.Address));
        }
    }
}
