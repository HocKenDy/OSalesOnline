using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace Erp.Domain.Sale.Repositories
{
    public class ProductRepository : GenericRepository<ErpSaleDbContext, Product>, IProductRepository
    {
        public ProductRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all Product
        /// </summary>
        /// <returns>Product list</returns>
        /// 
        public IQueryable<Product> GetAllProduct()
        {
            return Context.Product.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwProduct> GetAllvwProduct()
        {
            return Context.vwProduct.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<Product> GetAllProductByType(string type)
        {
            return Context.Product.Where(item => item.Type == type && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwProduct> GetAllvwProductByType(string type)
        {
            return Context.vwProduct.Where(item => item.Type == type && (item.IsDeleted == null || item.IsDeleted == false));
        }


        /// <summary>
        /// Get Product information by specific id
        /// </summary>
        /// <param name="ProductId">Id of Product</param>
        /// <returns></returns>
        
        public Product GetProductById(int Id)
        {
            return Context.Product.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public vwProduct GetvwProductById(int Id)
        {
            return Context.vwProduct.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert Product into database
        /// </summary>
        /// <param name="Product">Object infomation</param>
        public void InsertProduct(Product Product)
        {
            Context.Product.Add(Product);
            Context.Entry(Product).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete Product with specific id
        /// </summary>
        /// <param name="Id">Product Id</param>
        public void DeleteProduct(int Id)
        {
            Product deletedProduct = GetProductById(Id);
            Context.Product.Remove(deletedProduct);
            Context.Entry(deletedProduct).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a Product with its Id and Update IsDeleted IF that Product has relationship with others
        /// </summary>
        /// <param name="ProductId">Id of Product</param>
        public void DeleteProductRs(int Id)
        {
            Product deleteProductRs = GetProductById(Id);
            deleteProductRs.IsDeleted = true;
            UpdateProduct(deleteProductRs);
        }

        /// <summary>
        /// Update Product into database
        /// </summary>
        /// <param name="Product">Product object</param>
        public void UpdateProduct(Product Product)
        {
            Context.Entry(Product).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public IQueryable<PriceLog> GetAllPriceLog()
        {
            return Context.PriceLog.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public PriceLog GetPriceLogById(int Id)
        {
            return Context.PriceLog.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertPriceLog(PriceLog PriceLog)
        {
            Context.PriceLog.Add(PriceLog);
            Context.Entry(PriceLog).State = EntityState.Added;
            Context.SaveChanges(); ;
        }

        public void DeletePriceLog(int Id)
        {
            PriceLog deletedPriceLog = GetPriceLogById(Id);
            Context.PriceLog.Remove(deletedPriceLog);
            Context.Entry(deletedPriceLog).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeletePriceLogRs(int Id)
        {
            PriceLog deletePriceLogRs = GetPriceLogById(Id);
            deletePriceLogRs.IsDeleted = true;
            UpdatePriceLog(deletePriceLogRs);
        }

        public void UpdatePriceLog(PriceLog PriceLog)
        {
            Context.Entry(PriceLog).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
