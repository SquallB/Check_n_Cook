using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ModifyReceipeList : Page, View
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private AppModel Model;
        private string date;
        private bool addedView;

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

        public ModifyReceipeList()
        {
            this.InitializeComponent();
            this.addedView = false;
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
            GoToModifyReceipeListEvent evnt = e.NavigationParameter as GoToModifyReceipeListEvent;
            if (evnt != null)
            {
                this.Model = evnt.AppModel;
                if (this.addedView == false)
                {
                    this.Model.AddView(this);
                    this.addedView = true;
                }
                Time time = evnt.Time;

                if (this.Model.ReceipeList.ContainsKey(time.Date))
                {
                    ReceipeDate receipeDate = this.Model.ReceipeList[time.Date];
                    this.date = receipeDate.Time.Date;
                    morningReceipeViewSource.Source = receipeDate.ReceipeTimeOfDay["Matin"].Receipes.Values;
                    noonReceipeViewSource.Source = receipeDate.ReceipeTimeOfDay["Midi"].Receipes.Values;
                    evenningReceipeViewSource.Source = receipeDate.ReceipeTimeOfDay["Soir"].Receipes.Values;
                }
                
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

        public void Refresh(Event e)
        {
            if (e is RemovedReceipeListEvent)
            {
                RemovedReceipeListEvent srcEvnt = (RemovedReceipeListEvent)e;
                Time time = srcEvnt.Time;
                Dictionary<string, Receipe> newList = new Dictionary<string, Receipe>();

                if (this.Model.ReceipeList.ContainsKey(time.Date))
                {
                    Dictionary<string, Receipe> previousList = this.Model.ReceipeList[time.Date].ReceipeTimeOfDay[time.TimeOfDay].Receipes;
                    foreach (Receipe receipe in previousList.Values)
                    {
                        newList[receipe.Title] = receipe;
                    }
                }

                if (time.TimeOfDay.Equals("Matin"))
                {

                    morningReceipeViewSource.Source = newList;
                }
                else if (time.TimeOfDay.Equals("Midi"))
                {

                    noonReceipeViewSource.Source = newList;
                }
                else if (time.TimeOfDay.Equals("Soir"))
                {

                    evenningReceipeViewSource.Source = newList;
                }
            }
        }

        private void GoToDetailReceipe_Click(object sender, ItemClickEventArgs e)
        {

        }

        public async void RemoveReceipe_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.Frame != null && button != null)
            {
                Receipe re = (Receipe)button.DataContext;
                this.Model.RemoveReceipeList(re, (string)button.Tag, this.date);
                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile receipeFile = await folder.CreateFileAsync("receipes.json", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(receipeFile, this.Model.StringifyReceipesList());
            }
        }

    }
}
