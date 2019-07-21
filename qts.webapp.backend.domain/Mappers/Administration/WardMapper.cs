using qts.webapp.backend.domain.Models.Administration;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class WardMapper : EntityTypeConfiguration<Ward>
    {
        public WardMapper()
        {
            ToTable("System_Loc_Ward");
        }
    }
}

