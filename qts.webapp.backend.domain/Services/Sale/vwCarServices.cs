using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface IvwCarService : IServiceReadonly<vwCar>
    {
        IEnumerable<vwCar> GetByCustomerId(int? customerId);
    IEnumerable<vwCar> GetAll();
    }
    public class vwCarService : ServiceReadonly<vwCar>, IvwCarService
    {
        public vwCarService(IvwCarRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<vwCar> GetByCustomerId(int? customerId)
        {
            return repository.GetMany(n => n.CustomerId == customerId);
        }
        public IEnumerable<vwCar> GetAll()
        {
            return repository.GetAll();
        }
    }
}
