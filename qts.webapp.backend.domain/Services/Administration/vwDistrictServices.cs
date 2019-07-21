using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Administration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Administration
{
    public interface IvwDistrictService : IServiceReadonly<vwDistrict>
    {
        IEnumerable<vwDistrict> GetNotDelete();
    }
    public class vwDistrictService : ServiceReadonly<vwDistrict>, IvwDistrictService
    {
        public vwDistrictService(IvwDistrictRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<vwDistrict> GetNotDelete()
        {
            return repository.GetMany(n => n.IsDeleted != true);
        }
    }
}
