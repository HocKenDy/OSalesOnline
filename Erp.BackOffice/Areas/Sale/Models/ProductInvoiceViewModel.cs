using Erp.BackOffice.Account.Models;
using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class ProductInvoiceViewModel
    {
        public ProductInvoiceViewModel()
        {
            Discount = 0;
            TaxFee = 0;
            TotalAmount = 0;
        }

        public int Id { get; set; }

        [Display(Name = "IsDeleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "CreatedUser", ResourceType = typeof(Wording))]
        public int? CreatedUserId { get; set; }
        [Display(Name = "CreatedUserName", ResourceType = typeof(Wording))]
        public string CreatedUserName { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(Wording))]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "ModifiedUser", ResourceType = typeof(Wording))]
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }

        [Display(Name = "ModifiedDate", ResourceType = typeof(Wording))]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "MemberCard", ResourceType = typeof(Wording))]
        public string CardCode { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "CodeProductInvoice", ResourceType = typeof(Wording))]
        public string Code { get; set; }

        [Display(Name = "TotalNoVAT", ResourceType = typeof(Wording))]
        public decimal? TotalNoVAT { get; set; }
        [Display(Name = "TotalAmount", ResourceType = typeof(Wording))]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "TaxFee", ResourceType = typeof(Wording))]
        public double? TaxFee { get; set; }
        [Display(Name = "TongTienSauVAT", ResourceType = typeof(Wording))]
        public decimal TongTienSauVAT { get; set; }
        [Display(Name = "CustomerDiscount", ResourceType = typeof(Wording))]
        public double? Discount { get; set; }

        [Display(Name = "DiscountCode", ResourceType = typeof(Wording))]
        public string DiscountCode { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "Customer", ResourceType = typeof(Wording))]
        public Nullable<int> CustomerId { get; set; }

        [Display(Name = "CustomerDiscount", ResourceType = typeof(Wording))]
        public Nullable<int> CustomerDiscountId { get; set; }

        [Display(Name = "ShipName", ResourceType = typeof(Wording))]
        public string ShipName { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Wording))]
        public string ShipAddress { get; set; }

        [Display(Name = "WardName", ResourceType = typeof(Wording))]
        public string ShipWardId { get; set; }

        [Display(Name = "DistrictName", ResourceType = typeof(Wording))]
        public string ShipDistrictId { get; set; }

        [Display(Name = "CityName", ResourceType = typeof(Wording))]
        public string ShipCityId { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Wording))]
        public string Status { get; set; }
        [Display(Name = "Type", ResourceType = typeof(Wording))]
        public string Type { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Wording))]
        public string Note { get; set; }

        [Display(Name = "ShipPhone", ResourceType = typeof(Wording))]
        public string Phone { get; set; }

        [Display(Name = "PaymentNow", ResourceType = typeof(Wording))]
        public bool IsArchive { get; set; }

        public int? BranchId { get; set; }

        [Display(Name = "PaymentMethod", ResourceType = typeof(Wording))]
        public string PaymentMethod { get; set; }

        [Display(Name = "PaidAmount", ResourceType = typeof(Wording))]
        public decimal? PaidAmount { get; set; }

        [Display(Name = "RemainingAmount", ResourceType = typeof(Wording))]
        public decimal? RemainingAmount { get; set; }

        [Display(Name = "CancelReason", ResourceType = typeof(Wording))]
        public string CancelReason { get; set; }

        [Display(Name = "BarCode", ResourceType = typeof(Wording))]
        public string BarCode { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "Saler", ResourceType = typeof(Wording))]
        public int? SalerId { get; set; }

        [Display(Name = "Saler", ResourceType = typeof(Wording))]
        public string SalerName { get; set; }

        [Display(Name = "CustomerCode", ResourceType = typeof(Wording))]
        public string CustomerCode { get; set; }

        [Display(Name = "Customer", ResourceType = typeof(Wording))]
        public string CustomerName { get; set; }

        [Display(Name = "WardName", ResourceType = typeof(Wording))]
        public string ShipWardName { get; set; }

        [Display(Name = "DistrictName", ResourceType = typeof(Wording))]
        public string ShipDistrictName { get; set; }

        [Display(Name = "CityName", ResourceType = typeof(Wording))]
        public string ShipCityName { get; set; }

        [Display(Name = "BranchName", ResourceType = typeof(Wording))]
        public string BranchName { get; set; }

        [Display(Name = "ProductOutboundCode", ResourceType = typeof(Wording))]
        public string ProductOutboundCode { get; set; }

        public int? ProductOutboundId { get; set; }

        public bool? IsReturn { get; set; }

        public List<ProductInvoiceDetailViewModel> DetailList { get; set; }
        public List<ProductInvoiceDetailViewModel> GroupProduct { get; set; }
        public ReceiptViewModel ReceiptViewModel { get; set; }

        [Display(Name = "NextPaymentDate", ResourceType = typeof(Wording))]
        public DateTime? NextPaymentDate { get; set; }

        [Display(Name = "NextPaymentDate", ResourceType = typeof(Wording))]
        public DateTime? NextPaymentDate_Temp { get; set; }
        [Display(Name = "CodeInvoiceRed", ResourceType = typeof(Wording))]
        public string CodeInvoiceRed { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "ProductItemCount", ResourceType = typeof(Wording))]
        public Nullable<int> ProductItemCount { get; set; }
        [Display(Name = "DisCountAmount", ResourceType = typeof(Wording))]
        public int? DisCountAmount { get; set; }
        public bool AllowEdit { get; set; }
        public ProductOutboundViewModel ProductOutboundViewModel { get; set; }
        public List<TransactionLiabilitiesViewModel> ListTransactionLiabilities { get; set; }
        public List<TransactionRelationshipViewModel> ListTransactionRelationship { get; set; }
        public List<ProcessPaymentViewModel> ListProcessPayment { get; set; }
        public int? QuantityCodeSaleReturns { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "WarehouseSource", ResourceType = typeof(Wording))]
        public int? WarehouseSourceId { get; set; }
        public bool? IsSale { get; set; }
        public string ProductImage { get; set; }
        public bool? IsPayment { get; set; }
        public string SaleOrderCode { get; set; }
        [Display(Name = "AccumulatedPoint", ResourceType = typeof(Wording))]
        public double? AccumulatedPoint { get; set; }
        public bool? CheckUsePoint { get; set; }
        [Display(Name = "UsePoint", ResourceType = typeof(Wording))]
        public double? UsePoint { get; set; }
        [Display(Name = "UsePointAmount", ResourceType = typeof(Wording))]
        public decimal? UsePointAmount { get; set; }
        public double? AvailabilityPoint { get; set; }
        [Display(Name = "Frequency", ResourceType = typeof(Wording))]
        public int? Frequency { get; set; }
        public bool? IsEdited { get; set; }
        [Display(Name = "EditNote", ResourceType = typeof(Wording))]
        public string EditNote { get; set; }
    }
    public class ProductInvoiceStatus
    {
        public const string Pending = "pending";
        public const string Inprogress = "inprogress";
        public const string Shipping = "shipping";
        public const string Complete = "complete";
        public const string Delete = "delete";
    }
    public class ProductInvoiceType
    {
        public const string Product = "product";
        public const string Service = "service";
        public const string All = "all";
    }
}