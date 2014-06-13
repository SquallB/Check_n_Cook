using Check_n_Cook.Common;
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
        private URLDataRetriever retriever;
        public AppModel Model { get; set; }

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
                    StorageFile receipesFile = await folder.GetFileAsync("receipes.json");
                    String jsonString = await FileIO.ReadTextAsync(receipesFile);
                    JsonObject jsonObject = JsonObject.Parse(jsonString);
                    JsonArray jsonArray = jsonObject.GetNamedArray("Receipes");
                    foreach (var jsonReceipe in jsonArray)
                    {
                        this.Model.FavouriteReceipes.Add(new Receipe(jsonReceipe.Stringify()));
                    }
                }
                catch (FileNotFoundException ex) { }
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

        public void Refresh(Event e)
        {
            if (e is ReceipeEvent)
            {
                ReceipeEvent receipeE = (ReceipeEvent)e;
                Receipe receipe = receipeE.Receipe;

                if (receipeE is AddedReceipeEvent)
                {
                    string title = receipe.Title;
                    TextBlock toto = new TextBlock();

                    toto.Text = title;
                    toto.Measure(new Size(1200, 1200));
                    if (toto.DesiredSize.Width > 90)
                    {
                        while (toto.DesiredSize.Width > 90)
                        {
                            title = title.Remove(title.Length - 1, 1);
                            toto = new TextBlock();
                            toto.Text = title;
                            toto.Measure(new Size(1200, 1200));

                        }
                        title = title.PadRight(title.Length + 3, '.');
                    }
                    receipe.Title = title;

                    ItemResult item = new ItemResult { Receipe = receipe };
                    this.ItemsResult.Add(item);
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
            this.Model.ClearReceipes();
            bool error = await this.retriever.GetData(keyWord, 100, 1, Model);
            this.resultsFoundViewSource.Source = this.ItemsResult;
            
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

        private void GoToReceipeList(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(AllReceipeList), this.Model);
            }
        }

    }
}
