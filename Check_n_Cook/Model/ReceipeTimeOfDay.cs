using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class ReceipeTimeOfDay
    {
        public List<Receipe> Receipes { get; set; }
        public string TimeOfDay { get; set; }

        public ReceipeTimeOfDay(string timeOfDay)
        {
            this.Receipes = new List<Receipe>();
            this.TimeOfDay = timeOfDay;
        }

        public ReceipeTimeOfDay(JsonObject jsonObject)
        {
            JsonObject receipeToDJson = JsonObject.Parse(jsonObject.Stringify());
            this.TimeOfDay = receipeToDJson.GetNamedString("TimeOfDay");
            JsonArray receipesJson = receipeToDJson.GetNamedArray("Receipes");
            this.Receipes = new List<Receipe>();

            foreach (var receipeJson in receipesJson)
            {
                this.Receipes.Add(new Receipe(receipeJson.Stringify()));
            }

        }
        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            JsonArray receipes = new JsonArray();

            foreach (Receipe receipe in Receipes)
            {
                receipes.Add(receipe.ToJsonObject());
            }
            jsonObject.SetNamedValue("Receipes", receipes);
            jsonObject.SetNamedValue("TimeOfDay", JsonValue.CreateStringValue(this.TimeOfDay));

            return jsonObject;
        }
    }
}
