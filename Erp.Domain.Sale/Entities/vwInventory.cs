using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwInventory
    {
        public vwInventory()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<int> WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> Quantity { get; set; }

        public string CategoryCode { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPriceInbound { get; set; }
        public decimal? ProductPriceOutbound { get; set; }
        public string ProductUnit { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarcode { get; set; }
        public string ProductManufacturer { get; set; }
        public Nullable<int> CBTK { get; set; }
        public bool? IsSale { get; set; }
        public string ProductImage { get; set; }
        public string ProductGroup { get; set; }
        public int? ProductMinInventory { get; set; }
        public int? ProductMinInventoryAlarms { get; set; }
        public string StatusInventory { get; set; }
        public double? TargetPoint { get; set; }
        public int? Point { get; set; }
        public string ProductType { get; set; }
        public int? RedemptionPoints { get; set; }
    }
}
