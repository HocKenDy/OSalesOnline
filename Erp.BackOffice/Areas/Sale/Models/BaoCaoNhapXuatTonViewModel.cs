using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.BackOffice.Sale.Models
{
    public class BaoCaoNhapXuatTonViewModel
    {

        //public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        //public string ProductImage { get; set; }

        public int First_Remain { get; set; }
        //public int First_Remain_LS { get; set; }
        //public int First_Remain_KG { get; set; }
        //public int First_Remain_ML { get; set; }

        public int Center_InboundQuantity { get; set; }
        //public int Center_Quantity_Inbound_LS { get; set; }
        //public int Center_Quantity_Inbound_KG { get; set; }
        //public int Center_Quantity_Inbound_ML { get; set; }

        public int Center_OutboundQuantity { get; set; }
        //public int Center_Quantity_Outbound_LS { get; set; }
        //public int Center_Quantity_Outbound_KG { get; set; }
        //public int Center_Quantity_Outbound_ML { get; set; }

        public int Last_Remain { get; set; }
        //public int Last_Remain_LS { get; set; }
        //public int Last_Remain_KG { get; set; }
        //public int Last_Remain_ML { get; set; }
    }
}