using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwProductInvoice
    {
        public vwProductInvoice()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string Code { get; set; }
        public decimal? TotalNoVAT { get; set; }
        public decimal? TotalAmount { get; set; }
        public double? TaxFee { get; set; }
        public double? Discount { get; set; }
        public string DiscountCode { get; set; }

        public Nullable<int> CustomerId { get; set; }
        public string ShipName { get; set; }
        public string Phone { get; set; }
        public string ShipAddress { get; set; }
        public string ShipWardId { get; set; }
        public string ShipDistrictId { get; set; }
        public string ShipCityId { get; set; }
       // public string Type { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool IsArchive { get; set; }
        public int? BranchId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public string CancelReason { get; set; }
        public string BarCode { get; set; }

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNameSearch { get; set; }
        public string ShipWardName { get; set; }
        public string ShipDistrictName { get; set; }
        public string ShipCityName { get; set; }
        public string ProductOutboundCode { get; set; }
        public string BranchName { get; set; }

        public int? CustomerDiscountId { get; set; }

        public int? SalerId { get; set; }
        public string CodeInvoiceRed { get; set; }
        public string SalerFullName { get; set; }
        public int? ProductOutboundId { get; set; }
        public Nullable<bool> IsReturn { get; set; }
        public Nullable<System.DateTime> NextPaymentDate { get; set; }
        public string UserFullName { get; set; }
        public int? WarehouseSourceId { get; set; }
        public double? AccumulatedPoint { get; set; }
        public double? UsePoint { get; set; }
        public bool? CheckUsePoint { get; set; }
        public decimal? UsePointAmount { get; set; }
        public int? Frequency { get; set; }
   //     public bool? IsEdited { get; set; }
     //   public string EditNote { get; set; }
    }
}
