using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class CommisionSaleMap : EntityTypeConfiguration<CommisionSale>
    {
        public CommisionSaleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
                        this.Property(t => t.Note).HasMaxLength(300);


            // Table & Column Mappings
            this.ToTable("Sale_Commision_Sale");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.SaleId).HasColumnName("SaleId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.PercentOfCommision).HasColumnName("PercentOfCommision");
            this.Property(t => t.AmountOfCommision).HasColumnName("AmountOfCommision");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.CommisionId).HasColumnName("CommisionId");
            this.Property(t => t.Note).HasColumnName("Note");

        }
    }
}
