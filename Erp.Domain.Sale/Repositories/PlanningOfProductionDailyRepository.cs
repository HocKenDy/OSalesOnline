using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class PlanningOfProductionDailyRepository : GenericRepository<ErpSaleDbContext, PlanningOfProductionDaily>, IPlanningOfProductionDailyRepository
    {
        public PlanningOfProductionDailyRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all PlanningOfProductionDaily
        /// </summary>
        /// <returns>PlanningOfProductionDaily list</returns>
        public IQueryable<PlanningOfProductionDaily> GetAllPlanningOfProductionDaily()
        {
            return Context.PlanningOfProductionDaily.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwPlanningOfProductionDaily> GetAllvwPlanningOfProductionDaily()
        {
            return Context.vwPlanningOfProductionDaily.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get PlanningOfProductionDaily information by specific id
        /// </summary>
        /// <param name="PlanningOfProductionDailyId">Id of PlanningOfProductionDaily</param>
        /// <returns></returns>
        public PlanningOfProductionDaily GetPlanningOfProductionDailyById(int Id)
        {
            return Context.PlanningOfProductionDaily.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert PlanningOfProductionDaily into database
        /// </summary>
        /// <param name="PlanningOfProductionDaily">Object infomation</param>
        public void InsertPlanningOfProductionDaily(PlanningOfProductionDaily PlanningOfProductionDaily)
        {
            Context.PlanningOfProductionDaily.Add(PlanningOfProductionDaily);
            Context.Entry(PlanningOfProductionDaily).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete PlanningOfProductionDaily with specific id
        /// </summary>
        /// <param name="Id">PlanningOfProductionDaily Id</param>
        public void DeletePlanningOfProductionDaily(int Id)
        {
            PlanningOfProductionDaily deletedPlanningOfProductionDaily = GetPlanningOfProductionDailyById(Id);
            Context.PlanningOfProductionDaily.Remove(deletedPlanningOfProductionDaily);
            Context.Entry(deletedPlanningOfProductionDaily).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a PlanningOfProductionDaily with its Id and Update IsDeleted IF that PlanningOfProductionDaily has relationship with others
        /// </summary>
        /// <param name="PlanningOfProductionDailyId">Id of PlanningOfProductionDaily</param>
        public void DeletePlanningOfProductionDailyRs(int Id)
        {
            PlanningOfProductionDaily deletePlanningOfProductionDailyRs = GetPlanningOfProductionDailyById(Id);
            deletePlanningOfProductionDailyRs.IsDeleted = true;
            UpdatePlanningOfProductionDaily(deletePlanningOfProductionDailyRs);
        }

        /// <summary>
        /// Update PlanningOfProductionDaily into database
        /// </summary>
        /// <param name="PlanningOfProductionDaily">PlanningOfProductionDaily object</param>
        public void UpdatePlanningOfProductionDaily(PlanningOfProductionDaily PlanningOfProductionDaily)
        {
            Context.Entry(PlanningOfProductionDaily).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void DeletelanningOfProductionDailyByDetailId(int DetailId)
        {
            Helper.SqlHelper.ExecuteSQL("delete from Sale_PlanningOfProductionDaily where PlanningOfProductionDetailId = @PlanningOfProductionDetailId", new { PlanningOfProductionDetailId = DetailId });
        }
    }
}
