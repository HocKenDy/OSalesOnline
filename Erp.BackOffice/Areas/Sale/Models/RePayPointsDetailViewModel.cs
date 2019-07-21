using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class RePayPointsDetailViewModel
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

        [Display(Name = "AssignedUserId", ResourceType = typeof(Wording))]
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        //[StringLength(100, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError", ErrorMessage = null)]
        
		[Display(Name = "RePayPointId", ResourceType = typeof(Wording))]
        public Nullable<int> RePayPointId { get; set; }
        [Display(Name = "GiftId", ResourceType = typeof(Wording))]
        public Nullable<int> GiftId { get; set; }
        [Display(Name = "Quantity", ResourceType = typeof(Wording))]
        public Nullable<int> Quantity { get; set; }
        public int? QuantityInInventory { get; set; }
        public string Unit { get; set; }
        [Display(Name = "Point", ResourceType = typeof(Wording))]
        public double? Point { get; set; }
        [Display(Name = "TotalPoint", ResourceType = typeof(Wording))]
        public double? TotalPoint { get; set; }
        public int OrderNo { get;  set; }
        public string GiftCode { get; set; }
        public string GiftName { get;  set; }
        public string Images { get; set; }
        public string RePayPointsCode { get; set; }
        public Nullable<System.DateTime> RePayPointsDate { get; set; }
        public string CategoryCode { get; set; }

    }
}