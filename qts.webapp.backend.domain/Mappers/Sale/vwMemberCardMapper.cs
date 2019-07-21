using qts.webapp.backend.domain.Models.Sale;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class vwMemberCardMapper : EntityTypeConfiguration<vwMemberCard>
    {
        public vwMemberCardMapper()
        {
            ToTable("vwSale_MemberCard");
        }
    }
}

