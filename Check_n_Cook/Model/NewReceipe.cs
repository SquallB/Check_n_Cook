using Check_n_Cook.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents a new receipe, added by the user.
    /// </summary>
    public class NewReceipe : AbstractModel
    {
        /// <summary>
        /// The ingredients added
        /// </summary>
        public Dictionary<string, Ingredient> ingredientsAdded;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewReceipe"/> class.
        /// </summary>
        public NewReceipe()
        {
            ingredientsAdded = new Dictionary<string, Ingredient>();
        }

        /// <summary>
        /// Gets the ingredients added.
        /// </summary>
        /// <returns>the ingredients added</returns>
        public Dictionary<string, Ingredient> GetIngredientsAdded()
        {
            return ingredientsAdded;
        }

        /// <summary>
        /// Adds the ingredient.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="unity">The unity.</param>
        public void AddIngredient(string name, string quantity, string unity)
        {
            if (!ingredientsAdded.ContainsKey(name))
            {
                this.ingredientsAdded[name] = new Ingredient(name, quantity, unity);
                RefreshViews(new ModifyIngredients(this));
            }
        }

        /// <summary>
        /// Removes the ingredient.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="unity">The unity.</param>
        public void RemoveIngredient(string name, string quantity, string unity)
        {
            if (ingredientsAdded.ContainsKey(name))
            {
                this.ingredientsAdded.Remove(name);
                RefreshViews(new ModifyIngredients(this));
            }
        }


    }
}
