using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.BackOffice.Sale.Models
{
    public class BaoCaoTongHopThuChiViewModel
    {

        public DateTime VoucherDate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Category { get; set; }
        public string ReceiverUserName { get; set; }
        public string ProductNamePurchase { get; set; }
        public string ProductNameInvoice { get; set; }
        public string MaChungTuGoc { get; set; }
        public string LoaiChungTuGoc { get; set; }
        public decimal? Amount_Payment { get; set; }
        public decimal? Amount_Receipt { get; set; }
        public decimal? FirstAmount { get; set; }
        public decimal? LastAmount { get; set; }
    }
}