
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class BaoCaoCongNoKHVuotHanMucViewModel
    {
        public int? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public decimal? ConLai { get; set; }
        public decimal? DaThu { get; set; }
        public decimal? MoneyLimit { get; set; }
        public decimal? MoneyVuot { get; set; }
        public string CustomerName { get; set; }

    }
}