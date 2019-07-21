using qts.webapp.domain.Mappers;
using System.Data.Entity;

namespace qts.webapp.domain
{
    public class BackEndEntities : DbContext
    {
        public BackEndEntities() : base("name=ErpDbContext") { }

        public virtual int Commit()
        {
            return base.SaveChanges(); 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BackEndEntities>(null);
            base.OnModelCreating(modelBuilder);

            //Crm
            //modelBuilder.Configurations.Add(new TreeSpeciesMapper());

            modelBuilder.Configurations.Add(new NoteMapper());

            //admin
            modelBuilder.Configurations.Add(new ProvinceMapper());
            modelBuilder.Configurations.Add(new WardMapper());
            modelBuilder.Configurations.Add(new DistrictMapper());
            modelBuilder.Configurations.Add(new vwDistrictMapper());
            modelBuilder.Configurations.Add(new vwWardMapper());
            modelBuilder.Configurations.Add(new vwProvinceMapper());

            modelBuilder.Configurations.Add(new CarsMapper());

            modelBuilder.Configurations.Add(new CarLineMapper());

            modelBuilder.Configurations.Add(new vwCarMapper());

            modelBuilder.Configurations.Add(new MemberCardTypeMapper());

            modelBuilder.Configurations.Add(new HistoryPointMapper());

            modelBuilder.Configurations.Add(new MemberCardMapper());

            modelBuilder.Configurations.Add(new vwMemberCardMapper());

            modelBuilder.Configurations.Add(new RePayPointsMapper());

            modelBuilder.Configurations.Add(new RePayPointsDetailMapper());

            modelBuilder.Configurations.Add(new vwRePayPointsMapper());

            modelBuilder.Configurations.Add(new vwRePayPointsDetailMapper());

            //<appen_backend_entities>
        }
    }
}
