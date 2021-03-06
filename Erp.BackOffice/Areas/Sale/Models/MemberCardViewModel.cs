using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class MemberCardViewModel
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
        
		        [Display(Name = "Status", ResourceType = typeof(Wording))]
        public string Status { get; set; }
        [Display(Name = "Code", ResourceType = typeof(Wording))]
        public string Code { get; set; }
        [Display(Name = "BranchId", ResourceType = typeof(Wording))]
        public Nullable<int> BranchId { get; set; }
        [Display(Name = "Type", ResourceType = typeof(Wording))]
        public string Type { get; set; }
        [Display(Name = "CustomerId", ResourceType = typeof(Wording))]
        public Nullable<int> CustomerId { get; set; }
        [Display(Name = "DateOfIssue", ResourceType = typeof(Wording))]
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        [Display(Name = "MemberCardTypeId", ResourceType = typeof(Wording))]
        public Nullable<int> MemberCardTypeId { get; set; }
        public string MemberCardTypeImage { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public List<MemberCardTypeViewModel> ListMemberCardType { get; set; }
        public List<MemberCardViewModel> ListMemberCard { get;  set; }
        public const string Active = "active";
        public const string Inactive = "inactive";
    }
}