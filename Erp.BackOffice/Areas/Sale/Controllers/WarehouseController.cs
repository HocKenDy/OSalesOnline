using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Sale.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using Erp.Domain.Sale.Entities;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IBranchRepository branchRepository;
        private readonly IObjectAttributeRepository ObjectAttributeRepository;
        private readonly IUserRepository userRepository;

        public WarehouseController(
            IWarehouseRepository _Warehouse
            , IBranchRepository _branch
            , IObjectAttributeRepository _ObjectAttribute
            , IUserRepository _user
            )
        {
            WarehouseRepository = _Warehouse;
            branchRepository = _branch;
            ObjectAttributeRepository = _ObjectAttribute;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch, string txtCode, string txtAddress)
        {
            IEnumerable<WarehouseViewModel> q = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId)
                .Select(item => new WarehouseViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                    Code = item.Code,
                    Address = item.Address
                }).OrderByDescending(m => m.ModifiedDate);

            if (string.IsNullOrEmpty(txtSearch) == false || string.IsNullOrEmpty(txtCode) == false || string.IsNullOrEmpty(txtAddress) == false)
            {
                txtSearch = txtSearch == "" ? "~" : Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtSearch);
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                txtAddress = txtAddress == "" ? "~" : txtAddress.ToLower();
                q = q.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.Name).Contains(txtSearch) || x.Code.ToLowerOrEmpty().Contains(txtCode) || Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.Address).Contains(txtAddress));
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
            var model = new WarehouseViewModel();

            ViewBag.branchList = branchRepository.GetAllBranch().AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(WarehouseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Warehouse = new Domain.Sale.Entities.Warehouse();
                AutoMapper.Mapper.Map(model, Warehouse);
                Warehouse.IsDeleted = false;
                Warehouse.CreatedUserId = WebSecurity.CurrentUserId;
                Warehouse.ModifiedUserId = WebSecurity.CurrentUserId;
                Warehouse.CreatedDate = DateTime.Now;
                Warehouse.ModifiedDate = DateTime.Now;
                Warehouse.KeeperId = Request["KeeperId"];
                Warehouse.Categories = Request["Categories"];
                Warehouse.IsSale = model.IsSale;
                Warehouse.BranchId = Helpers.Common.CurrentUser.BranchId;

                //tạo đặc tính động cho kho hàng nếu có danh sách đặc tính động truyền vào và đặc tính đó phải có giá trị
                ObjectAttributeController.CreateOrUpdateForObject(Warehouse.Id, model.AttributeValueList);
                
                WarehouseRepository.InsertWarehouse(Warehouse);

                //if(string.IsNullOrEmpty(Request["IsPopup"]) == false)
                //{
                //    ViewBag.closePopup = "close and append to page parent";
                //    model.Id = Warehouse.Id;
                //    return View(model);
                //}


                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Warehouse = WarehouseRepository.GetWarehouseById(Id.Value);
            if (Warehouse != null && Warehouse.IsDeleted != true)
            {
                var model = new WarehouseViewModel();
                AutoMapper.Mapper.Map(Warehouse, model);
                
                //if (model.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
                //{
                //    TempData["FailedMessage"] = "NotOwner";
                //    return RedirectToAction("Index");
                //}

                ViewBag.branchList = branchRepository.GetAllBranch().AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
                List<string> listCategories = new List<string>();
                if(!string.IsNullOrEmpty(Warehouse.Categories))
                { 
                listCategories = Warehouse.Categories.Split(',').ToList();
                 }
                ViewBag.listCategories = listCategories;
                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(WarehouseViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Warehouse = WarehouseRepository.GetWarehouseById(model.Id);
                    AutoMapper.Mapper.Map(model, Warehouse);
                    Warehouse.KeeperId = Request["KeeperId"];
                    Warehouse.Categories = Request["Categories"];
                    Warehouse.ModifiedUserId = WebSecurity.CurrentUserId;
                    Warehouse.ModifiedDate = DateTime.Now;
                    Warehouse.IsSale = model.IsSale;
                    //tạo đặc tính động cho kho hàng nếu có danh sách đặc tính động truyền vào và đặc tính đó phải có giá trị
                    //ObjectAttributeController.CreateOrUpdateForObject(Warehouse.Id, model.AttributeValueList);

                    WarehouseRepository.UpdateWarehouse(Warehouse);

                    if (string.IsNullOrEmpty(Request["IsPopup"]) == false)
                    {
                        return RedirectToAction("Edit", new { Id = model.Id, IsPopup = Request["IsPopup"] });
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
                    var item = WarehouseRepository.GetWarehouseById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        //if (item.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
                        //{
                        //    TempData["FailedMessage"] = "NotOwner";
                        //    return RedirectToAction("Index");
                        //}

                        item.IsDeleted = true;
                        WarehouseRepository.UpdateWarehouse(item);
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

        #region
        public JsonResult GetListUserByBranchId(int? branchId)
        {
            if (branchId == null)
                return Json(new List<int>(), JsonRequestBehavior.AllowGet);

            var list = userRepository.GetAllUsers().Where(x => x.BranchId == branchId)
                .Select(x => new { 
                    e = x.FullName,
                    a = x.Id,
                    b = x.UserTypeId,
                    c = x.UserName, 
                    d = x.Status
                });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
