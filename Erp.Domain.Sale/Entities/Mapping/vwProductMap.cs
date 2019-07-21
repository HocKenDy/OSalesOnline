using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwProductMap : EntityTypeConfiguration<vwProduct>
    {
        public vwProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("vwSale_Product");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.PriceInbound).HasColumnName("PriceInbound");
            this.Property(t => t.PriceOutbound).HasColumnName("PriceOutbound");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.CategoryName).HasColumnName("CategoryName");
            this.Property(t => t.MinInventory).HasColumnName("MinInventory");
            this.Property(t => t.Barcode).HasColumnName("Barcode");
            this.Property(t => t.Image_Name).HasColumnName("Image_Name");
            this.Property(t => t.QuantityTotalInventory).HasColumnName("QuantityTotalInventory");
            this.Property(t => t.IsServicePackage).HasColumnName("IsServicePackage");
            this.Property(t => t.ServicesChild).HasColumnName("ServicesChild");
            this.Property(t => t.Size).HasColumnName("Size");
            this.Property(t => t.ProductGroup).HasColumnName("ProductGroup");
            this.Property(t => t.Manufacturer).HasColumnName("Manufacturer");
            this.Property(t => t.Categories).HasColumnName("Categories");
            this.Property(t => t.NoInbound).HasColumnName("NoInbound");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.TargetPoint).HasColumnName("TargetPoint");
        }
    }
}
