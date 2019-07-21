using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class CommisionBranchMap : EntityTypeConfiguration<CommisionBranch>
    {
        public CommisionBranchMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);


            // Table & Column Mappings
            this.ToTable("Sale_Commision_Branch");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CommisionId).HasColumnName("CommisionId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");

        }
    }
}
