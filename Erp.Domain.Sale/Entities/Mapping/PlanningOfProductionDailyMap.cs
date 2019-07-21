using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class PlanningOfProductionDailyMap : EntityTypeConfiguration<PlanningOfProductionDaily>
    {
        public PlanningOfProductionDailyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(150);
            

            // Table & Column Mappings
            this.ToTable("Sale_PlanningOfProductionDaily");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.PlanningOfProductionDetailId).HasColumnName("PlanningOfProductionDetailId");
            this.Property(t => t.TargetDay).HasColumnName("TargetDay");
            this.Property(t => t.TargetMonth).HasColumnName("TargetMonth");
            this.Property(t => t.TargetYear).HasColumnName("TargetYear");
            this.Property(t => t.MachineEstimates).HasColumnName("MachineEstimates");
            this.Property(t => t.MachineReality).HasColumnName("MachineReality");
            this.Property(t => t.QuantityEstimates).HasColumnName("QuantityEstimates");
            this.Property(t => t.QuantityReality).HasColumnName("QuantityReality");
            this.Property(t => t.Date).HasColumnName("Date"); 

        }
    }
}
