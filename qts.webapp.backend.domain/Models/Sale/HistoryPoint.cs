using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class HistoryPoint : BaseModel
    {
        public HistoryPoint()
        {

        }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> TargetId { get; set; }
        public string TargetName { get; set; }
        public double? AccumulatedPoint { get; set; }
        public double? UsePoint { get; set; }

    }

}
