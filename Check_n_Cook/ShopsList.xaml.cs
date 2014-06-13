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

// Pour en savoir plus sur le modèle d'élément Page Hub, consultez la page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class ShopsList : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public AppModel Model { get; set; }
        public List<ShopView> ShopViews { get; set; }
        public Map MyMap { get; set; }

        /// <summary>
        /// Cela peut être remplacé par un modèle d'affichage fortement typé.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper est utilisé sur chaque page pour faciliter la navigation et 
        /// gestion de la durée de vie des processus
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ShopsList()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }


        /// <summary>
        /// Remplit la page à l'aide du contenu passé lors de la navigation. Tout état enregistré est également
        /// fourni lorsqu'une page est recréée à partir d'une session antérieure.
        /// </summary>
        /// <param name="sender">
        /// La source de l'événement ; en général <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Données d'événement qui fournissent le paramètre de navigation transmis à
        /// <see cref="Frame.Navigate(Type, Object)"/> lors de la requête initiale de cette page et
        /// un dictionnaire d'état conservé par cette page durant une session
        /// antérieure.  L'état n'aura pas la valeur Null lors de la première visite de la page.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.Model = (AppModel)e.NavigationParameter;
        }

        #region Inscription de NavigationHelper

        /// Les méthodes fournies dans cette section sont utilisées simplement pour permettre
        /// NavigationHelper pour répondre aux méthodes de navigation de la page.
        /// 
        /// La logique spécifique à la page doit être placée dans les gestionnaires d'événements pour  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// et <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// Le paramètre de navigation est disponible dans la méthode LoadState 
        /// en plus de l'état de page conservé durant une session antérieure.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void myMap_Loaded(object sender, RoutedEventArgs e)
        {
            this.MyMap = (Map)sender;
            Geolocator geolocator = new Geolocator();
            Geoposition position = await geolocator.GetGeopositionAsync();
            double latitude = position.Coordinate.Point.Position.Latitude;
            double longitude = position.Coordinate.Point.Position.Longitude;
            Location location = new Location(latitude, longitude);

            SearchManager searchManager = MyMap.SearchManager;
            ReverseGeocodeRequestOptions requestOptions = new ReverseGeocodeRequestOptions(location);
            LocationDataResponse response = await searchManager.ReverseGeocodeAsync(requestOptions);
            GeocodeLocation currentLocation = response.LocationData[0];

            CustomPushPin userPin = new CustomPushPin(location);
            userPin.Pin.Style = this.Resources["PushPinStyle"] as Style;

            ToolTipService.SetToolTip(userPin.Pin, "Vous");
            MyMap.Children.Add(userPin.Pin);

            MyMap.ZoomLevel = 12;
            MyMap.Center = location;

            PagesJaunesShopRetriever retriever = new PagesJaunesShopRetriever();
            bool nothing = await retriever.GetShops(currentLocation.Address.Locality, this.Model);
            this.shopsViewSource.Source = this.Model.Shops;

            this.ShopViews = new List<ShopView>();
            
            this.ShopViews = await this.createShopViews(this.Model.Shops, searchManager);

            foreach (ShopView shopView in this.ShopViews)
            {
                this.MyMap.Children.Add(shopView.Pin.Pin);
            }
        }

        private async Task<List<ShopView>> createShopViews(List<Shop> shops, SearchManager searchManager)
        {
            List<ShopView> shopViews = new List<ShopView>();

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
