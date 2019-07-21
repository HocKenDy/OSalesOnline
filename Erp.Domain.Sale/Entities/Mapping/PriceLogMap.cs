using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class PriceLogMap : EntityTypeConfiguration<PriceLog>
    {
        public PriceLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            // Table & Column Mappings
            this.ToTable("Sale_PriceLog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.StartDateApply).HasColumnName("StartDateApply");
            this.Property(t => t.PriceInbound).HasColumnName("PriceInbound");
            this.Property(t => t.PriceOutbound).HasColumnName("PriceOutbound");
        }
    }
}
