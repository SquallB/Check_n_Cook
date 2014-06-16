using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class SampleDataGroup
    {
        public SampleDataGroup(String title)
        {
            this.Title = title;
            this.Items = new List<ViewReceipeTimeOfDay>();
        }

        public string Title { get; private set; }
        public List<ViewReceipeTimeOfDay> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
