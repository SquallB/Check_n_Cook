using Check_n_Cook.Common;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page fractionnée, consultez la page http://go.microsoft.com/fwlink/?LinkId=234234

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant un titre de groupe, une liste des éléments contenus dans ce groupe, ainsi que les détails concernant
    /// l'élément actuellement sélectionné.
    /// </summary>
    public sealed partial class AboutCrew : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

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

        public AboutCrew()
        {
            this.InitializeComponent();

            // Configurer l'assistant de navigation
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            // Configurer les composants de navigation de page logique qui permettent
            // la page pour afficher un seul volet à la fois.
            this.navigationHelper.GoBackCommand = new Check_n_Cook.Common.RelayCommand(() => this.GoBack(), () => this.CanGoBack());
            this.itemListView.SelectionChanged += itemListView_SelectionChanged;

            // Commencer à écouter les modifications de la taille de la fenêtre 
            // pour passer de l'affichage de deux volets à l'affichage d'un volet
            Window.Current.SizeChanged += Window_SizeChanged;
            this.InvalidateVisualState();
        }

        void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation())
            {
                this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Remplit la page à l'aide du contenu passé lors de la navigation.  Tout état enregistré est également
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
            // TODO: affectez un groupe pouvant être lié à Me.DefaultViewModel("Group")
            // TODO: affectez une collection d'éléments pouvant être liés à Me.DefaultViewModel("Items")
            List<ItemAbout> favorites = new List<ItemAbout>();
            string descriptionHugo = "Développeur web depuis 2006, Hugo a eu l’occasion d'acquérir des compétences managériales et organisationnelles\nau cours de projets qui l’ont confronté avec le monde professionnelle tout en conciliant sa vie étudiante dans une école d’ingénieur lilloise (ISEN).\n\nSoucieux de l’accessibilité et de la simplicité de ses produits et projets pour leurs utilisateurs, Hugo a tendance a placé le mot easy sur tous ses projets.\nAfin de partager avec d’autres cette volonté de simplifier la vie des utilisateurs de ses projets, il a tout naturellement décidé de rejoindre l’équipe easyLifer.\nAvec cette équipe, il a modernisé le jeu que tout le monde connait sous le nom Casse brique pour en faire un Casse Tuile; reprenant ainsi le thème de Windows 8.\n\nSuite à cela, l’équipe a eu l’idée de développer une application pour Windows 8 permettant de stocker ses recettes de cuisine, d’ajouter les ingrédients\ndans une liste de course et trouver le magasin le plus proche de sa position.\n\nLes compétences managériales aquises ces dernières années lui ont values d’être promu chef de projet pour ces deux derniers projets.\n\nHugo est joignable par mail : hdufossez@gmail.com ";
            favorites.Add(new ItemAbout("http://www.graindorge.fr/mediatheque/fromages/camembert.jpg", "Sylvain BARDIN", "Responsable technique", "Blablablalblablab"));
            favorites.Add(new ItemAbout("http://www.graindorge.fr/mediatheque/fromages/camembert.jpg", "Mehdi BOUCHAGOUR", "Responsable marketing", "Blablablalblablab"));
            favorites.Add(new ItemAbout("Assets/hugo_CheckNCook.png", "Hugo DUFOSSEZ", "Chef de projet", descriptionHugo));
            favorites.Add(new ItemAbout("http://www.graindorge.fr/mediatheque/fromages/camembert.jpg", "Stéphane EINTRAZI", "Responsable design", "Blablablalblablab"));

            itemsViewSource.Source = favorites;

            if (e.PageState == null)
            {
                // Quand il s'agit d'une nouvelle page, sélectionne automatiquement le premier élément, sauf si la navigation
                // de page logique est utilisée (voir #region navigation de page logique ci-dessous.)
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restaure l'état précédemment enregistré associé à cette page
                if (e.PageState.ContainsKey("SelectedItem") && this.itemsViewSource.View != null)
                {
                    // TODO: appelez Me.itemsViewSource.View.MoveCurrentTo() avec l'élément
                    //       sélectionné, comme spécifié par la valeur de pageState("SelectedItem")

                }
            }
        }

        /// <summary>
        /// Conserve l'état associé à cette page en cas de suspension de l'application ou de la
        /// suppression de la page du cache de navigation.  Les valeurs doivent être conformes aux
        /// exigences en matière de sérialisation de <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">La source de l'événement ; en général <see cref="NavigationHelper"/></param>
        /// <param name="e">Données d'événement qui fournissent un dictionnaire vide à remplir à l'aide de
        /// état sérialisable.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (this.itemsViewSource.View != null)
            {
                // TODO: dérivez un paramètre de navigation sérialisable et assignez-le à la valeur
                //       la valeur de pageState("SelectedItem")

            }
        }

        #region Navigation entre pages logiques

        // La page fractionnée est conçue de sorte que si la fenêtre ne dispose pas d'assez d'espace pour afficher
        // la liste et les détails, un seul volet s'affichera à la fois.
        //
        // Le tout est implémenté à l'aide d'une seule page physique pouvant représenter deux pages logiques.
        // Le code ci-dessous parvient à ce but sans que l'utilisateur ne se rende compte de
        // la distinction.

        private const int MinimumWidthForSupportingTwoPanes = 768;

        /// <summary>
        /// Invoqué pour déterminer si la page doit agir en tant qu'une ou deux pages logiques.
        /// </summary>
        /// <returns>True si la fenêtre doit agir en tant qu'une page logique, false
        /// .</returns>
        private bool UsingLogicalPageNavigation()
        {
            return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes;
        }

        /// <summary>
        /// Appelé avec la modification de la taille de la fenêtre
        /// </summary>
        /// <param name="sender">La fenêtre active</param>
        /// <param name="e">Données d'événement décrivant la nouvelle taille de la fenêtre</param>
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoqué lorsqu'un élément d'une liste est sélectionné.
        /// </summary>
        /// <param name="sender">GridView qui affiche l'élément sélectionné.</param>
        /// <param name="e">Données d'événement décrivant la façon dont la sélection a été modifiée.</param>
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidez l'état d'affichage lorsque la navigation entre pages logiques est en cours, car une modification
            // apportée à la sélection pourrait entraîner la modification de la page logique active correspondante.  Lorsqu'un
            // élément est sélectionné, l'affichage passe de la liste d'éléments
            // aux détails concernant l'élément sélectionné.  Lorsque cet élément est désélectionné, l'effet inverse
            // est produit.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }

        private bool CanGoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                return true;
            }
            else
            {
                return this.navigationHelper.CanGoBack();
            }
        }
        private void GoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                // Lorsque la navigation entre pages logiques est en cours et qu'un élément est sélectionné,
                // les détails de l'élément sont actuellement affichés.  La suppression de la sélection entraîne le retour à
                // la liste d'éléments.  Du point de vue de l'utilisateur, ceci est un état visuel précédent logique
                // de logique inversée.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                this.navigationHelper.GoBack();
            }
        }

        private void InvalidateVisualState()
        {
            var visualState = DetermineVisualState();
            VisualStateManager.GoToState(this, visualState, false);
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Invoqué pour déterminer le nom de l'état visuel correspondant à l'état d'affichage
        /// d'une application.
        /// </summary>
        /// <returns>Nom de l'état visuel désiré.  Il s'agit du même nom que celui
        /// de l'état d'affichage, sauf si un élément est sélectionné dans l'affichage Portrait ou Snapped où
        /// cette page logique supplémentaire est représentée par l'ajout du suffixe _Detail.</returns>
        private string DetermineVisualState()
        {
            if (!UsingLogicalPageNavigation())
                return "PrimaryView";

            // Modifiez l'état d'activation du bouton Précédent lorsque l'état d'affichage est modifié
            var logicalPageBack = this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null;

            return logicalPageBack ? "SinglePane_Detail" : "SinglePane";
        }

        #endregion

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
    }
}
