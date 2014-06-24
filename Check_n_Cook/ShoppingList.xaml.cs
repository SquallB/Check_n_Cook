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
    /// Page displaying the shopping list of the user, and provides tools to edit it.
    /// </summary>
    public sealed partial class ShoppingList : BasePrintPage, View
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
        private AppModel model;

        /// <summary>
        /// The groups of the shopping list
        /// </summary>
        private List<ShoppingListGroup> shoppingListGroup;

        /// <summary>
        /// True if the list is being modified, false otherwise.
        /// </summary>
        private bool isModifyingList;

        /// <summary>
        /// The list of buttons allowing to delete ingrdients.
        /// </summary>
        private List<Button> buttonIngredient;

        /// <summary>
        /// The combobox allowing to the group to which the ingredient will be added to.
        /// </summary>
        private ComboBox groupIngredient;

        /// <summary>
        /// The button adding the ingredient.
        /// </summary>
        private Button addItemButton;

        /// <summary>
        /// The textbox allowing to enter the name of the ingredient.
        /// </summary>
        private TextBox nameIngredient;

        /// <summary>
        /// The textbox allowing to enter the quantity of the ingredient.
        /// </summary>
        private TextBox quantityIngredient;

        /// <summary>
        /// The combobox allowing to choose the unity of the ingredient.
        /// </summary>
        private ComboBox unityIngredient;

        /// <summary>
        /// The list of butons allowing to delete a group of ingredients.
        /// </summary>
        private List<Button> deleteButtons;

        /// <summary>
        /// The button that (des)activates the edition mode.
        /// </summary>
        private Button modifyListButton;

        /// <summary>
        /// Used to display the groups of the shopping list along with the ingredients inside them.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingList"/> class.
        /// </summary>
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

        /// <summary>Fills the page with content given during the navigation. Every saved sate is also given when the page
        /// is created again based on a previous session.<summary>
        /// /// <param name="sender">The source of the event; in general <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data giving the first navigation parameter transmitted to 
        /// <see cref="Frame.Navigate(Type, Object)"/> during the first request of the page and
        /// a state dictionnary saved by the page during a previous session.
        /// The state won't a Null value during the first visit of the page
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

        /// <summary>
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Raises the <see cref="E:NavigatedFrom" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.UnregisterForPrinting();
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// Provide print content for the shopping list.
        /// </summary>
        protected override void PreparePrintContent()
        {
            this.PagesToPrint.Add(new ShoppingListPrintPage(this.shoppingListGroup));
        }

        /// <summary>
        /// Handles the Click event of the print button. Displays the print menu.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();
        }

        /// <summary>
        /// Handles the Click event of the ModifyList button. Turn on or off the edition mode of the list, by making visible or hidden the corresponding controls. Also changes the text of the button clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ModifyList_Click(object sender, RoutedEventArgs e)
        {
            if (this.isModifyingList)
            {
                this.modifyListButton.Content = "Modifier la liste de courses";

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
                this.modifyListButton.Content = "Arrêter les modifications sur la liste de courses";

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

        /// <summary>
        /// Handles the Loaded event of the ButtonShoppingList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ButtonShoppingList_Loaded(object sender, RoutedEventArgs e)
        {
            this.deleteButtons.Add((Button)sender);

            if (this.isModifyingList)
            {
                ((Button)sender).Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Refreshes this page with the data contained in the fired event.
        /// </summary>
        /// <param name="e">The fired event.</param>
        public void Refresh(Event e)
        {
            if (e is IngredientEvent || e is ShoppingListGroupEvent)
            {
                this.shoppingListGroup.Clear();
                this.shoppingListGroup = new List<ShoppingListGroup>();
                foreach (ShoppingListGroup listGroup in this.model.ShoppingList.Values)
                {
                    this.shoppingListGroup.Add(listGroup);
                }

                this.DefaultViewModel["Groups"] = shoppingListGroup;
            }
        }

        /// <summary>
        /// Handles the Loaded event of the ModifyList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ModifyList_Loaded(object sender, RoutedEventArgs e)
        {
            this.modifyListButton = (Button)sender;
        }

        /// <summary>
        /// Handles the Loaded event of the TextBoxNameIngredient control.
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
        /// Handles the Loaded event of the TextBoxQuantity control.
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
        /// Handles the Loaded event of the ComboBoxUnity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Click event of the AddIngredient button. Add a ingredient to the shopping list, in the selected group.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Loaded event of the AddIngredientButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddIngredientButton_Loaded(object sender, RoutedEventArgs e)
        {
            this.addItemButton = (Button)sender;
        }


        /// <summary>
        /// Handles the Loaded event of the ComboBoxGroup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Click event of the RemoveShoppingList button. Removes a group of ingredients from the shopping list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Loaded event of the ButtonIngredient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Click event of the RemoveIngredient control. Removes an ingredient from the corresponding group of the shopping list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Focus event of the EnterIngredient textbox. Removes the default text if still there.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the LostFocus event of the EnterIngredient textbox. Adds the default text if empty.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Focus event of the Quantity textbox. Removes the default text if still there.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the LostFocus event of the Quantity textbox. Adds the default text if empty.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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
