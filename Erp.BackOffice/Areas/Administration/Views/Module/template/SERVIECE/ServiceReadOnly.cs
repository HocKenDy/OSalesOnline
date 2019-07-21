using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.<AREA_NAME>;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.<AREA_NAME>
{
    public interface I<MODULE_NAME>Service : IServiceReadonly<<MODULE_NAME>>
    {
    }
    public class <MODULE_NAME>Service : ServiceReadonly<<MODULE_NAME>>, I<MODULE_NAME>Service
    {
        public <MODULE_NAME>Service(I<MODULE_NAME>Repository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

    }
}
