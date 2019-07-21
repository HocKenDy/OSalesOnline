using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class vwRePayPointsDetailMapper : EntityTypeConfiguration<vwRePayPointsDetail>
    {
        public vwRePayPointsDetailMapper()
        {
            ToTable("vwSale_RePayPointsDetail");
        }
    }
}

