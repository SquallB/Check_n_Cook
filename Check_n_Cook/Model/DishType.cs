using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class DishType
    {
        public String Name { get; set; }

        private static Dictionary<String, DishType> instances = new Dictionary<String, DishType>();

        private DishType(String name)
        {
            this.Name = name;
        }

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

        public static bool Exists(String name)
        {
            return instances.ContainsKey(name);
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue("Name", JsonValue.CreateStringValue(this.Name));

            return jsonObject;
        }

        public String Stringify()
        {
            return this.ToJsonObject().Stringify();
        }
    }
}
