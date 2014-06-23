using Check_n_Cook.Common;
using Check_n_Cook.Events;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class PlanningReceipe : Page, View
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private AppModel appModel;
        private Dictionary<string, CheckBox> checkBoxs;
        private Button printReceipeList;
        private ReceipeListSelected receipeListSelectedModel;
        private List<ItemReceipeListSelected> listReceipeListSelected;
        private Button delteReceipeList;
        private Button selectionMode;

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
            this.checkBoxs = new Dictionary<string, CheckBox>();
            this.receipeListSelectedModel = new ReceipeListSelected();
            this.listReceipeListSelected = new List<ItemReceipeListSelected>();
            this.receipeListSelectedModel.AddView(this);
            //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
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
            if (this.appModel == null)
            {
                this.appModel = e.NavigationParameter as AppModel;
                this.appModel.AddView(this);
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

                        sampleDataGroup.Items.Add(new ItemReceipeTimeOfDay(receipeDate.Time.Date, imgs, receipeTimeOfDay.Time.TimeOfDay));
                    }

                    sampleDataGroups.Add(sampleDataGroup);
                }
            }
            catch (FileNotFoundException ex)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("Error: " + ex.Message + " \nLocation: PlanningReceipe class.");
            }

            BubbleSort(sampleDataGroups);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        public void BubbleSort(List<SampleDataGroup> sampleDataGroups)
        {
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
            if (e.ClickedItem is ItemReceipeTimeOfDay)
            {
                ItemReceipeTimeOfDay viewReceipeTimeOfDay = (ItemReceipeTimeOfDay)e.ClickedItem;
                Time time = viewReceipeTimeOfDay.Time;
                if (this.appModel.ReceipeList.ContainsKey(time.Date) && this.appModel.ReceipeList[time.Date].ReceipeTimeOfDay.ContainsKey(time.TimeOfDay))
                {
                    ReceipeTimeOfDay receipeTImeOfDay = this.appModel.ReceipeList[time.Date].ReceipeTimeOfDay[time.TimeOfDay];
                    this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.appModel, time, receipeTImeOfDay));
                }

            }
        }
        private void GoToReceipeListAll_CLick(object sender, RoutedEventArgs e)
        {
            var group = (sender as FrameworkElement).DataContext;
            SampleDataGroup data = (SampleDataGroup)group;
            string date = (string)data.Title;
            Time time = new Time();
            time.Date = date;

            if (this.appModel.ReceipeList.ContainsKey(date))
            {
                ReceipeDate receipeDate = this.appModel.ReceipeList[date];
                this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.appModel, time, receipeDate));
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                SampleDataGroup data = (SampleDataGroup)cb.DataContext;
                receipeListSelectedModel.AddReceipeList(data);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                SampleDataGroup data = (SampleDataGroup)cb.DataContext;
                receipeListSelectedModel.RemoveReceipeList(data);
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                SampleDataGroup data = (SampleDataGroup)cb.DataContext;
                if (!checkBoxs.ContainsKey(data.Title))
                {
                    checkBoxs.Add(data.Title, cb);
                }
                else
                {
                    //Checkbox has already loaded
                    cb.Visibility = Visibility.Visible;
                }
            }
        }

        private void SelectionMode_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            if (printReceipeList.Visibility == Visibility.Collapsed)
            {
                printReceipeList.Visibility = Visibility.Visible;
                but.Content = "Désactiver la sélection";

                foreach (CheckBox ch in checkBoxs.Values)
                {
                    ch.Visibility = Visibility.Visible;
                }

                this.receipeListHubSection.Visibility = Visibility.Visible;
                this.delteReceipeList.Visibility = Visibility.Visible;
                this.selectionMode.Visibility = Visibility.Visible;
            }
            else if (printReceipeList.Visibility == Visibility.Visible)
            {
                printReceipeList.Visibility = Visibility.Collapsed;
                but.Content = "Activer la sélection";

                foreach (CheckBox ch in checkBoxs.Values)
                {
                    ch.IsChecked = false;
                    ch.Visibility = Visibility.Collapsed;
                }

                this.receipeListHubSection.Visibility = Visibility.Collapsed;
                this.delteReceipeList.Visibility = Visibility.Collapsed;
                this.selectionMode.Visibility = Visibility.Collapsed;
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
                ReceipeListSelected modelEvnt = (ReceipeListSelected)srcEvnt.Model;
                this.listReceipeListSelected.Clear();
                this.listReceipeListSelected = new List<ItemReceipeListSelected>();

                foreach (SampleDataGroup ing in modelEvnt.GetReceipeListSelected())
                {
                    List<string> imgs = new List<string>();
                    int nbImgFound = 0;
                    int i = 0;
                    int j = 0;
                    while (i < ing.Items.Count && nbImgFound < 4)
                    {
                        j = 0;
                        while (nbImgFound < 4 && j < ing.Items[i].ImagePaths.Count)
                        {
                            imgs.Add(ing.Items[i].ImagePaths[j]);
                            nbImgFound++;
                            j++;
                        }
                        i++;
                    }
                    listReceipeListSelected.Add(new ItemReceipeListSelected(ing.Title, imgs));
                }

                this.receipeListViewSource.Source = listReceipeListSelected;
            }
            else if (e is RemovedReceipeDateEvent)
            {
                RemovedReceipeDateEvent srcEvnt = (RemovedReceipeDateEvent)e;
                AppModel modelEvnt = (AppModel)srcEvnt.Model;
                List<SampleDataGroup> sampleDataGroups = new List<SampleDataGroup>();

                foreach (ReceipeDate receipeDate in modelEvnt.ReceipeList.Values)
                {
                    SampleDataGroup sampleDataGroup = new SampleDataGroup(receipeDate.Time.Date);

                    foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                    {
                        List<string> imgs = new List<string>();

                        foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                        {
                            imgs.Add(receipe.Image);
                        }

                        sampleDataGroup.Items.Add(new ItemReceipeTimeOfDay(receipeDate.Time.Date, imgs, receipeTimeOfDay.Time.TimeOfDay));
                    }

                    sampleDataGroups.Add(sampleDataGroup);
                }

                foreach (CheckBox cb in checkBoxs.Values)
                {
                    cb.Visibility = Visibility.Visible;
                }

                BubbleSort(sampleDataGroups);
                this.DefaultViewModel["Groups"] = sampleDataGroups;
            }
        }

        private void AddAllIngredients_Click(object sender, RoutedEventArgs e)
        {
            foreach (SampleDataGroup data in this.receipeListSelectedModel.GetReceipeListSelected())
            {
                string date = data.Title;

            }
        }

        private async void DeleteReceipeList_Click(object sender, RoutedEventArgs e)
        {
            List<SampleDataGroup> sampleDataGroups = new List<SampleDataGroup>();

            foreach (SampleDataGroup data in this.receipeListSelectedModel.GetReceipeListSelected())
            {
                sampleDataGroups.Add(data);
            }

            foreach (SampleDataGroup dataGroup in sampleDataGroups)
            {
                Time time = null;
                if (dataGroup.Items.Count >= 1)
                {
                    time = dataGroup.Items[0].Time;
                }

                //update the AppModel model
                this.appModel.RemoveReceipeDate(time);
                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile receipeFile = await folder.CreateFileAsync("receipes.json", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(receipeFile, this.appModel.StringifyReceipesList());

                //update the eceipeListSelected model
                this.receipeListSelectedModel.RemoveReceipeList(dataGroup);
            }
        }

        private void DeleteReceipeList_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                this.delteReceipeList = (Button)sender;
            }
        }

        private void UnselectReceipeList_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in checkBoxs.Values)
            {
                cb.IsChecked = false;
            }
        }

        private void SelectionMode_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                this.selectionMode = (Button)sender;
            }
        }

        private void ItemReceipeListSelected_Click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ItemReceipeListSelected)
            {
                ItemReceipeListSelected item = (ItemReceipeListSelected)e.ClickedItem;
                Time time = new Time();
                time.Date = item.Title;

                if (this.appModel.ReceipeList.ContainsKey(time.Date))
                {
                    ReceipeDate receipeDate = this.appModel.ReceipeList[time.Date];
                    this.Frame.Navigate(typeof(ReceipeList), new GoToReceipeListEvent(this.appModel, time, receipeDate));
                }
            }
        }

    }
}
