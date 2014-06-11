using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    class PlanningEntry
    {
        public DateTime PlanningDate { get; set; }

        public PlanningType PlanningType { get; set; }

        public PlanningEntry()
        {

        }

        public PlanningEntry(DateTime date, PlanningType plantype)
        {
            this.PlanningDate = date;
            this.PlanningType = plantype;
        }
    }
}
