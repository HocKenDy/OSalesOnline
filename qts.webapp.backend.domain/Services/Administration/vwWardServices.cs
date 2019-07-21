using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Administration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Administration
{
    public interface IvwWardService : IServiceReadonly<vwWard>
    {
        IEnumerable<vwWard> GetNotDelete();
    }
    public class vwWardService : ServiceReadonly<vwWard>, IvwWardService
    {
        public vwWardService(IvwWardRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<vwWard> GetNotDelete()
        {
            return repository.GetMany(n => n.IsDeleted != true);
        }
    }
}
