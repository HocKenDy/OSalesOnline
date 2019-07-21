using System.Globalization;
using Erp.BackOffice.Account.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Account.Entities;
using Erp.Domain.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;

namespace Erp.BackOffice.Account.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class ReceiptDetailController : Controller
    {
        private readonly IReceiptDetailRepository ReceiptDetailRepository;
        private readonly IUserRepository userRepository;

        public ReceiptDetailController(
            IReceiptDetailRepository _ReceiptDetail
            , IUserRepository _user
            )
        {
            ReceiptDetailRepository = _ReceiptDetail;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {

            IQueryable<ReceiptDetailViewModel> q = ReceiptDetailRepository.GetAllReceiptDetail()
                .Select(item => new ReceiptDetailViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
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
            var model = new ReceiptDetailViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReceiptDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ReceiptDetail = new ReceiptDetail();
                AutoMapper.Mapper.Map(model, ReceiptDetail);
                ReceiptDetail.IsDeleted = false;
                ReceiptDetail.CreatedUserId = WebSecurity.CurrentUserId;
                ReceiptDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                ReceiptDetail.AssignedUserId = WebSecurity.CurrentUserId;
                ReceiptDetail.CreatedDate = DateTime.Now;
                ReceiptDetail.ModifiedDate = DateTime.Now;
                ReceiptDetailRepository.InsertReceiptDetail(ReceiptDetail);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var ReceiptDetail = ReceiptDetailRepository.GetReceiptDetailById(Id.Value);
            if (ReceiptDetail != null && ReceiptDetail.IsDeleted != true)
            {
                var model = new ReceiptDetailViewModel();
                AutoMapper.Mapper.Map(ReceiptDetail, model);
                
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

        [HttpPost]
        public ActionResult Edit(ReceiptDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var ReceiptDetail = ReceiptDetailRepository.GetReceiptDetailById(model.Id);
                    AutoMapper.Mapper.Map(model, ReceiptDetail);
                    ReceiptDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                    ReceiptDetail.ModifiedDate = DateTime.Now;
                    ReceiptDetailRepository.UpdateReceiptDetail(ReceiptDetail);

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
            var ReceiptDetail = ReceiptDetailRepository.GetReceiptDetailById(Id.Value);
            if (ReceiptDetail != null && ReceiptDetail.IsDeleted != true)
            {
                var model = new ReceiptDetailViewModel();
                AutoMapper.Mapper.Map(ReceiptDetail, model);
                
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
                    var item = ReceiptDetailRepository.GetReceiptDetailById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }

                        item.IsDeleted = true;
                        ReceiptDetailRepository.UpdateReceiptDetail(item);
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
