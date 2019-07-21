using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwUsingService
    {
        public vwUsingService()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public int? PackageProductId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public int? QuantityRemaining { get; set; }
        public Nullable<int> InvoiceId { get; set; }

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string PackageProductName { get; set; }
        public string PackageProductCode { get; set; }
        public string InvoiceCode { get; set; }
        public int? InvoiceSalerId { get; set; }
    }
}
