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
            List<ItemReceipe> receipes = new List<ItemReceipe>();

            if (evnt.ReceipeTime is ReceipeTimeOfDay)
            {
                ReceipeTimeOfDay receipeTimeOfDay = (ReceipeTimeOfDay)evnt.ReceipeTime;
                foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                {
                    receipes.Add(new ItemReceipe(receipe.Image, receipe.Title, receipe.Description));
                }
                this.pageTitle.Text = "Liste de recette pour le " + receipeTimeOfDay.Time.Date + " (" + receipeTimeOfDay.Time.TimeOfDay + ")";
                this.time = receipeTimeOfDay.Time;
            }
            else if (evnt.ReceipeTime is ReceipeDate)
            {
                ReceipeDate receipeDate = (ReceipeDate)evnt.ReceipeTime;
                foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                {
                    foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                    {
                        receipes.Add(new ItemReceipe(receipe.Image, receipe.Title, receipe.Description));
                    }
                }
                this.pageTitle.Text = "Liste de recette pour le " + receipeDate.Time.Date + " (La journée)";
                this.time = receipeDate.Time;
            }

            listReceipeViewSource.Source = receipes;
            this.ReceipeTextBlock = sender as TextBlock;
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
                List<ItemReceipe> ingredients = new List<ItemReceipe>();
                ingredients.Add(new ItemReceipe("http://www.google.fr/url?source=imglanding&ct=img&q=http://www.jefaismoimeme.com/wp-content/uploads/2013/10/tomate.jpg&sa=X&ei=4d6SU-WuEs_44QTT2oCwDA&ved=0CAkQ8wc&usg=AFQjCNGbe9N_JdYBZgth7_yOjPPDVlb9ow", "Tomato !", "200g"));
                ingredients.Add(new ItemReceipe("http://www.google.fr/url?source=imglanding&ct=img&q=http://www.jefaismoimeme.com/wp-content/uploads/2013/10/tomate.jpg&sa=X&ei=4d6SU-WuEs_44QTT2oCwDA&ved=0CAkQ8wc&usg=AFQjCNGbe9N_JdYBZgth7_yOjPPDVlb9ow", "Tomato !", "200g"));
                ingredients.Add(new ItemReceipe("http://www.google.fr/url?source=imglanding&ct=img&q=http://www.jefaismoimeme.com/wp-content/uploads/2013/10/tomate.jpg&sa=X&ei=4d6SU-WuEs_44QTT2oCwDA&ved=0CAkQ8wc&usg=AFQjCNGbe9N_JdYBZgth7_yOjPPDVlb9ow", "Tomato !", "200g"));

                this.listIngredientsViewSource.Source = ingredients;

                if (this.ReceipeTextBlock != null)
                {
                    this.ReceipeTextBlock.Text = (string)b.Tag;
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
                this.Frame.Navigate(typeof(ShoppingList), this.Model);
            }
        }

    }
}
