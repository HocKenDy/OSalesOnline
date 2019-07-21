using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.domain.Services
{
    public interface IServiceBase<T> where T : class
    {
        IEnumerable<T> Get();
        T Get(int id);
        bool Create(T entity);
        bool CreateRange(IList<T> entities);
        bool Update(T entity);

        bool Delete(T entity);
        bool DeleteRs(T entity);
    }
    public class ServiceBase<T> : IServiceBase<T> where T : class
    {
        protected readonly IRepository<T> repository;
        private readonly IUnitOfWork unitOfWork;

        public ServiceBase(IRepository<T> _repository, IUnitOfWork _unitOfWork){
            this.repository = _repository;
            this.unitOfWork = _unitOfWork;
        }

        public bool Create(T entity)
        {
            repository.Add(entity);
            return Save();
        }

        public bool Save()
        {
            return unitOfWork.Commit();
        }

        public bool Delete(T entity)
        {
            repository.Delete(entity);
            return Save();
        }

        public bool DeleteRs(T entity)
        {
            repository.DeleteRs(entity);
            return Save();
        }

        public IEnumerable<T> Get()
        {
            return repository.GetAll();
        }

        public T Get(int id)
        {
            return repository.GetById(id);
        }

        public bool Update(T entity)
        {
            repository.Update(entity);
            return Save();
        }

        public bool CreateRange(IList<T> entities)
        {
            repository.AddRange(entities);
            return Save();
        }
    }
}
