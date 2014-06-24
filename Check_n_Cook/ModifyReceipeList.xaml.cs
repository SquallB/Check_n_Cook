using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Check_n_Cook
{
    /// <summary>
    /// This page displays a collection of element
    /// </summary>
    public sealed partial class ModifyReceipeList : Page, View
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
        /// The model
        /// </summary>
        private AppModel Model;
        /// <summary>
        /// The date
        /// </summary>
        private string date;
        /// <summary>
        /// The added view
        /// </summary>
        private bool addedView;


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
        /// Initializes a new instance of the <see cref="ModifyReceipeList"/> class.
        /// </summary>
        public ModifyReceipeList()
        {
            this.InitializeComponent();
            this.addedView = false;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }


        /// <summary>
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
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            GoToModifyReceipeListEvent evnt = e.NavigationParameter as GoToModifyReceipeListEvent;
            if (evnt != null)
            {
                this.Model = evnt.AppModel;
                if (this.addedView == false)
                {
                    this.Model.AddView(this);
                    this.addedView = true;
                }
                Time time = evnt.Time;

                this.pageTitle.Text = "Liste de recette du " + time.Date;

                if (this.Model.ReceipeList.ContainsKey(time.Date))
                {
                    ReceipeDate receipeDate = this.Model.ReceipeList[time.Date];
                    this.date = receipeDate.Time.Date;

                    List<ItemReceipe> itemsMorning = new List<ItemReceipe>();
                    foreach (Receipe receipe in receipeDate.ReceipeTimeOfDay["Matin"].Receipes.Values)
                    {
                        itemsMorning.Add(ToolItem.CreateItemReceipe(receipe));
                    }
                    morningReceipeViewSource.Source = itemsMorning;

                    List<ItemReceipe> itemsNoon = new List<ItemReceipe>();
                    foreach (Receipe receipe in receipeDate.ReceipeTimeOfDay["Midi"].Receipes.Values)
                    {
                        itemsNoon.Add(ToolItem.CreateItemReceipe(receipe));
                    }
                    noonReceipeViewSource.Source = itemsNoon;

                    List<ItemReceipe> itemsEvenning = new List<ItemReceipe>();
                    foreach (Receipe receipe in receipeDate.ReceipeTimeOfDay["Soir"].Receipes.Values)
                    {
                        itemsEvenning.Add(ToolItem.CreateItemReceipe(receipe));
                    }
                    evenningReceipeViewSource.Source = itemsEvenning;
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
        /// Refreshes the page.
        /// </summary>
        /// <param name="e">The event.</param>
        public void Refresh(Event e)
        {
            if (e is RemovedReceipeListEvent)
            {
                RemovedReceipeListEvent srcEvnt = (RemovedReceipeListEvent)e;
                Time time = srcEvnt.Time;
                List<ItemReceipe> newList = new List<ItemReceipe>();

                if (this.Model.ReceipeList.ContainsKey(time.Date))
                {
                    Dictionary<string, Receipe> previousList = this.Model.ReceipeList[time.Date].ReceipeTimeOfDay[time.TimeOfDay].Receipes;
                    foreach (Receipe receipe in previousList.Values)
                    {
                        newList.Add(ToolItem.CreateItemReceipe(receipe));
                    }
                }

                if (time.TimeOfDay.Equals("Matin"))
                {
                    morningReceipeViewSource.Source = newList;
                }

                else if (time.TimeOfDay.Equals("Midi"))
                {

                    noonReceipeViewSource.Source = newList;
                }
                else if (time.TimeOfDay.Equals("Soir"))
                {

                    evenningReceipeViewSource.Source = newList;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the GoToDetailReceipe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void GoToDetailReceipe_Click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ItemReceipe)
            {
                ItemReceipe item = (ItemReceipe)e.ClickedItem;
                Receipe receipe = item.Receipe;
                this.Model.SelectReceipe(receipe);

                //navigate to the ReceipeDetail page
                this.Frame.Navigate(typeof(ReceipeDetail), this.Model);
            }
        }

        /// <summary>
        /// Handles the Click event of the RemoveReceipe, update the model and the json file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        public async void RemoveReceipe_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.Frame != null && button != null && button.DataContext is ItemReceipe)
            {
                ItemReceipe item = (ItemReceipe)button.DataContext;
                Receipe re = item.Receipe;
                this.Model.RemoveReceipeList(re, (string)button.Tag, this.date);
                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile receipeFile = await folder.CreateFileAsync("receipes.json", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(receipeFile, this.Model.StringifyReceipesList());
            }
        }

    }
}
