using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using qts.webapp.domain.Services;
using qts.webapp.backend.domain.Models.Crm;
using System.Collections.Generic;
using System;
using System.Linq;

namespace qts.webapp.backend.domain.Services.Crm
{
    public interface INoteService : IServiceBase<Note>
    {
    }
    public class NoteService : ServiceBase<Note>, INoteService
    {
        public NoteService(INoteRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { }

    }
}
