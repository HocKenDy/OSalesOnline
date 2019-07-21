using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class RePayPoints  : BaseModel
    {
        public RePayPoints()
        {
            
        }
        public string Code { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public double? TotalPoint { get; set; }
        public Nullable<bool> IsArchive { get; set; }
        public string CancelReason { get; set; }
        public Nullable<int> SaleId { get; set; }
        public string Note { get; set; }
        public double? AvailabilityPoint { get; set; }
        public int? WarehouseSourceId { get; set; }
        public string Status { get; set; }
        public Nullable<int> BranchId { get; set; }
    }
}
