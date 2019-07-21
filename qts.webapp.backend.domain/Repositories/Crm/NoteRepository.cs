using qts.webapp.backend.domain.Models.Crm;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Interfaces;

namespace qts.webapp.domain.Repositories
{
    public class NoteRepository : RepositoryBase<Note>, INoteRepository
    {
        public NoteRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }

    public interface INoteRepository : IRepository<Note>
    {
    }
}
