using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents an ingredient of a receipe.
    /// </summary>
    public class Ingredient
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public string quantity { get; set; }

        /// <summary>
        /// Gets or sets the unity.
        /// </summary>
        /// <value>
        /// The unity.
        /// </value>
        public string unity { get; set; }

        /// <summary>
        /// Gets the description. Created using the quantity and unity.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get { if (unity != null && quantity != null) { return quantity + " " + unity; } else { return ""; } } }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ingredient"/> class.
        /// </summary>
        public Ingredient()
        {
            this.Image = "/Assets/ingredient1.png";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ingredient"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="qty">The quantity.</param>
        /// <param name="unity">The unity.</param>
        public Ingredient(string name, string qty, string unity)
        {
            this.name = name;
            this.quantity = qty;
            this.unity = unity;
            this.Image = "/Assets/ingredient1.png";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ingredient"/> class.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        public Ingredient(JsonObject jsonObject)
        {
            this.unity = jsonObject.GetNamedString("unity");
            this.quantity = jsonObject.GetNamedString("quantity");
            this.name = jsonObject.GetNamedString("name");
            this.Image = "/Assets/ingredient1.png";
        }

        /// <summary>
        /// Creates and returns a JSON Object of the instance.
        /// </summary>
        /// <returns>a JSON object of the instance</returns>
        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject.SetNamedValue("name", JsonValue.CreateStringValue(this.name));
            jsonObject.SetNamedValue("quantity", JsonValue.CreateStringValue(this.quantity));
            jsonObject.SetNamedValue("unity", JsonValue.CreateStringValue(this.unity));

            return jsonObject;
        }


        /// <summary>
        /// Creates a clone of the instance
        /// </summary>
        /// <returns>a clone of the instance</returns>
        public Ingredient ToClone()
        {
            return new Ingredient(this.name, this.quantity, this.unity);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
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
