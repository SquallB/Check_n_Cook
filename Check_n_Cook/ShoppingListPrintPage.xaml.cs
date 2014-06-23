using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;


namespace Check_n_Cook.Model
{
    /// <summary>
    /// Page de base qui inclut des caractéristiques communes à la plupart des applications.
    /// </summary>
    public sealed partial class ShoppingListPrintPage : Page
    {
        public ShoppingListPrintPage() : this(new List<ShoppingListGroup>()) { }

        public ShoppingListPrintPage(List<ShoppingListGroup> shoppingList)
        {
            InitializeComponent();

            foreach (ShoppingListGroup group in shoppingList)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run { Text = group.Name });
                paragraph.Inlines.Add(new LineBreak());
                paragraph.Inlines.Add(new LineBreak());

                foreach (Ingredient ingredient in group.Items)
                {
                    String ingredientText = String.Format("- {0} {1} {2}", ingredient.quantity, ingredient.unity, ingredient.name);
                    paragraph.Inlines.Add(new Run { Text = ingredientText });
                    paragraph.Inlines.Add(new LineBreak());
                }

                this.ingredientsBlock.Blocks.Add(paragraph);
            }

        }
    }
}
