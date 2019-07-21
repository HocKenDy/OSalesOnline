using Erp.Domain.Sale;
using Erp.Domain.Sale.Repositories;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class BranchDepartmentRepository : GenericRepository<ErpSaleDbContext, BranchDepartment>, IBranchDepartmentRepository
    {
        public BranchDepartmentRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all BranchDepartment
        /// </summary>
        /// <returns>BranchDepartment list</returns>
        public IQueryable<BranchDepartment> GetAllBranchDepartment()
        {
            return Context.BranchDepartment.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwBranchDepartment> GetAllvwBranchDepartment()
        {
            return Context.vwBranchDepartment.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get BranchDepartment information by specific id
        /// </summary>
        /// <param name="BranchDepartmentId">Id of BranchDepartment</param>
        /// <returns></returns>
        public BranchDepartment GetBranchDepartmentById(int Id)
        {
            return Context.BranchDepartment.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwBranchDepartment GetvwBranchDepartmentById(int Id)
        {
            return Context.vwBranchDepartment.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert BranchDepartment into database
        /// </summary>
        /// <param name="BranchDepartment">Object infomation</param>
        public void InsertBranchDepartment(BranchDepartment BranchDepartment)
        {
            Context.BranchDepartment.Add(BranchDepartment);
            Context.Entry(BranchDepartment).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete BranchDepartment with specific id
        /// </summary>
        /// <param name="Id">BranchDepartment Id</param>
        public void DeleteBranchDepartment(int Id)
        {
            BranchDepartment deletedBranchDepartment = GetBranchDepartmentById(Id);
            Context.BranchDepartment.Remove(deletedBranchDepartment);
            Context.Entry(deletedBranchDepartment).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a BranchDepartment with its Id and Update IsDeleted IF that BranchDepartment has relationship with others
        /// </summary>
        /// <param name="BranchDepartmentId">Id of BranchDepartment</param>
        public void DeleteBranchDepartmentRs(int Id)
        {
            BranchDepartment deleteBranchDepartmentRs = GetBranchDepartmentById(Id);
            deleteBranchDepartmentRs.IsDeleted = true;
            UpdateBranchDepartment(deleteBranchDepartmentRs);
        }

        /// <summary>
        /// Update BranchDepartment into database
        /// </summary>
        /// <param name="BranchDepartment">BranchDepartment object</param>
        public void UpdateBranchDepartment(BranchDepartment BranchDepartment)
        {
            Context.Entry(BranchDepartment).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
