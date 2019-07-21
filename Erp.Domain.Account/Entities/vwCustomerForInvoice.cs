using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities
{
    public class vwCustomerForInvoice
    {
        public vwCustomerForInvoice()
        {
            
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Images { get; set; }
        public double? Point { get; set; }
        public double? PaidPoint { get; set; }
        public double? RemainingPoint { get; set; }
        public string CardCode { get; set; }
        public string MemberCardTypeImage { get; set; }
        public string MemberCardTypeName { get; set; }
        public int? MemberCardId { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public int? Frequency { get; set; }
        public int? CountInvoice { get; set; }
        public Nullable<System.DateTime> EndDateProductInvoice { get; set; }
        public decimal? TotalAmountInvoice { get; set; }
    }
}
