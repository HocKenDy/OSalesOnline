using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwUsingServiceDetailMap : EntityTypeConfiguration<vwUsingServiceDetail>
    {
        public vwUsingServiceDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            

            // Table & Column Mappings
            this.ToTable("vwSale_UsingServiceDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.ServiceSaleId).HasColumnName("ServiceSaleId");
            this.Property(t => t.TransactionSalerId).HasColumnName("TransactionSalerId");
            this.Property(t => t.UsingServiceId).HasColumnName("UsingServiceId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.GroupCode).HasColumnName("GroupCode");

            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");

            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.PackageProductCode).HasColumnName("PackageProductCode");
            this.Property(t => t.PackageProductName).HasColumnName("PackageProductName");
            this.Property(t => t.InvoiceCode).HasColumnName("InvoiceCode");
            this.Property(t => t.InvoiceSalerId).HasColumnName("InvoiceSalerId");
        }
    }
}
