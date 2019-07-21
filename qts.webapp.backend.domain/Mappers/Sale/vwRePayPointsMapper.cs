using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class vwRePayPointsMapper : EntityTypeConfiguration<vwRePayPoints>
    {
        public vwRePayPointsMapper()
        {
            ToTable("vwSale_RePayPoints");
        }
    }
}

