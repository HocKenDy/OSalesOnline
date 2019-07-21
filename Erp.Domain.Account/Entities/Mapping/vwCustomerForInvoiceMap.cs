using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities.Mapping
{
    public class vwCustomerForInvoiceMap : EntityTypeConfiguration<vwCustomerForInvoice>
    {
        public vwCustomerForInvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("vwSale_CustomerForInvoice");
            this.Property(t => t.Id).HasColumnName("Id");
        }
    }
}
