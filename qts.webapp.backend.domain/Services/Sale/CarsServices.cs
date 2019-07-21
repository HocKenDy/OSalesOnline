using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface ICarsService : IServiceBase<Cars>
    {
        IEnumerable<Cars> GetByCustomerId(int? customerId);
    }
    public class CarsService : ServiceBase<Cars>, ICarsService
    {
        public CarsService(ICarsRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<Cars> GetByCustomerId(int? customerId)
        {
            return repository.GetMany(n => n.CustomerId == customerId && n.IsDeleted!=true);
        }
    }
}
