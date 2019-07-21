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
    public class DistrictController : BaseController
    {
        private readonly IDistrictService DistrictService;
        private readonly IvwDistrictService vwDistrictService;
        private readonly IUserRepository userRepository;

        public DistrictController(
            IDistrictService _District
            , IUserRepository _user
            , IvwDistrictService _vwDistrictService
            )
        {
            DistrictService = _District;
            userRepository = _user;
            vwDistrictService = _vwDistrictService;
        }

        #region Index

        public ViewResult Index(string Name, string Type, string ProvinceId)
        {
            IEnumerable<DistrictViewModel> q = vwDistrictService.GetNotDelete()
                .Select(item => new DistrictViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Type = item.Type,
                    ProvinceId = item.ProvinceId,
                    DistrictId = item.DistrictId,
                    Name = item.Name,
                    Location = item.Location,
                    ProvinceName = item.ProvinceName,
                    ProvinceType = item.ProvinceType
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

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create(string ProvinceId)
        {
            var model = new DistrictViewModel();
            model.ProvinceId = ProvinceId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DistrictViewModel model)
        {
            if (ModelState.IsValid)
            {
                var District = new District();
                AutoMapper.Mapper.Map(model, District);
                SetModifier(District);
                DistrictService.Create(District);

                District.DistrictId = District.Id.ToString();
                DistrictService.Update(District);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                if (Request["IsPopup"] != null && Request["IsPopup"].ToString().ToLower().Equals("true"))
                {
                    return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "updateDistrict(" + District.Id + ", '" + Helpers.Common.Capitalize(string.Format("{0} {1}", District.Type, District.Name)) + "')" });
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var District = DistrictService.Get(Id.Value);
            if (District != null && District.IsDeleted != true)
            {
                var model = new DistrictViewModel();
                AutoMapper.Mapper.Map(District, model);

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
        public ActionResult Edit(DistrictViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var District = DistrictService.Get(model.Id);
                    AutoMapper.Mapper.Map(model, District);
                    SetModifier(District, true);
                    DistrictService.Update(District);

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
            var District = DistrictService.Get(Id.Value);
            if (District != null && District.IsDeleted != true)
            {
                var model = new DistrictViewModel();
                AutoMapper.Mapper.Map(District, model);

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
                    var item = DistrictService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        DistrictService.DeleteRs(item);
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
