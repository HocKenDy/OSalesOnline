using qts.webapp.backend.domain.Models.<AREA_NAME>;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class <MODULE_NAME>Mapper : EntityTypeConfiguration<<MODULE_NAME>>
    {
        public <MODULE_NAME>Mapper()
        {
            ToTable("<AREA_NAME>_<MODULE_NAME>");
        }
    }
}

