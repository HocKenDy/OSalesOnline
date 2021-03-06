using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Account.Models
{
    public class ProcessPaymentViewModel
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

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [StringLength(100, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError", ErrorMessage = null)]
        [Display(Name = "NamePayment", ResourceType = typeof(Wording))]
        public string Name { get; set; }

        [Display(Name = "OrderNo", ResourceType = typeof(Wording))]
        public Nullable<int> OrderNo { get; set; }
        [Display(Name = "DayPayment", ResourceType = typeof(Wording))]
        public Nullable<System.DateTime> DayPayment { get; set; }
        [Display(Name = "MoneyPayment", ResourceType = typeof(Wording))]
        public Nullable<int> MoneyPayment { get; set; }
        [Display(Name = "FormPayment", ResourceType = typeof(Wording))]
        public string FormPayment { get; set; }
        [Display(Name = "CodeTrading", ResourceType = typeof(Wording))]
        public string CodeTrading { get; set; }
        [Display(Name = "Bank", ResourceType = typeof(Wording))]
        public string Bank { get; set; }
        [Display(Name = "Payer", ResourceType = typeof(Wording))]
        public string Payer { get; set; }
        public int? ContractId { get; set; }
        [Display(Name = "Status", ResourceType = typeof(Wording))]
        public string Status { get; set; }
    }
}