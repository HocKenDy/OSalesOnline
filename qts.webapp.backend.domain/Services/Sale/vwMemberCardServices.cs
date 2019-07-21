using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface IvwMemberCardService : IServiceReadonly<vwMemberCard>
    {
        IEnumerable<vwMemberCard> GetListMemberCardByCustomerId(int customerId, int id);
        IEnumerable<vwMemberCard> GetListMemberCardByCustomerId(int customerId);
    }
    public class vwMemberCardService : ServiceReadonly<vwMemberCard>, IvwMemberCardService
    {
        public vwMemberCardService(IvwMemberCardRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public IEnumerable<vwMemberCard> GetListMemberCardByCustomerId(int customerId)
        {
            return repository.GetMany(n => n.CustomerId == customerId);
        }

        public IEnumerable<vwMemberCard> GetListMemberCardByCustomerId(int customerId, int id)
        {
           return repository.GetMany(n => n.CustomerId == customerId && n.Id == id);
        }
    }
}
