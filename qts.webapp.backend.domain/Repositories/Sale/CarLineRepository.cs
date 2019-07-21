using qts.webapp.backend.domain.Models.Sale;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class CarLineRepository : RepositoryBase<CarLine>, ICarLineRepository
    {
        public CarLineRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface ICarLineRepository : IRepository<CarLine>
    {
    }
}
