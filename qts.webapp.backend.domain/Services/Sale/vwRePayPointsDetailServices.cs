using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface IvwRePayPointsDetailService : IServiceReadonly<vwRePayPointsDetail>
    {
        IEnumerable<vwRePayPointsDetail> GetAllvwRePayPointsDetailByRePayPointId(int RePayPointId);
    }
    public class vwRePayPointsDetailService : ServiceReadonly<vwRePayPointsDetail>, IvwRePayPointsDetailService
    {
        public vwRePayPointsDetailService(IvwRePayPointsDetailRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<vwRePayPointsDetail> GetAllvwRePayPointsDetailByRePayPointId(int RePayPointId)
        {
            return repository.GetMany(x => x.RePayPointId == RePayPointId);
        }
    }
}
