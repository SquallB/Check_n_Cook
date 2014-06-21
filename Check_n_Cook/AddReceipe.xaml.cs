using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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
    public sealed partial class AddReceipe : Page, View
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private NewReceipe newReceipe;
        private Dictionary<string, Ingredient> ingredientsNews;
        private TextBox nameIngredient;
        private TextBox quantityIngredient;
        private ComboBox untityIngredient;
        private string receipeName;
        private string authorName;
        private int mealTypeIndex;
        private string cookingTime;
        private int difficultyLevelIndex;
        private int costLevelIndex;
        private string toDoInstructions;
        private string urlForImage;

        
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
            costLevelIndex = 0 ;
            toDoInstructions = "";
            urlForImage = "";

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

        private void TextBoxNameIngredient_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                this.nameIngredient = (TextBox)sender;
            }
        }

        private void TextBoxQuantity_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                this.quantityIngredient = (TextBox)sender;
            }
        }
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }

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

        private void ComboBox_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                var difficultyLevel = (ComboBox)sender;
                difficultyLevel.Items.Add(" ");

                for (int i = 1; i <= 5; i++)
                {
                    difficultyLevel.Items.Add(i);

                }

                    difficultyLevel.SelectedIndex = 0;
            }

        }

        private void CostLevelCombo_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                var costlevel = (ComboBox)sender;
                costlevel.Items.Add(" ");

                for (int i = 1; i <= 5; i++)
                {
                    costlevel.Items.Add(i);

                }

                costlevel.SelectedIndex = 0;
            }


        }

        private void ReceipeTitle_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox) sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.receipeName = tb.Text;
            }

        }

        private void authorName_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.authorName = tb.Text;
            }

        }

        private void MealType_Changed(object sender, SelectionChangedEventArgs e)
        {
            var tb = (ComboBox)sender;
            if (tb.SelectedIndex != null)
            {
                this.mealTypeIndex = tb.SelectedIndex;
            }


        }

        private void CookingTime_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.cookingTime = tb.Text;
            }

        }

        private void DifficultyLevel_Changed(object sender, SelectionChangedEventArgs e)
        {
            var tb = (ComboBox)sender;
            if (tb.SelectedIndex != null)
            {
                this.difficultyLevelIndex = tb.SelectedIndex;
            }


        }

        private void CostLevel_Changed(object sender, SelectionChangedEventArgs e)
        {
            var tb = (ComboBox)sender;
            if (tb.SelectedIndex != null)
            {
                this.costLevelIndex = tb.SelectedIndex;
            }


        }

        private void ReceipeContent_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.toDoInstructions = tb.Text;
            }

        }

        private void ImageLink_Changed(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text != "" && tb.Text.Length > 0)
            {
                this.urlForImage = tb.Text;
            }


        }
        private async Task<Boolean> fileExists(StorageFolder sf, string fileName)
        {
            try
            {
                StorageFile sfile =  await sf.GetFileAsync(fileName);
                return true;
            } 
            catch {
                return false;
            }
        }
        private async void AddReceipe_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.receipeName != "")
            {
                DishType dt = null;
                if(mealTypeIndex == 0) {
                    dt = DishType.GetInstance("Entrée");
                } else if(mealTypeIndex == 1){
                    dt = DishType.GetInstance("Plat");


                } else if(mealTypeIndex == 2) {

                                  
                     dt = DishType.GetInstance("Dessert");

                }

                Receipe rec = new Receipe(this.receipeName, this.authorName, DateTime.Today, dt, -1, this.difficultyLevelIndex + 1, this.costLevelIndex + 1, false, false);
                rec.Id = -1;
                rec.Image = this.urlForImage;
                rec.ToDoInstructions = this.toDoInstructions;
                foreach(Ingredient ing in this.ingredientsNews.Values) {
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

    }

}
