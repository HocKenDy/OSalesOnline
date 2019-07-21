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
using System.IO;

namespace Erp.BackOffice.Sale.Controllers
{
    public class MemberCardTypeController : BaseController
    {
        private readonly IMemberCardTypeService MemberCardTypeService;
        private readonly IUserRepository userRepository;

        public MemberCardTypeController(
            IMemberCardTypeService _MemberCardType
            , IUserRepository _user
            )
        {
            MemberCardTypeService = _MemberCardType;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {
            IEnumerable<MemberCardTypeViewModel> q = MemberCardTypeService.Get()
                .Select(item => new MemberCardTypeViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                    TargetPoint = item.TargetPoint
                }).OrderBy(m => m.Name);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create()
        {
            var model = new MemberCardTypeViewModel();
            model.TargetPoint = 0;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MemberCardTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var MemberCardType = new MemberCardType();
                AutoMapper.Mapper.Map(model, MemberCardType);
                SetModifier(MemberCardType);

                var path = "/files/membercardtype/";
                var filepath = System.Web.HttpContext.Current.Server.MapPath("~" + path);
                if (Request.Files["file-image"] != null)
                {
                    var file = Request.Files["file-image"];
                    if (file.ContentLength > 0)
                    {
                        FileInfo fi = new FileInfo(Server.MapPath("~" + path) + MemberCardType.Image);
                        if (fi.Exists)
                        {
                            fi.Delete();
                        }
                        var FileName = model.Name.Replace(" ", "_");
                        var name = Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(FileName).ToLower();
                        string image_name = name + Guid.NewGuid().ToString() + "." + file.FileName.Split('.').Last();

                        bool isExists = System.IO.Directory.Exists(filepath);
                        if (!isExists)
                            System.IO.Directory.CreateDirectory(filepath);
                        file.SaveAs(filepath + image_name);
                        MemberCardType.Image = image_name;
                    }
                }
                MemberCardTypeService.Create(MemberCardType);
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
            var MemberCardType = MemberCardTypeService.Get(Id.Value);
            if (MemberCardType != null && MemberCardType.IsDeleted != true)
            {
                var model = new MemberCardTypeViewModel();
                AutoMapper.Mapper.Map(MemberCardType, model);
                
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
        public ActionResult Edit(MemberCardTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var MemberCardType = MemberCardTypeService.Get(model.Id);
                    AutoMapper.Mapper.Map(model, MemberCardType);
                    SetModifier(MemberCardType, true);
                    var path = "/files/membercardtype/";
                    var filepath = System.Web.HttpContext.Current.Server.MapPath("~" + path);
                    if (Request.Files["file-image"] != null)
                    {
                        var file = Request.Files["file-image"];
                        if (file.ContentLength > 0)
                        {
                            FileInfo fi = new FileInfo(Server.MapPath("~" + path) + MemberCardType.Image);
                            if (fi.Exists)
                            {
                                fi.Delete();
                            }
                            var FileName = model.Name.Replace(" ", "_");
                            var name = Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(FileName).ToLower();
                            string image_name = name + Guid.NewGuid().ToString() + "." + file.FileName.Split('.').Last();

                            bool isExists = System.IO.Directory.Exists(filepath);
                            if (!isExists)
                                System.IO.Directory.CreateDirectory(filepath);
                            file.SaveAs(filepath + image_name);
                            MemberCardType.Image = image_name;
                        }
                    }
                    MemberCardTypeService.Update(MemberCardType);

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
            var MemberCardType = MemberCardTypeService.Get(Id.Value);
            if (MemberCardType != null && MemberCardType.IsDeleted != true)
            {
                var model = new MemberCardTypeViewModel();
                AutoMapper.Mapper.Map(MemberCardType, model);
                
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
                    var item = MemberCardTypeService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        MemberCardTypeService.DeleteRs(item);
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
