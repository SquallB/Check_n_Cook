using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents to type of a dish (desert for exemple).
    /// </summary>
    public class DishType
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// The instances. Allows not to have multiple instances of the same type of dish.
        /// </summary>
        private static Dictionary<String, DishType> instances = new Dictionary<String, DishType>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DishType"/> class. It is private because "GetInstance" is used to get an instance of the class from the outside.
        /// </summary>
        /// <param name="name">The name.</param>
        private DishType(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets an instance of the class, with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static DishType GetInstance(String name)
        {
            DishType instance;

            if (instances.ContainsKey(name))
            {
                instance = instances[name];
            }
            else
            {
                instance = new DishType(name);
                instances.Add(name, instance);
            }

            return instance;
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
            if (obj is DishType)
            {
                return this.Name.Equals(((DishType)obj).Name);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if an instance with the specified name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>true if the instance exists, false otherwise</returns>
        public static bool Exists(String name)
        {
            return instances.ContainsKey(name);
        }

        /// <summary>
        /// Creates and returns a JSON Object of the instance.
        /// </summary>
        /// <returns>a JSON object of the instance</returns>
        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue("Name", JsonValue.CreateStringValue(this.Name));

            return jsonObject;
        }

        /// <summary>
        /// Stringifies this instance and returns a JSON string of the insance.
        /// </summary>
        /// <returns>a JSON string of the instance</returns>
        public String Stringify()
        {
            return this.ToJsonObject().Stringify();
        }
    }
}
