using Check_n_Cook.Common;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace Check_n_Cook
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class AllReceipeList : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public AllReceipeList()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.SizeChanged += GroupedItemsPage_SizeChanged;
        }

        /// <summary>
        /// If the page is resized to less than 500 pixels, use the layout for narrow widths. 
        /// Otherwise, use the default layout.
        /// </summary>
        /// <param name="sender">
        /// The source of the event
        /// </param>
        /// <param name="e">
        /// Event data related to the SizeChanged event.
        /// </param>
        void GroupedItemsPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 500)
            {
                VisualStateManager.GoToState(this, "MinimalLayout", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            /* int i = 1; 
             string filename = "RECETTE23.txt";
             StorageFile file = await ApplicationData.Current.RoamingFolder.GetFileAsync(filename);
             String receipe = await FileIO.ReadTextAsync(file);*/
            /*do
            {
                try
                {

                    file =
                    Receipe receipe = (Receipe)await FileIO.ReadBufferAsync(file);
                    sampleDataGroup.Items.Add(new SampleDataItem("1", "Matin", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
                    i++;
                }
                catch (Exception exp)
                {
                    this.pageTitle.Text = exp.Message;
                }

            } while (file != null);*/

            List<SampleDataGroup> sampleDataGroups = new List<SampleDataGroup>();
            SampleDataGroup sampleDataGroup = new SampleDataGroup("1", "Le 02/10/14", "subTittle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descirption de foflie");

             sampleDataGroup.Items.Add(new SampleDataItem("1", "Matin", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
             sampleDataGroup.Items.Add(new SampleDataItem("1", "Midi", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
             sampleDataGroup.Items.Add(new SampleDataItem("1", "Soir", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));

             SampleDataGroup sampleDataGroup2 = new SampleDataGroup("2", "2", "subTittle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descirption de foflie");
             sampleDataGroup2.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
             sampleDataGroup2.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
             sampleDataGroup2.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));

             SampleDataGroup sampleDataGroup3 = new SampleDataGroup("2", "2", "subTittle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descirption de foflie");
             sampleDataGroup3.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
             sampleDataGroup3.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
             sampleDataGroup3.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));

             SampleDataGroup sampleDataGroup4 = new SampleDataGroup("2", "2", "subTittle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descirption de foflie");
             sampleDataGroup4.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
             sampleDataGroup4.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));
             sampleDataGroup4.Items.Add(new SampleDataItem("1", "titl", "subtitle", "http://www.freecapsules.com/187-790-large/carotte-60-gelules.jpg", "descprion", "contenet"));

             sampleDataGroups.Add(sampleDataGroup);
             sampleDataGroups.Add(sampleDataGroup2);
             sampleDataGroups.Add(sampleDataGroup3);
             sampleDataGroups.Add(sampleDataGroup4);

            sampleDataGroups.Add(sampleDataGroup);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        public void GoToReceipeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ReceipeList));
            }
        }

        public void GoToReceipeList_CLick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ReceipeList));
            }
        }
    }
}