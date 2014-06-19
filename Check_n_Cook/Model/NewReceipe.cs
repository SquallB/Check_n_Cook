using Check_n_Cook.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class NewReceipe : AbstractModel
    {
        public Dictionary<string, Ingredient> ingredientsAdded;

        public NewReceipe()
        {
            ingredientsAdded = new Dictionary<string, Ingredient>();
        }

        public Dictionary<string, Ingredient> GetIngredientsAdded()
        {
            return ingredientsAdded;
        }

        public void AddIngredient(string name, string quantity, string unity)
        {
            if (!ingredientsAdded.ContainsKey(name))
            {
                this.ingredientsAdded[name] = new Ingredient(name, quantity, unity);
                RefreshViews(new ModifyIngredients(this));
            }
        }

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
