using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code).HasMaxLength(20);
            this.Property(t => t.Name).HasMaxLength(150);


            // Table & Column Mappings
            this.ToTable("Sale_Customer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.NameSearch).HasColumnName("NameSearch");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.WardId).HasColumnName("WardId");
            this.Property(t => t.DistrictId).HasColumnName("DistrictId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.MemberCardId).HasColumnName("MemberCardId");
        }
    }
}
