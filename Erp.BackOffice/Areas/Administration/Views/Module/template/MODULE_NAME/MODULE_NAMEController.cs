using System.Globalization;
using <APP_NAME>.BackOffice.<AREA_NAME>.Models;
using <APP_NAME>.BackOffice.Filters;
using <APP_NAME>.Domain.Entities;
using <APP_NAME>.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using <APP_NAME>.Utilities;
using WebMatrix.WebData;
using <APP_NAME>.BackOffice.Helpers;
using Erp.BackOffice.Controllers;
using qts.webapp.backend.domain.Services.<AREA_NAME>;
using qts.webapp.backend.domain.Models.<AREA_NAME>;

namespace <APP_NAME>.BackOffice.<AREA_NAME>.Controllers
{
    public class <MODULE_NAME>Controller : BaseController
    {
        private readonly I<MODULE_NAME>Service <MODULE_NAME>Service;
        private readonly IUserRepository userRepository;

        public <MODULE_NAME>Controller(
            I<MODULE_NAME>Service _<MODULE_NAME>
            , IUserRepository _user
            )
        {
            <MODULE_NAME>Service = _<MODULE_NAME>;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {
            IEnumerable<<MODULE_NAME>ViewModel> q = <MODULE_NAME>Service.Get()
                .Select(item => new <MODULE_NAME>ViewModel
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
            var model = new <MODULE_NAME>ViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(<MODULE_NAME>ViewModel model)
        {
            if (ModelState.IsValid)
            {
                var <MODULE_NAME> = new <MODULE_NAME>();
                AutoMapper.Mapper.Map(model, <MODULE_NAME>);
                SetModifier(<MODULE_NAME>);
                <MODULE_NAME>Service.Create(<MODULE_NAME>);
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
            var <MODULE_NAME> = <MODULE_NAME>Service.Get(Id.Value);
            if (<MODULE_NAME> != null && <MODULE_NAME>.IsDeleted != true)
            {
                var model = new <MODULE_NAME>ViewModel();
                AutoMapper.Mapper.Map(<MODULE_NAME>, model);
                
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
        public ActionResult Edit(<MODULE_NAME>ViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var <MODULE_NAME> = <MODULE_NAME>Service.Get(model.Id);
                    AutoMapper.Mapper.Map(model, <MODULE_NAME>);
                    SetModifier(<MODULE_NAME>, true);
                    <MODULE_NAME>Service.Update(<MODULE_NAME>);

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
            var <MODULE_NAME> = <MODULE_NAME>Service.Get(Id.Value);
            if (<MODULE_NAME> != null && <MODULE_NAME>.IsDeleted != true)
            {
                var model = new <MODULE_NAME>ViewModel();
                AutoMapper.Mapper.Map(<MODULE_NAME>, model);
                
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
                    var item = <MODULE_NAME>Service.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        <MODULE_NAME>Service.DeleteRs(item);
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
