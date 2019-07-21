using System;
using System.Collections.Generic;

namespace Erp.Domain.Entities
{
    public partial class UserTypePage
    {
        public int UserTypeId { get; set; }
        public int PageId { get; set; }
        public int UserId { get; set; }
        //public virtual Page Page { get; set; }
    }
}
