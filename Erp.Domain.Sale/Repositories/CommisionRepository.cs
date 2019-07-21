using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class CommisionRepository : GenericRepository<ErpSaleDbContext, Commision>, ICommisionRepository
    {
        public CommisionRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        public IQueryable<Commision> GetAllCommision()
        {
            return Context.Commision.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwCommision_Branch> GetListCommision_Branch(int commisionId)
        {
            return Context.vwCommision_Branch.Where(item => item.CommisionId == commisionId);
        }


        public Commision GetCommisionById(int Id)
        {
            return Context.Commision.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertCommision(Commision Commision)
        {
            Context.Commision.Add(Commision);
            Context.Entry(Commision).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void DeleteCommision(int Id)
        {
            Commision deletedCommision = GetCommisionById(Id);
            Context.Commision.Remove(deletedCommision);
            Context.Entry(deletedCommision).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        

        public void DeleteCommisionRs(int Id)
        {
            Commision deleteCommisionRs = GetCommisionById(Id);
            deleteCommisionRs.IsDeleted = true;
            UpdateCommision(deleteCommisionRs);
        }


        public void UpdateCommision(Commision Commision)
        {
            Context.Entry(Commision).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void sp_Sale_Commision_Branch_Update(int commisionId, int branchId)
        {
            Erp.Domain.Sale.Helper.SqlHelper.ExecuteSP("sp_Sale_Commision_Branch_Update", new { CommisionId = commisionId, BranchId = branchId });
        }


        public IQueryable<CommisionBranch> GetListCommisionBranch(int commisionId)
        {
            return Context.CommisionBranch.Where(item => item.CommisionId == commisionId);
        }

        public void InsertCommisionBranch(CommisionBranch CommisionBranch)
        {
            Context.CommisionBranch.Add(CommisionBranch);
            Context.Entry(CommisionBranch).State = EntityState.Added;
            Context.SaveChanges();
        }

        public CommisionBranch GetCommisionBranchById(int Id)
        {
            return Context.CommisionBranch.SingleOrDefault(item => item.Id == Id );
        }

        public void DeleteCommisionBranch(int Id)
        {
            CommisionBranch deletedCommisionBranch = GetCommisionBranchById(Id);
            Context.CommisionBranch.Remove(deletedCommisionBranch);
            Context.Entry(deletedCommisionBranch).State = EntityState.Deleted;
            Context.SaveChanges();
        }
    }
}
