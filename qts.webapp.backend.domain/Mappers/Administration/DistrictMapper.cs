using qts.webapp.backend.domain.Models.Administration;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class DistrictMapper : EntityTypeConfiguration<District>
    {
        public DistrictMapper()
        {
            ToTable("System_Loc_District");
        }
    }
}

