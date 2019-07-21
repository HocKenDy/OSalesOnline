using qts.webapp.backend.domain.Models.Administration;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class vwWardMapper : EntityTypeConfiguration<vwWard>
    {
        public vwWardMapper()
        {
            ToTable("vwSystem_Loc_Ward");
        }
    }
}

