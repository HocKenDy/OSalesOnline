using qts.webapp.backend.domain.Models.Administration;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class WardRepository : RepositoryBase<Ward>, IWardRepository
    {
        public WardRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IWardRepository : IRepository<Ward>
    {
    }
}
