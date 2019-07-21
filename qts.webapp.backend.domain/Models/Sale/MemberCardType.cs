using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class MemberCardType : BaseModel
    {
        public MemberCardType()
        {

        }
        public string Name { get; set; }
        public string Image { get; set; }
        public double? TargetPoint { get; set; }
        public Nullable<int> ServiceDiscount { get; set; }
        public int? CardId { get; set; }
    }
}
