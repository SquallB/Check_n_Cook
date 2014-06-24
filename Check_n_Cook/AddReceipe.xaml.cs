using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace Check_n_Cook
{
    /// <summary>
    /// This page displays a collection of element
    /// </summary>
    public sealed partial class AddReceipe : Page, View
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
        /// The new receipe
        /// </summary>
        private NewReceipe newReceipe;
        /// <summary>
        /// The ingredients news
        /// </summary>
        private Dictionary<string, Ingredient> ingredientsNews;
        /// <summary>
        /// The name ingredient
        /// </summary>
        private TextBox nameIngredient;
        /// <summary>
        /// The quantity ingredient
        /// </summary>
        private TextBox quantityIngredient;
        /// <summary>
        /// The untity ingredient
        /// </summary>
        private ComboBox untityIngredient;
        /// <summary>
        /// The receipe name
        /// </summary>
        private string receipeName;
        /// <summary>
        /// The author name
        /// </summary>
        private string authorName;
        /// <summary>
        /// The meal type index
        /// </summary>
        private int mealTypeIndex;
        /// <summary>
        /// The cooking time
        /// </summary>
        private string cookingTime;
        /// <summary>
        /// The difficulty level index
        /// </summary>
        private int difficultyLevelIndex;
        /// <summary>
        /// The cost level index
        /// </summary>
        private int costLevelIndex;
        /// <summary>
        /// To do instructions
        /// </summary>
        private string toDoInstructions;
        /// <summary>
        /// The URL for image
        /// </summary>
        private string urlForImage;
        /// <summary>
        /// The instruction
        /// </summary>
        private TextBox instruction;
        /// <summary>
        /// The image
        /// </summary>
        private Image image;

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
        /// Initializes a new instance of the <see cref="AddReceipe"/> class.
        /// </summary>
        public AddReceipe()
        {
            this.InitializeComponent();
            this.ingredientsNews = new Dictionary<string, Ingredient>();
            this.navigationHelper = new NavigationHelper(this);
            this.newReceipe = new NewReceipe();
            this.newReceipe.AddView(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            receipeName = "";
            authorName = "";
            mealTypeIndex = 0;
            cookingTime = "";
            difficultyLevelIndex = 0;
            costLevelIndex = 0;
            toDoInstructions = "";
            urlForImage = "";

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
        /// Delete an ingredient
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteIngredient_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.Frame != null && sender is Button)
            {
                Button but = (Button)sender;
                Ingredient ing = but.DataContext as Ingredient;

                if (ing != null)
                {
                    this.newReceipe.RemoveIngredient(ing.name, ing.quantity, ing.unity);
                }
            }
        }

        /// <summary>
        /// Add an ingredient
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (nameIngredient.Text != null && nameIngredient.Text != "" && nameIngredient.Text != "Nom de l'ingrédient")
            {
                this.newReceipe.AddIngredient(nameIngredient.Text, quantityIngredient.Text, untityIngredient.Items[untityIngredient.SelectedIndex].ToString());
                nameIngredient.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
                nameIngredient.Text = "";
            }
            else
            {
                nameIngredient.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            }
        }

        /// <summary>
        /// Store the textbox of the name of the receipe
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBoxNameIngredient_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                this.nameIngredient = (TextBox)sender;
            }
        }

        /// <summary>
        /// Store the textbox of the name of the quanity of the ingredient
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBoxQuantity_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                this.quantityIngredient = (TextBox)sender;
            }
        }
        /// <summary>
        /// Handles the Loaded event of the TextBlock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Initialise the combobox of quantity with the quantity available
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ComboBoxUnity_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                this.untityIngredient = (ComboBox)sender;
                foreach (string unity in UnityAvailable.GetUnity())
                {
                    this.untityIngredient.Items.Add(unity);
                }
                this.untityIngredient.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Refreshes the page with the specific event given.
        /// </summary>
        /// <param name="e">The e.</param>
        public void Refresh(Event e)
        {
            if (e is ModifyIngredients)
            {
                ModifyIngredients srcEvnt = (ModifyIngredients)e;
                NewReceipe modelEvnt = (NewReceipe)srcEvnt.Model;
                this.ingredientsNews.Clear();
                this.ingredientsNews = null;
                this.ingredientsNews = new Dictionary<string, Ingredient>();

                foreach (Ingredient ing in modelEvnt.GetIngredientsAdded().Values)
                {
                    ingredientsNews.Add(ing.name, ing);
                }

                this.ingredientsViewSource.Source = ingredientsNews.Values;
            }
        }

        /// <summary>
        /// Initialize the ComboBox of the type of the receipe
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                var mealType = (ComboBox)sender;

                mealType.Items.Add("Entrée");
                mealType.Items.Add("Plat");
                mealType.Items.Add("Dessert");

                mealType.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// Initialize the ComboBox of the difficulty of the receipe
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ComboBox_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                var difficultyLevel = (ComboBox)sender;
                difficultyLevel.Items.Add(" ");

                for (int i = 1; i < 5; i++)
                {
                    difficultyLevel.Items.Add(i);

                }

                difficultyLevel.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// Initialize the ComboBox of the cost of the receipe
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CostLevelCombo_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                var costlevel = (ComboBox)sender;
                costlevel.Items.Add(" ");

                for (int i = 1; i <= 3; i++)
                {
                    costlevel.Items.Add(i);

                }

                costlevel.SelectedIndex = 0;
            }


        }

        /// <summary>
        /// Store the name of the receipe given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void ReceipeTitle_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.receipeName = tb.Text;
            }

        }

        /// <summary>
        /// Store the name of the author of the receipe given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void authorName_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.authorName = tb.Text;
            }

        }

        /// <summary>
        /// Store the meal type given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void MealType_Changed(object sender, SelectionChangedEventArgs e)
        {
            var tb = (ComboBox)sender;
            if (tb.SelectedIndex != null)
            {
                this.mealTypeIndex = tb.SelectedIndex;
            }


        }

        /// <summary>
        /// Store the time of cook given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void CookingTime_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.cookingTime = tb.Text;
            }

        }

        /// <summary>
        /// Store difficulty of the receipe given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void DifficultyLevel_Changed(object sender, SelectionChangedEventArgs e)
        {
            var tb = (ComboBox)sender;
            if (tb.SelectedIndex != null)
            {
                this.difficultyLevelIndex = tb.SelectedIndex;
            }


        }

        /// <summary>
        /// Store the cost of the receipe given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void CostLevel_Changed(object sender, SelectionChangedEventArgs e)
        {
            var tb = (ComboBox)sender;
            if (tb.SelectedIndex != null)
            {
                this.costLevelIndex = tb.SelectedIndex;
            }


        }

        /// <summary>
        /// Store the instruction of the receipe given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void ReceipeContent_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.toDoInstructions = tb.Text;
            }

        }

        /// <summary>
        /// Store the image of the receipe given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void ImageLink_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.urlForImage = tb.Text;
                var bitmapImage = new BitmapImage();
                try
                {

                    bitmapImage.UriSource = new Uri(tb.Text);
                    image.Source = bitmapImage;
                }
                catch (Exception ex)
                {

                }
            }


        }

        /// <summary>
        /// Check if the file exists
        /// </summary>
        /// <param name="sf">The sf.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private async Task<Boolean> fileExists(StorageFolder sf, string fileName)
        {
            try
            {
                StorageFile sfile = await sf.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Add the receipe given by the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void AddReceipe_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.receipeName != "")
            {
                DishType dt = null;
                if (mealTypeIndex == 0)
                {
                    dt = DishType.GetInstance("Entrée");
                }
                else if (mealTypeIndex == 1)
                {
                    dt = DishType.GetInstance("Plat");


                }
                else if (mealTypeIndex == 2)
                {


                    dt = DishType.GetInstance("Dessert");

                }

                Receipe rec = new Receipe(this.receipeName, this.authorName, DateTime.Today, dt, -1, this.difficultyLevelIndex, this.costLevelIndex, false, false);
                rec.Id = -1;
                rec.Image = this.urlForImage;
                rec.ToDoInstructions = this.toDoInstructions;
                foreach (Ingredient ing in this.ingredientsNews.Values)
                {
                    rec.ingredients.Add(ing);
                }

                StorageFolder folder = KnownFolders.PicturesLibrary;
                if (await fileExists(folder, "personalReceipes.json") == false)
                {
                    StorageFile receipeFile = await folder.CreateFileAsync("personalReceipes.json", CreationCollisionOption.ReplaceExisting);
                    JsonObject jsonObject = new JsonObject();
                    JsonArray jsonArray = new JsonArray();

                    jsonArray.Add(rec.ToJsonObject());


                    jsonObject["Receipes"] = jsonArray;

                    jsonObject.Stringify();
                    await Windows.Storage.FileIO.WriteTextAsync(receipeFile, jsonObject.Stringify());


                }
                else
                {
                    StorageFile receipeFile = await folder.GetFileAsync("personalReceipes.json");
                    String jsonString = await FileIO.ReadTextAsync(receipeFile);
                    JsonObject jsonObject = JsonObject.Parse(jsonString);
                    JsonArray jsonArray = jsonObject.GetNamedArray("Receipes");
                    jsonArray.Add(rec.ToJsonObject());


                    jsonObject["Receipes"] = jsonArray;

                    jsonObject.Stringify();
                    await Windows.Storage.FileIO.WriteTextAsync(receipeFile, jsonObject.Stringify());

                }
            }

        }

        /// <summary>
        /// Handles the ingredient textbox when he got focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void NameIngredientGotFocus(object sender, RoutedEventArgs e)
        {

            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals("Nom de l'ingrédient..."))
                {
                    textbox.Text = "";
                }
            }
        }

        /// <summary>
        /// Handles the texbox of the ingredient when he lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void NameINgredientLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals(""))
                {
                    textbox.Text = "Nom de l'ingrédient...";
                }
            }

        }

        /// <summary>
        /// Handles the quantity textbox when he got focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void QuantityGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals("Quantité..."))
                {
                    textbox.Text = "";
                }
            }
        }

        /// <summary>
        /// Handles the quantity textbox when he lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void QuanityLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals(""))
                {
                    textbox.Text = "Quantité...";
                }
            }
        }

        /// <summary>
        /// Store the textbox of the instruction
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void InstructionLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                this.instruction = (TextBox)sender;
            }
        }

        /// <summary>
        /// Handles the instruction textbox when he got focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void InstructionFocus(object sender, RoutedEventArgs e)
        {

            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;
                if (textbox.Text.Equals("Vos instructions..."))
                {
                    textbox.Text = "";
                }
            }
        }

        /// <summary>
        /// Handles the instruction textbox when he lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void InstructionLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals(""))
                {
                    textbox.Text = "Vos instruction...";
                }
            }
        }

        /// <summary>
        /// Store the image
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ImageLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image)
            {
                this.image = (Image)sender;
            }
        }

        /// <summary>
        /// Handles the image textbox when he got focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ImageFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals("URL de l'image..."))
                {
                    textbox.Text = "";
                }
            }
        }

        /// <summary>
        /// Handles the image textbox when he lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ImageLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals(""))
                {
                    textbox.Text = "URL de l'image...";
                }
            }
        }
    }

}
