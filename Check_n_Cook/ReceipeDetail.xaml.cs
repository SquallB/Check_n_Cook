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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page Hub, consultez la page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class ReceipeDetail : BasePrintPage
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public AppModel Model { get; set; }
        public Image ImageReceipe { get; set; }
        public TextBlock ReceipeInstruction { get; set; }
        private string Date;
        private string TimeOfDay;
        private Receipe receipe;
        private List<ItemIngredient> ingredients;
        private List<Ingredient> currentShoppingList;
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

        private async void AddReceipeFavorite_Click(object sender, RoutedEventArgs e)
        {
            this.Model.AddReceipeFavorite(this.receipe);
            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile receipeFile = await folder.CreateFileAsync("receipesFavorite.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(receipeFile, this.Model.StringifyFavouriteReceipes());
        }

        private async void AddReceipeList_Click(object sender, RoutedEventArgs e)
        {
            this.Model.AddReceipeList(this.receipe, this.TimeOfDay, this.Date);
            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile receipeFile = await folder.CreateFileAsync("receipes.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(receipeFile, this.Model.StringifyReceipesList());
        }

        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                DateTime dateTime = datePicker.Date.Date;
                this.Date = dateTime.Date.ToString("d");
            }
        }

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


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                this.TimeOfDay = comboBox.Items[comboBox.SelectedIndex].ToString();
            }

        }
        public WebView wb;
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

        private async void PrintReceipe_Click(object sender, RoutedEventArgs e)
        {
            await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();
        }

        private void SelectIngredients_Click(object sender, RoutedEventArgs e)
        {
            this.button_validateIngredients.Visibility = Visibility.Visible;
            foreach (CheckBox checkbox in this.checkboxes)
            {
                checkbox.Visibility = Visibility.Visible;
                checkbox.IsChecked = true;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox checkbox = (CheckBox)sender;
                if (checkbox.DataContext is Ingredient)
                {
                    Ingredient ingredient = (Ingredient)checkbox.DataContext;
                    this.currentShoppingList.Add(ingredient);
                }
            }
        }

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
        private void button_validateIngredients_Loaded(object sender, RoutedEventArgs e)
        {
            this.button_validateIngredients = (Button)sender;
        }

        private List<CheckBox> checkboxes;
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            checkboxes.Add((CheckBox)sender);
        }


    }
}
