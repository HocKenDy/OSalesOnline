using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwCommision_BranchMap : EntityTypeConfiguration<vwCommision_Branch>
    {
        public vwCommision_BranchMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BranchName).HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("vwSale_Commision_Branch");
            this.Property(t => t.Id).HasColumnName("Id");

            this.Property(t => t.CommisionId).HasColumnName("CommisionId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.CommisionName).HasColumnName("CommisionName");
            this.Property(t => t.PercentOfCommision).HasColumnName("PercentOfCommision");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
        }
    }
}
