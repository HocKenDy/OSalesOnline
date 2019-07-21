using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace Erp.Domain.Sale.Repositories
{
    public class ProductInvoiceRepository : GenericRepository<ErpSaleDbContext, ProductInvoice>, IProductInvoiceRepository
    {
        public ProductInvoiceRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all ProductInvoice
        /// </summary>
        /// <returns>ProductInvoice list</returns>
        public IQueryable<ProductInvoice> GetAllProductInvoice()
        {
            return Context.ProductInvoice.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwProductInvoice> GetAllvwProductInvoice()
        {
            return Context.vwProductInvoice.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwProductInvoice> GetAllvwProductInvoiceFull()
        {
            return Context.vwProductInvoice;
        }

        /// <summary>
        /// Get ProductInvoice information by specific id
        /// </summary>
        /// <param name="ProductInvoiceId">Id of ProductInvoice</param>
        /// <returns></returns>
        public ProductInvoice GetProductInvoiceById(int Id)
        {
            return Context.ProductInvoice.SingleOrDefault(item => item.Id == Id);
        }
        public ProductInvoice GetProductInvoiceByCode(string Code)
        {
            return Context.ProductInvoice.SingleOrDefault(item => item.Code == Code);
        }
        public vwProductInvoice GetvwProductInvoiceById(int Id)
        {
            return Context.vwProductInvoice.SingleOrDefault(item => item.Id == Id);
        }

        public vwProductInvoice GetvwProductInvoiceByCode(string code)
        {
            return Context.vwProductInvoice.SingleOrDefault(item => item.Code == code);
        }

        /// <summary>
        /// Insert ProductInvoice into database
        /// </summary>
        /// <param name="ProductInvoice">Object infomation</param>
        public int InsertProductInvoice(ProductInvoice ProductInvoice, List<ProductInvoiceDetail> orderDetails)
        {
            Context.ProductInvoice.Add(ProductInvoice);
            Context.Entry(ProductInvoice).State = EntityState.Added;
            Context.SaveChanges();

            for(int i=0; i< orderDetails.Count; i++)
            {
                orderDetails[i].ProductInvoiceId = ProductInvoice.Id;
                InsertProductInvoiceDetail(orderDetails[i]);
            }

            return ProductInvoice.Id;
        }

        /// <summary>
        /// Delete ProductInvoice with specific id
        /// </summary>
        /// <param name="Id">ProductInvoice Id</param>
        public void DeleteProductInvoice(int Id)
        {
            ProductInvoice deletedProductInvoice = GetProductInvoiceById(Id);
            Context.ProductInvoice.Remove(deletedProductInvoice);
            Context.Entry(deletedProductInvoice).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a ProductInvoice with its Id and Update IsDeleted IF that ProductInvoice has relationship with others
        /// </summary>
        /// <param name="ProductInvoiceId">Id of ProductInvoice</param>
        public void DeleteProductInvoiceRs(int Id)
        {
            ProductInvoice deleteProductInvoiceRs = GetProductInvoiceById(Id);
            deleteProductInvoiceRs.IsDeleted = true;
            UpdateProductInvoice(deleteProductInvoiceRs);
        }

        /// <summary>
        /// Update ProductInvoice into database
        /// </summary>
        /// <param name="ProductInvoice">ProductInvoice object</param>
        public void UpdateProductInvoice(ProductInvoice ProductInvoice)
        {
            Context.Entry(ProductInvoice).State = EntityState.Modified;
            Context.SaveChanges();
        }


        //order detail

        public IQueryable<vwProductInvoiceDetail> GetAllvwInvoiceDetails()
        {
            return Context.vwProductInvoiceDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<ProductInvoiceDetail> GetAllInvoiceDetails()
        {
            return Context.ProductInvoiceDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<ProductInvoiceDetail> GetAllInvoiceDetailsByInvoiceId(int InvoiceId)
        {
            return Context.ProductInvoiceDetail.Where(item => item.ProductInvoiceId == InvoiceId && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwProductInvoiceDetail> GetAllvwInvoiceDetailsByInvoiceId(int InvoiceId)
        {
            return Context.vwProductInvoiceDetail.Where(item => item.ProductInvoiceId == InvoiceId && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwProductInvoiceDetail> GetAllvwInvoiceDetailsByCustomerId(int CustomerId)
        {
            return Context.vwProductInvoiceDetail.Where(item => item.CustomerId == CustomerId && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public ProductInvoiceDetail GetProductInvoiceDetailById(int Id)
        {
            return Context.ProductInvoiceDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertProductInvoiceDetail(ProductInvoiceDetail ProductInvoiceDetail)
        {
            Context.ProductInvoiceDetail.Add(ProductInvoiceDetail);
            Context.Entry(ProductInvoiceDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void DeleteProductInvoiceDetail(int Id)
        {
            ProductInvoiceDetail deletedProductInvoiceDetail = GetProductInvoiceDetailById(Id);
            Context.ProductInvoiceDetail.Remove(deletedProductInvoiceDetail);
            Context.Entry(deletedProductInvoiceDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeleteProductInvoiceDetail(IEnumerable<ProductInvoiceDetail> list)
        {
            for (int i = 0; i < list.Count(); i++ )
            {
                ProductInvoiceDetail deletedProductInvoiceDetail = GetProductInvoiceDetailById(list.ElementAt(i).Id);
                Context.ProductInvoiceDetail.Remove(deletedProductInvoiceDetail);
                Context.Entry(deletedProductInvoiceDetail).State = EntityState.Deleted;
            }
            Context.SaveChanges();
        }

        public void DeleteProductInvoiceDetailRs(int Id)
        {
            ProductInvoiceDetail deleteProductInvoiceDetailRs = GetProductInvoiceDetailById(Id);
            deleteProductInvoiceDetailRs.IsDeleted = true;
            UpdateProductInvoiceDetail(deleteProductInvoiceDetailRs);
        }

        public void UpdateProductInvoiceDetail(ProductInvoiceDetail ProductInvoiceDetail)
        {
            Context.Entry(ProductInvoiceDetail).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public IEnumerable<vwProductInvoice> GetAllvwInvoiceByCustomer(int? customerId)
        {
            return Context.vwProductInvoice.Where(item => customerId!=null && item.CustomerId == customerId);
        }
    }
}
