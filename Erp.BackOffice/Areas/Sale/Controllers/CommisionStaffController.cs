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
using Erp.Domain.Sale.Repositories;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class CommisionSaleController : Controller
    {
        private readonly ICommisionSaleRepository CommisionSaleRepository;
        private readonly IUserRepository userRepository;

        public CommisionSaleController(
            ICommisionSaleRepository _CommisionSale
            , IUserRepository _user
            )
        {
            CommisionSaleRepository = _CommisionSale;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {
            IQueryable<CommisionSaleViewModel> q = CommisionSaleRepository.GetAllCommisionSale()
                .Select(item => new CommisionSaleViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                }).OrderByDescending(m => m.ModifiedDate);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create()
        {
            var model = new CommisionSaleViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CommisionSaleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var CommisionSale = new CommisionSale();
                AutoMapper.Mapper.Map(model, CommisionSale);
                CommisionSale.IsDeleted = false;
                CommisionSale.CreatedUserId = WebSecurity.CurrentUserId;
                CommisionSale.ModifiedUserId = WebSecurity.CurrentUserId;
                //CommisionSale.AssignedUserId = WebSecurity.CurrentUserId;
                CommisionSale.CreatedDate = DateTime.Now;
                CommisionSale.ModifiedDate = DateTime.Now;
                CommisionSaleRepository.InsertCommisionSale(CommisionSale);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public static void Create(int InvoiceId, decimal? TotalAmount)
        {
            CommisionRepository CommisionRepository = new Domain.Sale.Repositories.CommisionRepository(new Domain.Sale.ErpSaleDbContext());
            var commisions = CommisionRepository.GetAllCommision().Where(x => x.StartDate <= DateTime.Now && DateTime.Now <= x.EndDate).ToList();

            Commision commisionApply = null;
            foreach(var item in commisions)
            {
                // tìm chương trình chiếc khấu thỏa đk ngày ở trên và áp dụng cho chi nhánh của nhân viên đang đăng nhập
                int? brandIdCurrent = Helpers.Common.CurrentUser.BranchId;
                var commision_branch = CommisionRepository.GetListCommisionBranch(item.Id).Where(x => x.BranchId == brandIdCurrent).FirstOrDefault();
                if (commision_branch == null)
                    continue;

                commisionApply = item;
                if (commisionApply != null)
                    break;
            }

            //nếu có chương trình chiếc khấu thì mới thêm
            if (commisionApply != null)
            {
                CommisionSale commisionSale = new CommisionSale { 
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedUserId = WebSecurity.CurrentUserId,
                    BranchId = Helpers.Common.CurrentUser.BranchId,
                    CommisionId = commisionApply.Id,
                    AmountOfCommision = Math.Round(Convert.ToDecimal(commisionApply.PercentOfCommision * TotalAmount) / 100),
                    PercentOfCommision = commisionApply.PercentOfCommision,
                    ProductInvoiceId = InvoiceId,
                    IsDeleted = false,
                };
            }
        }

        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var CommisionSale = CommisionSaleRepository.GetCommisionSaleById(Id.Value);
            if (CommisionSale != null && CommisionSale.IsDeleted != true)
            {
                var model = new CommisionSaleViewModel();
                AutoMapper.Mapper.Map(CommisionSale, model);
                
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
        public ActionResult Edit(CommisionSaleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var CommisionSale = CommisionSaleRepository.GetCommisionSaleById(model.Id);
                    AutoMapper.Mapper.Map(model, CommisionSale);
                    CommisionSale.ModifiedUserId = WebSecurity.CurrentUserId;
                    CommisionSale.ModifiedDate = DateTime.Now;
                    CommisionSaleRepository.UpdateCommisionSale(CommisionSale);

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
        public ActionResult Detail(int? Id)
        {
            var CommisionSale = CommisionSaleRepository.GetCommisionSaleById(Id.Value);
            if (CommisionSale != null && CommisionSale.IsDeleted != true)
            {
                var model = new CommisionSaleViewModel();
                AutoMapper.Mapper.Map(CommisionSale, model);
                
                if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                {
                    TempData["FailedMessage"] = "NotOwner";
                    return RedirectToAction("Index");
                }                

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
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
                    var item = CommisionSaleRepository.GetCommisionSaleById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        //if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        //{
                        //    TempData["FailedMessage"] = "NotOwner";
                        //    return RedirectToAction("Index");
                        //}

                        item.IsDeleted = true;
                        CommisionSaleRepository.UpdateCommisionSale(item);
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
