using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Administration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Administration
{
    public interface IWardService : IServiceBase<Ward>
    {
    }
    public class WardService : ServiceBase<Ward>, IWardService
    {
        public WardService(IWardRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

    }
}
