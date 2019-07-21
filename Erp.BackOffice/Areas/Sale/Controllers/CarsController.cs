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
    public class CarsController : BaseController
    {
        private readonly ICarsService CarsService;
        private readonly IvwCarService vwCarsService;
        private readonly IUserRepository userRepository;

        public CarsController(
            ICarsService _Cars
           , IvwCarService _vwCars
            , IUserRepository _user
            )
        {
            CarsService = _Cars;
            vwCarsService = _vwCars;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch, int? CustomerId)
        {
            IEnumerable<CarsViewModel> q = vwCarsService.GetByCustomerId(CustomerId)
                .Select(item => new CarsViewModel
                {
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    CreatedDate = item.CreatedDate,
                    CreatedUserId = item.CreatedUserId,
                    ModifiedDate = item.ModifiedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    AssignedUserId = item.AssignedUserId,
                    Name = item.Name,
                    Manufacturer = item.Manufacturer,
                    Vehicles = item.Vehicles,
                    Color = item.Color,
                    Frames = item.Frames,
                    Number = item.Number,
                    Note = item.Note,
                    Plate = item.Plate,
                    CustomerId = item.CustomerId,
                    CarLineName = item.CarLineName
                }).OrderByDescending(m => m.ModifiedDate);
            ViewBag.CustomerId = CustomerId;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create(int? CustomerId)
        {
            var model = new CarsViewModel();
            model.CustomerId = CustomerId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CarsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Cars = new Cars();
                AutoMapper.Mapper.Map(model, Cars);
                SetModifier(Cars);
                CarsService.Create(Cars);
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
            var Cars = CarsService.Get(Id.Value);
            if (Cars != null && Cars.IsDeleted != true)
            {
                var model = new CarsViewModel();
                AutoMapper.Mapper.Map(Cars, model);
                
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
        public ActionResult Edit(CarsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Cars = CarsService.Get(model.Id);
                    AutoMapper.Mapper.Map(model, Cars);
                    SetModifier(Cars, true);
                    CarsService.Update(Cars);

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
            var Cars = CarsService.Get(Id.Value);
            if (Cars != null && Cars.IsDeleted != true)
            {
                var model = new CarsViewModel();
                AutoMapper.Mapper.Map(Cars, model);
                
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
                    var item = CarsService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        CarsService.DeleteRs(item);
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
        [HttpPost]
        public JsonResult DeleteJS(int id)
        {
            try
            {
                var item = CarsService.Get(id);
                CarsService.DeleteRs(item);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
