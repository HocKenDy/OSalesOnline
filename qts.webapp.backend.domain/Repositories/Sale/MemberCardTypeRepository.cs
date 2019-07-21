using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class MemberCardTypeRepository : RepositoryBase<MemberCardType>, IMemberCardTypeRepository
    {
        public MemberCardTypeRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IMemberCardTypeRepository : IRepository<MemberCardType>
    {
    }
}
