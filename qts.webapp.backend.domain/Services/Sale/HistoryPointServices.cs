using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface IHistoryPointService : IServiceBase<HistoryPoint>
    {
        IEnumerable<HistoryPoint> GetAllHistoryPoint();
    }
    public class HistoryPointService : ServiceBase<HistoryPoint>, IHistoryPointService
    {
        public HistoryPointService(IHistoryPointRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<HistoryPoint> GetAllHistoryPoint()
        {
            return repository.GetMany(x => (x.IsDeleted != null || x.IsDeleted == false));
        }
    }
}
