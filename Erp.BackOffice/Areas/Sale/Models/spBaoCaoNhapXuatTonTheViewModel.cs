using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Erp.BackOffice.Sale.Models
{
    public class spBaoCaoNhapXuatTonTheViewModel
    {
        public Nullable<int> ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }

        public Nullable<int> First_Remain { get; set; }
        public Nullable<int> Center_InboundQuantity { get; set; }
        public Nullable<int> Center_OutboundQuantity { get; set; }
        public Nullable<int> Last_Remain { get; set; }

    }
}