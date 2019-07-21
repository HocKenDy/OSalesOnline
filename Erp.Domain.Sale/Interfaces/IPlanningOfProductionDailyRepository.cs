using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IPlanningOfProductionDailyRepository
    {
        /// <summary>
        /// Get all PlanningOfProductionDaily
        /// </summary>
        /// <returns>PlanningOfProductionDaily list</returns>
        IQueryable<PlanningOfProductionDaily> GetAllPlanningOfProductionDaily();
        IQueryable<vwPlanningOfProductionDaily> GetAllvwPlanningOfProductionDaily();

        /// <summary>
        /// Get PlanningOfProductionDaily information by specific id
        /// </summary>
        /// <param name="Id">Id of PlanningOfProductionDaily</param>
        /// <returns></returns>
        PlanningOfProductionDaily GetPlanningOfProductionDailyById(int Id);

        /// <summary>
        /// Insert PlanningOfProductionDaily into database
        /// </summary>
        /// <param name="PlanningOfProductionDaily">Object infomation</param>
        void InsertPlanningOfProductionDaily(PlanningOfProductionDaily PlanningOfProductionDaily);

        /// <summary>
        /// Delete PlanningOfProductionDaily with specific id
        /// </summary>
        /// <param name="Id">PlanningOfProductionDaily Id</param>
        void DeletePlanningOfProductionDaily(int Id);

        /// <summary>
        /// Delete a PlanningOfProductionDaily with its Id and Update IsDeleted IF that PlanningOfProductionDaily has relationship with others
        /// </summary>
        /// <param name="Id">Id of PlanningOfProductionDaily</param>
        void DeletePlanningOfProductionDailyRs(int Id);

        /// <summary>
        /// Update PlanningOfProductionDaily into database
        /// </summary>
        /// <param name="PlanningOfProductionDaily">PlanningOfProductionDaily object</param>
        void UpdatePlanningOfProductionDaily(PlanningOfProductionDaily PlanningOfProductionDaily);
        void DeletelanningOfProductionDailyByDetailId(int DetailId);
    }
}
