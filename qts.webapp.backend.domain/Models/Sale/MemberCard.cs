using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class MemberCard  : BaseModel
    {
        public MemberCard()
        {
            
        }
                public string Status { get; set; }
        public string Code { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Type { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        public Nullable<int> MemberCardTypeId { get; set; }

    }
}
