using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class vwRePayPointsDetailRepository : RepositoryReadonly<vwRePayPointsDetail>, IvwRePayPointsDetailRepository
    {
        public vwRePayPointsDetailRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IvwRePayPointsDetailRepository : IRepositoryReadonly<vwRePayPointsDetail>
    {
    }
}
