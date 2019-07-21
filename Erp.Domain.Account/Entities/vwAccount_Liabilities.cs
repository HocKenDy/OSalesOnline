using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities
{
    public class vwAccount_Liabilities
    {
        public vwAccount_Liabilities()
        {
            
        }

        public string TargetModule { get; set; }
        public string TargetCode { get; set; }
        public string TargetName { get; set; }
        public decimal Remain { get; set; }

    }
}
