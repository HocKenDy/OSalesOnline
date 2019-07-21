using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "IsDeleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "CreatedUser", ResourceType = typeof(Wording))]
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(Wording))]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "ModifiedUser", ResourceType = typeof(Wording))]
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }

        [Display(Name = "ModifiedDate", ResourceType = typeof(Wording))]
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [StringLength(100, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError", ErrorMessage = null)]
        [Display(Name = "Name", ResourceType = typeof(Wording))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "ProductCode", ResourceType = typeof(Wording))]
        public string Code { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Wording))]
        public string Description { get; set; }

        [Display(Name = "Unit", ResourceType = typeof(Wording))]
        public string Unit { get; set; }

        [Display(Name = "PriceInbound", ResourceType = typeof(Wording))]
        public decimal? PriceInbound { get; set; }

        [Display(Name = "PriceOutbound", ResourceType = typeof(Wording))]
        public decimal? PriceOutbound { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Wording))]
        public string Type { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "CategoryCode", ResourceType = typeof(Wording))]
        public string CategoryCode { get; set; }

        [Display(Name = "CategoryCode", ResourceType = typeof(Wording))]
        public string CategoryName { get; set; }

        [Display(Name = "MinInventory", ResourceType = typeof(Wording))]
        public int? MinInventory { get; set; }

        [Display(Name = "ServicesChild", ResourceType = typeof(Wording))]
        public string ServicesChild { get; set; }

        [Display(Name = "IsServicePackage", ResourceType = typeof(Wording))]
        public bool? IsServicePackage { get; set; }

        [Display(Name = "Barcode", ResourceType = typeof(Wording))]
        public string Barcode { get; set; }

        //[Display(Name = "Barcode", ResourceType = typeof(Wording))]
        public string Image_Name { get; set; }

        public int? QuantityTotalInventory { get; set; }

        public List<ObjectAttributeValueViewModel> AttributeValueList { get; set; }

        public List<ServiceInPackage> ServicesInPackage { get; set; }

        [Display(Name = "ProductCapacity", ResourceType = typeof(Wording))]
        public string Size { get; set; }
        [Display(Name = "ProductGroup", ResourceType = typeof(Wording))]
        public string ProductGroup { get; set; }

        [Display(Name = "Manufacturer", ResourceType = typeof(Wording))]
        public string Manufacturer { get; set; }
        [Display(Name = "CategoriesProduct", ResourceType = typeof(Wording))]
        public string Categories { get; set; }
        [Display(Name = "NoInbound", ResourceType = typeof(Wording))]
        public bool? NoInbound { get; set; }

        [Display(Name = "Point", ResourceType = typeof(Wording))]
        public int? Point { get; set; }

        [Display(Name = "TargetPoint", ResourceType = typeof(Wording))]
        public double? TargetPoint { get; set; }
        [Display(Name = "Alias", ResourceType = typeof(Wording))]
        public string Alias { get; set; }
        [Display(Name = "MinInventoryAlarms", ResourceType = typeof(Wording))]
        public int? MinInventoryAlarms { get; set; }

        public List<PriceLogViewModel> ListPriceLog { get; set; }
        [Display(Name = "CycleKM", ResourceType = typeof(Wording))]
        public int? CycleKM { get; set; }
        [Display(Name = "CycleTime", ResourceType = typeof(Wording))]
        public int? CycleTime { get; set; }
        [Display(Name = "RedemptionPoints", ResourceType = typeof(Wording))]
        public int? RedemptionPoints { get; set; }
    }
    public class ProductType
    {
        public const string Product = "product";
        public const string Service = "service";
        public const string Card = "card";
        public const string Gift = "gift";
    }
    public class ServiceInPackage
    {
        public int? ProductId { set; get; }
        public int? Quantity { set; get; }
    }
}