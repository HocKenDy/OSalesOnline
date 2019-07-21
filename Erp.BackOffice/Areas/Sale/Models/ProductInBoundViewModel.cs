using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Erp.BackOffice.Account.Models;

namespace Erp.BackOffice.Sale.Models
{
    public class ProductInboundViewModel
    {
        public int Id { get; set; }

        [Display(Name = "IsDeleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "CreatedUser", ResourceType = typeof(Wording))]
        public int? CreatedUserId { get; set; }

        [Display(Name = "ReceiverName", ResourceType = typeof(Wording))]
        public string CreatedUserName { get; set; }

        [Display(Name = "ProductInboundCreatedDate", ResourceType = typeof(Wording))]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "ModifiedUser", ResourceType = typeof(Wording))]
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }

        [Display(Name = "ModifiedDate", ResourceType = typeof(Wording))]
        public DateTime? ModifiedDate { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "ProductInboundCode", ResourceType = typeof(Wording))]
        public string Code { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Wording))]
        public string Note { get; set; }

        [Display(Name = "WarehouseSource", ResourceType = typeof(Wording))]
        public Nullable<int> WarehouseSourceId { get; set; }

        [Display(Name = "WarehouseSource", ResourceType = typeof(Wording))]
        public string WarehouseSourceName { get; set; }

        [Display(Name = "WarehouseKeeperId", ResourceType = typeof(Wording))]
        public Nullable<int> WarehouseKeeperId { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Wording))]
        public string Type { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "WarehouseDestination", ResourceType = typeof(Wording))]
        public Nullable<int> WarehouseDestinationId { get; set; }

        [Display(Name = "WarehouseDestination", ResourceType = typeof(Wording))]
        public string WarehouseDestinationName { get; set; }

        [Display(Name = "PurchaseOrderCode", ResourceType = typeof(Wording))]
        public int? PurchaseOrderId { get; set; }
        [Display(Name = "PurchaseOrderCode", ResourceType = typeof(Wording))]
        public string PurchaseOrderCode { get; set; }

        [Display(Name = "ThanhTien", ResourceType = typeof(Wording))]
        public decimal TotalAmount { get; set; }
        public int VAT { get; set; }
        public decimal TotalVAT { get; set; }

        [Display(Name = "TongCong", ResourceType = typeof(Wording))]
        public decimal Total { get; set; }

        public bool? IsDone { get; set; }

        [Display(Name = "ShipperName", ResourceType = typeof(Wording))]
        public string ShipperName { get; set; }

        [Display(Name = "ShipperPhone", ResourceType = typeof(Wording))]
        public string ShipperPhone { get; set; }

        [Display(Name = "ShipCompany", ResourceType = typeof(Wording))]
        public Nullable<int> ShipSupplierId { get; set; }

        [Display(Name = "Supplier", ResourceType = typeof(Wording))]
        public Nullable<int> SupplierId { get; set; }

        [Display(Name = "Supplier", ResourceType = typeof(Wording))]
        public string SupplierName { get; set; }

        public List<ProductInboundDetailViewModel> DetailList { get; set; }
        public PaymentViewModel PaymentViewModel { get; set; }
        public List<WarehouseLocationItemViewModel> LocationItemList { get; set; }
        [Display(Name = "NextPaymentDate", ResourceType = typeof(Wording))]
        public DateTime? NextPaymentDate { get; set; }
        [Display(Name = "PaymentNow", ResourceType = typeof(Wording))]
        public bool IsPayment { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "ProductItemCount", ResourceType = typeof(Wording))]
        public Nullable<int> ProductItemCount { get; set; }

        public bool IsArchive { get; set; }
        public bool AllowEdit { get; set; }
    }

    public class ProductInboundType
    {
        public const string Manual = "manual";
        public const string External = "external";
        public const string Physical = "physical";
        public const string PhysicalCard = "physicalCard";
        public const string Internal = "internal";
        public const string Card = "card";
        public const string Gift = "gift";
    }
}