using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class ReceipeTimeOfDay : ReceipeTime
    {
        public Dictionary<string, Receipe> Receipes { get; set; }
        public ReceipeTimeOfDay(string timeOfDay)
        {
            this.Receipes = new Dictionary<string, Receipe>();
            this.Time.TimeOfDay = timeOfDay;
        }

        public void AddReceipe(Receipe receipe)
        {
            this.Receipes[receipe.Title] = receipe;
        }

        public void RemoveReceipe(Receipe receipe)
        {
            this.Receipes.Remove(receipe.Title);
        }
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
