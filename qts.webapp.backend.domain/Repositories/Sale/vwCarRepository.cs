using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class vwCarRepository : RepositoryReadonly<vwCar>, IvwCarRepository
    {
        public vwCarRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IvwCarRepository : IRepositoryReadonly<vwCar>
    {
    }
}
