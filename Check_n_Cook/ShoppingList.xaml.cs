using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Views;
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

// Pour en savoir plus sur le modèle d'élément Page Hub, consultez la page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class ShoppingList : BasePrintPage, View
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private AppModel model;
        private Time time;
        private Dictionary<string, Ingredient> ingredients;
        private Dictionary<string, Dictionary<string, Ingredient>> tempIngredients;
        private bool isModifyingList;

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

        public ShoppingList()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            this.deleteButtons = new List<Button>();
            this.isModifyingList = false;
            this.ingredients = new Dictionary<string, Ingredient>();
            this.tempIngredients = new Dictionary<string, Dictionary<string, Ingredient>>();
        }

        private void addIngredient(Ingredient ingredient, Dictionary<string, Ingredient> groupDictionnary)
        {
            if (groupDictionnary.ContainsKey(ingredient.name))
            {
                if (ingredient.quantity != String.Empty && ingredient.quantity != null)
                {
                    String quantity = HandleQuantity(groupDictionnary[ingredient.name].quantity, ingredient.quantity).ToString();
                    groupDictionnary[ingredient.name].quantity = quantity;
                }
            }
            else
            {
                groupDictionnary[ingredient.name] = ingredient.ToClone();
            }
        }

        private void removeIngredient(Ingredient ingredient, Dictionary<string, Ingredient> groupDictionnary)
        {
            groupDictionnary.Remove(ingredient.name);
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
            GoToModifyReceipeListEvent evnt = e.NavigationParameter as GoToModifyReceipeListEvent;

            if (evnt != null)
            {
                this.model = evnt.AppModel;
                this.model.AddView(this);
                this.time = evnt.Time;

                foreach (ShoppingListGroup group in this.model.ShoppingList.Values)
                {
                    Dictionary<string, Ingredient> groupDictionnary = new Dictionary<string, Ingredient>();

                    foreach (Ingredient ingredient in group.Ingredients)
                    {
                        this.addIngredient(ingredient, groupDictionnary);
                        this.addIngredient(ingredient, this.ingredients);
                    }

                    this.tempIngredients[group.Name] = groupDictionnary;
                }

                this.ingredientsViewSource.Source = ingredients.Values;
            }

            this.RegisterForPrinting();
        }

        public int HandleQuantity(string quantity, string quantityToAdd)
        {

            string[] splitFraction1 = quantity.Split(new Char[] { '/' });
            string[] splitFraction2 = quantityToAdd.Split(new Char[] { '/' });
            int a = -1, b = -1, c = -1, d = -1;

            if (splitFraction1.Length == 1)
            {
                a = Int32.Parse(splitFraction1[0]);
            }
            if (splitFraction1.Length == 2)
            {
                a = Int32.Parse(splitFraction1[0]);
                b = Int32.Parse(splitFraction1[1]);
            }
            if (splitFraction2.Length == 1)
            {
                c = Int32.Parse(splitFraction2[0]);
            }
            if (splitFraction2.Length == 2)
            {
                c = Int32.Parse(splitFraction2[0]);
                d = Int32.Parse(splitFraction2[1]);
            }

            double result = 0;

            if (b != -1 && d != -1)
            {
                result = (a * d + c * b) / (b * d);
            }
            else if (b != -1 && d == -1)
            {
                result = (a + c * b) / b;
            }
            else if (b == -1 && d != -1)
            {
                result = (a * d + c) / d;
            }
            else if (b == -1 && d == -1)
            {
                result = a + c;
            }

            double resultTrun = Math.Truncate(result);

            if (result == resultTrun)
            {
                return (int)result;
            }
            else
            {

                return ((int)resultTrun) + 1;
            }
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
        /// Provide print content for scenario 1 first page
        /// </summary>
        protected override void PreparePrintContent()
        {
            this.PagesToPrint.Add(new ShoppingListPrintPage(this.tempIngredients));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();
        }

        private void ModifyList_Click(object sender, RoutedEventArgs e)
        {
            if (this.isModifyingList)
            {
                this.modifyListButton.Content = "Modifier la liste de course";

                foreach (Button button in this.deleteButtons)
                {
                    button.Visibility = Visibility.Collapsed;
                }

                this.newIngredientText.Visibility = Visibility.Collapsed;
                this.nameIngredient.Visibility = Visibility.Collapsed;
                this.quantityIngredient.Visibility = Visibility.Collapsed;
                this.unityIngredient.Visibility = Visibility.Collapsed;
                this.addItemButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.modifyListButton.Content = "Arrêter les modifications sur la liste de course";

                foreach (Button button in this.deleteButtons)
                {
                    button.Visibility = Visibility.Visible;
                }

                this.newIngredientText.Visibility = Visibility.Visible;

                this.nameIngredient.Text = "";
                this.nameIngredient.Visibility = Visibility.Visible;

                this.quantityIngredient.Text = "";
                this.quantityIngredient.Visibility = Visibility.Visible;

                this.unityIngredient.SelectedItem = this.unityIngredient.Items[0];
                this.unityIngredient.Visibility = Visibility.Visible;

                this.addItemButton.Visibility = Visibility.Visible;
            }

            this.isModifyingList = !isModifyingList;
        }

        private List<Button> deleteButtons;
        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            this.deleteButtons.Add((Button)sender);

            if (this.isModifyingList)
            {
                ((Button)sender).Visibility = Visibility.Visible;
            }
        }

        private async void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button) {
                Button button = (Button)sender;

                if(button.DataContext is Ingredient) {
                    Ingredient ingredient = (Ingredient)button.DataContext;
                    //this.model.RemoveIngredientFromShoppingList(ingredient);
                }

                this.deleteButtons.Remove(button);

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile shoppingListFile = await folder.CreateFileAsync("shoppingList.json", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(shoppingListFile, this.model.StringifyShoppingList());
            }
        }

        public void Refresh(Event e)
        {
            if (e is IngredientEvent)
            {
                IngredientEvent ingredientE = (IngredientEvent)e;
                Ingredient ingredient = ingredientE.Ingredient;
                String groupName = ingredientE.GroupName;

                if (ingredientE is AddedIngredientEvent)
                {
                    this.addIngredient(ingredient, this.tempIngredients[groupName]);
                }
                else if (ingredientE is RemovedIngredientEvent)
                {
                    this.removeIngredient(ingredient, this.tempIngredients[groupName]);
                }

                this.ingredients.Clear();
                this.ingredients = new Dictionary<string, Ingredient>();

                foreach (Dictionary<string, Ingredient> group in this.tempIngredients.Values)
                {
                    foreach (Ingredient groupIngredient in group.Values)
                    {
                        this.ingredients.Add(groupIngredient.name, groupIngredient);
                    }
                }
                
                this.ingredientsViewSource.Source = ingredients.Values;
            }
        }

        private Button modifyListButton;
        private void ModifyList_Loaded(object sender, RoutedEventArgs e)
        {
            this.modifyListButton = (Button)sender;
        }

        private TextBox nameIngredient;
        private void TextBoxNameIngredient_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                this.nameIngredient = (TextBox)sender;
            }
        }

        private TextBox quantityIngredient;
        private void TextBoxQuantity_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                this.quantityIngredient = (TextBox)sender;
            }
        }

        private ComboBox unityIngredient;
        private void ComboBoxUnity_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                this.unityIngredient = (ComboBox)sender;
                foreach (string unity in UnityAvailable.GetUnity())
                {
                    this.unityIngredient.Items.Add(unity);
                }
                this.unityIngredient.SelectedIndex = 0;
            }
        }

        private async void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            String name = this.nameIngredient.Text;
            String quantity = this.quantityIngredient.Text;
            String unity = this.unityIngredient.SelectedItem.ToString();
            Ingredient ingredient = new Ingredient(name, quantity, unity);
            String groupName = this.groupIngredient.SelectedItem.ToString();
            this.model.AddIngredientToShoppingList(ingredient, groupName);

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile shoppingListFile = await folder.CreateFileAsync("shoppingList.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(shoppingListFile, this.model.StringifyShoppingList());
        }

        private Button addItemButton;
        private void AddIngredientButton_Loaded(object sender, RoutedEventArgs e)
        {
            this.addItemButton = (Button)sender;
        }

        private TextBlock newIngredientText;
        private void NewIngredientText_Loaded(object sender, RoutedEventArgs e)
        {
            this.newIngredientText = (TextBlock)sender;
        }

        private ComboBox groupIngredient;
        private void ComboBoxGroup_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                this.groupIngredient = (ComboBox)sender;

                foreach (ShoppingListGroup group in this.model.ShoppingList.Values)
                {
                    this.groupIngredient.Items.Add(group.Name);
                }

                this.unityIngredient.SelectedIndex = 0;
            }
        }
    }
}
