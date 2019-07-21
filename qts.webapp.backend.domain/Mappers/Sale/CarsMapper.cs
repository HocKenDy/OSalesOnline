using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class CarsMapper : EntityTypeConfiguration<Cars>
    {
        public CarsMapper()
        {
            ToTable("Sale_Cars");
        }
    }
}

