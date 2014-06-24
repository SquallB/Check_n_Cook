using Check_n_Cook.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interface that contains methods shared by all the views.
/// </summary>
namespace Check_n_Cook.Views
{
    public interface View
    {
        /// <summary>
        /// Refreshes the view using the fired event.
        /// </summary>
        /// <param name="e">The fired event.</param>
        void Refresh(Event e);
    }
}
