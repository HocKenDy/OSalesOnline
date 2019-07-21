using qts.webapp.backend.domain.Models.Administration;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class vwWardRepository : RepositoryReadonly<vwWard>, IvwWardRepository
    {
        public vwWardRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IvwWardRepository : IRepositoryReadonly<vwWard>
    {
    }
}
