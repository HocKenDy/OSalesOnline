using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class HistoryPointViewModel
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

        [Display(Name = "CustomerId", ResourceType = typeof(Wording))]
        public Nullable<int> CustomerId { get; set; }
        public string CustomerName { get; set; }
        [Display(Name = "TargetId", ResourceType = typeof(Wording))]
        public Nullable<int> TargetId { get; set; }
        [Display(Name = "TargetName", ResourceType = typeof(Wording))]
        public string TargetName { get; set; }
        [Display(Name = "AccumulatedPoint", ResourceType = typeof(Wording))]
        public double? AccumulatedPoint { get; set; }
        [Display(Name = "UsePoint", ResourceType = typeof(Wording))]
        public double? UsePoint { get; set; }
        public double? AmountPoint { get; set; }
        public int? MemberCardId { get; set; }


    }
}