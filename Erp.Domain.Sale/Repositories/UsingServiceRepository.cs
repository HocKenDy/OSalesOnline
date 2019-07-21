using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class UsingServiceRepository : GenericRepository<ErpSaleDbContext, UsingService>, IUsingServiceRepository
    {
        public UsingServiceRepository(ErpSaleDbContext context)
            : base(context)
        {

        }


        public IQueryable<UsingService> GetAllUsingService()
        {
            return Context.UsingService.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwUsingService> GetAllvwUsingService()
        {
            return Context.vwUsingService.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public UsingService GetUsingServiceById(int Id)
        {
            return Context.UsingService.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public vwUsingService GetvwUsingServiceById(int Id)
        {
            return Context.vwUsingService.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertUsingService(UsingService UsingService)
        {
            Context.UsingService.Add(UsingService);
            Context.Entry(UsingService).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void InsertUsingService(List<UsingService> ListUsingService)
        {
            for (int i = 0; i < ListUsingService.Count; i++)
            {
                Context.UsingService.Add(ListUsingService[i]);
                Context.Entry(ListUsingService[i]).State = EntityState.Added;
            }
            Context.SaveChanges();
        }

   
        public void DeleteUsingService(int Id)
        {
            UsingService deletedUsingService = GetUsingServiceById(Id);
            Context.UsingService.Remove(deletedUsingService);
            Context.Entry(deletedUsingService).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
   
        public void DeleteUsingServiceRs(int Id)
        {
            UsingService deleteUsingServiceRs = GetUsingServiceById(Id);
            deleteUsingServiceRs.IsDeleted = true;
            UpdateUsingService(deleteUsingServiceRs);
        }


        public void UpdateUsingService(UsingService UsingService)
        {
            Context.Entry(UsingService).State = EntityState.Modified;
            Context.SaveChanges();
        }

        //UsingServiceDetail

        public IQueryable<UsingServiceDetail> GetAllUsingServiceDetail()
        {
            return Context.UsingServiceDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwUsingServiceDetail> GetAllvwUsingServiceDetail()
        {
            return Context.vwUsingServiceDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }



        public UsingServiceDetail GetUsingServiceDetailById(int Id)
        {
            return Context.UsingServiceDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public vwUsingServiceDetail GetvwUsingServiceDetailById(int Id)
        {
            return Context.vwUsingServiceDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }



        public void InsertUsingServiceDetail(UsingServiceDetail UsingServiceDetail)
        {
            Context.UsingServiceDetail.Add(UsingServiceDetail);
            Context.Entry(UsingServiceDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void InsertUsingServiceDetail(List<UsingServiceDetail> ListUsingServiceDetail)
        {
            for (int i = 0; i < ListUsingServiceDetail.Count; i++)
            {
                Context.UsingServiceDetail.Add(ListUsingServiceDetail[i]);
                Context.Entry(ListUsingServiceDetail[i]).State = EntityState.Added;
            }
            Context.SaveChanges();
        }
    }
}
