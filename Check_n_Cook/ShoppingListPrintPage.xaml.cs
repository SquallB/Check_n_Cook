using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// Pour en savoir plus sur le modèle d'élément Page de base, consultez la page http://go.microsoft.com/fwlink/?LinkId=234237

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Page de base qui inclut des caractéristiques communes à la plupart des applications.
    /// </summary>
    public sealed partial class ShoppingListPrintPage : Page
    {
        public ShoppingListPrintPage() : this(new Dictionary<string, Dictionary<string, Ingredient>>()) { }

        public ShoppingListPrintPage(Dictionary<string, Dictionary<string, Ingredient>> ingredients)
        {
            InitializeComponent();

            Paragraph paragraph = new Paragraph();

            foreach (Dictionary<string, Ingredient> group in ingredients.Values)
            {

                foreach (Ingredient ingredient in group.Values)
                {
                    String ingredientText = String.Format("- {0} {1} {2}", ingredient.quantity, ingredient.unity, ingredient.name);
                    paragraph.Inlines.Add(new Run { Text = ingredientText });
                    paragraph.Inlines.Add(new LineBreak());
                }
            }

            this.ingredientsBlock.Blocks.Add(paragraph);
        }
    }
}
