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
using Erp.Domain.Sale.Interfaces;
using Erp.BackOffice.Helpers;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Sale.Entities;
using Erp.BackOffice.App_GlobalResources;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class CommisionController : Controller
    {
        private readonly ICommisionRepository CommisionRepository;
        private readonly ICommisionSaleRepository CommisionSaleRepository;
        private readonly IBranchRepository BranchRepository;

        private readonly IUserRepository userRepository;

        public CommisionController(
            ICommisionRepository _Commision
            , ICommisionSaleRepository _CommisionSale
            , IBranchRepository _Branch
            , IUserRepository _user
            )
        {
            CommisionRepository = _Commision;
            CommisionSaleRepository = _CommisionSale;
            BranchRepository = _Branch;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {

            IQueryable<CommisionViewModel> q = CommisionRepository.GetAllCommision()
                .Select(item => new CommisionViewModel
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
            var model = new CommisionViewModel();

            ViewBag.branchList = BranchRepository.GetAllBranch().Where(x => x.Id == Helpers.Common.CurrentUser.BranchId || Helpers.Common.CurrentUser.UserTypeId == 1).AsEnumerable().Select(x => new
                SelectListItem { Text = x.Name, Value = x.Id + "" }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CommisionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Commision = new Domain.Sale.Entities.Commision();
                AutoMapper.Mapper.Map(model, Commision);
                Commision.IsDeleted = false;
                Commision.CreatedUserId = WebSecurity.CurrentUserId;
                Commision.ModifiedUserId = WebSecurity.CurrentUserId;
                Commision.CreatedDate = DateTime.Now;
                Commision.ModifiedDate = DateTime.Now;
                CommisionRepository.InsertCommision(Commision);

                if (model.BranchIdList != null)
                {
                    foreach (var id in model.BranchIdList)
                    {
                        CommisionRepository.InsertCommisionBranch(new CommisionBranch { BranchId = Convert.ToInt32(id), CommisionId = Commision.Id });
                    }
                }

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Commision = CommisionRepository.GetCommisionById(Id.Value);
            if (Commision != null && Commision.IsDeleted != true)
            {
                var model = new CommisionViewModel();
                AutoMapper.Mapper.Map(Commision, model);

                if (model.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
                {
                    TempData["FailedMessage"] = Wording.NotOwner;
                    return RedirectToAction("Index");
                }

                var listCommisionSale = CommisionSaleRepository.GetAllCommisionSale().Where(x => x.CommisionId == Commision.Id).FirstOrDefault();
                if(listCommisionSale != null)
                {
                    TempData["FailedMessage"] = Wording.CommisionApplied;
                    return RedirectToAction("Index");
                }

                ViewBag.branchList = BranchRepository.GetAllBranch().Where(x => x.Id == Helpers.Common.CurrentUser.BranchId || Helpers.Common.CurrentUser.UserTypeId == 1).AsEnumerable().Select(x => new
                SelectListItem { Text = x.Name, Value = x.Id + "" }).ToList();

                var listCommisionBranch = CommisionRepository.GetListCommisionBranch(Commision.Id).ToList();

                model.BranchIdList = listCommisionBranch.Select(x => x.BranchId.ToString()).ToArray();

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(CommisionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Commision = CommisionRepository.GetCommisionById(model.Id);
                    AutoMapper.Mapper.Map(model, Commision);
                    Commision.ModifiedUserId = WebSecurity.CurrentUserId;
                    Commision.ModifiedDate = DateTime.Now;
                    CommisionRepository.UpdateCommision(Commision);

                    var listCommisionBranch = CommisionRepository.GetListCommisionBranch(Commision.Id).ToList();
                    for (int i = 0; i < listCommisionBranch.Count; i++)
                    {
                        CommisionRepository.DeleteCommisionBranch(listCommisionBranch[i].Id);
                    }

                    if (model.BranchIdList != null)
                    {
                        foreach (var id in model.BranchIdList)
                        {
                            CommisionRepository.InsertCommisionBranch(new CommisionBranch { BranchId = Convert.ToInt32(id), CommisionId = Commision.Id });
                        }
                    }

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
            var Commision = CommisionRepository.GetCommisionById(Id.Value);
            if (Commision != null && Commision.IsDeleted != true)
            {
                var model = new CommisionViewModel();
                AutoMapper.Mapper.Map(Commision, model);

                if (model.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
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
                    var item = CommisionRepository.GetCommisionById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        if (item.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }

                        item.IsDeleted = true;
                        CommisionRepository.UpdateCommision(item);
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

        #region sp
        [HttpPost]
        public ActionResult CreateCommision_Branch(int commisionId, int branchId)
        {
            CommisionRepository.sp_Sale_Commision_Branch_Update(commisionId, branchId);
            return Content("OK");
        }
        #endregion
    }
}
