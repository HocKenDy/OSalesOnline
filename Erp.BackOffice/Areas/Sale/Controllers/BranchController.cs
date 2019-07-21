using System.Globalization;
using Erp.BackOffice.Filters;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Sale.Models;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class BranchController : Controller
    {
        private readonly IBranchRepository BranchRepository;
        private readonly IUserRepository userRepository;
        private readonly IBranchDepartmentRepository branchDepartmentRepository;
        private readonly ILocationRepository locationRepository;
        public BranchController(
            IBranchRepository _Branch
            , IUserRepository _user
              , IBranchDepartmentRepository branchDepartment
             , ILocationRepository location
            )
        {
            BranchRepository = _Branch;
            userRepository = _user;
            branchDepartmentRepository = branchDepartment;
            locationRepository = location;
        }

        #region Index
        public ViewResult Index(string txtSearch)
        {

            IQueryable<BranchViewModel> q = BranchRepository.GetAllBranch()
                .Select(item => new BranchViewModel
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

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Branch = BranchRepository.GetBranchById(Id.Value);
            if (Branch != null && Branch.IsDeleted != true)
            {
                var model = new BranchViewModel();
                AutoMapper.Mapper.Map(Branch, model);
                model.ProvinceList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, "0", model.CityId);
                model.DistrictList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, model.CityId, model.DistrictId);
                model.WardList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, model.DistrictId, model.WardId);
                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(BranchViewModel model)
        {
            var urlRefer = Request["UrlReferrer"];
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Branch = BranchRepository.GetBranchById(model.Id);
                    AutoMapper.Mapper.Map(model, Branch);
                    Branch.ModifiedUserId = WebSecurity.CurrentUserId;
                    Branch.ModifiedDate = DateTime.Now;
                    BranchRepository.UpdateBranch(Branch);

                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    if (Request["IsPopup"] == "true" || Request["IsPopup"] == "True")
                    {
                        TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                        ViewBag.closePopup = "true";
                        model.Id = Branch.Id;
                        ViewBag.urlRefer = urlRefer;
                        model.ProvinceList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, "0", model.CityId);
                        model.DistrictList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, model.CityId, model.DistrictId);
                        model.WardList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, model.DistrictId, model.WardId);
                        return View(model);
                    }
                }

                return Redirect(urlRefer);
            }

            return View(model);
        }

        #endregion

        #region Create
        public ViewResult Create()
        {
            var model = new BranchViewModel();
            model.ProvinceList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, "0", null);
            model.DistrictList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, null, null);
            model.WardList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, null, null);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(BranchViewModel model)
        {
            var urlRefer = Request["UrlReferrer"];
            if (ModelState.IsValid)
            {
                var Branch = new Domain.Sale.Entities.Branch();
                AutoMapper.Mapper.Map(model, Branch);
                Branch.IsDeleted = false;
                Branch.CreatedUserId = WebSecurity.CurrentUserId;
                Branch.ModifiedUserId = WebSecurity.CurrentUserId;
                Branch.CreatedDate = DateTime.Now;
                Branch.ModifiedDate = DateTime.Now;
                BranchRepository.InsertBranch(Branch);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                if (Request["IsPopup"] == "true" || Request["IsPopup"] == "True")
                {
                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                    ViewBag.closePopup = "true";
                    model.Id = Branch.Id;
                    ViewBag.urlRefer = urlRefer;
                    model.ProvinceList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, "0", null);
                    model.DistrictList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, null, null);
                    model.WardList = Helpers.SelectListHelper.GetSelectList_Location(locationRepository, null, null);
                    return View(model);
                }
                return Redirect(urlRefer);
            }
            return View(model);
        }
        #endregion

        #region Delete
    
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                string idDeleteAll = Request["DeleteAll-checkbox"];
                string[] arrDeleteId = idDeleteAll.Split(',');
                for (int i = 0; i < arrDeleteId.Count(); i++)
                {
                    int id = Convert.ToInt32(arrDeleteId[i]);
                    if (!userRepository.GetAllUsers().Any(item => item.BranchId == id))
                    {
                        var item = BranchRepository.GetBranchById(id);
                        item.IsDeleted = true;
                        BranchRepository.UpdateBranch(item);
                        var branchDepartment = branchDepartmentRepository.GetAllBranchDepartment().Where(x => x.Sale_BranchId == item.Id);

                        TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                    }
                    else
                    {
                        TempData[Globals.FailedMessageKey] = "Vui lòng xóa dữ liệu người dùng có tham chiếu đến chi nhánh này trước!";
                    }
                }
                return RedirectToAction("Index", "Branch", new { area = "Sale" });
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return RedirectToAction("Index", "Branch", new { area = "Sale" });
            }
        }
        #endregion

        #region DetailBasic
        public ActionResult DetailBasic(int? Id)
        {
            var branch = BranchRepository.GetvwBranchById(Id.Value);
            if (branch != null)
            {
                var model = new BranchViewModel();
                AutoMapper.Mapper.Map(branch, model);
                return View(model);
            }
            return View();
        }
        #endregion        
    }
}
