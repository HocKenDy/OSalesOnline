using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class PurchaseOrderDetail
    {
        public PurchaseOrderDetail()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<int> PurchaseOrderId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public decimal? Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Unit { get; set; }
        public int? QuantityRemaining { get; set; }
        public int? DisCount { get; set; }
        public int? DisCountAmount { get; set; }
    }
}
