using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Administration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Administration
{
    public interface IDistrictService : IServiceBase<District>
    {
    }
    public class DistrictService : ServiceBase<District>, IDistrictService
    {
        public DistrictService(IDistrictRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

    }
}
