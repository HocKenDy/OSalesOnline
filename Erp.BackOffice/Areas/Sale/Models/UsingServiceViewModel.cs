using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class UsingServiceViewModel
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

        [Display(Name = "Customer", ResourceType = typeof(Wording))]
        public Nullable<int> CustomerId { get; set; }
        [Display(Name = "Customer", ResourceType = typeof(Wording))]
        public string CustomerName { get; set; }
        [Display(Name = "CustomerCode", ResourceType = typeof(Wording))]
        public string CustomerCode { get; set; }


        [Display(Name = "Product", ResourceType = typeof(Wording))]
        public Nullable<int> ProductId { get; set; }
        [Display(Name = "Product", ResourceType = typeof(Wording))]
        public string ProductName { get; set; }
        [Display(Name = "ProductCode", ResourceType = typeof(Wording))]
        public string ProductCode { get; set; }

        [Display(Name = "PackageProduct", ResourceType = typeof(Wording))]
        public int? PackageProductId { get; set; }
        [Display(Name = "PackageProduct", ResourceType = typeof(Wording))]
        public string PackageProductName { get; set; }
        [Display(Name = "PackageProductCode", ResourceType = typeof(Wording))]
        public string PackageProductCode { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(Wording))]
        public Nullable<int> Quantity { get; set; }
        public int? QuantityRemaining { get; set; }

        [Display(Name = "Invoice", ResourceType = typeof(Wording))]
        public Nullable<int> InvoiceId { get; set; }
        [Display(Name = "InvoiceCode", ResourceType = typeof(Wording))]
        public string InvoiceCode { get; set; }

    }
}