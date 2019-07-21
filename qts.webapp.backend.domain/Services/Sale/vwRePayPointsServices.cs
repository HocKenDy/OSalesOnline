using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface IvwRePayPointsService : IServiceReadonly<vwRePayPoints>
    {
    }
    public class vwRePayPointsService : ServiceReadonly<vwRePayPoints>, IvwRePayPointsService
    {
        public vwRePayPointsService(IvwRePayPointsRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

    }
}
