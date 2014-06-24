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
            List<ItemAbout> favorites = new List<ItemAbout>();

            string copyRight = "Mentions légales et copyright : \nCertains contenus proviennent du site Marmiton ou de sources externes.\nToute reproduction ou utilisation de ces contenus à des fins commercials sont strictement interdits sous peine de poursuite judiciaire.\nL'application a été éditée par l'équipe easyLifer dont les membres sont présentés sur cette page. \nPour toutes informations supplémentaires ou signaler un problème, veuillez contacter le membre de l'équipe qui pourra vous renseigner.\n\nNotes de version :\nDate de dernière mise à jour : 24/06/2014\nVersion actuelle : 1.0";

            string descriptionSylvain = "Passionné d’informatique depuis qu’il est au collège, Sylvain a commencé par apprendre quelques langages par lui-même, grâce aux cours disponibles sur Internet. Par la suite il a tout naturellement choisi de faire des études dans ce domaine. Pour cela il a choisi l’école d’ingénieur ISEN, située à Lille. Cela lui a permis d’approfondir ses connaissances en informatique, mais aussi d’acquérir d’autres compétences nécessaires à un ingénieur.\n\nIl a ainsi pu acquérir de solides connaissances en informatique, notamment dans le développement. Il a ainsi pu assurer le poste de responsable technique lorsqu’il a rejoint l’équipe easyLifer. En travaillant avec les autres membres, il a pu donner à l’équipe les moyens de développer des applications robustes afin de limiter les éventuels bugs. Cela a permis de se rapprocher du but de l’équipe, qui est de créer des applications fiables et simples d’utilisations pour tous.\n\nPour contacter Sylvain, voici son adresse e-mail : sylvain.bardin@sfr.fr";
            string descriptionHugo = "Développeur web depuis 2006, Hugo a eu l’occasion d'acquérir des compétences managériales et organisationnelles au cours de projets qui l’ont confronté avec le monde professionnelle tout en conciliant sa vie étudiante dans une école d’ingénieur lilloise (ISEN).\n\nSoucieux de l’accessibilité et de la simplicité de ses produits et projets pour leurs utilisateurs, Hugo a tendance à placer le mot easy sur tous ses projets. Afin de partager avec d’autres cette volonté de simplifier la vie des utilisateurs de ses projets, il a tout naturellement décidé de rejoindre l’équipe easyLifer. Avec cette équipe, il a modernisé le jeu que tout le monde connait sous le nom Casse brique pour en faire un Casse Tuile; reprenant ainsi le thème de Windows 8.\n\nSuite à cela, l’équipe a eu l’idée de développer une application pour Windows 8 permettant de stocker ses recettes de cuisine, d’ajouter les ingrédients dans une liste de course et trouver le magasin le plus proche de sa position.\n\nLes compétences managériales aquises ces dernières années lui ont values d’être promu chef de projet pour ces deux derniers projets.\n\nHugo est joignable par mail : hdufossez@gmail.com. \nSuivez le sur Twitter : @HugoDufossez ";
            string descriptionMehdi = "Mehdi est actuellement étudiant de 20 ans, il suit ses études à Lille, dans une école d’ingénieur généraliste nommée ISEN (Institut Supérieur de l’Électronique et du Numérique). Il fait partie de l’équipe EasyLifer, en tant que responsable commercial, car il \ncherche avant tout à faciliter la vie quotidienne d’autrui. Pour cela il s’efforce de sélectionner des éléments qui lui semblent gênant dans sa propre vie et les superpose aux attentes du marché.\n\n C’est ainsi, dans un souci de confort et de compréhension, que l’équipe développe ses applications, car chaque membre sait que les utilisateurs n’ont pas forcément envie de faire l’effort de prendre en main un outil compliqué, mais au contraire, souhaitent se focaliser sur le contenu mis à disposition.\n\nVous pouvez le contacter par mail à l’adresse suivante : mehdi.bouchagour@laposte.net \n\nOu bien par téléphone au : 06.20.12.29.86";
            string descriptionStephane = "Jeune étudiant dans l'école d'ingénieur ISEN à Lille, Stéphane Eintrazi a pu acquérir des compétences dans le domaine de l'informatique. Suite à de nombreux projets qu'il a fait, l'équipe easyLifer était très intéressé par son travail et son goût pour le désign, il fut très vite embauché par cette équipe en tant que responsable design.\n\nL'équipe easyLifer a pour principal objectif de créer des applications simples à utiliser et à la fois complète. Le design que Stéphane a pu imaginer a amélioré la simplicité des applications que l'équipe easyLifer a produite. Mais l'équipe easyLifer a une certaine exigence concernant la qualité du code, alors Stépahne a dû être très polyvalent car certains désign demandés des compétences très techniques.De plus, il est à la fois dynamique et travailleur et il est maintenant un membre indispensable pour l'équipe easyLifer.\n\nPour contacter Stéphane voici son adresse mail: stephane.eintrazi@isen-lille.fr\nOu vous pouvez le contacter par téléphone: 06.46.08.41.08 ";

            favorites.Add(new ItemAbout("http://www.partenaire-carriere.com/images/reference-legal-droit-mention.gif", "Mentions légales et copyright", "", copyRight));

            favorites.Add(new ItemAbout("http://www.graindorge.fr/mediatheque/fromages/camembert.jpg", "Sylvain BARDIN", "Responsable technique", descriptionSylvain));
            favorites.Add(new ItemAbout("Assets/mehdi_CheckNCook.png", "Mehdi BOUCHAGOUR", "Responsable marketing", descriptionMehdi));
            favorites.Add(new ItemAbout("Assets/hugo_CheckNCook.png", "Hugo DUFOSSEZ", "Chef de projet", descriptionHugo));
            favorites.Add(new ItemAbout("Assets/stephane_CheckNCook.png", "Stéphane EINTRAZI", "Responsable design", descriptionStephane));

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
