using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model.Grade
{
    public class Grade
    {
        private int grade;
        private int min;
        private int max;

        public int Value
        {
            get { return grade; }
            set
            {
                if (min <= value && value <= max)
                {
                    grade = value;
                }
            }
        }
        public Grade(int min, int max, int value)
        {
            this.min = min;
            this.max = max;
            this.Value = value;
        }

        public int GetMax()
        {
            return max;
        }

        public int GetGrade()
        {
            return grade;
        }
        public Grade(String jsonString)
        {
            JsonObject jsonObject = JsonObject.Parse(jsonString);
            this.min = (int)jsonObject.GetNamedNumber("min", 0.0);
            this.max = (int)jsonObject.GetNamedNumber("max", 0.0);
            this.Value = (int)jsonObject.GetNamedNumber("Value", 0.0);
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue("min", JsonValue.CreateNumberValue(this.min));
            jsonObject.SetNamedValue("max", JsonValue.CreateNumberValue(this.max));
            jsonObject.SetNamedValue("Value", JsonValue.CreateNumberValue(this.Value));

            return jsonObject;
        }

        public String Stringify()
        {
            return this.ToJsonObject().Stringify();
        }
    }
}
