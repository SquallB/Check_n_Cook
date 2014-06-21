using Check_n_Cook.Events;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class ReceipeListSelected : AbstractModel
    {
        private List<SampleDataGroup> receipeListSelected;

        public ReceipeListSelected()
        {
            receipeListSelected = new List<SampleDataGroup>();
        }

        public List<SampleDataGroup> GetReceipeListSelected()
        {
            return this.receipeListSelected;
        }

        public void AddReceipeList(SampleDataGroup dataGroup)
        {
            receipeListSelected.Add(dataGroup);
            RefreshViews(new ModifyReceipeListPrint(this));    
        }

        public void RemoveReceipeList(SampleDataGroup dataGroup)
        {
            receipeListSelected.Remove(dataGroup);
            RefreshViews(new ModifyReceipeListPrint(this));
        }
    }
}
