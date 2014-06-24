using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents the operation for an itemReceipe or itemIngredient
    /// </summary>
    public static class ToolItem
    {
        /// <summary>
        /// Creates the item receipe.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <returns></returns>
        public static ItemReceipe CreateItemReceipe(Receipe receipe)
        {
            ItemReceipe item = new ItemReceipe(receipe);
            item.Name = receipe.Title;
            TextBlock toto = new TextBlock();

            toto.Text = item.Name;
            toto.Measure(new Size(1200, 1200));
            if (toto.DesiredSize.Width > 90)
            {
                while (toto.DesiredSize.Width > 90)
                {
                    item.Name = item.Name.Remove(item.Name.Length - 1, 1);
                    toto.Text = item.Name;
                    toto.Measure(new Size(1200, 1200));

                }
                item.Name = item.Name.PadRight(item.Name.Length + 3, '.');
            }

            return item;
        }

        /// <summary>
        /// Creates the item ingredient.
        /// </summary>
        /// <param name="ingredient">The ingredient.</param>
        /// <returns></returns>
        public static ItemIngredient CreateItemIngredient(Ingredient ingredient)
        {
            ItemIngredient item = new ItemIngredient(ingredient);
            item.Name = ingredient.name;
            TextBlock toto = new TextBlock();

            toto.Text = item.Name;
            toto.Measure(new Size(1200, 1200));
            if (toto.DesiredSize.Width > 90)
            {
                while (toto.DesiredSize.Width > 90)
                {
                    item.Name = item.Name.Remove(item.Name.Length - 1, 1);
                    toto.Text = item.Name;
                    toto.Measure(new Size(1200, 1200));

                }
                item.Name = item.Name.PadRight(item.Name.Length + 3, '.');
            }

            return item;
        }
       
    }
}
