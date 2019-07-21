using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwCommisionSale
    {
        public vwCommisionSale()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public Nullable<int> SaleId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> ProductInvoiceId { get; set; }
        public Nullable<int> CommisionId { get; set; }
        public Nullable<int> PercentOfCommision { get; set; }
        public decimal? AmountOfCommision { get; set; }
        public string Note { get; set; }
        public string CommisionName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ProductInvoiceCode { get; set; }
        public string SaleCode { get; set; }
        public string BranchName { get; set; }

    }
}
