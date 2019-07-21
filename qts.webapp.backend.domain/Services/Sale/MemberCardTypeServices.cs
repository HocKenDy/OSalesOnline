using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface IMemberCardTypeService : IServiceBase<MemberCardType>
    {
        MemberCardType GetByPoint(double? accumulatedPoint);
        MemberCardType MemberCardTypeGetById(int Id);
    }
    public class MemberCardTypeService : ServiceBase<MemberCardType>, IMemberCardTypeService
    {
        public MemberCardTypeService(IMemberCardTypeRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public MemberCardType GetByPoint(double? accumulatedPoint)
        {
            var model = repository.GetMany(n => n.TargetPoint <= accumulatedPoint).OrderByDescending(n => n.TargetPoint);
            if (model != null && model.Count() > 0)
            {
                return model.FirstOrDefault();
            }
            return null;
        }

        public MemberCardType MemberCardTypeGetById(int Id)
        {
            return repository.GetById(Id);
        }
    }
}
