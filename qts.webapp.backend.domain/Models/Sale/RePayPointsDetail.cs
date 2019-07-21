using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class RePayPointsDetail  : BaseModel
    {
        public RePayPointsDetail()
        {
            
        }
        public Nullable<int> RePayPointId { get; set; }
        public Nullable<int> GiftId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public double? Point { get; set; }
        public double? TotalPoint { get; set; }
    }
}
