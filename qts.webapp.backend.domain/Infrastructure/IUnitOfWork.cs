using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.domain.Infrastructure
{
    public interface IUnitOfWork
    {
        bool Commit();
    }
}
