using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class CommisionSaleRepository : GenericRepository<ErpSaleDbContext, CommisionSale>, ICommisionSaleRepository
    {
        public CommisionSaleRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all CommisionSale
        /// </summary>
        /// <returns>CommisionSale list</returns>
        public IQueryable<CommisionSale> GetAllCommisionSale()
        {
            return Context.CommisionSale.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get CommisionSale information by specific id
        /// </summary>
        /// <param name="CommisionSaleId">Id of CommisionSale</param>
        /// <returns></returns>
        public CommisionSale GetCommisionSaleById(int Id)
        {
            return Context.CommisionSale.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert CommisionSale into database
        /// </summary>
        /// <param name="CommisionSale">Object infomation</param>
        public void InsertCommisionSale(CommisionSale CommisionSale)
        {
            Context.CommisionSale.Add(CommisionSale);
            Context.Entry(CommisionSale).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete CommisionSale with specific id
        /// </summary>
        /// <param name="Id">CommisionSale Id</param>
        public void DeleteCommisionSale(int Id)
        {
            CommisionSale deletedCommisionSale = GetCommisionSaleById(Id);
            Context.CommisionSale.Remove(deletedCommisionSale);
            Context.Entry(deletedCommisionSale).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a CommisionSale with its Id and Update IsDeleted IF that CommisionSale has relationship with others
        /// </summary>
        /// <param name="CommisionSaleId">Id of CommisionSale</param>
        public void DeleteCommisionSaleRs(int Id)
        {
            CommisionSale deleteCommisionSaleRs = GetCommisionSaleById(Id);
            deleteCommisionSaleRs.IsDeleted = true;
            UpdateCommisionSale(deleteCommisionSaleRs);
        }

        /// <summary>
        /// Update CommisionSale into database
        /// </summary>
        /// <param name="CommisionSale">CommisionSale object</param>
        public void UpdateCommisionSale(CommisionSale CommisionSale)
        {
            Context.Entry(CommisionSale).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
