using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Erp.BackOffice.Sale.Models;
namespace Erp.BackOffice.Account.Models
{
    public class CustomerViewModel
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

        [Display(Name = "Code", ResourceType = typeof(Wording))]
        public string Code { get; set; }

        [Display(Name = "Birthday", ResourceType = typeof(Wording))]
        public Nullable<System.DateTime> Birthday { get; set; }
        [Display(Name = "Gender", ResourceType = typeof(Wording))]
        public Nullable<bool> Gender { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Wording))]
        public string Note { get; set; }
        [Display(Name = "SoNhaTenDuong", ResourceType = typeof(Wording))]
        public string Address { get; set; }
        [Display(Name = "WardName", ResourceType = typeof(Wording))]
        public string WardId { get; set; }
        [Display(Name = "DistrictName", ResourceType = typeof(Wording))]
        public string DistrictId { get; set; }
        [Display(Name = "CityName", ResourceType = typeof(Wording))]
        public string CityId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "Phone", ResourceType = typeof(Wording))]
        public string Phone { get; set; }
        [Display(Name = "Mobile", ResourceType = typeof(Wording))]
        public string Mobile { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Wording))]
        public string Email { get; set; }
        [Display(Name = "Occupations", ResourceType = typeof(Wording))]
        public string Occupations { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "Name", ResourceType = typeof(Wording))]
        public string Name { get; set; }

        public string NameSearch { get; set; }

        [Display(Name = "BonusScore", ResourceType = typeof(Wording))]
        public double? Point { get; set; }

        [Display(Name = "Province", ResourceType = typeof(Wording))]
        public string ProvinceName { get; set; }

        [Display(Name = "District", ResourceType = typeof(Wording))]
        public string DistrictName { get; set; }

        [Display(Name = "Ward", ResourceType = typeof(Wording))]
        public string WardName { get; set; }
        [Display(Name = "GenderName", ResourceType = typeof(Wording))]
        public string GenderName { get; set; }
        public string CardCode { get; set; }
        public string MemberCardTypeImage { get; set; }
        [Display(Name = "MemberCardTypeName", ResourceType = typeof(Wording))]
        public string MemberCardTypeName { get; set; }
        public DateTime? DateOfIssue { get; set; }
        [Display(Name = "MemberCardTypeName", ResourceType = typeof(Wording))]
        public int? MemberCardTypeId { get; set; }
        public List<ObjectAttributeValueViewModel> AttributeValueList { get; set; }
        [Display(Name = "PaidPoint", ResourceType = typeof(Wording))]
        public double? PaidPoint { get; set; }
        [Display(Name = "RemainingPoint", ResourceType = typeof(Wording))]
        public double? RemainingPoint { get; set; }

        [Display(Name = "Frequency", ResourceType = typeof(Wording))]
        public int? Frequency { get; set; }

        public Nullable<System.DateTime> EndDateProductInvoice { get; set; }
        public double? SumPointProduct { get; set; }
        public double? SumPointService { get; set; }
        [Display(Name = "MemberCardCode", ResourceType = typeof(Wording))]
        public string MemberCardCode { get; set; }
        [Display(Name = "IsCreateCard", ResourceType = typeof(Wording))]
        public bool? IsCreateCard { get; set; } 

    }
    public class CustomerJsonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public double Point { get; set; }
        public double PaidPoint { get; set; }
        public double RemainingPoint { get; set; }
        public int? MemberCardId { get; set; }
        public string CardCode { get; set; }
        public DateTime? EndDateProductInvoice { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public string MemberCardTypeImage { get; set; }
        public string MemberCardTypeName { get; set; }
        public decimal? TotalAmountInvoice { get; set; }
        public int? CountInvoice { get; set; }
        public string HtmlAppend { get; set; }
    }
}