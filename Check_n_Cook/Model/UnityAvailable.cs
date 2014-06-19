using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public static class UnityAvailable
    {
        public static List<string> unitys = null;

        public static List<string> GetUnity()
        {
            if (unitys == null)
            {
                unitys = new List<string>();
                unitys.Add("kg");
                unitys.Add("tasse");
                unitys.Add("bol");
                unitys.Add("cuillère");
                unitys.Add("cuillères");
                unitys.Add("g");
                unitys.Add("l");
                unitys.Add("cl");
                unitys.Add("kg");

            }
            return unitys;
        }

    }
}
