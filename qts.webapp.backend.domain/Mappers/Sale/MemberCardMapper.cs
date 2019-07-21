using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class MemberCardMapper : EntityTypeConfiguration<MemberCard>
    {
        public MemberCardMapper()
        {
            ToTable("Sale_MemberCard");
        }
    }
}

