using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class RePayPointsDetailRepository : RepositoryBase<RePayPointsDetail>, IRePayPointsDetailRepository
    {
        public RePayPointsDetailRepository(IDbFactory dbFactory) : base(dbFactory) { }
       
    }

    public interface IRePayPointsDetailRepository : IRepository<RePayPointsDetail>
    {
        
    }
}
