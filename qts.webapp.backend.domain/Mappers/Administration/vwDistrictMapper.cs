using qts.webapp.backend.domain.Models.Administration;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class vwDistrictMapper : EntityTypeConfiguration<vwDistrict>
    {
        public vwDistrictMapper()
        {
            ToTable("vwSystem_Loc_District");
        }
    }
}

