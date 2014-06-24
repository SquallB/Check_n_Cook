using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;


namespace Check_n_Cook.Model
{
    /// <summary>
    /// Page used to format the receipe in an A4 page, which is meant to be printed.
    /// </summary>
    public sealed partial class ReceipePrintPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipePrintPage"/> class.
        /// </summary>
        public ReceipePrintPage() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipePrintPage"/> class.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
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
                String ingredientText = String.Format("- {0} {1} {2}", ingredient.quantity, ingredient.unity, ingredient.name);
                paragraph.Inlines.Add(new Run { Text = ingredientText});
                paragraph.Inlines.Add(new LineBreak());
            }

            this.ingredientsBlock.Blocks.Add(paragraph);
        }
    }
}
