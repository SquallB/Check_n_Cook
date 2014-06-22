using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class SampleDataGroup
    {
        public string Title { get; private set; }
        public List<ItemReceipeTimeOfDay> Items { get; private set; }

        public SampleDataGroup(String title)
        {
            this.Title = title;
            this.Items = new List<ItemReceipeTimeOfDay>();
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
