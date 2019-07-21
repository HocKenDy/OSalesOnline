using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class SaleReportSumaryViewModel
    {
        public bool IsFilter_ByProductProperty { get; set; }
        public double Revenue { get; set; }
        public int NumberOfProductInvoice { get; set; }
        public int NumberOfProductInvoice_Pendding { get; set; }
        public int NumberOfProductInvoice_InProgress { get; set; }
        public int NumberOfSaleOrder { get; set; }
        public double SalesReturnAmount { get; set; }
        public int NumberOfSalesReturn { get; set; }
        public string ProductGroup { get; set; }
        public string Manufacturer { get; set; }
    }
}