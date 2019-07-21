using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class vwRePayPointsRepository : RepositoryReadonly<vwRePayPoints>, IvwRePayPointsRepository
    {
        public vwRePayPointsRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IvwRePayPointsRepository : IRepositoryReadonly<vwRePayPoints>
    {
    }
}
