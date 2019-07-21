using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities
{
    public class vwCustomer
    {
        public vwCustomer()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public int? BranchId { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string CityId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Images { get; set; }
        public double? Point { get; set; }
        public string Occupations { get; set; }

        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string GenderName { get; set; }

        public string CardCode { get; set; }
       // public int? MemberCardTypeId { get; set; }
        public string MemberCardTypeImage { get; set; }
        public string MemberCardTypeName { get; set; }
        public int? MemberCardId { get; set; }
        public double? PaidPoint { get; set; }
        public double? RemainingPoint { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public int? Frequency { get; set; }

        public Nullable<System.DateTime> EndDateProductInvoice { get; set; }
        //public double? SumPointProduct { get; set; }
        //public double? SumPointService { get; set; }
    }
}
