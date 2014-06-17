using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Json;
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
    public sealed partial class PlanningReceipe : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public AppModel Model { get; set; }

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

        public PlanningReceipe()
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
            if (this.Model == null)
            {
                this.Model = e.NavigationParameter as AppModel;
            }
            StorageFolder folder = KnownFolders.PicturesLibrary;
            List<SampleDataGroup> sampleDataGroups = new List<SampleDataGroup>();
            try
            {
                StorageFile receipesFile = await folder.GetFileAsync("receipes.json");
                String jsonString = await FileIO.ReadTextAsync(receipesFile);
                JsonObject jsonObject = JsonObject.Parse(jsonString);
                JsonArray jsonArray = jsonObject.GetNamedArray("Receipes");

                foreach (var jsonDate in jsonArray)
                {
                    JsonObject receipeDateJson = JsonObject.Parse(jsonDate.Stringify());
                    ReceipeDate receipeDate = new ReceipeDate(receipeDateJson);
                    SampleDataGroup sampleDataGroup = new SampleDataGroup(receipeDate.Time.Date);

                    foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                    {
                        List<string> imgs = new List<string>();

                        foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                        {
                            imgs.Add(receipe.Image);
                        }

                        sampleDataGroup.Items.Add(new ViewReceipeTimeOfDay(receipeDate.Time.Date, imgs, receipeTimeOfDay.Time.TimeOfDay));
                    }

                    sampleDataGroups.Add(sampleDataGroup);

                }

            }
            catch (FileNotFoundException ex)
            {

            }

            int n = sampleDataGroups.Count;
            bool swapped = true;
            while (swapped)
            {
                swapped = false;
                for (int i = 0; i < n - 1; i++)
                {
                    DateTime date1 = Convert.ToDateTime(sampleDataGroups[i].Title);
                    DateTime date2 = Convert.ToDateTime(sampleDataGroups[i + 1].Title);

                    if (DateTime.Compare(date1, date2) > 0)
                    {
                        swapped = true;
                        SampleDataGroup tmp = sampleDataGroups[i];
                        sampleDataGroups[i] = sampleDataGroups[i + 1];
                        sampleDataGroups[i + 1] = tmp;
                    }
                    n--;
                }
            }

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
                this.Frame.Navigate(typeof(ModifyReceipeList));
            }
        }

        public void GoToReceipeList_CLick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.Frame != null && button != null)
            {
                Time time = (Time)button.DataContext;

                ReceipeDate receipeDate = this.Model.ReceipeList[time.Date];
                if (receipeDate != null)
                {
                    ReceipeTimeOfDay receipeTimeOfDay = receipeDate.ReceipeTimeOfDay[time.TimeOfDay];
                    if (receipeTimeOfDay != null)
                    {
                        receipeTimeOfDay.Time.Date = receipeDate.Time.Date;
                        this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.Model, time, receipeTimeOfDay));
                    }
                }
            }
        }

        private void GoToReceipeListAll_CLick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.Frame != null && button != null)
            {
                string date = (string)button.DataContext;
                Time time = new Time();
                time.Date = date;
                ReceipeDate receipeDate = this.Model.ReceipeList[date];

                this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.Model, time, receipeDate));
            }
        }

    }
}