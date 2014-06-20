using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class Ingredient
    {
        public String name { get; set; }
        public string quantity { get; set; }
        public string unity { get; set; }
        public string Description { get { if (unity != null && quantity != null) { return quantity + " " + unity; } else { return ""; } } }
        public string Image { get; set; }
        public Ingredient()
        {

        }
        public Ingredient(string name, string qty, string unity)
        {
            this.name = name;
            this.quantity = qty;
            this.unity = unity;
            this.Image = "/Assets/ingredient1.png";
        }

        public Ingredient(JsonObject jsonObject)
        {
            this.unity = jsonObject.GetNamedString("unity");
            this.quantity = jsonObject.GetNamedString("quantity");
            this.name = jsonObject.GetNamedString("name");
            this.Image = "/Assets/ingredient1.png";
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject.SetNamedValue("name", JsonValue.CreateStringValue(this.name));
            jsonObject.SetNamedValue("quantity", JsonValue.CreateStringValue(this.quantity));
            jsonObject.SetNamedValue("unity", JsonValue.CreateStringValue(this.unity));

            return jsonObject;
        }

        public Ingredient ToClone()
        {
            return new Ingredient(this.name, this.quantity, this.unity);
        }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is Ingredient)
            {
                Ingredient other = (Ingredient)obj;

                if (this.name == other.name)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
