using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwProduct
    {
        public vwProduct()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal? PriceInbound { get; set; }
        public decimal? PriceOutbound { get; set; }
        public string Type { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Image_Name { get; set; }
        public int? MinInventory { get; set; }
        public string ServicesChild { get; set; }
        public bool? IsServicePackage { get; set; }
        public string Barcode { get; set; }
        public int? QuantityTotalInventory { get; set; }
        public string Size { get; set; }
        public string ProductGroup { get; set; }
        public string Manufacturer { get; set; }
        public string Categories { get; set; }
        public bool? NoInbound { get; set; }
        public int? Point { get; set; }
        public double? TargetPoint { get; set; }
        public string Alias { get; set; }
        public int? MinInventoryAlarms { get; set; }
        public string StatusInventory { get; set; }
        public int? CycleKM { get; set; }
        public int? CycleTime { get; set; }
        public int? RedemptionPoints { get; set; }
    }
}
