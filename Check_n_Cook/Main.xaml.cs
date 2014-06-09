﻿using Check_n_Cook.Common;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page Hub, consultez la page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class Main : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<ItemResult> ItemsResult { get; set; }
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
            // TODO: assignez une collection de groupes pouvant être liés à this.DefaultViewModel["Groups"]
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

        public async void search(string keyWord)
        {
            URLDataRetriever retriever = new URLDataRetriever();
            AppModel model = new AppModel();
            bool error = await retriever.GetData(keyWord, 100, 1, model);
            ItemsResult = new List<ItemResult>();
            List<Receipe> searchResult = model.Receipes;
            foreach (Receipe receipe in searchResult)
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
                
                addItemResult(receipe);

            }
            resultsFoundViewSource.Source = ItemsResult;

        }

        public void addItemResult(Receipe receipe)
        {
            if (ItemsResult == null)
            {
                ItemsResult = new List<ItemResult>();
            }
            else
            {
                ItemsResult.Add(new ItemResult { Receipe = receipe });
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            /*URLDataRetriever retriever = new URLDataRetriever();
            AppModel model = new AppModel();
            bool error = await retriever.GetData(this.textBoxSearch.Text, model);
            textBoxSearch.Text = model.Receipes.Count.ToString();*/

            search(this.textBoxSearch.Text);
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
                this.Frame.Navigate(typeof(ShoppingList));
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
            var item = ((ItemResult)e.ClickedItem);
            this.Frame.Navigate(typeof(ReceipeDetail), item);
        }

    }
}
