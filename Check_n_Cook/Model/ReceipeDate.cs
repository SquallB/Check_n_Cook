using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class ReceipeDate : ReceipeTime
    {
        public Dictionary<string, ReceipeTimeOfDay> ReceipeTimeOfDay { get; set; }

        public ReceipeDate(string date)
        {
            this.ReceipeTimeOfDay = new Dictionary<string, ReceipeTimeOfDay>();
            this.ReceipeTimeOfDay.Add("Matin", new ReceipeTimeOfDay("Matin"));
            this.ReceipeTimeOfDay.Add("Midi", new ReceipeTimeOfDay("Midi"));
            this.ReceipeTimeOfDay.Add("Soir", new ReceipeTimeOfDay("Soir"));
            this.Time.Date = date;
        }

        public ReceipeDate(JsonObject jsonObject)
        {
            this.Time.Date = jsonObject.GetNamedString("Date", "");
            this.ReceipeTimeOfDay = new Dictionary<string, ReceipeTimeOfDay>();

            JsonArray receipeDate = jsonObject.GetNamedArray("ReceipeDate");

            foreach (var receipeTimeOfDayJson in receipeDate)
            {
                JsonObject reTimeOfDayJson = JsonObject.Parse(receipeTimeOfDayJson.Stringify());
                ReceipeTimeOfDay reTimeOfDay = new ReceipeTimeOfDay(reTimeOfDayJson);
                this.ReceipeTimeOfDay.Add(reTimeOfDay.Time.TimeOfDay, reTimeOfDay);
            }

        }
        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue("Date", JsonValue.CreateStringValue(this.Time.Date));
            JsonArray receipeDate = new JsonArray();
            foreach (ReceipeTimeOfDay reTimeOfDay in this.ReceipeTimeOfDay.Values)
            {
                receipeDate.Add(reTimeOfDay.ToJsonObject());
            }
            jsonObject.SetNamedValue("ReceipeDate", receipeDate);

            return jsonObject;
        }

    }
}
