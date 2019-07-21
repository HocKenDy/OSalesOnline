using System.Globalization;
using Erp.BackOffice.Crm.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;

namespace Erp.BackOffice.Crm.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class ProcessAppliedController : Controller
    {
        private readonly IProcessAppliedRepository ProcessAppliedRepository;
        private readonly IUserRepository userRepository;

        public ProcessAppliedController(
            IProcessAppliedRepository _ProcessApplied
            , IUserRepository _user
            )
        {
            ProcessAppliedRepository = _ProcessApplied;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {

            IQueryable<ProcessAppliedViewModel> q = ProcessAppliedRepository.GetAllProcessApplied()
                .Select(item => new ProcessAppliedViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                  //  Name = item.Name,
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
            var model = new ProcessAppliedViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProcessAppliedViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ProcessApplied = new ProcessApplied();
                AutoMapper.Mapper.Map(model, ProcessApplied);
                ProcessApplied.IsDeleted = false;
                ProcessApplied.CreatedUserId = WebSecurity.CurrentUserId;
                ProcessApplied.ModifiedUserId = WebSecurity.CurrentUserId;
                ProcessApplied.AssignedUserId = WebSecurity.CurrentUserId;
                ProcessApplied.CreatedDate = DateTime.Now;
                ProcessApplied.ModifiedDate = DateTime.Now;
                ProcessAppliedRepository.InsertProcessApplied(ProcessApplied);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var ProcessApplied = ProcessAppliedRepository.GetProcessAppliedById(Id.Value);
            if (ProcessApplied != null && ProcessApplied.IsDeleted != true)
            {
                var model = new ProcessAppliedViewModel();
                AutoMapper.Mapper.Map(ProcessApplied, model);
                
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
        public ActionResult Edit(ProcessAppliedViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var ProcessApplied = ProcessAppliedRepository.GetProcessAppliedById(model.Id);
                    AutoMapper.Mapper.Map(model, ProcessApplied);
                    ProcessApplied.ModifiedUserId = WebSecurity.CurrentUserId;
                    ProcessApplied.ModifiedDate = DateTime.Now;
                    ProcessAppliedRepository.UpdateProcessApplied(ProcessApplied);

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
            var ProcessApplied = ProcessAppliedRepository.GetProcessAppliedById(Id.Value);
            if (ProcessApplied != null && ProcessApplied.IsDeleted != true)
            {
                var model = new ProcessAppliedViewModel();
                AutoMapper.Mapper.Map(ProcessApplied, model);
                
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
                    var item = ProcessAppliedRepository.GetProcessAppliedById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }

                        item.IsDeleted = true;
                        ProcessAppliedRepository.UpdateProcessApplied(item);
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
