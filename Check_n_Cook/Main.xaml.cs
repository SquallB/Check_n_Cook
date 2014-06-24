using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;


namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class Main : Page, View
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
        /// Gets or sets the items receipe.
        /// </summary>
        /// <value>
        /// The items receipe.
        /// </value>
        private List<ItemReceipe> ItemsReceipe { get; set; }
        /// <summary>
        /// The dish type search
        /// </summary>
        List<string> dishTypeSearch = new List<string>();
        /// <summary>
        /// The ingredient search
        /// </summary>
        public string[] ingredientSearch;
        /// <summary>
        /// The text ingredient search
        /// </summary>
        public TextBox txtIngredientSearch;
        /// <summary>
        /// The retriever
        /// </summary>
        private URLDataRetriever retriever;
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public AppModel Model { get; set; }
        /// <summary>
        /// The slider search
        /// </summary>
        public Slider sliderSearch;
        /// <summary>
        /// The progress bar
        /// </summary>
        public ProgressBar progressBar;
        /// <summary>
        /// Cela peut être remplacé par un modèle d'affichage fortement typé.
        /// </summary>
        /// <value>
        /// The default view model.
        /// </value>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper est utilisé sur chaque page pour faciliter la navigation et
        /// gestion de la durée de vie des processus
        /// </summary>
        /// <value>
        /// The navigation helper.
        /// </value>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.Model = new AppModel();
            this.Model.AddView(this);
            this.retriever = new URLDataRetriever();
            this.retriever.AdvancedSearch = this.dishTypeSearch;
            this.retriever.AdvancedDifficulty = 0;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }


        /// <summary>
        /// Remplit la page à l'aide du contenu passé lors de la navigation. Tout état enregistré est également
        /// fourni lorsqu'une page est recréée à partir d'une session antérieure.
        /// </summary>
        /// <param name="sender">La source de l'événement ; en général <see cref="NavigationHelper" /></param>
        /// <param name="e">Données d'événement qui fournissent le paramètre de navigation transmis à
        /// <see cref="Frame.Navigate(Type, Object)" /> lors de la requête initiale de cette page et
        /// un dictionnaire d'état conservé par cette page durant une session
        /// antérieure.  L'état n'aura pas la valeur Null lors de la première visite de la page.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (this.Model.FavouriteReceipes.Count > 0)
            {
                this.Model.FavouriteReceipes.Clear();
                this.Model.FavouriteReceipes = new Dictionary<string, Receipe>();
            }

            string msgError = "";
            bool error = false;
            StorageFolder folder = KnownFolders.PicturesLibrary;

            try
            {
                StorageFile receipesFile = await folder.GetFileAsync("receipesFavorite.json");
                String jsonString = await FileIO.ReadTextAsync(receipesFile);
                JsonObject jsonObject = JsonObject.Parse(jsonString);
                JsonArray jsonArray = jsonObject.GetNamedArray("Receipes");
                foreach (var jsonReceipe in jsonArray)
                {
                    Receipe receipe = new Receipe(jsonReceipe.Stringify());
                    this.Model.AddFavoriteReceipe(receipe);
                }
            }
            catch (FileNotFoundException ex)
            {
                error = true;
                msgError = ex.Message;
            }
            if (error)
            {
                var messageDialog = new MessageDialog("favourite");
                await messageDialog.ShowAsync();
            }

            this.favoriteViewSource.Source = this.Model.FavouriteReceipes.Values;

            if (this.Model.ReceipeList.Count == 0)
            {
                msgError = "";
                error = false;
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
                        this.Model.ReceipeList.Add(receipeDate.Time.Date, receipeDate);
                    }

                }
                catch (Exception ex)
                {
                    error = true;
                    msgError = ex.Message;
                }
                if (error)
                {
                    var messageDialog = new MessageDialog("list");
                    await messageDialog.ShowAsync();
                }
            }

            if (this.Model.ShoppingList.Count == 0)
            {
                msgError = "";
                error = false;
                try
                {
                    StorageFile shoppingListFile = await folder.GetFileAsync("shoppingList.json");
                    String jsonString = await FileIO.ReadTextAsync(shoppingListFile);
                    JsonObject jsonObject = JsonObject.Parse(jsonString);
                    JsonArray jsonArray = jsonObject.GetNamedArray("ShoppingList");

                    foreach (var jsonShoppingListGroup in jsonArray)
                    {
                        JsonObject groupObject = JsonObject.Parse(jsonShoppingListGroup.Stringify());
                        String groupName = groupObject.GetNamedString("Name");
                        this.Model.AddShoppingListGroup(groupName);
                        JsonArray ingredientsArray = groupObject.GetNamedArray("Ingredients");

                        foreach (var ingredientJson in ingredientsArray)
                        {
                            Ingredient ingredient = new Ingredient(ingredientJson.GetObject());
                            this.Model.AddIngredientToShoppingList(ingredient, groupName);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        #region Inscription de NavigationHelper

        /// <summary>
        /// Appelé lorsque la Page est chargée et devient la source actuelle d'un Frame parent.
        /// </summary>
        /// <param name="e">Données d'événement qui peuvent être examinés en remplaçant le code. Les données d'événement représentent la navigation en attente qui chargera la Page active. Généralement la propriété la plus appropriée à examiner est Parameter.</param>
        /// Les méthodes fournies dans cette section sont utilisées simplement pour permettre
        /// NavigationHelper pour répondre aux méthodes de navigation de la page.
        /// La logique spécifique à la page doit être placée dans les gestionnaires d'événements pour
        /// <see cref="GridCS.Common.NavigationHelper.LoadState" />
        /// et <see cref="GridCS.Common.NavigationHelper.SaveState" />.
        /// Le paramètre de navigation est disponible dans la méthode LoadState
        /// en plus de l'état de page conservé durant une session antérieure.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Refreshes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        public void Refresh(Event e)
        {
            if (e is ReceipeEvent)
            {
                ReceipeEvent receipeE = (ReceipeEvent)e;
                Receipe receipe = receipeE.Receipe;

                if (receipeE is AddedReceipeEvent)
                {
                    this.ItemsReceipe.Add(ToolItem.CreateItemReceipe(receipe));
                }
                else if (receipeE is RemovedReceipeEvent)
                {
                    foreach (ItemReceipe item in this.ItemsReceipe)
                    {
                        if (item.Receipe == receipe)
                        {
                            this.ItemsReceipe.Remove(item);
                        }
                    }
                }
                else if (receipeE is ClearedReceipeEvent)
                {
                    this.ItemsReceipe = new List<ItemReceipe>();
                }
            }
        }

        /// <summary>
        /// Searches the specified key word.
        /// </summary>
        /// <param name="keyWord">The key word.</param>
        /// <returns></returns>
        public async Task<bool> search(string keyWord)
        {
            bool error = false;
            try
            {
                progressBar.Visibility = Visibility.Visible;
                this.Model.ClearReceipes();
                error = await this.retriever.GetData(keyWord, 200, 1, Model);
                this.resultsFoundViewSource.Source = this.ItemsReceipe;
                progressBar.Visibility = Visibility.Collapsed;
                Check_n_Cook.ScrollToSection(Check_n_Cook.Sections[2]);
            }
            catch (Exception e)
            {
                error = true;
            }
            
            return error;
        }
        /// <summary>
        /// Appelé immédiatement après le déchargement de la Page et ne représente plus la source actuelle d'un Frame parent.
        /// </summary>
        /// <param name="e">Données d'événement qui peuvent être examinés en remplaçant le code. Les données d'événement représentent la navigation qui a déchargé la Page active.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        /// <summary>
        /// Searches the confirmation.
        /// </summary>
        public async void searchConfirmation()
        {

            await Model.ExtractPersonnalReceipes();


            if (!this.textBoxSearch.Text.Equals("Entrer une recette...") || txtIngredientSearch == null || txtIngredientSearch.Text == null || txtIngredientSearch.Text.Equals("") || txtIngredientSearch.Text.Equals("Entrez une liste ex : citron-riz-poulet"))
            {

                if (!await this.search(this.textBoxSearch.Text))
                {
                    emptyResultMessage();
                }
                if (txtIngredientSearch != null)
                {
                    txtIngredientSearch.Text = "Entrez une liste ex : citron-riz-poulet";
                }

            }
            else
            {
                if (txtIngredientSearch != null)
                {
                    ingredientSearch = txtIngredientSearch.Text.Split('-', ',', '_', '\n', '/');
                    if (await this.searchIngredients(ingredientSearch))
                    {
                        emptyResultMessage();
                    }
                    textBoxSearch.Text = "Entrer une recette...";

                }

            }

            
        }

        /// <summary>
        /// Empties the result message.
        /// </summary>
        public void emptyResultMessage(){
            if (Model.Receipes.Count == 0)
            {

                FlyoutBase fly = FlyoutBase.GetAttachedFlyout(this.resultsSection);
                fly.ShowAt(this.resultsSection);
            }
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            searchConfirmation();

        }

        /// <summary>
        /// Searches the ingredients.
        /// </summary>
        /// <param name="keyWords">The key words.</param>
        /// <returns></returns>
        public async Task<bool> searchIngredients(string[] keyWords)
        {
            bool error = false;
            try
            {

                progressBar.Visibility = Visibility.Visible;


                error = await this.retriever.GetDataByIngredients(keyWords, 100, 1, Model);
                this.resultsFoundViewSource.Source = this.ItemsReceipe;


                progressBar.Visibility = Visibility.Collapsed;

                Check_n_Cook.ScrollToSection(Check_n_Cook.Sections[2]);

            }
            catch (Exception ex)
            {
                error = true;
            }
            return error;

        }

        /// <summary>
        /// Handles the GotFocus event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text == "Entrer une recette...")
                {
                    textbox.Text = "";
                }
            }
        }

        /// <summary>
        /// Handles the LostFocus event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_LostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text == "")
                {
                    textbox.Text = "Entrer une recette...";
                }
            }
        }

        /// <summary>
        /// The text box search
        /// </summary>
        private TextBox textBoxSearch;

        /// <summary>
        /// Handles the Loaded event of the textboxSearchReceipe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void textboxSearchReceipe_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            textBoxSearch = sender as TextBox;
        }

        /// <summary>
        /// Goes to shopping list.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToShoppingList(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ShoppingList), this.Model);
            }
        }

        /// <summary>
        /// Goes to about crew.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToAboutCrew(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(AboutCrew));
            }
        }

        /// <summary>
        /// Handles the Click event of the ItemResult control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        public void ItemResult_Click(object sender, ItemClickEventArgs e)
        {
            ItemReceipe item = ((ItemReceipe)e.ClickedItem);
            this.Model.SelectReceipe(item.Receipe);
            this.Frame.Navigate(typeof(ReceipeDetail), this.Model);
        }

        /// <summary>
        /// Handles the Click event of the GoToPlanningReceipe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToPlanningReceipe_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(PlanningReceipe), this.Model);
            }
        }

        /// <summary>
        /// Gets or sets the advanced.
        /// </summary>
        /// <value>
        /// The advanced.
        /// </value>
        public Button advanced { get; set; }
        /// <summary>
        /// Handles the Click event of the advancedSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void advancedSearch_Click(object sender, RoutedEventArgs e)
        {
            advanced = sender as Button;
            advanced.Flyout.ShowAt(advanced);

        }

        /// <summary>
        /// Handles the Click event of the DeleteConfirmation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteConfirmation_Click(object sender, RoutedEventArgs e)
        {
            Button delete = sender as Button;
            advanced.Flyout.Hide();
            searchConfirmation();

        }

        /// <summary>
        /// Handles the Checked event of the chkDishType1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void chkDishType1_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox control = sender as CheckBox;
            if (((string)control.Content).Equals("Plat"))
            {
                dishTypeSearch.Add("Plat principal");
            }
            else if (((string)control.Content).Equals("Dessert"))
            {
                dishTypeSearch.Add("Dessert");
            }
            else if (((string)control.Content).Equals("Entrée"))
            {
                dishTypeSearch.Add("Entree");
            }

            if (((string)control.Content).Equals("Toutes"))
            {

                retriever.AdvancedDifficulty = 0;
                if (sliderSearch != null)
                {
                    sliderSearch.IsEnabled = false;

                }


            }

            if (((string)control.Content).Equals("Végétarien"))
            {
                this.retriever.AdvancedVegetarian = true;
            }

            if (((string)control.Content).Equals("Sans alcool"))
            {
                this.retriever.AdvancedAlcool = false;
            }

        }

        /// <summary>
        /// Handles the Unchecked event of the chkDishType3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void chkDishType3_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox control = sender as CheckBox;
            if (((string)control.Content).Equals("Plat"))
            {
                dishTypeSearch.Remove("Plat principal");
            }
            else if (((string)control.Content).Equals("Dessert"))
            {
                dishTypeSearch.Remove("Dessert");
            }
            else if (((string)control.Content).Equals("Entrée"))
            {
                dishTypeSearch.Remove("Entree");
            }

            if (((string)control.Content).Equals("Toutes"))
            {

                retriever.AdvancedDifficulty = (int)sliderSearch.Value;
                sliderSearch.IsEnabled = true;

            }
            if (((string)control.Content).Equals("Végétarien"))
            {
                this.retriever.AdvancedVegetarian = false;
            }

            if (((string)control.Content).Equals("Sans alcool"))
            {
                this.retriever.AdvancedAlcool = true;
            }


        }

        /// <summary>
        /// Handles the ValueChanged event of the Slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RangeBaseValueChangedEventArgs"/> instance containing the event data.</param>
        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

            Slider slider = sender as Slider;
            retriever.AdvancedDifficulty = (int)slider.Value;

        }

        /// <summary>
        /// Handles the KeyDown event of the textboxSearchReceipe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Input.KeyRoutedEventArgs"/> instance containing the event data.</param>
        private void textboxSearchReceipe_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                searchConfirmation();
            }
        }

        /// <summary>
        /// Handles the Loaded event of the Slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            sliderSearch = sender as Slider;
        }

        /// <summary>
        /// Handles the Loaded event of the progress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void progress_Loaded(object sender, RoutedEventArgs e)
        {
            this.progressBar = sender as ProgressBar;
        }

        /// <summary>
        /// Handles the Click event of the GoToShop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToShop_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ShopsList), this.Model);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            txtIngredientSearch = sender as TextBox;
        }

        /// <summary>
        /// Handles the Click event of the GoToShoppingList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToShoppingList_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShoppingList), this.Model);
        }

        /// <summary>
        /// Handles the Click event of the GoToAddReceipe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GoToAddReceipe_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(AddReceipe));
            }
        }

        /// <summary>
        /// Handles the Click event of the ItemFavorite control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void ItemFavorite_Click(object sender, ItemClickEventArgs e)
        {
            Receipe item = ((Receipe)e.ClickedItem);
            this.Model.SelectReceipe(item);
            this.Frame.Navigate(typeof(ReceipeDetail), this.Model);
        }
    }
}
