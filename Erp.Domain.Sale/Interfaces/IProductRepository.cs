using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Get all Product
        /// </summary>
        /// <returns>Product list</returns>
        IQueryable<Product> GetAllProduct();
        IQueryable<vwProduct> GetAllvwProduct();
        IQueryable<Product> GetAllProductByType(string type);
        IQueryable<vwProduct> GetAllvwProductByType(string type);
        /// <summary>
        /// Get Product information by specific id
        /// </summary>
        /// <param name="Id">Id of Product</param>
        /// <returns></returns>
        Product GetProductById(int Id);
        vwProduct GetvwProductById(int Id);

        /// <summary>
        /// Insert Product into database
        /// </summary>
        /// <param name="Product">Object infomation</param>
        void InsertProduct(Product Product);

        /// <summary>
        /// Delete Product with specific id
        /// </summary>
        /// <param name="Id">Product Id</param>
        void DeleteProduct(int Id);

        /// <summary>
        /// Delete a Product with its Id and Update IsDeleted IF that Product has relationship with others
        /// </summary>
        /// <param name="Id">Id of Product</param>
        void DeleteProductRs(int Id);

        /// <summary>
        /// Update Product into database
        /// </summary>
        /// <param name="Product">Product object</param>
        void UpdateProduct(Product Product);


        //****************************
        /// <summary>
        /// Get all PriceLog
        /// </summary>
        /// <returns>Product list</returns>
        IQueryable<PriceLog> GetAllPriceLog();
        /// <summary>
        /// Get PriceLog information by specific id
        /// </summary>
        /// <param name="Id">Id of PriceLog</param>
        /// <returns></returns>
        PriceLog GetPriceLogById(int Id);
        /// <summary>
        /// Insert PriceLog into database
        /// </summary>
        /// <param name="Product">Object infomation</param>
        void InsertPriceLog(PriceLog PriceLog);

        /// <summary>
        /// Delete PriceLog with specific id
        /// </summary>
        /// <param name="Id">PriceLog Id</param>
        void DeletePriceLog(int Id);

        /// <summary>
        /// Delete a PriceLog with its Id and Update IsDeleted IF that PriceLog has relationship with others
        /// </summary>
        /// <param name="Id">Id of PriceLog</param>
        void DeletePriceLogRs(int Id);

        /// <summary>
        /// Update PriceLog into database
        /// </summary>
        /// <param name="PriceLog">PriceLog object</param>
        void UpdatePriceLog(PriceLog PriceLog);
    }
}
