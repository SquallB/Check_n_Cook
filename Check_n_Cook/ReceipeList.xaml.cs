using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Printing;
// Pour en savoir plus sur le modèle d'élément Page Hub, consultez la page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class ReceipeList : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public AppModel Model { get; set; }
        private Time time;
        private HubSection hubReceipe;
        private int nbCountReceipe;
        private bool day;
        public TextBlock ReceipeTextBlock { get; set; }

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

        public ReceipeList()
        {
            this.InitializeComponent();
            this.day = false;
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
            GoToReceipeListEvent evnt = e.NavigationParameter as GoToReceipeListEvent;
            this.Model = evnt.AppModel;
            this.time = evnt.Time;
            List<Receipe> receipes = new List<Receipe>();

            if (evnt.ReceipeTime is ReceipeTimeOfDay)
            {
                ReceipeTimeOfDay receipeTimeOfDay = (ReceipeTimeOfDay)evnt.ReceipeTime;
                foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                {
                    receipes.Add(receipe);
                }

                HandleTitle(receipeTimeOfDay);
                this.time = receipeTimeOfDay.Time;
            }
            else if (evnt.ReceipeTime is ReceipeDate)
            {
                ReceipeDate receipeDate = (ReceipeDate)evnt.ReceipeTime;
                foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                {
                    foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                    {
                        receipes.Add(receipe);
                    }
                }
                HandleTitle(receipeDate);
                this.time = receipeDate.Time;
            }

            listReceipeViewSource.Source = receipes;
            this.ReceipeTextBlock = sender as TextBlock;
        }

        public void HandleTitle(ReceipeTime receipeTime)
        {
            this.nbCountReceipe = 0;

            if (receipeTime is ReceipeTimeOfDay)
            {
                nbCountReceipe = ((ReceipeTimeOfDay)receipeTime).Receipes.Count;

            }
            else if (receipeTime is ReceipeDate)
            {
                ReceipeDate receipeDate = (ReceipeDate)receipeTime;
                day = true;
                foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                {
                    nbCountReceipe += receipeTimeOfDay.Receipes.Count;
                }
            }


            if (nbCountReceipe > 1)
            {
                this.pageTitle.Text = "Liste de recettes";
            }
            else
            {
                this.pageTitle.Text = "Liste de recette";
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
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ClickHandleIngredients(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (b != null)
            {
                Receipe receipe = (Receipe)b.DataContext;
                List<Ingredient> ingredients = receipe.ingredients;

                this.listIngredientsViewSource.Source = ingredients;

                if (this.ReceipeTextBlock != null)
                {
                    this.ReceipeTextBlock.Text = receipe.Title;
                }
            }
        }

        private void TextBlockReceipe_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceipeTextBlock = sender as TextBlock;
        }

        private void GoToModifyReceipeList_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ModifyReceipeList), new GoToModifyReceipeListEvent(this.Model, this.time));
            }
        }

        private void GoToShoppingList_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ShoppingList), new GoToModifyReceipeListEvent(this.Model, this.time));
            }
        }

        private void HubSection_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is HubSection)
            {
                this.hubReceipe = (HubSection)sender;
                if (nbCountReceipe > 1)
                {
                    if (!day)
                    {
                        this.hubReceipe.Header = "Recettes : " + this.time.Date + " (" + this.time.TimeOfDay + ")";
                    }
                    else
                    {
                        this.hubReceipe.Header = "Recettes : " + this.time.Date + " (La journée)";
                    }
                }
                else
                {
                    if (!day)
                    {
                        this.hubReceipe.Header = "Recette : " + this.time.Date + " (" + this.time.TimeOfDay + ")";
                    }
                    else
                    {
                        this.hubReceipe.Header = "Recette : " + this.time.Date + " (La journée)";
                    }
                }
            }

        }

    }
}
