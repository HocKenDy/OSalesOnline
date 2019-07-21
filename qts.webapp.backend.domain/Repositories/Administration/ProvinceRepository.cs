using qts.webapp.backend.domain.Models.Administration;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class ProvinceRepository : RepositoryBase<Province>, IProvinceRepository
    {
        public ProvinceRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface IProvinceRepository : IRepository<Province>
    {
    }
}
