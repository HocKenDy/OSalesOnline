using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.domain.Services
{
    public interface IServiceReadonly<T> where T : class
    {
        IEnumerable<T> Get();
        T Get(int id);
    }
    public class ServiceReadonly<T> : IServiceReadonly<T> where T : class
    {
        protected readonly IRepositoryReadonly<T> repository;
        protected readonly IUnitOfWork unitOfWork;

        public ServiceReadonly(IRepositoryReadonly<T> _repository, IUnitOfWork _unitOfWork){
            this.repository = _repository;
            this.unitOfWork = _unitOfWork;
        }

        public IEnumerable<T> Get()
        {
            return repository.GetAll();
        }

        public T Get(int id)
        {
            return repository.GetById(id);
        }
    }
}
