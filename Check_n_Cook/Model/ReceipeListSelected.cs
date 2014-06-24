using Check_n_Cook.Events;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents the list of receipes selected.
    /// </summary>
    public class ReceipeListSelected : AbstractModel
    {
        /// <summary>
        /// The receipe list selected
        /// </summary>
        private List<SampleDataGroup> receipeListSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeListSelected"/> class.
        /// </summary>
        public ReceipeListSelected()
        {
            receipeListSelected = new List<SampleDataGroup>();
        }

        /// <summary>
        /// Gets the receipe list selected.
        /// </summary>
        /// <returns></returns>
        public List<SampleDataGroup> GetReceipeListSelected()
        {
            return this.receipeListSelected;
        }

        /// <summary>
        /// Adds the receipe list.
        /// </summary>
        /// <param name="dataGroup">The data group.</param>
        public void AddReceipeList(SampleDataGroup dataGroup)
        {
            receipeListSelected.Add(dataGroup);
            RefreshViews(new ModifyReceipeListPrint(this));    
        }

        /// <summary>
        /// Removes the receipe list.
        /// </summary>
        /// <param name="dataGroup">The data group.</param>
        public void RemoveReceipeList(SampleDataGroup dataGroup)
        {
            receipeListSelected.Remove(dataGroup);
            RefreshViews(new ModifyReceipeListPrint(this));
        }
    }
}
