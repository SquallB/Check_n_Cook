using Check_n_Cook.Common;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Check_n_Cook
{
    /// <summary>
    /// Page displays a title for the group, a element list contains in this group, and the detail for the element selected
    /// </summary>
    public sealed partial class AboutCrew : Page
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
        /// Cela peut être remplacé par un modèle d'affichage fortement typé.
        /// </summary>
        /// <value>
        /// The default view model.
        /// </value>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper est utilisé sur chaque page pour faciliter la navigation et
        /// gestion de la durée de vie des processus
        /// </summary>
        /// <value>
        /// The navigation helper.
        /// </value>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutCrew"/> class.
        /// </summary>
        public AboutCrew()
        {
            this.InitializeComponent();

            // Configure the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;

            // Configure the navigation composant of the logic page that allow the
            // page to display just one sub-page
            this.navigationHelper.GoBackCommand = new Check_n_Cook.Common.RelayCommand(() => this.GoBack(), () => this.CanGoBack());
            this.itemListView.SelectionChanged += itemListView_SelectionChanged;

            Window.Current.SizeChanged += Window_SizeChanged;
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the itemListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation())
            {
                this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Fills the page with the previous elements while the navigation. Any state loaded is given when the page
        /// is recreated from a previous session.
        /// </summary>
        /// <param name="sender">
        /// The event source ; generaly <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e"> Event data that give the parameter of the navigation transmitted
        /// <see cref="Frame.Navigate(Type, Object)"/> during the initial request of this page and
        /// a state dictionnary preserved by this page during a previous session
        /// The state will not take the value Null when the first visit of this page.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            List<ItemAbout> favorites = new List<ItemAbout>();

            string copyRight = "Mentions légales et copyright : \nCertains contenus proviennent du site Marmiton ou de sources externes.\nToute reproduction ou utilisation de ces contenus à des fins commercials sont strictement interdits sous peine de poursuite judiciaire.\nL'application a été éditée par l'équipe easyLifer dont les membres sont présentés sur cette page. \nPour toutes informations supplémentaires ou signaler un problème, veuillez contacter le membre de l'équipe qui pourra vous renseigner.\n\nNotes de version :\nDate de dernière mise à jour : 24/06/2014\nVersion actuelle : 1.0";

            string descriptionSylvain = "Passionné d’informatique depuis qu’il est au collège, Sylvain a commencé par apprendre quelques langages par lui-même, grâce aux cours disponibles sur Internet. Par la suite il a tout naturellement choisi de faire des études dans ce domaine. Pour cela il a choisi l’école d’ingénieur ISEN, située à Lille. Cela lui a permis d’approfondir ses connaissances en informatique, mais aussi d’acquérir d’autres compétences nécessaires à un ingénieur.\n\nIl a ainsi pu acquérir de solides connaissances en informatique, notamment dans le développement. Il a ainsi pu assurer le poste de responsable technique lorsqu’il a rejoint l’équipe easyLifer. En travaillant avec les autres membres, il a pu donner à l’équipe les moyens de développer des applications robustes afin de limiter les éventuels bugs. Cela a permis de se rapprocher du but de l’équipe, qui est de créer des applications fiables et simples d’utilisations pour tous.\n\nPour contacter Sylvain, voici son adresse e-mail : sylvain.bardin@sfr.fr";
            string descriptionHugo = "Développeur web depuis 2006, Hugo a eu l’occasion d'acquérir des compétences managériales et organisationnelles au cours de projets qui l’ont confronté avec le monde professionnelle tout en conciliant sa vie étudiante dans une école d’ingénieur lilloise (ISEN).\n\nSoucieux de l’accessibilité et de la simplicité de ses produits et projets pour leurs utilisateurs, Hugo a tendance à placer le mot easy sur tous ses projets. Afin de partager avec d’autres cette volonté de simplifier la vie des utilisateurs de ses projets, il a tout naturellement décidé de rejoindre l’équipe easyLifer. Avec cette équipe, il a modernisé le jeu que tout le monde connait sous le nom Casse brique pour en faire un Casse Tuile; reprenant ainsi le thème de Windows 8.\n\nSuite à cela, l’équipe a eu l’idée de développer une application pour Windows 8 permettant de stocker ses recettes de cuisine, d’ajouter les ingrédients dans une liste de course et trouver le magasin le plus proche de sa position.\n\nLes compétences managériales aquises ces dernières années lui ont values d’être promu chef de projet pour ces deux derniers projets.\n\nHugo est joignable par mail : hdufossez@gmail.com. \nSuivez le sur Twitter : @HugoDufossez ";
            string descriptionMehdi = "Mehdi est actuellement étudiant de 20 ans, il suit ses études à Lille, dans une école d’ingénieur généraliste nommée ISEN (Institut Supérieur de l’Électronique et du Numérique). Il fait partie de l’équipe EasyLifer, en tant que responsable commercial, car il cherche avant tout à faciliter la vie quotidienne d’autrui. Pour cela il s’efforce de sélectionner des éléments qui lui semblent gênant dans sa propre vie et les superpose aux attentes du marché.\n\nC’est ainsi, dans un souci de confort et de compréhension, que l’équipe développe ses applications, car chaque membre sait que les utilisateurs n’ont pas forcément envie de faire l’effort de prendre en main un outil compliqué, mais au contraire, souhaitent se focaliser sur le contenu mis à disposition.\n\nVous pouvez le contacter par mail à l’adresse suivante : mehdi.bouchagour@laposte.net\n\nOu bien par téléphone au : 06.20.12.29.86";
            string descriptionStephane = "Jeune étudiant dans l'école d'ingénieur ISEN à Lille, Stéphane Eintrazi a pu acquérir des compétences dans le domaine de l'informatique. Suite à de nombreux projets qu'il a faits, l'équipe easyLifer était très intéressé par son travail et son goût pour le désign, il fut très vite embauché par cette équipe en tant que responsable design.\n\nL'équipe easyLifer a pour principal objectif de créer des applications simples à utiliser et à la fois complète. Le design que Stéphane a pu imaginer a amélioré la simplicité des applications que l'équipe easyLifer a produite. Mais l'équipe easyLifer a une certaine exigence concernant la qualité du code, alors Stépahne a dû être très polyvalent car certains désigns demandés des compétences très techniques.De plus, il est à la fois dynamique et travailleur et il est maintenant un membre indispensable pour l'équipe easyLifer.\n\nPour contacter Stéphane voici son adresse mail: stephane.eintrazi@isen-lille.fr\n\nOu vous pouvez le contacter par téléphone: 06.46.08.41.08 ";

            favorites.Add(new ItemAbout("http://www.partenaire-carriere.com/images/reference-legal-droit-mention.gif", "Mentions légales et copyright", "", copyRight));

            favorites.Add(new ItemAbout("Assets/sylvain_CheckNCook.png", "Sylvain BARDIN", "Responsable technique", descriptionSylvain));
            favorites.Add(new ItemAbout("Assets/mehdi_CheckNCook.png", "Mehdi BOUCHAGOUR", "Responsable marketing", descriptionMehdi));
            favorites.Add(new ItemAbout("Assets/hugo_CheckNCook.png", "Hugo DUFOSSEZ", "Chef de projet", descriptionHugo));
            favorites.Add(new ItemAbout("Assets/stephane_CheckNCook.png", "Stéphane EINTRAZI", "Responsable design", descriptionStephane));

            itemsViewSource.Source = favorites;

            if (e.PageState == null)
            {
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
        }

        #region Navigation entre pages logiques


        private const int MinimumWidthForSupportingTwoPanes = 768;

        /// <summary>
        /// Invoked to determinate if the page must display as one logic page or two.
        /// </summary>
        /// <returns>True if the page must display as one logic page, false
        /// .</returns>
        private bool UsingLogicalPageNavigation()
        {
            return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes;
        }

        /// <summary>
        /// Called with the modification of the size of the frame
        /// </summary>
        /// <param name="sender">The active frame</param>
        /// <param name="e">Event data that give information about the new size of the frame</param>
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Called when a element of the list is selected.
        /// </summary>
        /// <param name="sender">GridView that displays the selected elementé.</param>
        /// <param name="e">Event data that give information about how the selection have been modified.</param>
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }

        /// <summary>
        /// Determines whether this instance [can go back].
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Goes the back.
        /// </summary>
        private void GoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                this.itemListView.SelectedItem = null;
            }
            else
            {
                this.navigationHelper.GoBack();
            }
        }

        /// <summary>
        /// Invalidates the state of the visual.
        /// </summary>
        private void InvalidateVisualState()
        {
            var visualState = DetermineVisualState();
            VisualStateManager.GoToState(this, visualState, false);
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Called to determinate if the visual state name corresponding to the state of the display
        /// of an application
        /// </summary>
        /// <returns>Visual state name desired. It is the same name that
        /// the state of the display, expcet if an element is selected</returns>
        private string DetermineVisualState()
        {
            if (!UsingLogicalPageNavigation())
                return "PrimaryView";

            var logicalPageBack = this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null;

            return logicalPageBack ? "SinglePane_Detail" : "SinglePane";
        }

        #endregion

        #region Inscription de NavigationHelper

        /// <summary>
        /// Called when the page is loaded and becomes the actual source of a parent frame.
        /// </summary>
        /// <param name="e">Event data that may be examinate by replcaing the code. The event data represents the navigation waiting that will load the active page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Called immediately after the unload of the page and the page do not represent the actual source of the parent frame.
        /// </summary>
        /// <param name="e">Event data that may be examinate by replcaing the code. The event data represents the navigation waiting that will unload the active page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
