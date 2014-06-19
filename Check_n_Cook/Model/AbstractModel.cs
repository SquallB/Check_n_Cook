using Check_n_Cook.Events;
using Check_n_Cook.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class AbstractModel
    {
        private List<View> views;

        public AbstractModel()
        {
            this.views = new List<View>();
        }

        public void AddView(View view)
        {
            this.views.Add(view);
        }

        public void RemoveView(View view)
        {
            this.views.Remove(view);
        }

        public void RefreshViews(Event e)
        {
            foreach (View view in this.views)
            {
                view.Refresh(e);
            }
        }
    }
}
