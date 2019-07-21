using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class vwRePayPointsDetail : BaseModel
    {
        public vwRePayPointsDetail()
        {

        }
        public Nullable<int> RePayPointId { get; set; }
        public Nullable<int> GiftId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public double? Point { get; set; }
        public double? TotalPoint { get; set; }
        public string RePayPointsCode { get; set; }
        public Nullable<System.DateTime> RePayPointsDate { get; set; }
        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public string CategoryCode { get; set; }
        public string Unit { get; set; }
        public string Images { get; set; }
    }
}
