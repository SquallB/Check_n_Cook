using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents a time of a day, with the all the receipes added to it.
    /// </summary>
    public class ReceipeTimeOfDay : ReceipeTime
    {
        /// <summary>
        /// Gets or sets the receipes.
        /// </summary>
        /// <value>
        /// The receipes.
        /// </value>
        public Dictionary<string, Receipe> Receipes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeTimeOfDay"/> class.
        /// </summary>
        /// <param name="timeOfDay">The time of day.</param>
        public ReceipeTimeOfDay(string timeOfDay)
        {
            this.Receipes = new Dictionary<string, Receipe>();
            this.Time.TimeOfDay = timeOfDay;
        }

        /// <summary>
        /// Adds the receipe.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public void AddReceipe(Receipe receipe)
        {
            this.Receipes[receipe.Title] = receipe;
        }

        /// <summary>
        /// Removes the receipe.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public void RemoveReceipe(Receipe receipe)
        {
            this.Receipes.Remove(receipe.Title);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeTimeOfDay"/> class.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        public ReceipeTimeOfDay(JsonObject jsonObject)
        {
            JsonObject receipeToDJson = JsonObject.Parse(jsonObject.Stringify());
            this.Time.TimeOfDay = receipeToDJson.GetNamedString("TimeOfDay");
            JsonArray receipesJson = receipeToDJson.GetNamedArray("Receipes");
            this.Receipes = new Dictionary<string, Receipe>();

            foreach (var receipeJson in receipesJson)
            {
                Receipe re = new Receipe(receipeJson.Stringify());
                this.Receipes[re.Title] = re;
            }

        }

        /// <summary>
        /// Creates and returns a JSON Object of the instance.
        /// </summary>
        /// <returns>a JSON object of the instance</returns>
        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            JsonArray receipes = new JsonArray();

            foreach (Receipe receipe in Receipes.Values)
            {
                receipes.Add(receipe.ToJsonObject());
            }
            jsonObject.SetNamedValue("Receipes", receipes);
            jsonObject.SetNamedValue("TimeOfDay", JsonValue.CreateStringValue(this.Time.TimeOfDay));

            return jsonObject;
        }
    }
}
