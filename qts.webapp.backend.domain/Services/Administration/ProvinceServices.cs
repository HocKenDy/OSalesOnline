using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Administration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Administration
{
    public interface IProvinceService : IServiceBase<Province>
    {
        IEnumerable<Province> GetNotDelete();
    }
    public class ProvinceService : ServiceBase<Province>, IProvinceService
    {
        public ProvinceService(IProvinceRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<Province> GetNotDelete()
        {
           return  repository.GetMany(n => n.IsDeleted!=true);
        }
    }
}
