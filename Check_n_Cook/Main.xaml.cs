﻿using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page Hub, consultez la page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class Main : Page, View
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<ItemResult> ItemsResult { get; set; }
        List<string> dishTypeSearch = new List<string>();
        private URLDataRetriever retriever;
        public AppModel Model { get; set; }
        public Slider sliderSearch;
        public ProgressBar progress;
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

        public Main()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.Model = new AppModel();
            this.Model.AddView(this);
            this.retriever = new URLDataRetriever();
            this.retriever.AdvancedSearch = this.dishTypeSearch;

            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
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
            if (this.Model.FavouriteReceipes.Count == 0)
            {
                StorageFolder folder = KnownFolders.PicturesLibrary;
                try
                {
                    StorageFile receipesFile = await folder.GetFileAsync("receipesFavorite.json");
                    String jsonString = await FileIO.ReadTextAsync(receipesFile);
                    JsonObject jsonObject = JsonObject.Parse(jsonString);
                    JsonArray jsonArray = jsonObject.GetNamedArray("Receipes");
                    foreach (var jsonReceipe in jsonArray)
                    {
                        this.Model.AddReceipeFavorite(new Receipe(jsonReceipe.Stringify()));
                    }
                }
                catch (FileNotFoundException ex) { }

                this.favoriteViewSource.Source = this.Model.FavouriteReceipes;
            }

            StorageFolder folderModel = KnownFolders.PicturesLibrary;
            try
            {
                StorageFile receipesFile = await folderModel.GetFileAsync("receipes.json");
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

        private ItemResult CreateItemResult(Receipe receipe)
        {
            ItemResult item = new ItemResult { Receipe = receipe };
            item.Name = receipe.Title;
            TextBlock toto = new TextBlock();

            toto.Text = item.Name;
            toto.Measure(new Size(1200, 1200));
            if (toto.DesiredSize.Width > 90)
            {
                while (toto.DesiredSize.Width > 90)
                {
                    item.Name = item.Name.Remove(item.Name.Length - 1, 1);
                    toto.Text = item.Name;
                    toto.Measure(new Size(1200, 1200));

                }
                item.Name = item.Name.PadRight(item.Name.Length + 3, '.');
            }

            return item;
        }

        public void Refresh(Event e)
        {
            if (e is ReceipeEvent)
            {
                ReceipeEvent receipeE = (ReceipeEvent)e;
                Receipe receipe = receipeE.Receipe;

                if (receipeE is AddedReceipeEvent)
                {
                    this.ItemsResult.Add(this.CreateItemResult(receipe));
                }
                else if (receipeE is RemovedReceipeEvent)
                {
                    foreach (ItemResult item in this.ItemsResult)
                    {
                        if (item.Receipe == receipe)
                        {
                            this.ItemsResult.Remove(item);
                        }
                    }
                }
                else if (receipeE is ClearedReceipeEvent)
                {
                    this.ItemsResult = new List<ItemResult>();
                }
            }
        }

        public async void search(string keyWord)
        {
            progress.Visibility = Visibility.Visible;
            this.Model.ClearReceipes();

            bool error = await this.retriever.GetData(keyWord, 200, 1, Model);
            this.resultsFoundViewSource.Source = this.ItemsResult;
            progress.Visibility = Visibility.Collapsed;

            Check_n_Cook.ScrollToSection(Check_n_Cook.Sections[2]);

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            this.search(this.textBoxSearch.Text);

        }

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

        private TextBox textBoxSearch;

        private void textboxSearchReceipe_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            textBoxSearch = sender as TextBox;
        }

        private void GoToShoppingList(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ShoppingList), this.Model);
            }
        }

        private void GoToAboutCrew(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(AboutCrew));
            }
        }

        public void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ItemResult item = ((ItemResult)e.ClickedItem);
            this.Model.SelectReceipe(item.Receipe);
            this.Frame.Navigate(typeof(ReceipeDetail), this.Model);
        }

        private void GoToPlanningReceipe_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(PlanningReceipe), this.Model);
            }
        }

        public Button advanced { get; set; }
        private void advancedSearch_Click(object sender, RoutedEventArgs e)
        {
            advanced = sender as Button;
            advanced.Flyout.ShowAt(advanced);

        }

        private void DeleteConfirmation_Click(object sender, RoutedEventArgs e)
        {
            Button delete = sender as Button;
            advanced.Flyout.Hide();

        }

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

            if (((string)control.Content).Equals("Alcool"))
            {
                this.retriever.AdvancedAlcool = true;
            }

        }

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

            if (((string)control.Content).Equals("Alcool"))
            {
                this.retriever.AdvancedAlcool = false;
            }


        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

            Slider slider = sender as Slider;
            retriever.AdvancedDifficulty = (int)slider.Value;

        }

        private void textboxSearchReceipe_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.search(this.textBoxSearch.Text);
            }
        }

        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            sliderSearch = sender as Slider;
        }

        private void progress_Loaded(object sender, RoutedEventArgs e)
        {
            this.progress = sender as ProgressBar;
        }

        private void GoToShop_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ShopsList), this.Model);
            }
        }
    }
}
