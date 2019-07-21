using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface ICommisionSaleRepository
    {
        /// <summary>
        /// Get all CommisionSale
        /// </summary>
        /// <returns>CommisionSale list</returns>
        IQueryable<CommisionSale> GetAllCommisionSale();

        /// <summary>
        /// Get CommisionSale information by specific id
        /// </summary>
        /// <param name="Id">Id of CommisionSale</param>
        /// <returns></returns>
        CommisionSale GetCommisionSaleById(int Id);

        /// <summary>
        /// Insert CommisionSale into database
        /// </summary>
        /// <param name="CommisionSale">Object infomation</param>
        void InsertCommisionSale(CommisionSale CommisionSale);

        /// <summary>
        /// Delete CommisionSale with specific id
        /// </summary>
        /// <param name="Id">CommisionSale Id</param>
        void DeleteCommisionSale(int Id);

        /// <summary>
        /// Delete a CommisionSale with its Id and Update IsDeleted IF that CommisionSale has relationship with others
        /// </summary>
        /// <param name="Id">Id of CommisionSale</param>
        void DeleteCommisionSaleRs(int Id);

        /// <summary>
        /// Update CommisionSale into database
        /// </summary>
        /// <param name="CommisionSale">CommisionSale object</param>
        void UpdateCommisionSale(CommisionSale CommisionSale);
    }
}
