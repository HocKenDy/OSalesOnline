using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class RePayPointsMapper : EntityTypeConfiguration<RePayPoints>
    {
        public RePayPointsMapper()
        {
            ToTable("Sale_RePayPoints");
        }
    }
}

