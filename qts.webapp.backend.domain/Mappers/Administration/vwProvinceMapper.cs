using qts.webapp.backend.domain.Models.Administration;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class vwProvinceMapper : EntityTypeConfiguration<vwProvince>
    {
        public vwProvinceMapper()
        {
            ToTable("vwSystem_Loc_Province");
        }
    }
}

