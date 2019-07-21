using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwProductInvoiceDetail
    {
        public vwProductInvoiceDetail()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<int> ProductInvoiceId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public decimal? Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public int? QuantitySerivceRemaining { get; set; }
        public string Unit { get; set; }
        public string ProductType { get; set; }

        public int? PromotionId { get; set; }
        public int? PromotionDetailId { get; set; }
        public double? PromotionValue { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductGroup { get; set; }
        public string ProductInvoiceCode { get; set; }
        //public int? CustomerId { get; set; }
        public int? DisCount { get; set; }
        public int? DisCountAmount { get; set; }
        public Nullable<bool> CheckPromotion { get; set; }
        public bool IsReturn { get; set; }
        public bool IsArchive { get; set; }
        public System.DateTime ProductInvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public int SalerId { get; set; }
        public string SalerFullName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public int? QuantitySaleReturn { get; set; }
        public double? Point { get;  set; }
        public int? CycleKM { get; set; }
        public int? CycleTime { get; set; }
        public double? ProductTargetPoint { get; set; }
        public int? ProductPoint { get; set; }
    }
}
