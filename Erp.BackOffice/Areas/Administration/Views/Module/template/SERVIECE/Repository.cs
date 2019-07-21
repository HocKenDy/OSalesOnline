using qts.webapp.backend.domain.Models.<AREA_NAME>;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class <MODULE_NAME>Repository : RepositoryBase<<MODULE_NAME>>, I<MODULE_NAME>Repository
    {
        public <MODULE_NAME>Repository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface I<MODULE_NAME>Repository : IRepository<<MODULE_NAME>>
    {
    }
}
