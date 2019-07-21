using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.domain.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        BackEndEntities dbContext;

        public BackEndEntities Init()
        {
            return dbContext ?? (dbContext = new BackEndEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
