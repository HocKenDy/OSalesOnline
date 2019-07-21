using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using Erp.Domain.Account.Interfaces;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class UsingServiceController : Controller
    {
        private readonly IUsingServiceRepository UsingServiceRepository;
        private readonly ICustomerRepository CustomerRepository;
        private readonly IProductInvoiceRepository ProductInvoiceRepository;
        private readonly IUserRepository userRepository;

        public UsingServiceController(
            IUsingServiceRepository _UsingService
            , ICustomerRepository _Customer
            , IProductInvoiceRepository _ProductInvoice
            , IUserRepository _user
            )
        {
            UsingServiceRepository = _UsingService;
            CustomerRepository = _Customer;
            ProductInvoiceRepository = _ProductInvoice;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {
            IQueryable<UsingServiceDetailViewModel> q = UsingServiceRepository.GetAllvwUsingServiceDetail()
                .Select(item => new UsingServiceDetailViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    CustomerCode = item.CustomerCode,
                    InvoiceCode = item.InvoiceCode,
                    ProductName = item.ProductName,
                    GroupCode = item.GroupCode
                }).OrderByDescending(m => m.ModifiedDate).ThenBy(x => x.CustomerCode);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create(string txtCheckCode)
        {
            List<UsingServiceViewModel> listInvoiceDetail = new List<UsingServiceViewModel>();
            int? customerId = -111;
            int? invoiceId = -111;

            txtCheckCode = txtCheckCode == null ? "" : txtCheckCode;

            string prefixCodeCustomer = Helpers.Common.GetSetting("prefixOrderNo_Customer");
            if (txtCheckCode.Contains(prefixCodeCustomer))
            {
                ViewBag.isCustomer = false;
                var customer = CustomerRepository.GetvwCustomerByCode(txtCheckCode);
                if (customer != null)
                {
                    customerId = customer.Id;
                    invoiceId = -222;
                    ViewBag.isCustomer = true;
                }
            }
            else
            {
                ViewBag.isInvoice = false;
                var invoice = ProductInvoiceRepository.GetvwProductInvoiceByCode(txtCheckCode);
                if(invoice != null)
                {
                    customerId = invoice.CustomerId;
                    invoiceId = invoice.Id;
                    ViewBag.isInvoice = true;
                }
            }

            int? createUserId = Helpers.Common.CurrentUser.Id;

            listInvoiceDetail = UsingServiceRepository.GetAllvwUsingService().Where(x => 
                    x.CustomerId == customerId 
                    && (x.InvoiceId == invoiceId || invoiceId == -222 ) 
                    && x.QuantityRemaining > 0
                    && (x.CreatedUserId == createUserId || x.InvoiceSalerId == createUserId)
                )
                .Select(item => new UsingServiceViewModel
                {
                    CustomerId = item.CustomerId,
                    InvoiceId = item.InvoiceId,
                    InvoiceCode = item.InvoiceCode,
                    ProductId = item.ProductId,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    QuantityRemaining = item.QuantityRemaining,
                    PackageProductId = item.PackageProductId,
                    PackageProductCode = item.PackageProductCode,
                    PackageProductName = item.PackageProductName,
                    Id = item.Id
                }).ToList();

            ViewBag.customerId = customerId;
            return View(listInvoiceDetail);
        }

        [HttpPost]
        public ActionResult Create(FormCollection fc)
        {
            if (string.IsNullOrEmpty(Request["using-service-item"]) == false)
            {
                string groupCode = Request["CustomerId"] + "-" + DateTime.Now.ToString("dd-MM-yyyy-H-mm");

                List<UsingServiceDetail> listDetail = new List<UsingServiceDetail>();
                foreach (var itemId in Request["using-service-item"].Split(','))
                {
                    int id = 0;
                    if(int.TryParse(itemId, out id))
                    {
                        var usingService = UsingServiceRepository.GetUsingServiceById(id);

                        int TransactionSalerId = 0;
                        int.TryParse(Request["TransactionSalerId"], out TransactionSalerId);

                        listDetail.Add( new UsingServiceDetail { 
                            CreatedDate = DateTime.Now,
                            CreatedUserId = Helpers.Common.CurrentUser.Id,
                            IsDeleted = false,
                            ProductId = usingService.ProductId,
                            TransactionSalerId = TransactionSalerId,
                            UsingServiceId = usingService.Id,
                            GroupCode = groupCode
                        });

                        usingService.QuantityRemaining -= 1;
                        usingService.ModifiedDate = DateTime.Now;
                        usingService.ModifiedUserId = Helpers.Common.CurrentUser.Id;
                        UsingServiceRepository.UpdateUsingService(usingService);
                    }
                }

                if (listDetail.Count != 0)
                {
                    UsingServiceRepository.InsertUsingServiceDetail(listDetail);
                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                    return RedirectToAction("Detail", new { groupCode = groupCode });

                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.NoRowInsert;
                return RedirectToAction("Index");
            }

            TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.InsertFailed;
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var UsingService = UsingServiceRepository.GetUsingServiceById(Id.Value);
            if (UsingService != null && UsingService.IsDeleted != true)
            {
                var model = new UsingServiceViewModel();
                AutoMapper.Mapper.Map(UsingService, model);
                
                //if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                //{
                //    TempData["FailedMessage"] = "NotOwner";
                //    return RedirectToAction("Index");
                //}                

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(UsingServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var UsingService = UsingServiceRepository.GetUsingServiceById(model.Id);
                    AutoMapper.Mapper.Map(model, UsingService);
                    UsingService.ModifiedUserId = WebSecurity.CurrentUserId;
                    UsingService.ModifiedDate = DateTime.Now;
                    UsingServiceRepository.UpdateUsingService(UsingService);

                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    return RedirectToAction("Index");
                }

                return View(model);
            }

            return View(model);

            //if (Request.UrlReferrer != null)
            //    return Redirect(Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index");
        }

        #endregion

        #region Detail
        public ActionResult Detail(string groupCode)
        {
            IEnumerable<UsingServiceDetailViewModel> listDetail = new List<UsingServiceDetailViewModel>();

            if (string.IsNullOrEmpty(groupCode) == false)
            {
                listDetail = UsingServiceRepository.GetAllvwUsingServiceDetail().Where(x => x.GroupCode == groupCode).Select(detail =>
                new UsingServiceDetailViewModel
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    TransactionSalerId = detail.TransactionSalerId,
                    UsingServiceId = detail.UsingServiceId,
                    ServiceSaleId = detail.ServiceSaleId,
                    CustomerCode = detail.CustomerCode,
                    CustomerName = detail.CustomerName,
                    InvoiceCode = detail.InvoiceCode,
                    PackageProductCode = detail.PackageProductCode,
                    PackageProductName = detail.PackageProductName,
                    ProductCode = detail.ProductCode,
                    ProductName = detail.ProductName
                });
            }

            return View(listDetail);
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                string idDeleteAll = Request["DeleteId-checkbox"];
                string[] arrDeleteId = idDeleteAll.Split(',');
                for (int i = 0; i < arrDeleteId.Count(); i++)
                {
                    var item = UsingServiceRepository.GetUsingServiceById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        //if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        //{
                        //    TempData["FailedMessage"] = "NotOwner";
                        //    return RedirectToAction("Index");
                        //}

                        item.IsDeleted = true;
                        UsingServiceRepository.UpdateUsingService(item);
                    }
                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}
