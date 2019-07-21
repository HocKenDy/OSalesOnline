using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class UsingServiceDetailViewModel
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

        
        [Display(Name = "Product", ResourceType = typeof(Wording))]
        public Nullable<int> ProductId { get; set; }
        [Display(Name = "TransactionSaler", ResourceType = typeof(Wording))]
        public Nullable<int> TransactionSalerId { get; set; }
        [Display(Name = "ServiceSale", ResourceType = typeof(Wording))]
        public Nullable<int> ServiceSaleId { get; set; }
        [Display(Name = "UsingService", ResourceType = typeof(Wording))]
        public Nullable<int> UsingServiceId { get; set; }

        public string GroupCode { get; set; }

        public Nullable<int> CustomerId { get; set; }
        public int? PackageProductId { get; set; }
        public Nullable<int> InvoiceId { get; set; }

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string PackageProductName { get; set; }
        public string PackageProductCode { get; set; }
        public string InvoiceCode { get; set; }

    }
}