using qts.webapp.backend.domain.Models.Administration;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class ProvinceMapper : EntityTypeConfiguration<Province>
    {
        public ProvinceMapper()
        {
            ToTable("System_Loc_Province");
        }
    }
}

