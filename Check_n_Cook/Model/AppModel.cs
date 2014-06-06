using Check_n_Cook.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class AppModel : AbstractModel
    {
        public List<Receipe> Receipes { get; set; }

        public List<DishType> DishTypes { get; set; }

        public AppModel()
        {
            this.Receipes = new List<Receipe>();
            this.DishTypes = new List<DishType>();
        }

        public void AddReceipe(Receipe receipe)
        {
            this.Receipes.Add(receipe);

            if (!this.DishTypes.Contains(receipe.DishType))
            {
                this.DishTypes.Add(receipe.DishType);
            }


            this.RefreshViews(new AddedReceipeEvent(this, receipe));
        }

        public void RemoveReceipe(Receipe receipe)
        {
            this.Receipes.Remove(receipe);
            this.RefreshViews(new RemovedReceipeEvent(this, receipe));
        }
    }
}
