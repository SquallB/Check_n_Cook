using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


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
        private List<ShoppingListGroup> shoppingListGroup;
        private bool isModifyingList;
        private List<Button> buttonIngredient;
        private ComboBox groupIngredient;
        private Button addItemButton;

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
            this.shoppingListGroup = new List<ShoppingListGroup>();
            this.buttonIngredient = new List<Button>();
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
            if (e.NavigationParameter is AppModel)
            {
                this.model = (AppModel)e.NavigationParameter;
                this.model.AddView(this);

                this.shoppingListGroup.Clear();
                this.shoppingListGroup = new List<ShoppingListGroup>();
                foreach (ShoppingListGroup group in this.model.ShoppingList.Values)
                {
                    this.shoppingListGroup.Add(group);
                }

            }

            this.DefaultViewModel["Groups"] = this.shoppingListGroup;
            this.RegisterForPrinting();
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
            this.PagesToPrint.Add(new ShoppingListPrintPage(this.shoppingListGroup));
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

                foreach (Button b in this.buttonIngredient)
                {
                    b.Visibility = Visibility.Collapsed;
                }

                this.nameIngredient.Visibility = Visibility.Collapsed;
                this.quantityIngredient.Visibility = Visibility.Collapsed;
                this.unityIngredient.Visibility = Visibility.Collapsed;
                this.groupIngredient.Visibility = Visibility.Collapsed;
                this.addItemButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.modifyListButton.Content = "Arrêter les modifications sur la liste de course";

                foreach (Button button in this.deleteButtons)
                {
                    button.Visibility = Visibility.Visible;
                }

                foreach (Button b in this.buttonIngredient)
                {
                    b.Visibility = Visibility.Visible;
                }

                this.nameIngredient.Visibility = Visibility.Visible;
                this.quantityIngredient.Visibility = Visibility.Visible;
                this.unityIngredient.SelectedItem = this.unityIngredient.Items[0];
                this.unityIngredient.Visibility = Visibility.Visible;
                this.groupIngredient.Visibility = Visibility.Visible;
                this.addItemButton.Visibility = Visibility.Visible;
            }

            this.isModifyingList = !isModifyingList;
        }

        private List<Button> deleteButtons;
        private void ButtonShoppingList_Loaded(object sender, RoutedEventArgs e)
        {
            this.deleteButtons.Add((Button)sender);

            if (this.isModifyingList)
            {
                ((Button)sender).Visibility = Visibility.Visible;
            }
        }
        public void Refresh(Event e)
        {
            if (e is IngredientEvent)
            {
                this.shoppingListGroup.Clear();
                this.shoppingListGroup = new List<ShoppingListGroup>();
                foreach (ShoppingListGroup listGroup in this.model.ShoppingList.Values)
                {
                    this.shoppingListGroup.Add(listGroup);
                }

                this.DefaultViewModel["Groups"] = shoppingListGroup;
            }
            else if (e is RemovedShoppingListGroupEvent)
            {
                RemovedShoppingListGroupEvent srcEvt = (RemovedShoppingListGroupEvent)e;
                AppModel model = (AppModel)srcEvt.Model;

                this.shoppingListGroup.Clear();
                this.shoppingListGroup = new List<ShoppingListGroup>();
                foreach (ShoppingListGroup listGroup in this.model.ShoppingList.Values)
                {
                    this.shoppingListGroup.Add(listGroup);
                }


                this.DefaultViewModel["Groups"] = this.shoppingListGroup;

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
                this.unityIngredient.SelectedItem = this.unityIngredient.Items[0];
            }
        }

        private async void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            String name = this.nameIngredient.Text;
            String quantity = this.quantityIngredient.Text;
            String unity = this.unityIngredient.SelectedItem.ToString();
            Ingredient ingredient = new Ingredient(name, quantity, unity);


            if (name != "" && this.groupIngredient.SelectedItem != null)
            {
                String groupName = this.groupIngredient.SelectedItem.ToString();
                this.model.AddIngredientToShoppingList(ingredient, groupName);

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile shoppingListFile = await folder.CreateFileAsync("shoppingList.json", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(shoppingListFile, this.model.StringifyShoppingList());
            }
        }

        private void AddIngredientButton_Loaded(object sender, RoutedEventArgs e)
        {
            this.addItemButton = (Button)sender;
        }


        private void ComboBoxGroup_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                this.groupIngredient = (ComboBox)sender;

                foreach (ShoppingListGroup group in this.model.ShoppingList.Values)
                {
                    this.groupIngredient.Items.Add(group.Name);
                }

                this.groupIngredient.SelectedItem = this.groupIngredient.Items[0];
            }
        }

        private async void RemoveShoppingList_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button but = (Button)sender;
                ShoppingListGroup data = (ShoppingListGroup)but.DataContext;

                this.model.RemoveShoppingListGroup(data.Name);

                object searchedItem = null;
                int i = 0;
                while (i < this.groupIngredient.Items.Count && searchedItem == null)
                {
                    object item = this.groupIngredient.Items[i];
                    if (item.ToString() == data.Name)
                    {
                        searchedItem = item;
                    }

                    i++;
                }

                this.groupIngredient.Items.Remove(searchedItem);

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile shoppingListFile = await folder.CreateFileAsync("shoppingList.json", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(shoppingListFile, this.model.StringifyShoppingList());

                this.PagesToPrint.Clear();
                PreparePrintContent();
            }
        }

        private void ButtonIngredient_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button but = (Button)sender;
                this.buttonIngredient.Add(but);

                if (this.isModifyingList)
                {
                    but.Visibility = Visibility.Visible;
                }
            }
        }

        private async void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button but = (Button)sender;
                ItemIngredient item = (ItemIngredient)but.DataContext;

                this.model.RemoveIngredientFromShoppingList(item, item.Group);

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile shoppingListFile = await folder.CreateFileAsync("shoppingList.json", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(shoppingListFile, this.model.StringifyShoppingList());

                this.PagesToPrint.Clear();
                PreparePrintContent();
            }
        }

        private void EnterIngredient_Focus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals("Entrer un ingrédient..."))
                {
                    textbox.Text = "";
                }
            }
        }

        private void EnterIngredient_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals(""))
                {
                    textbox.Text = "Entrer un ingrédient...";
                }
            }
        }

        private void Quantity_Focus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals("Quantité"))
                {
                    textbox.Text = "";
                }
            }
        }

        private void Quantity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;

                if (textbox.Text.Equals(""))
                {
                    textbox.Text = "Quantité";
                }
            }
        }

    }
}
