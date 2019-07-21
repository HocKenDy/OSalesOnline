using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class vwPost
    {
        public vwPost()
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

        public string Content { get; set; }
        public Nullable<int> TargetId { get; set; }
        public string TargetModule { get; set; }
        public Nullable<int> StudentId { get; set; }
        public string Attachment { get; set; }
        public string CreatedUserName { get; set; }
        public string ProfileImage { get; set; }
    }
}
