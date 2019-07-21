using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class CarsRepository : RepositoryBase<Cars>, ICarsRepository
    {
        public CarsRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface ICarsRepository : IRepository<Cars>
    {
    }
}
