using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;


namespace Check_n_Cook.Model
{
    /// <summary>
    /// Page used to format the shopping list in an A4 page, which is meant to be printed.
    /// </summary>
    public sealed partial class ShoppingListPrintPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingListPrintPage"/> class.
        /// </summary>
        public ShoppingListPrintPage() : this(new List<ShoppingListGroup>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingListPrintPage"/> class.
        /// </summary>
        /// <param name="shoppingList">The shopping list.</param>
        public ShoppingListPrintPage(List<ShoppingListGroup> shoppingList)
        {
            InitializeComponent();

            foreach (ShoppingListGroup group in shoppingList)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run { Text = group.Name });
                paragraph.Inlines.Add(new LineBreak());
                paragraph.Inlines.Add(new LineBreak());

                foreach (ItemIngredient itemIngredient in group.Items)
                {
                    Ingredient ingredient = itemIngredient.Ingredient;
                    String ingredientText = String.Format("- {0} {1} {2}", ingredient.quantity, ingredient.unity, ingredient.name);
                    paragraph.Inlines.Add(new Run { Text = ingredientText });
                    paragraph.Inlines.Add(new LineBreak());
                }

                this.ingredientsBlock.Blocks.Add(paragraph);
            }

        }
    }
}
