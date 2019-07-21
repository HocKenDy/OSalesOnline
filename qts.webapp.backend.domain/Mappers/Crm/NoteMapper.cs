using qts.webapp.backend.domain.Models.Crm;
using System.Data.Entity.ModelConfiguration;

namespace qts.webapp.domain.Mappers
{
    public class NoteMapper : EntityTypeConfiguration<Note>
    {
        public NoteMapper()
        {
            ToTable("Crm_Note");
        }
    }
}

