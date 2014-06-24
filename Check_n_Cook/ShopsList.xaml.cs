using Bing.Maps;
using Bing.Maps.Search;
using Check_n_Cook.Common;
using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// To learn more about the item template Hub Page, see page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page displaying a collection of grouped items.
    /// </summary>
    public sealed partial class ShopsList : Page
    {
        /// <summary>
        /// The navigation helper
        /// </summary>
        private NavigationHelper navigationHelper;
        /// <summary>
        /// The default view model
        /// </summary>
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public AppModel Model { get; set; }
        /// <summary>
        /// Gets or sets the shop views.
        /// </summary>
        /// <value>
        /// The shop views.
        /// </value>
        public List<ShopView> ShopViews { get; set; }
        /// <summary>
        /// Gets or sets my map.
        /// </summary>
        /// <value>
        /// My map.
        /// </value>
        public Map MyMap { get; set; }

        /// <summary>
        /// Can be replaced by a strongly typed display model
        /// </summary>
        /// <value>
        /// The default view model.
        /// </value>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page for making navigation easy and
        /// to handle the process lifetime duration
        /// </summary>
        /// <value>
        /// The navigation helper.
        /// </value>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopsList"/> class.
        /// </summary>
        public ShopsList()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }


        /// <summary>
        /// Fills the page with the content passed during navigation. Any registered state is also
        /// provided when a page is recreated from a previous session.
        /// </summary>
        /// <param name="sender">The source of the event; in general <see cref="NavigationHelper" /></param>
        /// <param name="e">Event data that provide the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)" /> during the initial application of this page and
        /// a dictionary stored by this page during a previous session state
        /// the previous state won't have the Null value during the first visit of the page.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.Model = (AppModel)e.NavigationParameter;
        }

        #region Inscription de NavigationHelper

        /// <summary>
        /// Called when the page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by changing the code. The event data represent the pending navigation that will load the Active Page. Usually the most appropriate property to consider is Parameter.</param>
        /// The methods provided in this section are used simply to allow
        /// The NavigationHelper to respond to the navigation methods of the page.
        /// Specific logic to the page should be placed in event handlers for
        /// <see cref="GridCS.Common.NavigationHelper.LoadState" />
        /// et <see cref="GridCS.Common.NavigationHelper.SaveState" />.
        /// the navigation parameter is available in the LoadState method
        /// in addition to the page state conserved during the previous session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Called immediately after the page unloading, doesn't represent anymore the actual source of a frame parent.
        /// </summary>
        /// <param name="e">Event data that can be examined by replacing code. The event data represent the navigation that have discharged the active Page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// Handles the Loaded event of the myMap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void myMap_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.MyMap = (Map)sender;
                Geolocator geolocator = new Geolocator();
                //used to obtain user location
                Geoposition position = await geolocator.GetGeopositionAsync();
                double latitude = position.Coordinate.Point.Position.Latitude;
                double longitude = position.Coordinate.Point.Position.Longitude;
                Location location = new Location(latitude, longitude);

                SearchManager searchManager = MyMap.SearchManager;
                ReverseGeocodeRequestOptions requestOptions = new ReverseGeocodeRequestOptions(location);
                LocationDataResponse response;
                do
                {
                    response = await searchManager.ReverseGeocodeAsync(requestOptions);
                } while (response.LocationData.Count == 0 || response.LocationData == null);

                GeocodeLocation currentLocation = response.LocationData[0];
                //place a PushPin at the right place (user)
                CustomPushPin userPin = new CustomPushPin(location);
                userPin.Pin.Style = this.Resources["PushPinStyle"] as Style;
                //create the associated toolTip
                ToolTipService.SetToolTip(userPin.Pin, "Vous");
                MyMap.Children.Add(userPin.Pin);

                MyMap.ZoomLevel = 12;
                MyMap.Center = location;
                //used to retrieve shops
                PagesJaunesShopRetriever retriever = new PagesJaunesShopRetriever();
                bool nothing = await retriever.GetShops(currentLocation.Address.Locality, this.Model);
                this.shopsViewSource.Source = this.Model.Shops;

                this.ShopViews = new List<ShopView>();
                //used to display shops
                this.ShopViews = await this.createShopViews(this.Model.Shops, searchManager);

                foreach (ShopView shopView in this.ShopViews)
                {
                    this.MyMap.Children.Add(shopView.Pin.Pin);
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        /// <summary>
        /// Creates the shop views.
        /// </summary>
        /// <param name="shops">The shops.</param>
        /// <param name="searchManager">The search manager.</param>
        /// <returns></returns>
        private async Task<List<ShopView>> createShopViews(List<Shop> shops, SearchManager searchManager)
        {
            List<ShopView> shopViews = new List<ShopView>();
            //get the location associated to all shops and create their views
            foreach (Shop shop in shops)
            {
                var options = new GeocodeRequestOptions(shop.Address);
                var response = await searchManager.GeocodeAsync(options);
                GeocodeLocation shopLocation = null;

                if (response.LocationData.Count > 0)
                {
                    shopLocation = response.LocationData[0];
                    ShopView shopView = new ShopView(shop, shopLocation, this.Resources["PushPinMarketStyle"] as Style);
                    shopViews.Add(shopView);
                }
            }

            return shopViews;
        }
    }
}
