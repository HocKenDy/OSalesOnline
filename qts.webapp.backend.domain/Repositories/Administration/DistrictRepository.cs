using qts.webapp.backend.domain.Models.Administration;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class DistrictRepository : RepositoryBase<District>, IDistrictRepository
    {
        public DistrictRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IDistrictRepository : IRepository<District>
    {
    }
}
