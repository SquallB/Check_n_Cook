using Check_n_Cook.Common;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Check_n_Cook
{
    /// <summary>
    /// This page displays a collection of element
    /// </summary>
    public sealed partial class ReceipeDetail : BasePrintPage
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
        /// Gets or sets the image receipe.
        /// </summary>
        /// <value>
        /// The image receipe.
        /// </value>
        public Image ImageReceipe { get; set; }
        /// <summary>
        /// Gets or sets the receipe instruction.
        /// </summary>
        /// <value>
        /// The receipe instruction.
        /// </value>
        public TextBlock ReceipeInstruction { get; set; }
        /// <summary>
        /// The date
        /// </summary>
        private string Date;
        /// <summary>
        /// The time of day
        /// </summary>
        private string TimeOfDay;
        /// <summary>
        /// The receipe
        /// </summary>
        private Receipe receipe;
        /// <summary>
        /// The ingredients
        /// </summary>
        private List<ItemIngredient> ingredients;
        /// <summary>
        /// The current shopping list
        /// </summary>
        private List<Ingredient> currentShoppingList;
        /// <summary>
        /// The is favourite
        /// </summary>
        private bool isFavourite;

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
        /// Initializes a new instance of the <see cref="ReceipeDetail"/> class.
        /// </summary>
        public ReceipeDetail()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.Date = DateTime.Today.ToString("d");
            this.ingredients = new List<ItemIngredient>();
            this.checkboxes = new List<CheckBox>();
            this.currentShoppingList = new List<Ingredient>();
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
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.progressBar.Visibility = Visibility.Visible;
            this.Model = (AppModel)e.NavigationParameter;
            this.receipe = this.Model.SelectedReceipe;
            this.pageTitle.Text = receipe.Title;
            List<ItemReceipe> receipeView = new List<ItemReceipe>();
            List<ReceipeDescription> receipeDescription = new List<ReceipeDescription>();
            receipeDescription.Add(new ReceipeDescription(receipe));
            if (receipe.Id != -1)
            {
                ReceipeRetriever rr = new ReceipeRetriever();
                var task = rr.extractReceipeFromMarmiton(receipe);
                var error = false;
                try
                {
                    if ((await task) == true)
                    {
                        var task2 = rr.cleanHtmlEntities(receipe.HtmlReceipe, receipe);
                        rr.handleIngredients(rr.ingredientPart, receipe);

                        wb.NavigateToString(receipe.ToDoInstructions);
                    }
                }
                catch
                {
                    error = true;
                    
                }
                if (error)
                {
                    var messageDialog = new MessageDialog("Une erreur est survenue ! Vous n'êtes pas connecté à Internet ou le serveur est indisponible.");
                    await messageDialog.ShowAsync();
                }
            }
            this.receipeViewSource.Source = receipeView;
            foreach (var ing in receipe.ingredients)
            {
                ingredients.Add(ToolItem.CreateItemIngredient(ing));

            }

            if(this.Model.FavouriteReceipes.ContainsKey(this.receipe.Title)) {
                this.isFavourite = true;
            }
            else
            {
                this.isFavourite = false;
            }

            this.ingredientsViewSource.Source = ingredients;
            this.descriptionViewSource.Source = receipeDescription;
            this.RegisterForPrinting();

            this.descriptionHub.Visibility = Visibility.Visible;
            this.instructionsHub.Header = "Instructions";
            this.imageEasyLifer.Visibility = Visibility.Collapsed;
            this.ingredientsHub.Visibility = Visibility.Visible;
            this.gestionHub.Visibility = Visibility.Visible;
            this.progressBar.Visibility = Visibility.Collapsed;
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
            this.UnregisterForPrinting();
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// Handle the control when the user add the receipe to his favorite.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void AddReceipeFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (!isFavourite)
            {
                this.Model.AddFavoriteReceipe(this.receipe);
                this.favouriteButton.Content = "Enlever cette recette de vos favoris";
            }
            else
            {
                this.Model.RemoveFavoriteReceipe(this.receipe);
                this.favouriteButton.Content = "Ajouter cette recette à vos favoris";
            }

            isFavourite = !isFavourite;

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile receipeFile = await folder.CreateFileAsync("receipesFavorite.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(receipeFile, this.Model.StringifyFavouriteReceipes());
        }

        /// <summary>
        /// handle the control when the user add the receipe to his planning
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void AddReceipeList_Click(object sender, RoutedEventArgs e)
        {
            this.Model.AddReceipeList(this.receipe, this.TimeOfDay, this.Date);
            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile receipeFile = await folder.CreateFileAsync("receipes.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(receipeFile, this.Model.StringifyReceipesList());
        }

        /// <summary>
        /// Store the date given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Controls.DatePickerValueChangedEventArgs"/> instance containing the event data.</param>
        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                DateTime dateTime = datePicker.Date.Date;
                this.Date = dateTime.Date.ToString("d");
            }
        }

        /// <summary>
        /// Initialize the combox for the time of the day
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox != null)
            {
                comboBox.Items.Add("Matin");
                comboBox.Items.Add("Midi");
                comboBox.Items.Add("Soir");
                comboBox.SelectedIndex = 0;
                this.TimeOfDay = comboBox.Items[comboBox.SelectedIndex].ToString();
            }
        }


        /// <summary>
        /// Get the time of day selected by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                this.TimeOfDay = comboBox.Items[comboBox.SelectedIndex].ToString();
            }

        }
        public WebView wb;
        /// <summary>
        /// Initialize the instruction for the receipe
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void description_Loaded(object sender, RoutedEventArgs e)
        {
            WebView wb = sender as WebView;
            this.wb = wb;
            if (receipe.Id == -1)
            {
                wb.NavigateToString(receipe.ToDoInstructions);
            }
        }

        /// <summary>
        /// Provide print content for scenario 1 first page
        /// </summary>
        protected override void PreparePrintContent()
        {
            this.PagesToPrint.Add(new ReceipePrintPage(this.receipe));
        }

        /// <summary>
        /// Handles the Click event of the PrintReceipe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void PrintReceipe_Click(object sender, RoutedEventArgs e)
        {
            await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();
        }

        /// <summary>
        /// Displays the checkbox for select the ingredient
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void SelectIngredients_Click(object sender, RoutedEventArgs e)
        {
            this.button_validateIngredients.Visibility = Visibility.Visible;
            foreach (CheckBox checkbox in this.checkboxes)
            {
                checkbox.Visibility = Visibility.Visible;
                checkbox.IsChecked = true;
            }
        }

        /// <summary>
        /// Store the ingredient selected by the user in the list of ingredient selected
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox checkbox = (CheckBox)sender;
                if (checkbox.DataContext is ItemIngredient)
                {
                    Ingredient ingredient = ((ItemIngredient)checkbox.DataContext).Ingredient;
                    this.currentShoppingList.Add(ingredient);
                }

            }
        }

        /// <summary>
        /// Remove the ingredient of the list of ingredient selected
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox) {
                CheckBox checkbox = (CheckBox)sender;
                if (checkbox.DataContext is Ingredient)
                {
                    Ingredient ingredient = (Ingredient)checkbox.DataContext;
                    this.currentShoppingList.Remove(ingredient);
                }
            }
        }

        /// <summary>
        /// Put all the ingredient selected by the user in the shopping list
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void ValidateIngredients_Click(object sender, RoutedEventArgs e)
        {
            this.button_validateIngredients.Visibility = Visibility.Collapsed;
            foreach (CheckBox checkbox in this.checkboxes)
            {
                checkbox.Visibility = Visibility.Collapsed;
            }

            String groupName = this.receipe.Title;
            this.Model.AddShoppingListGroup(groupName);

            foreach (Ingredient ingredient in this.currentShoppingList)
            {
                this.Model.AddIngredientToShoppingList(ingredient, groupName);
            }

            
            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile shoppingListFile = await folder.CreateFileAsync("shoppingList.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(shoppingListFile, this.Model.StringifyShoppingList());
        }

        private Button button_validateIngredients;
        /// <summary>
        /// Store the button to validate the ingredient selected by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void button_validateIngredients_Loaded(object sender, RoutedEventArgs e)
        {
            this.button_validateIngredients = (Button)sender;
        }

        private List<CheckBox> checkboxes;
        /// <summary>
        /// Store all the checkbox in the list
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            checkboxes.Add((CheckBox)sender);
        }

        private Button favouriteButton;
        /// <summary>
        /// Store the button to add the receipe in favorite
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddReceipeFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            this.favouriteButton = (Button)sender;

            if (this.isFavourite && sender is Button)
            {
                this.favouriteButton.Content = "Enlever cette recette de vos favoris";
            }
        }


    }
}
