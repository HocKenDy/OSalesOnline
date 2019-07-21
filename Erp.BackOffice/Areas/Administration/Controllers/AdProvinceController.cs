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
    public class AdProvinceController : BaseController
    {
        private readonly IProvinceService ProvinceService;
        private readonly IUserRepository userRepository;

        public AdProvinceController(
            IProvinceService _Province
            , IUserRepository _user
            )
        {
            ProvinceService = _Province;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string Name, string Type)
        {
            IEnumerable<ProvinceViewModel> q = ProvinceService.GetNotDelete()
                .Select(item => new ProvinceViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                    Type = item.Type,
                    ProvinceId = item.ProvinceId
                }).OrderBy(m => m.Name);

            if (!string.IsNullOrEmpty(Name))
            {
                q = q.Where(n => n.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Type))
            {
                q = q.Where(n => n.Type.Equals(Type));
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create()
        {
            var model = new ProvinceViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProvinceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Province = new Province();
                AutoMapper.Mapper.Map(model, Province);
                SetModifier(Province);
                ProvinceService.Create(Province);
                Province.ProvinceId = Province.Id.ToString();
                ProvinceService.Update(Province);
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                 if (Request["IsPopup"] != null && Request["IsPopup"].ToString().ToLower().Equals("true"))
                    {
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "updateProvince("+Province.Id+", '"+ Helpers.Common.Capitalize( string.Format("{0} {1}", Province.Type, Province.Name)) + "')" });
                    }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Province = ProvinceService.Get(Id.Value);
            if (Province != null && Province.IsDeleted != true)
            {
                var model = new ProvinceViewModel();
                AutoMapper.Mapper.Map(Province, model);
                
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
        public ActionResult Edit(ProvinceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Province = ProvinceService.Get(model.Id);
                    AutoMapper.Mapper.Map(model, Province);
                    SetModifier(Province, true);
                    ProvinceService.Update(Province);

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
            var Province = ProvinceService.Get(Id.Value);
            if (Province != null && Province.IsDeleted != true)
            {
                var model = new ProvinceViewModel();
                AutoMapper.Mapper.Map(Province, model);
                
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
                    var item = ProvinceService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        ProvinceService.DeleteRs(item);
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
