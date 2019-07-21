using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class RePayPointsDetailMapper : EntityTypeConfiguration<RePayPointsDetail>
    {
        public RePayPointsDetailMapper()
        {
            ToTable("Sale_RePayPointsDetail");
        }
    }
}

