using System.Globalization;
using Erp.BackOffice.Administration.Models;
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
using qts.webapp.backend.domain.Services.Administration;
using qts.webapp.backend.domain.Models.Administration;

namespace Erp.BackOffice.Administration.Controllers
{
    public class WardController : BaseController
    {
        private readonly IWardService WardService;
        private readonly IvwWardService vwWardService;
        private readonly IUserRepository userRepository;

        public WardController(
            IWardService _Ward
            , IvwWardService _vwWardService
            , IUserRepository _user
            )
        {
            WardService = _Ward;
            vwWardService = _vwWardService;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string Name, string Type, string DistrictId, string ProvinceId)
        {
            IEnumerable<WardViewModel> q = vwWardService.GetNotDelete()
                .Select(item => new WardViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    ProvinceId = item.ProvinceId,
                    ProvinceName = item.ProvinceName,
                    ProvinceType = item.ProvinceType,
                    DistrictId = item.DistrictId,
                    DistrictName = item.DistrictName,
                    DistrictType = item.DistrictType,
                    Type = item.Type,
                    Name = item.Name
                }).OrderBy(m => m.Name);
            if (!string.IsNullOrEmpty(Name))
            {
                q = q.Where(n => n.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Type))
            {
                q = q.Where(n => n.Type.Equals(Type));
            }
            if (!string.IsNullOrEmpty(ProvinceId))
            {
                q = q.Where(n => n.ProvinceId.Equals(ProvinceId));
            }
            if (!string.IsNullOrEmpty(DistrictId))
            {
                q = q.Where(n => n.DistrictId.Equals(DistrictId));
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create(string ProvinceId, string DistrictId)
        {
            var model = new WardViewModel();
            model.ProvinceId = ProvinceId;
            model.DistrictId = DistrictId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(WardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Ward = new Ward();
                AutoMapper.Mapper.Map(model, Ward);
                SetModifier(Ward);
                WardService.Create(Ward);
                Ward.WardId = Ward.Id.ToString();
                WardService.Update(Ward);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                 if (Request["IsPopup"] != null && Request["IsPopup"].ToString().ToLower().Equals("true"))
                    {
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "updateWard(" + Ward.Id + ", '" + Helpers.Common.Capitalize(string.Format("{0} {1}",Ward.Type,Ward.Name)) + "')" });
                    }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Ward = vwWardService.Get(Id.Value);
            if (Ward != null && Ward.IsDeleted != true)
            {
                var model = new WardViewModel();
                AutoMapper.Mapper.Map(Ward, model);
                
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
        public ActionResult Edit(WardViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Ward = WardService.Get(model.Id);
                    AutoMapper.Mapper.Map(model, Ward);
                    SetModifier(Ward, true);
                    WardService.Update(Ward);
                   
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
            var Ward = WardService.Get(Id.Value);
            if (Ward != null && Ward.IsDeleted != true)
            {
                var model = new WardViewModel();
                AutoMapper.Mapper.Map(Ward, model);
                
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
                    var item = WardService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        WardService.DeleteRs(item);
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
