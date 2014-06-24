using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Check_n_Cook
{
    /// <summary>
    /// This page displays a collection of element
    /// </summary>
    public sealed partial class ReceipeList : Page
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
        /// The time
        /// </summary>
        private Time time;
        /// <summary>
        /// The hub receipe
        /// </summary>
        private HubSection hubReceipe;
        /// <summary>
        /// The nb count receipe
        /// </summary>
        private int nbCountReceipe;
        /// <summary>
        /// The day
        /// </summary>
        private bool day;
        /// <summary>
        /// Gets or sets the receipe text block.
        /// </summary>
        /// <value>
        /// The receipe text block.
        /// </value>
        public TextBlock ReceipeTextBlock { get; set; }
        /// <summary>
        /// Gets or sets the ingredient text block.
        /// </summary>
        /// <value>
        /// The ingredient text block.
        /// </value>
        public TextBlock IngredientTextBlock { get; set; }

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
        /// Initializes a new instance of the <see cref="ReceipeList"/> class.
        /// </summary>
        public ReceipeList()
        {
            this.InitializeComponent();
            this.day = false;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }


        /// <summary>
        /// Fills the page with the previous elements while the navigation. Any state loaded is given when the page
        /// is recreated from a previous session.
        /// </summary>
        /// <param name="sender">The event source ; generaly <see cref="NavigationHelper" /></param>
        /// <param name="e">Event data that give the parameter of the navigation transmitted
        /// <see cref="Frame.Navigate(Type, Object)" /> during the initial request of this page and
        /// a state dictionnary preserved by this page during a previous session
        /// The state will not take the value Null when the first visit of this page.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            GoToReceipeListEvent evnt = e.NavigationParameter as GoToReceipeListEvent;
            this.Model = evnt.AppModel;
            this.time = evnt.Time;
            List<ItemReceipe> receipes = new List<ItemReceipe>();

            if (evnt.ReceipeTime is ReceipeTimeOfDay)
            {
                ReceipeTimeOfDay receipeTimeOfDay = (ReceipeTimeOfDay)evnt.ReceipeTime;
                foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                {
                    receipes.Add(ToolItem.CreateItemReceipe(receipe));
                }

                HandleTitle(receipeTimeOfDay);
            }
            else if (evnt.ReceipeTime is ReceipeDate)
            {
                ReceipeDate receipeDate = (ReceipeDate)evnt.ReceipeTime;
                foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                {
                    foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                    {
                        receipes.Add(ToolItem.CreateItemReceipe(receipe));
                    }
                }
                HandleTitle(receipeDate);
            }

            listReceipeViewSource.Source = receipes;
        }

        /// <summary>
        /// Handles the title if there is more than one receipe
        /// </summary>
        /// <param name="receipeTime">The receipe time.</param>
        public void HandleTitle(ReceipeTime receipeTime)
        {
            this.nbCountReceipe = 0;

            if (receipeTime is ReceipeTimeOfDay)
            {
                nbCountReceipe = ((ReceipeTimeOfDay)receipeTime).Receipes.Count;

            }
            else if (receipeTime is ReceipeDate)
            {
                ReceipeDate receipeDate = (ReceipeDate)receipeTime;
                day = true;
                foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                {
                    nbCountReceipe += receipeTimeOfDay.Receipes.Count;
                }
            }


            if (nbCountReceipe > 1)
            {
                this.pageTitle.Text = "Liste de recettes";
            }
            else
            {
                this.pageTitle.Text = "Liste de recettes";
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
        /// Display all the ingredients of a receipe
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ClickHandleIngredients(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (b != null && b.DataContext is ItemReceipe)
            {
                ItemReceipe item = (ItemReceipe)b.DataContext;
                Receipe receipe = item.Receipe;

                List<ItemIngredient> itemsIng = new List<ItemIngredient>();

                foreach (Ingredient ing in receipe.ingredients)
                {
                    itemsIng.Add(ToolItem.CreateItemIngredient(ing));
                }

                this.listIngredientsViewSource.Source = itemsIng;

                if (this.IngredientTextBlock != null)
                {
                    this.IngredientTextBlock.Text = (string)receipe.Title;
                }
            }

        }

        /// <summary>
        /// Handles the Loaded event of the TextBlockReceipe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBlockReceipe_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock)
            {
                this.ReceipeTextBlock = (TextBlock)sender;
                if (nbCountReceipe > 1)
                {
                    if (!day)
                    {
                        this.ReceipeTextBlock.Text = "Recettes : " + this.time.Date + " (" + this.time.TimeOfDay + ")";
                    }
                    else
                    {
                        this.ReceipeTextBlock.Text = "Recettes : " + this.time.Date + " (La journée)";
                    }
                }
                else
                {
                    if (!day)
                    {
                        this.ReceipeTextBlock.Text = "Recette : " + this.time.Date + " (" + this.time.TimeOfDay + ")";
                    }
                    else
                    {
                        this.ReceipeTextBlock.Text = "Recette : " + this.time.Date + " (La journée)";
                    }
                }
            }
        }

        /// <summary>
        /// Store the TextBlock for the name of the receipe
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBlockIngredient_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock)
            {
                this.IngredientTextBlock = (TextBlock)sender; 
            }
        }

        /// <summary>
        /// Handles the Click event of the GoToModifyReceipeList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToModifyReceipeList_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ModifyReceipeList), new GoToModifyReceipeListEvent(this.Model, this.time));
            }
        }

        /// <summary>
        /// Handles the Click event of the GoToShoppingList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToShoppingList_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ShoppingList), new GoToModifyReceipeListEvent(this.Model, this.time));
            }
        }

        /// <summary>
        /// Go to the ReceipeDetail page when the user click on the receipe
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void ReceipeClick_Click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ItemReceipe)
            {
                ItemReceipe item = (ItemReceipe)e.ClickedItem;
                Receipe re = item.Receipe;
                this.Model.SelectReceipe(re);
                this.Frame.Navigate(typeof(ReceipeDetail), this.Model);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the HubSection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void HubSection_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is HubSection)
            {
                this.hubReceipe = (HubSection)sender;
                if (nbCountReceipe > 1)
                {
                    this.hubReceipe.Header = "Recettes";
                }
                else
                {
                    this.hubReceipe.Header = "Recette";
                }

            }

        }

    }
}