using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwCommisionSaleMap : EntityTypeConfiguration<vwCommisionSale>
    {
        public vwCommisionSaleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Note).HasMaxLength(300);
            this.Property(t => t.CommisionName).HasMaxLength(150);
            this.Property(t => t.ProductInvoiceCode).HasMaxLength(20);
            this.Property(t => t.SaleCode).HasMaxLength(50);
            this.Property(t => t.BranchName).HasMaxLength(100);


            // Table & Column Mappings
            this.ToTable("vwSale_Commision_Sale");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.SaleId).HasColumnName("SaleId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.CommisionId).HasColumnName("CommisionId");
            this.Property(t => t.PercentOfCommision).HasColumnName("PercentOfCommision");
            this.Property(t => t.AmountOfCommision).HasColumnName("AmountOfCommision");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.CommisionName).HasColumnName("CommisionName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.ProductInvoiceCode).HasColumnName("ProductInvoiceCode");
            this.Property(t => t.SaleCode).HasColumnName("SaleCode");
            this.Property(t => t.BranchName).HasColumnName("BranchName");

        }
    }
}
