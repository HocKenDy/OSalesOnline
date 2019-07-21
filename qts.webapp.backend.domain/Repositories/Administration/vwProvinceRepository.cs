using qts.webapp.backend.domain.Models.Administration;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class vwProvinceRepository : RepositoryBase<vwProvince>, IvwProvinceRepository
    {
        public vwProvinceRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IvwProvinceRepository : IRepository<vwProvince>
    {
    }
}
