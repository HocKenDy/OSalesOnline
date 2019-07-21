using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class MemberCardRepository : RepositoryBase<MemberCard>, IMemberCardRepository
    {
        public MemberCardRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IMemberCardRepository : IRepository<MemberCard>
    {
    }
}
