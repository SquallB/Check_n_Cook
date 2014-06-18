using Check_n_Cook.Common;
using Check_n_Cook.Model;
using Check_n_Cook.Model.Data;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page Hub, consultez la page http://go.microsoft.com/fwlink/?LinkId=321224

namespace Check_n_Cook
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class ReceipeDetailPrint : Page
    {
        private Receipe receipe;
        private List<ItemReceipe> ingredients;

        public ReceipeDetailPrint(Receipe receipe, List<ItemReceipe> ingredients)
        {
            this.InitializeComponent();
            this.receipe = receipe;
            this.ingredients = ingredients;
            List<ItemReceipe> receipeView = new List<ItemReceipe>();
            this.pageTitle.Text = receipe.Title;
            this.ingredientsViewSource.Source = ingredients;
        }
    }
}
