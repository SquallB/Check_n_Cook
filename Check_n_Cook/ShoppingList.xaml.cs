﻿using Check_n_Cook.Common;
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

// Pour en savoir plus sur le modèle d'élément Page Hub, consultez la page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class ShoppingList : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

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

        public ShoppingList()
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
            List<ItemReceipe> receipes = new List<ItemReceipe>();

            receipes.Add(new ItemReceipe("http://www.google.fr/url?source=imglanding&ct=img&q=http://3.bp.blogspot.com/-Q6PuvmuRGZc/T8_SMDwqmVI/AAAAAAAAAzM/mOBDOKKj0IE/s1600/hsrf.JPG&sa=X&ei=QPSRU9bOCuqS7AavlIHAAg&ved=0CAkQ8wc&usg=AFQjCNFA0-svML0apesgl4DXdO-laQlXZQ", "recette1", "Une bonne recette !"));

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
                    this.ReceipeTextBlock.Text = (string) b.Tag;
                }
            }

        }

        private void TextBlockReceipe_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReceipeTextBlock = sender as TextBlock;
        }

    }
}