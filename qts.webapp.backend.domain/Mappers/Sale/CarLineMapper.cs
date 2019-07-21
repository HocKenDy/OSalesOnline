using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class CarLineMapper : EntityTypeConfiguration<CarLine>
    {
        public CarLineMapper()
        {
            ToTable("Sale_CarLine");
        }
    }
}

