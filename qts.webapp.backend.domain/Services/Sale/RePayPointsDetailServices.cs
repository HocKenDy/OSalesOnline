using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface IRePayPointsDetailService : IServiceBase<RePayPointsDetail>
    {
        IEnumerable<RePayPointsDetail> GetRePayPointsDetailByPayPointId(int PayPointId);
    }

    public class RePayPointsDetailService : ServiceBase<RePayPointsDetail>, IRePayPointsDetailService
    {
        public RePayPointsDetailService(IRePayPointsDetailRepository repository, IUnitOfWork unitOfWork) :
                                    base(repository, unitOfWork)
        {

        }

        public IEnumerable<RePayPointsDetail> GetRePayPointsDetailByPayPointId(int PayPointId)
        {
            return repository.GetMany(x => x.RePayPointId == PayPointId);
        }
    }
}
