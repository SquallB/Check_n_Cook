using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
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
        private AppModel model;
        private Time time;

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
                this.time = evnt.Time;

                if (this.model.ReceipeList.ContainsKey(time.Date))
                {
                    ReceipeDate receipeDate = this.model.ReceipeList[time.Date];

                    Dictionary<string, Ingredient> ingredients = new Dictionary<string, Ingredient>();

                    foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                    {
                        foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                        {
                            foreach (Ingredient ingredient in receipe.ingredients)
                            {
                                if (ingredients.ContainsKey(ingredient.name))
                                {
                                    if (ingredient.quantity != String.Empty && ingredient.quantity != null)
                                    {
                                        // int previousQuantityInt = HandleQuantity(ingredients[ingredient.name].quantity);
                                        //  int newQuantitty = previousQuantityInt + HandleQuantity(ingredient.quantity);
                                        ingredients[ingredient.name].quantity = HandleQuantity(ingredients[ingredient.name].quantity, ingredient.quantity).ToString();
                                    }
                                }
                                else
                                {
                                    ingredients[ingredient.name] = ingredient.ToClone();
                                }
                            }
                        }
                    }

                    this.ingredientsViewSource.Source = ingredients.Values;
                }
            }
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
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
            base.OnNavigatedFrom(e);
        }

        #endregion
    }
}
