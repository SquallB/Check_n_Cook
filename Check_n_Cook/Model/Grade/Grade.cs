using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model.Grade
{
    /// <summary>
    /// 
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// The grade
        /// </summary>
        private int grade;
        /// <summary>
        /// The minimum
        /// </summary>
        private int min;
        /// <summary>
        /// The maximum
        /// </summary>
        private int max;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="Grade"/> class.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="value">The value.</param>
        public Grade(int min, int max, int value)
        {
            this.min = min;
            this.max = max;
            this.Value = value;
        }

        /// <summary>
        /// Gets the maximum.
        /// </summary>
        /// <returns></returns>
        public int GetMax()
        {
            return max;
        }

        /// <summary>
        /// Gets the grade.
        /// </summary>
        /// <returns></returns>
        public int GetGrade()
        {
            return grade;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Grade"/> class.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        public Grade(String jsonString)
        {
            JsonObject jsonObject = JsonObject.Parse(jsonString);
            this.min = (int)jsonObject.GetNamedNumber("min", 0.0);
            this.max = (int)jsonObject.GetNamedNumber("max", 0.0);
            this.Value = (int)jsonObject.GetNamedNumber("Value", 0.0);
        }

        /// <summary>
        /// To the json object.
        /// </summary>
        /// <returns></returns>
        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue("min", JsonValue.CreateNumberValue(this.min));
            jsonObject.SetNamedValue("max", JsonValue.CreateNumberValue(this.max));
            jsonObject.SetNamedValue("Value", JsonValue.CreateNumberValue(this.Value));

            return jsonObject;
        }

        /// <summary>
        /// Stringifies this instance.
        /// </summary>
        /// <returns></returns>
        public String Stringify()
        {
            return this.ToJsonObject().Stringify();
        }
    }
}
