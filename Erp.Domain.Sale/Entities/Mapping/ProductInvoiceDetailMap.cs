using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class ProductInvoiceDetailMap : EntityTypeConfiguration<ProductInvoiceDetail>
    {
        public ProductInvoiceDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            

            // Table & Column Mappings
            this.ToTable("Sale_ProductInvoiceDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.QuantitySerivceRemaining).HasColumnName("QuantitySerivceRemaining");
            this.Property(t => t.PromotionId).HasColumnName("PromotionId");
            this.Property(t => t.PromotionDetailId).HasColumnName("PromotionDetailId");
            this.Property(t => t.PromotionValue).HasColumnName("PromotionValue");
            this.Property(t => t.ProductType).HasColumnName("ProductType");
            this.Property(t => t.DisCount).HasColumnName("DisCount");
            this.Property(t => t.DisCountAmount).HasColumnName("DisCountAmount");
            this.Property(t => t.CheckPromotion).HasColumnName("CheckPromotion");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");

            this.Property(t => t.QuantitySaleReturn).HasColumnName("QuantitySaleReturn");
            this.Property(t => t.Point).HasColumnName("Point");

        }
    }
}
