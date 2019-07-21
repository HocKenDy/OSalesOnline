using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.domain.Mappers
{
    public class BaseMapper<T> :  EntityTypeConfiguration<T>  where T : BaseModel 
    {
        public BaseMapper()
        {
            //ToTable("Gadgets");
            Property(g => g.Id).HasColumnName("Id").IsRequired();
            Property(g => g.CreatedDate).HasColumnName("CreatedDate");
            Property(g => g.CreatedUserId).HasColumnName("CreatedUserId");
            Property(g => g.ModifiedDate).HasColumnName("ModifiedDate");
            Property(g => g.ModifiedUserId).HasColumnName("ModifiedUserId");
            Property(g => g.IsDeleted).HasColumnName("IsDeleted");
        }
    }
}
