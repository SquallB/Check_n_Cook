using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Json;
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
    public sealed partial class PlanningReceipe : Page, View
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private AppModel Model;
        private List<CheckBox> checkBoxs;
        private Button printReceipeList;
        private PrintReceipe printReceipe;
        private List<SampleDataGroup> newPrintReceipeList;

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

        public PlanningReceipe()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.checkBoxs = new List<CheckBox>();
            this.printReceipe = new PrintReceipe();
            this.newPrintReceipeList = new List<SampleDataGroup>();
            this.printReceipe.AddView(this);
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
            if (this.Model == null)
            {
                this.Model = e.NavigationParameter as AppModel;
            }
            StorageFolder folder = KnownFolders.PicturesLibrary;
            List<SampleDataGroup> sampleDataGroups = new List<SampleDataGroup>();
            try
            {
                StorageFile receipesFile = await folder.GetFileAsync("receipes.json");
                String jsonString = await FileIO.ReadTextAsync(receipesFile);
                JsonObject jsonObject = JsonObject.Parse(jsonString);
                JsonArray jsonArray = jsonObject.GetNamedArray("Receipes");

                foreach (var jsonDate in jsonArray)
                {
                    JsonObject receipeDateJson = JsonObject.Parse(jsonDate.Stringify());
                    ReceipeDate receipeDate = new ReceipeDate(receipeDateJson);
                    SampleDataGroup sampleDataGroup = new SampleDataGroup(receipeDate.Time.Date);

                    foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                    {
                        List<string> imgs = new List<string>();

                        foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                        {
                            imgs.Add(receipe.Image);
                        }

                        sampleDataGroup.Items.Add(new ViewReceipeTimeOfDay(receipeDate.Time.Date, imgs, receipeTimeOfDay.Time.TimeOfDay));
                    }

                    sampleDataGroups.Add(sampleDataGroup);
                }
            }
            catch (FileNotFoundException ex)
            {

            }

            int n = sampleDataGroups.Count;
            bool swapped = true;
            while (swapped)
            {
                swapped = false;
                for (int i = 0; i < n - 1; i++)
                {
                    DateTime date1 = Convert.ToDateTime(sampleDataGroups[i].Title);
                    DateTime date2 = Convert.ToDateTime(sampleDataGroups[i + 1].Title);

                    if (DateTime.Compare(date1, date2) > 0)
                    {
                        swapped = true;
                        SampleDataGroup tmp = sampleDataGroups[i];
                        sampleDataGroups[i] = sampleDataGroups[i + 1];
                        sampleDataGroups[i + 1] = tmp;
                    }
                    n--;
                }
            }

            this.DefaultViewModel["Groups"] = sampleDataGroups;
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

        public void GoToReceipeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ModifyReceipeList));
            }
        }

        public void GoToReceipeList_CLick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.Frame != null && button != null)
            {
                Time time = (Time)button.DataContext;

                ReceipeDate receipeDate = this.Model.ReceipeList[time.Date];
                if (receipeDate != null)
                {
                    ReceipeTimeOfDay receipeTimeOfDay = receipeDate.ReceipeTimeOfDay[time.TimeOfDay];
                    if (receipeTimeOfDay != null)
                    {
                        receipeTimeOfDay.Time.Date = receipeDate.Time.Date;
                        this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.Model, time, receipeTimeOfDay));
                    }
                }
            }
        }

        private void GoToReceipeListAll_CLick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.Frame != null && button != null)
            {
                string date = (string)button.DataContext;
                Time time = new Time();
                time.Date = date;
                ReceipeDate receipeDate = this.Model.ReceipeList[date];

                this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.Model, time, receipeDate));
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                SampleDataGroup data = (SampleDataGroup)cb.DataContext;
                printReceipe.AddReceipeList(data);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                SampleDataGroup data = (SampleDataGroup)cb.DataContext;
                printReceipe.RemoveReceipeList(data);
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                checkBoxs.Add((CheckBox)sender);
            }
        }

        private void SelectReceipeLIst_Click(object sender, RoutedEventArgs e)
        {
            printReceipeList.Visibility = Visibility.Visible;

            foreach (CheckBox ch in checkBoxs)
            {
                ch.Visibility = Visibility.Visible;
            }
        }

        private void PrintReceipeList_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                printReceipeList = (Button)sender;
            }
        }

        public void Refresh(Event e)
        {
            if (e is ModifyReceipeListPrint)
            {
                ModifyReceipeListPrint srcEvnt = (ModifyReceipeListPrint)e;
                PrintReceipe modelEvnt = (PrintReceipe)srcEvnt.Model;
                this.newPrintReceipeList.Clear();
                this.newPrintReceipeList = new List<SampleDataGroup>();

                foreach (SampleDataGroup ing in modelEvnt.GetReceipePrintList())
                {
                    newPrintReceipeList.Add(ing);
                }

                this.receipeListViewSource.Source = newPrintReceipeList;
            }
        }

    }
}
