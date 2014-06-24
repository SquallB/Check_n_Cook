using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Check_n_Cook
{
    /// <summary>
    /// This page displays a collection of element
    /// </summary>
    public sealed partial class PlanningReceipe : Page, View
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
        /// The application model
        /// </summary>
        private AppModel appModel;
        /// <summary>
        /// The check boxs
        /// </summary>
        private Dictionary<string, CheckBox> checkBoxs;
        /// <summary>
        /// The print receipe list
        /// </summary>
        private Button printReceipeList;
        /// <summary>
        /// The receipe list selected model
        /// </summary>
        private ReceipeListSelected receipeListSelectedModel;
        /// <summary>
        /// The list receipe list selected
        /// </summary>
        private List<ItemReceipeListSelected> listReceipeListSelected;
        /// <summary>
        /// The delte receipe list
        /// </summary>
        private Button delteReceipeList;
        /// <summary>
        /// The selection mode
        /// </summary>
        private Button selectionMode;

        /// <summary>
        /// Gets the default view model.
        /// </summary>
        /// <value>
        /// The default view model.
        /// </value>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Gets the navigation helper.
        /// </summary>
        /// <value>
        /// The navigation helper.
        /// </value>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningReceipe"/> class.
        /// </summary>
        public PlanningReceipe()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.checkBoxs = new Dictionary<string, CheckBox>();
            this.receipeListSelectedModel = new ReceipeListSelected();
            this.listReceipeListSelected = new List<ItemReceipeListSelected>();
            this.receipeListSelectedModel.AddView(this);
        }


        /// Fills the page with the previous elements while the navigation. Any state loaded is given when the page
        /// is recreated from a previous session.
        /// </summary>
        /// <param name="sender">
        /// The event source ; generaly <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e"> Event data that give the parameter of the navigation transmitted
        /// <see cref="Frame.Navigate(Type, Object)"/> during the initial request of this page and
        /// a state dictionnary preserved by this page during a previous session
        /// The state will not take the value Null when the first visit of this page.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (this.appModel == null)
            {
                this.appModel = e.NavigationParameter as AppModel;
                this.appModel.AddView(this);
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

                        sampleDataGroup.Items.Add(new ItemReceipeTimeOfDay(receipeDate.Time.Date, imgs, receipeTimeOfDay.Time.TimeOfDay));
                    }

                    sampleDataGroups.Add(sampleDataGroup);
                }
            }
            catch (FileNotFoundException ex)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("Error: " + ex.Message + " \nLocation: PlanningReceipe class.");
            }

            BubbleSort(sampleDataGroups);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Sort the collection given
        /// </summary>
        /// <param name="sampleDataGroups">The sample data groups.</param>
        public void BubbleSort(List<SampleDataGroup> sampleDataGroups)
        {
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
        }

        #region Inscription de NavigationHelper

        /// <summary>
        /// Called when the page is loaded and becomes the actual source of a parent frame.
        /// </summary>
        /// <param name="e">Event data that may be examinate by replcaing the code. The event data represents the navigation waiting that will load the active page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Called when the page is loaded and becomes the actual source of a parent frame.
        /// </summary>
        /// <param name="e">Event data that may be examinate by replcaing the code. The event data represents the navigation waiting that will unload the active page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// Listener that allow to go to the ReceipeList page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        public void GoToReceipeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ItemReceipeTimeOfDay)
            {
                ItemReceipeTimeOfDay viewReceipeTimeOfDay = (ItemReceipeTimeOfDay)e.ClickedItem;
                Time time = viewReceipeTimeOfDay.Time;
                if (this.appModel.ReceipeList.ContainsKey(time.Date) && this.appModel.ReceipeList[time.Date].ReceipeTimeOfDay.ContainsKey(time.TimeOfDay))
                {
                    ReceipeTimeOfDay receipeTImeOfDay = this.appModel.ReceipeList[time.Date].ReceipeTimeOfDay[time.TimeOfDay];
                    this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.appModel, time, receipeTImeOfDay));
                }

            }
        }
        /// <summary>
        ///  Listener that allow to go to the ReceipeList page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToReceipeListAll_CLick(object sender, RoutedEventArgs e)
        {
            var group = (sender as FrameworkElement).DataContext;
            SampleDataGroup data = (SampleDataGroup)group;
            string date = (string)data.Title;
            Time time = new Time();
            time.Date = date;

            if (this.appModel.ReceipeList.ContainsKey(date))
            {
                ReceipeDate receipeDate = this.appModel.ReceipeList[date];
                this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.appModel, time, receipeDate));
            }
        }

        /// <summary>
        /// Add the DataGroup to the list of receipeListSelected
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                SampleDataGroup data = (SampleDataGroup)cb.DataContext;
                receipeListSelectedModel.AddReceipeList(data);
            }
        }

        /// <summary>
        /// Remove the DataGroup to the list of receipeListSelected
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                SampleDataGroup data = (SampleDataGroup)cb.DataContext;
                receipeListSelectedModel.RemoveReceipeList(data);
            }
        }

        /// <summary>
        /// Store the checkbox loaded
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                SampleDataGroup data = (SampleDataGroup)cb.DataContext;
                if (!checkBoxs.ContainsKey(data.Title))
                {
                    checkBoxs.Add(data.Title, cb);
                }
                else
                {
                    //Checkbox has already loaded
                    cb.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Change the disposition of the page when the selection mode is actived or desactived
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SelectionMode_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            if (printReceipeList.Visibility == Visibility.Collapsed)
            {
                printReceipeList.Visibility = Visibility.Visible;
                but.Content = "Désactiver la sélection";

                foreach (CheckBox ch in checkBoxs.Values)
                {
                    ch.Visibility = Visibility.Visible;
                }

                this.receipeListHubSection.Visibility = Visibility.Visible;
                this.delteReceipeList.Visibility = Visibility.Visible;
                this.selectionMode.Visibility = Visibility.Visible;
            }
            else if (printReceipeList.Visibility == Visibility.Visible)
            {
                printReceipeList.Visibility = Visibility.Collapsed;
                but.Content = "Activer la sélection";

                foreach (CheckBox ch in checkBoxs.Values)
                {
                    ch.IsChecked = false;
                    ch.Visibility = Visibility.Collapsed;
                }

                this.receipeListHubSection.Visibility = Visibility.Collapsed;
                this.delteReceipeList.Visibility = Visibility.Collapsed;
                this.selectionMode.Visibility = Visibility.Collapsed;
            }


        }

        /// <summary>
        /// Store the button to print the receipeList
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PrintReceipeList_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                printReceipeList = (Button)sender;
            }
        }

        /// <summary>
        /// Refresh the page according to the event given
        /// </summary>
        /// <param name="e">The e.</param>
        public void Refresh(Event e)
        {
            if (e is ModifyReceipeListPrint)
            {
                ModifyReceipeListPrint srcEvnt = (ModifyReceipeListPrint)e;
                ReceipeListSelected modelEvnt = (ReceipeListSelected)srcEvnt.Model;
                this.listReceipeListSelected.Clear();
                this.listReceipeListSelected = new List<ItemReceipeListSelected>();

                foreach (SampleDataGroup ing in modelEvnt.GetReceipeListSelected())
                {
                    List<string> imgs = new List<string>();
                    int nbImgFound = 0;
                    int i = 0;
                    int j = 0;
                    while (i < ing.Items.Count && nbImgFound < 4)
                    {
                        j = 0;
                        while (nbImgFound < 4 && j < ing.Items[i].ImagePaths.Count)
                        {
                            imgs.Add(ing.Items[i].ImagePaths[j]);
                            nbImgFound++;
                            j++;
                        }
                        i++;
                    }
                    listReceipeListSelected.Add(new ItemReceipeListSelected(ing.Title, imgs));
                }

                this.receipeListViewSource.Source = listReceipeListSelected;
            }
            else if (e is RemovedReceipeDateEvent)
            {
                RemovedReceipeDateEvent srcEvnt = (RemovedReceipeDateEvent)e;
                AppModel modelEvnt = (AppModel)srcEvnt.Model;
                List<SampleDataGroup> sampleDataGroups = new List<SampleDataGroup>();

                foreach (ReceipeDate receipeDate in modelEvnt.ReceipeList.Values)
                {
                    SampleDataGroup sampleDataGroup = new SampleDataGroup(receipeDate.Time.Date);

                    foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                    {
                        List<string> imgs = new List<string>();

                        foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                        {
                            imgs.Add(receipe.Image);
                        }

                        sampleDataGroup.Items.Add(new ItemReceipeTimeOfDay(receipeDate.Time.Date, imgs, receipeTimeOfDay.Time.TimeOfDay));
                    }

                    sampleDataGroups.Add(sampleDataGroup);
                }

                foreach (CheckBox cb in checkBoxs.Values)
                {
                    cb.Visibility = Visibility.Visible;
                }

                BubbleSort(sampleDataGroups);
                this.DefaultViewModel["Groups"] = sampleDataGroups;
            }
        }

        /// <summary>
        /// Add the ingredients of the receipe in the receipeList, in the shoppingList
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void AddAllIngredients_Click(object sender, RoutedEventArgs e)
        {
            foreach (SampleDataGroup data in this.receipeListSelectedModel.GetReceipeListSelected())
            {
                string date = data.Title;
                ReceipeDate receipeDate = this.appModel.ReceipeList[date];

                foreach (ReceipeTimeOfDay receipetod in receipeDate.ReceipeTimeOfDay.Values)
                {
                    foreach (Receipe rec in receipetod.Receipes.Values)
                    {
                        this.appModel.CreateShoppingList(rec);
                    }
                }
            }

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile shoppingListFile = await folder.CreateFileAsync("shoppingList.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(shoppingListFile, this.appModel.StringifyShoppingList());
        }

        /// <summary>
        /// Delete all the receipeList selected
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void DeleteReceipeList_Click(object sender, RoutedEventArgs e)
        {
            List<SampleDataGroup> sampleDataGroups = new List<SampleDataGroup>();

            foreach (SampleDataGroup data in this.receipeListSelectedModel.GetReceipeListSelected())
            {
                sampleDataGroups.Add(data);
            }

            foreach (SampleDataGroup dataGroup in sampleDataGroups)
            {
                Time time = null;
                if (dataGroup.Items.Count >= 1)
                {
                    time = dataGroup.Items[0].Time;
                }

                //update the AppModel model
                this.appModel.RemoveReceipeDate(time);
                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile receipeFile = await folder.CreateFileAsync("receipes.json", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(receipeFile, this.appModel.StringifyReceipesList());

                //update the eceipeListSelected model
                this.receipeListSelectedModel.RemoveReceipeList(dataGroup);
            }
        }

        /// <summary>
        /// Store the button to delete the receipe list selected
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteReceipeList_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                this.delteReceipeList = (Button)sender;
            }
        }

        /// <summary>
        /// Unselected all the checkbox
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UnselectReceipeList_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in checkBoxs.Values)
            {
                cb.IsChecked = false;
            }
        }

        /// <summary>
        /// Store the button to get te selection mode
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SelectionMode_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                this.selectionMode = (Button)sender;
            }
        }

        /// <summary>
        /// Handle the button to go to the receipe list
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void ItemReceipeListSelected_Click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ItemReceipeListSelected)
            {
                ItemReceipeListSelected item = (ItemReceipeListSelected)e.ClickedItem;
                Time time = new Time();
                time.Date = item.Title;

                if (this.appModel.ReceipeList.ContainsKey(time.Date))
                {
                    ReceipeDate receipeDate = this.appModel.ReceipeList[time.Date];
                    this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.appModel, time, receipeDate));
                }
            }
        }

    }
}
