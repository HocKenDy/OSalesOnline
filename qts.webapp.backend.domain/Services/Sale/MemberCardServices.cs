using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Sale;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Sale
{
    public interface IMemberCardService : IServiceBase<MemberCard>
    {
        MemberCard GetMemberCardByCode(string code);
        MemberCard GetMemberCardById(int Id);
        bool GetAnyCode(string cardcode, string codeModel);
    }
    public class MemberCardService : ServiceBase<MemberCard>, IMemberCardService
    {
        public MemberCardService(IMemberCardRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

        public bool GetAnyCode(string cardcode, string codeModel)
        {
          return repository.GetMany(x => x.Code != cardcode).Any(x => x.Code == codeModel);
        }

        public MemberCard GetMemberCardByCode(string code)
        {
            var model = repository.GetMany(n => n.Code == code);
            if (model != null && model.Count() > 0)
                return model.FirstOrDefault();
            return null;
        }

        public MemberCard GetMemberCardById(int Id)
        {
            return repository.GetById(Id);
        }
    }
}
