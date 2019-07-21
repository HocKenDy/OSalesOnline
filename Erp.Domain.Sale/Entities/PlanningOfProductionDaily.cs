using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class PlanningOfProductionDaily
    {
        public PlanningOfProductionDaily()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        public string Name { get; set; }

        public Nullable<int> PlanningOfProductionDetailId { get; set; }
        public Nullable<int> TargetDay { get; set; }
        public Nullable<int> TargetMonth { get; set; }
        public Nullable<int> TargetYear { get; set; }
        public string MachineEstimates { get; set; }
        public string MachineReality { get; set; }
        public Nullable<int> QuantityEstimates { get; set; }
        public Nullable<int> QuantityReality { get; set; }
        public Nullable<System.DateTime> Date { get; set; }

        //heaert

    }
}
