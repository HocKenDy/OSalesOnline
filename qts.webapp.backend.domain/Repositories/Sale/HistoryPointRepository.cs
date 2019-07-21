using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class HistoryPointRepository : RepositoryBase<HistoryPoint>, IHistoryPointRepository
    {
        public HistoryPointRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IHistoryPointRepository : IRepository<HistoryPoint>
    {
    }
}
