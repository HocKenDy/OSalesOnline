using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class vwMemberCardRepository : RepositoryReadonly<vwMemberCard>, IvwMemberCardRepository
    {
        public vwMemberCardRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IvwMemberCardRepository : IRepositoryReadonly<vwMemberCard>
    {
    }
}
