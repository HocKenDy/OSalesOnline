using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class vwCarMapper : EntityTypeConfiguration<vwCar>
    {
        public vwCarMapper()
        {
            ToTable("vwSale_Cars");
        }
    }
}

