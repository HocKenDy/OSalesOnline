using Erp.BackOffice.Account.Models;
using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class RePayPointsViewModel
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

        [Display(Name = "Code", ResourceType = typeof(Wording))]
        public string Code { get; set; }
        [Display(Name = "CustomerId", ResourceType = typeof(Wording))]
        public Nullable<int> CustomerId { get; set; }
        [Display(Name = "TotalPoint", ResourceType = typeof(Wording))]
        public double? TotalPoint { get; set; }
        [Display(Name = "IsArchive", ResourceType = typeof(Wording))]
        public Nullable<bool> IsArchive { get; set; }
        [Display(Name = "CancelReason", ResourceType = typeof(Wording))]
        public string CancelReason { get; set; }
        [Display(Name = "Saler", ResourceType = typeof(Wording))]
        public Nullable<int> SaleId { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Wording))]
        public string Note { get; set; }
        public double? AvailabilityPoint { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "ProductItemCount", ResourceType = typeof(Wording))]
        public Nullable<int> ProductItemCount { get; set; }
        public List<RePayPointsDetailViewModel> DetailList { get; set; }
        public List<TransactionRelationshipViewModel> ListTransactionRelationship { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "WarehouseSource", ResourceType = typeof(Wording))]
        public int? WarehouseSourceId { get; set; }
        public int? BranchId { get; set; }
        public string Status { get; set; }
        [Display(Name = "CustomerName", ResourceType = typeof(Wording))]
        public string CustomerName { get; set; }
        [Display(Name = "WarehouseSource", ResourceType = typeof(Wording))]
        public string WarehouseSourceName { get; set; }
        [Display(Name = "BranchName", ResourceType = typeof(Wording))]
        public string BranchName { get; set; }
        [Display(Name = "Saler", ResourceType = typeof(Wording))]
        public string SaleName { get; set; }


        public bool AllowEdit { get; set; }
        public ProductOutboundViewModel ProductOutboundViewModel { get; set; }
    }

    public class RePayPointsStatus
    {
        public const string Pending = "pending";
        public const string Inprogress = "inprogress";
        public const string Complete = "complete";
        public const string Delete = "delete";
    }
}