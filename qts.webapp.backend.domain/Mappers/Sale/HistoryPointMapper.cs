using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class HistoryPointMapper : EntityTypeConfiguration<HistoryPoint>
    {
        public HistoryPointMapper()
        {
            ToTable("Sale_HistoryPoint");
        }
    }
}

