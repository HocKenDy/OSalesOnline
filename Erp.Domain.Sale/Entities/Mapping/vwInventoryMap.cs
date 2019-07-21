using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwInventoryMap : EntityTypeConfiguration<vwInventory>
    {
        public vwInventoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            

            // Table & Column Mappings
            this.ToTable("vwSale_Inventory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.WarehouseId).HasColumnName("WarehouseId");
            this.Property(t => t.WarehouseName).HasColumnName("WarehouseName");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.ProductPriceInbound).HasColumnName("ProductPriceInbound");
            this.Property(t => t.ProductPriceOutbound).HasColumnName("ProductPriceOutbound");
            this.Property(t => t.ProductUnit).HasColumnName("ProductUnit");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.ProductBarcode).HasColumnName("ProductBarcode");
            this.Property(t => t.ProductManufacturer).HasColumnName("ProductManufacturer");
            this.Property(t => t.CBTK).HasColumnName("CBTK");
            this.Property(t => t.ProductImage).HasColumnName("ProductImage");
            this.Property(t => t.IsSale).HasColumnName("IsSale");
            this.Property(t => t.TargetPoint).HasColumnName("TargetPoint");
            this.Property(t => t.Point).HasColumnName("Point");
        }
    }
}
