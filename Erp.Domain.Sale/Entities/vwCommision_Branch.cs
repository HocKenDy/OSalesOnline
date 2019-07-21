using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwCommision_Branch
    {
        public vwCommision_Branch()
        {
            
        }

        public int Id { get; set; }

        public Nullable<int> CommisionId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string CommisionName { get; set; }
        public Nullable<int> PercentOfCommision { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string BranchName { get; set; }

    }
}
