using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class CommisionBranch
    {
        public int Id { get; set; }
        public int? CommisionId { get; set; }
        public int? BranchId { get; set; }
    }
}
