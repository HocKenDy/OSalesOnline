using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using Erp.BackOffice.Controllers;
using qts.webapp.backend.domain.Services.Sale;
using qts.webapp.backend.domain.Models.Sale;

namespace Erp.BackOffice.Sale.Controllers
{
    public class HistoryPointController : BaseController
    {
        private readonly IHistoryPointService HistoryPointService;
        private readonly IUserRepository userRepository;

        public HistoryPointController(
            IHistoryPointService _HistoryPoint
            , IUserRepository _user
            )
        {
            HistoryPointService = _HistoryPoint;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {
            IEnumerable<HistoryPointViewModel> q = HistoryPointService.Get()
                .Select(item => new HistoryPointViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate
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
            var model = new HistoryPointViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(HistoryPointViewModel model)
        {
            if (ModelState.IsValid)
            {
                var HistoryPoint = new HistoryPoint();
                AutoMapper.Mapper.Map(model, HistoryPoint);
                SetModifier(HistoryPoint);
                HistoryPointService.Create(HistoryPoint);
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                 if (Request["IsPopup"] != null && Request["IsPopup"].ToString().ToLower().Equals("true"))
                    {
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                    }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var HistoryPoint = HistoryPointService.Get(Id.Value);
            if (HistoryPoint != null && HistoryPoint.IsDeleted != true)
            {
                var model = new HistoryPointViewModel();
                AutoMapper.Mapper.Map(HistoryPoint, model);
                
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
        public ActionResult Edit(HistoryPointViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var HistoryPoint = HistoryPointService.Get(model.Id);
                    AutoMapper.Mapper.Map(model, HistoryPoint);
                    SetModifier(HistoryPoint, true);
                    HistoryPointService.Update(HistoryPoint);

                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                      if (Request["IsPopup"] != null && Request["IsPopup"].ToString().ToLower().Equals("true"))
                        {
                            return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                        }
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
            var HistoryPoint = HistoryPointService.Get(Id.Value);
            if (HistoryPoint != null && HistoryPoint.IsDeleted != true)
            {
                var model = new HistoryPointViewModel();
                AutoMapper.Mapper.Map(HistoryPoint, model);
                
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
                    var item = HistoryPointService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        HistoryPointService.DeleteRs(item);
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

        #region GetPoint
        
        #endregion

        public static void CreatePoint(int? CustomerId,int? TargetId,string TargetName, double? AccumulatedPoint, double? UsePoint)
        {
            IHistoryPointService HistoryPointService = DependencyResolver.Current.GetService<IHistoryPointService>();
            var HistoryPoint = new HistoryPoint();
            HistoryPoint.IsDeleted = false;
            HistoryPoint.CreatedUserId = WebSecurity.CurrentUserId;
            HistoryPoint.CreatedDate = DateTime.Now;
            HistoryPoint.ModifiedUserId = WebSecurity.CurrentUserId;
            HistoryPoint.ModifiedDate = DateTime.Now;
            HistoryPoint.CustomerId = CustomerId;
            HistoryPoint.TargetId = TargetId;
            HistoryPoint.TargetName = TargetName;
            HistoryPoint.AccumulatedPoint = AccumulatedPoint;
            HistoryPoint.UsePoint = UsePoint;
            HistoryPointService.Create(HistoryPoint);
        }
        public static void DeletedPoint(int? TargetId, string TargetName)
        {
            IHistoryPointService HistoryPointService = DependencyResolver.Current.GetService<IHistoryPointService>();
            var HistoryPoint = HistoryPointService.GetAllHistoryPoint().Where(x => x.TargetId == TargetId && x.TargetName == TargetName).ToList();
            foreach (var item in HistoryPoint)
            {
                HistoryPointService.Delete(item);
            }
        }
    }
}
