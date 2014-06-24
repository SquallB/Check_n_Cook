using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represens a date, contaning all the receipes added to it.
    /// </summary>
    public class ReceipeDate : ReceipeTime
    {
        /// <summary>
        /// Gets or sets the receipe time of day.
        /// </summary>
        /// <value>
        /// The receipe time of day.
        /// </value>
        public Dictionary<string, ReceipeTimeOfDay> ReceipeTimeOfDay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeDate"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        public ReceipeDate(string date)
        {
            this.ReceipeTimeOfDay = new Dictionary<string, ReceipeTimeOfDay>();
            this.ReceipeTimeOfDay.Add("Matin", new ReceipeTimeOfDay("Matin"));
            this.ReceipeTimeOfDay.Add("Midi", new ReceipeTimeOfDay("Midi"));
            this.ReceipeTimeOfDay.Add("Soir", new ReceipeTimeOfDay("Soir"));
            this.Time.Date = date;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeDate"/> class.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        public ReceipeDate(JsonObject jsonObject)
        {
            this.Time.Date = jsonObject.GetNamedString("Date", "");
            this.ReceipeTimeOfDay = new Dictionary<string, ReceipeTimeOfDay>();

            JsonObject objectReceipeDate = JsonObject.Parse(jsonObject.Stringify());
            JsonArray receipeDate = jsonObject.GetNamedArray("ReceipeDate");

            foreach (var receipeTimeOfDayJson in receipeDate)
            {
                JsonObject reTimeOfDayJson = JsonObject.Parse(receipeTimeOfDayJson.Stringify());
                ReceipeTimeOfDay reTimeOfDay = new ReceipeTimeOfDay(reTimeOfDayJson);
                this.ReceipeTimeOfDay.Add(reTimeOfDay.Time.TimeOfDay, reTimeOfDay);
            }

        }

        /// <summary>
        /// Creates and returns a JSON Object of the instance.
        /// </summary>
        /// <returns>a JSON object of the instance</returns>
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
