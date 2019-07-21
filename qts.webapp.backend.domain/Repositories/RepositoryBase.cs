using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;
using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.domain.Repositories
{
    public abstract class RepositoryBase<T> where T : BaseModel
    {
        #region Properties
        private BackEndEntities dataContext;
        private readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected BackEndEntities DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void AddRange(IList<T> entities)
        {
            foreach (var e in entities)
                dbSet.Add(e);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void DeleteRs(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual void DeleteR(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                obj.IsDeleted = true;
                Update(obj);
            }
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.Where(m => m.IsDeleted != true).AsEnumerable();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).Where(m => m.IsDeleted != true).AsEnumerable();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).Where(m => m.IsDeleted != true).FirstOrDefault<T>();
        }

        #endregion

    }
}
