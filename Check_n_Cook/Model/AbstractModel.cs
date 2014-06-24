using Check_n_Cook.Events;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains all the common operations for models. Here contains the views, and operations to add, remove and refresh them.
/// </summary>
namespace Check_n_Cook.Model
{
    public class AbstractModel
    {
        /// <summary>
        /// The views
        /// </summary>
        private List<View> views;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractModel"/> class.
        /// </summary>
        public AbstractModel()
        {
            this.views = new List<View>();
        }

        /// <summary>
        /// Adds the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public void AddView(View view)
        {
            this.views.Add(view);
        }

        /// <summary>
        /// Removes the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public void RemoveView(View view)
        {
            this.views.Remove(view);
        }

        /// <summary>
        /// Refreshes the views.
        /// </summary>
        /// <param name="e">The e.</param>
        public void RefreshViews(Event e)
        {
            foreach (View view in this.views)
            {
                view.Refresh(e);
            }
        }
    }
}
