using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// Pour en savoir plus sur le modèle d'élément Page de base, consultez la page http://go.microsoft.com/fwlink/?LinkId=234237

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Page de base qui inclut des caractéristiques communes à la plupart des applications.
    /// </summary>
    public sealed partial class ReceipePrintPage : Page
    {
        public ReceipePrintPage() : this(null) { }

        public ReceipePrintPage(Receipe receipe)
        {
            this.InitializeComponent();

            this.pageTitle.Text = receipe.Title;

            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = receipe.ToDoInstructions });
            paragraph.Inlines.Add(new LineBreak());
            this.instructionsBlock.Blocks.Add(paragraph);

            paragraph = new Paragraph();

            foreach (Ingredient ingredient in receipe.ingredients)
            {
                String ingredientText = String.Format("- {0} {1} {2}", ingredient.name, ingredient.quantity, ingredient.unity);
                paragraph.Inlines.Add(new Run { Text = ingredientText});
                paragraph.Inlines.Add(new LineBreak());
            }

            this.ingredientsBlock.Blocks.Add(paragraph);
        }
    }
}
