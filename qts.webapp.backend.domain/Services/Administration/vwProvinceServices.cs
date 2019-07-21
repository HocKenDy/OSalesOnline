using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Administration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Administration
{
    public interface IvwProvinceService : IServiceBase<vwProvince>
    {
        IEnumerable<vwProvince> GetNotDelete();
    }
    public class vwProvinceService : ServiceBase<vwProvince>, IvwProvinceService
    {
        public vwProvinceService(IvwProvinceRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<vwProvince> GetNotDelete()
        {
           return  repository.GetMany(n => n.IsDeleted!=true);
        }
    }
}
