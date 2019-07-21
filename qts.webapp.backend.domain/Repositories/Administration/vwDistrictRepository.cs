using qts.webapp.backend.domain.Models.Administration;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class vwDistrictRepository : RepositoryReadonly<vwDistrict>, IvwDistrictRepository
    {
        public vwDistrictRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IvwDistrictRepository : IRepositoryReadonly<vwDistrict>
    {
    }
}
