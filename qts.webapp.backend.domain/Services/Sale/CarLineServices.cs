using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface ICarLineService : IServiceBase<CarLine>
    {
        IEnumerable<CarLine> GetByManufacturerCar(string manufacturerCar);
    }
    public class CarLineService : ServiceBase<CarLine>, ICarLineService
    {
        public CarLineService(ICarLineRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<CarLine> GetByManufacturerCar(string manufacturerCar)
        {
            return repository.GetMany(n => n.ManufacturerCar == manufacturerCar);
        }
    }
}
