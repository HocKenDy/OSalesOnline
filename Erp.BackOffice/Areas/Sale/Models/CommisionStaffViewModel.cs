using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class CommisionSaleViewModel
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

        [Display(Name = "SaleId", ResourceType = typeof(Wording))]
        public Nullable<int> SaleId { get; set; }

        [Display(Name = "BranchId", ResourceType = typeof(Wording))]
        public Nullable<int> BranchId { get; set; }

        [Display(Name = "ProductInvoiceId", ResourceType = typeof(Wording))]
        public Nullable<int> ProductInvoiceId { get; set; }

        [Display(Name = "CommisionId", ResourceType = typeof(Wording))]
        public Nullable<int> CommisionId { get; set; }

        [Display(Name = "PercentOfCommision", ResourceType = typeof(Wording))]
        public Nullable<int> PercentOfCommision { get; set; }

        [Display(Name = "AmountOfCommision", ResourceType = typeof(Wording))]
        public string AmountOfCommision { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Wording))]
        public string Note { get; set; }

        [Display(Name = "CommisionName", ResourceType = typeof(Wording))]
        public string CommisionName { get; set; }

        [Display(Name = "StartDate", ResourceType = typeof(Wording))]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Display(Name = "EndDate", ResourceType = typeof(Wording))]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "ProductInvoiceCode", ResourceType = typeof(Wording))]
        public string ProductInvoiceCode { get; set; }

        [Display(Name = "SaleCode", ResourceType = typeof(Wording))]
        public string SaleCode { get; set; }

        [Display(Name = "BranchName", ResourceType = typeof(Wording))]
        public string BranchName { get; set; }

    }
}