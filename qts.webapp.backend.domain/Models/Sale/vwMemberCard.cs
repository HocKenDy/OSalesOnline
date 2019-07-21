using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class vwMemberCard  : BaseModel
    {
        public vwMemberCard()
        {
            
        }
                public string Status { get; set; }
        public string Code { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Type { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        public Nullable<int> MemberCardTypeId { get; set; }
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CreateUserName { get; set; }
        public string MemberCardTypeImage { get; set; }

    }
}
