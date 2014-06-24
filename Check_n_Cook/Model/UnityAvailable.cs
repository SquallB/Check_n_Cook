using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents all the available unities.
    /// </summary>
    public static class UnityAvailable
    {
        /// <summary>
        /// The unities
        /// </summary>
        public static List<string> unitys = null;

        /// <summary>
        /// Gets the unities.
        /// </summary>
        /// <returns>a list of available unities</returns>
        public static List<string> GetUnity()
        {
            if (unitys == null)
            {
                unitys = new List<string>();
                unitys.Add(" ");

                unitys.Add("kg");
                unitys.Add("tasse");
                unitys.Add("bol");
                unitys.Add("cuillère");
                unitys.Add("cuillères");
                unitys.Add("g");
                unitys.Add("l");
                unitys.Add("cl");

            }
            return unitys;
        }

    }
}
