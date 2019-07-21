using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class RePayPointsRepository : RepositoryBase<RePayPoints>, IRePayPointsRepository
    {
        public RePayPointsRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IRePayPointsRepository :  IRepository<RePayPoints>
    {

    }
}
