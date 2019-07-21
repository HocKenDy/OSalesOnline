using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class MemberCardTypeMapper : EntityTypeConfiguration<MemberCardType>
    {
        public MemberCardTypeMapper()
        {
            ToTable("Sale_MemberCardType");
        }
    }
}

