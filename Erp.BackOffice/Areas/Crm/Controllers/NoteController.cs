using System.Globalization;
using Erp.BackOffice.Crm.Models;
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
using qts.webapp.backend.domain.Services.Crm;
using qts.webapp.backend.domain.Models.Crm;

namespace Erp.BackOffice.Crm.Controllers
{
    public class NoteController : BaseController
    {
        private readonly INoteService NoteService;
        private readonly IUserRepository userRepository;

        public NoteController(
            INoteService _Note
            , IUserRepository _user
            )
        {
            NoteService = _Note;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {
            IEnumerable<NoteViewModel> q = NoteService.Get()
                .Select(item => new NoteViewModel
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
            var model = new NoteViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Note = new Note();
                AutoMapper.Mapper.Map(model, Note);
                SetModifier(Note);
                NoteService.Create(Note);
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
            var Note = NoteService.Get(Id.Value);
            if (Note != null && Note.IsDeleted != true)
            {
                var model = new NoteViewModel();
                AutoMapper.Mapper.Map(Note, model);
                
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
        public ActionResult Edit(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Note = NoteService.Get(model.Id);
                    AutoMapper.Mapper.Map(model, Note);
                    SetModifier(Note, true);
                    NoteService.Update(Note);

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
            var Note = NoteService.Get(Id.Value);
            if (Note != null && Note.IsDeleted != true)
            {
                var model = new NoteViewModel();
                AutoMapper.Mapper.Map(Note, model);
                
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
                    var item = NoteService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        NoteService.DeleteRs(item);
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
