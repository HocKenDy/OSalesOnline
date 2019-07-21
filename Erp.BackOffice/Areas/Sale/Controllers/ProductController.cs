using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using System.Web.Script.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class ProductController : Controller
    {
        private readonly IProductRepository ProductRepository;
        private readonly IObjectAttributeRepository ObjectAttributeRepository;
        private readonly ISupplierRepository SupplierRepository;
        private readonly IUserRepository userRepository;

        public ProductController(
            IProductRepository _Product
            , IObjectAttributeRepository _ObjectAttribute
            , ISupplierRepository _Supplier
            , IUserRepository _user
            )
        {
            ProductRepository = _Product;
            ObjectAttributeRepository = _ObjectAttribute;
            SupplierRepository = _Supplier;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch, string txtCode, string Categories, string CategoryCode, SearchObjectAttributeViewModel SearchOjectAttr)
        {
            IEnumerable<ProductViewModel> q = ProductRepository.GetAllvwProduct().Where(x => x.Type == "product").AsEnumerable()
                .Select(item => new ProductViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                    Code = item.Code,
                    CategoryName = item.CategoryName,
                    PriceInbound = item.PriceInbound,
                    PriceOutbound = item.PriceOutbound,
                    Barcode = item.Barcode,
                    Type = item.Type,
                    Unit = item.Unit,
                    CategoryCode = item.CategoryCode,
                    Categories = item.Categories,
                    TargetPoint = item.TargetPoint,
                    Point = item.Point,
                    CycleKM = item.CycleKM,
                    CycleTime = item.CycleTime
                }).OrderByDescending(m => m.ModifiedDate);

            //nếu có tìm kiếm nâng cao thì lọc trước
            if (SearchOjectAttr.ListField != null)
            {
                if (SearchOjectAttr.ListField.Count > 0)
                {
                    //lấy các đối tượng ObjectAttributeValue nào thỏa đk có AttributeId trong ListField và có giá trị tương ứng trong ListField
                    var listObjectAttrValue = ObjectAttributeRepository.GetAllObjectAttributeValue().AsEnumerable().Where(attr => SearchOjectAttr.ListField.Any(item => item.Id == attr.AttributeId && Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(attr.Value).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(item.Value)))).ToList();

                    //tiếp theo tìm các sản phẩm có id bằng với ObjectId trong listObjectAttrValue vừa tìm được
                    q = q.Where(product => listObjectAttrValue.Any(item => item.ObjectId == product.Id));

                    ViewBag.ListOjectAttrSearch = new JavaScriptSerializer().Serialize(SearchOjectAttr.ListField.Select(x => new { Id = x.Id, Value = x.Value }));
                }
            }

            if (string.IsNullOrEmpty(txtSearch) == false || string.IsNullOrEmpty(txtCode) == false)
            {
                txtSearch = txtSearch == "" ? "~" : Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtSearch);
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.Name).Contains(txtSearch) || x.Code.ToLower().Contains(txtCode));
            }

            //decimal minPriceInbound;
            //if (decimal.TryParse(txtMinPrice, out minPriceInbound))
            //{
            //    q = q.Where(x => x.PriceInbound >= minPriceInbound);
            //}

            //decimal maxPriceInbound;
            //if (decimal.TryParse(txtMaxPrice, out maxPriceInbound))
            //{
            //    q = q.Where(x => x.PriceInbound <= maxPriceInbound);
            //}
            if (!string.IsNullOrEmpty(Categories))
            {
                q = q.Where(x => x.Categories == Categories);
            }
            if (!string.IsNullOrEmpty(CategoryCode))
            {
                q = q.Where(x => x.CategoryCode == CategoryCode);
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
            var model = new ProductViewModel();
            model.PriceInbound = 0;
            model.PriceOutbound = 0;
            model.MinInventory = 0;
            model.MinInventoryAlarms = 0;
            model.TargetPoint = 10000;
            model.Point = 1;
            model.CycleKM = 0;
            model.CycleTime = 0;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var check_Code = ProductRepository.GetAllProductByType("product").Any(x => x.Code == model.Code);
                if (check_Code)
                {
                    ViewBag.FailedMessage = "Mã " + model.Code + " bị trùng!";
                    return View(model);
                }
                var Product = new Domain.Sale.Entities.Product();
                AutoMapper.Mapper.Map(model, Product);
                Product.IsDeleted = false;
                Product.CreatedUserId = WebSecurity.CurrentUserId;
                Product.ModifiedUserId = WebSecurity.CurrentUserId;
                Product.CreatedDate = DateTime.Now;
                Product.ModifiedDate = DateTime.Now;
                Product.Point = 1;
                if (model.PriceInbound == null)
                    Product.PriceInbound = 0;
                Product.Code = Product.Code.Trim();

                if (Request.Files["file-image"] != null)
                {
                    var file = Request.Files["file-image"];
                    if (file.ContentLength > 0)
                    {
                        string image_name = "product_" + Helpers.Common.ChuyenThanhKhongDau(Regex.Replace(Product.Code, @"\s+", "_")) + "." + file.FileName.Split('.').Last();
                        file.SaveAs(Server.MapPath("~/files/product/") + image_name);
                        Product.Image_Name = image_name;
                    }
                }

                ProductRepository.InsertProduct(Product);

                //tạo đặc tính động cho sản phẩm nếu có danh sách đặc tính động truyền vào và đặc tính đó phải có giá trị
                ObjectAttributeController.CreateOrUpdateForObject(Product.Id, model.AttributeValueList);

                if (string.IsNullOrEmpty(Request["IsPopup"]) == false)
                {
                    return RedirectToAction("Edit", new { Id = model.Id, IsPopup = Request["IsPopup"] });
                }

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }

            string errors = string.Empty;
            foreach (var modalState in ModelState.Values)
            {
                errors += modalState.Value + ": ";
                foreach (var error in modalState.Errors)
                {
                    errors += error.ErrorMessage;
                }
            }

            ViewBag.errors = errors;

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult CheckCodeExsist(int? id, string code)
        {
            code = code.Trim();
            var product = ProductRepository.GetAllProduct()
                .Where(item => item.Code == code).FirstOrDefault();
            if (product != null)
            {
                if (id == null || (id != null && product.Id != id))
                    return Content("Trùng mã sản phẩm!");
                else
                {
                    return Content("");
                }
            }
            else
            {
                return Content("");
            }
        }
        #endregion

        #region Detail
        public ActionResult Detail(int? Id)
        {
            var Product = ProductRepository.GetvwProductById(Id.Value);
            if (Product != null && Product.IsDeleted != true)
            {
                var model = new ProductViewModel();
                AutoMapper.Mapper.Map(Product, model);
                model.ListPriceLog = new List<PriceLogViewModel>();
                var priceLogList = ProductRepository.GetAllPriceLog().Where(x => x.ProductId == Product.Id).OrderByDescending(x=> x.StartDateApply);
                AutoMapper.Mapper.Map(priceLogList, model.ListPriceLog);
                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Product = ProductRepository.GetProductById(Id.Value);
            if (Product != null && Product.IsDeleted != true)
            {
                var model = new ProductViewModel();
                AutoMapper.Mapper.Map(Product, model);


                string productId = "," + model.Id + ",";
                var supplierList = SupplierRepository.GetAllSupplier().AsEnumerable().Where(item => ("," + item.ProductIdOfSupplier + ",").Contains(productId) == true).ToList();
                ViewBag.supplierList = supplierList;

                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Product = ProductRepository.GetProductById(model.Id);
                    var check_Code = ProductRepository.GetAllProductByType("product").Where(x => x.Code != Product.Code).Any(x => x.Code == model.Code);
                    if (check_Code)
                    {
                        ViewBag.FailedMessage = "Mã " + model.Code + " bị trùng!";
                        return View(model);
                    }
                    // Ghi PriceLog nếu thay đổi giá nhập và giá xuất
                    if(Product.PriceInbound != model.PriceInbound && Product.PriceOutbound != model.PriceOutbound)
                    {
                        CreateProdeLog(Product);
                    }

                    AutoMapper.Mapper.Map(model, Product);
                    Product.ModifiedUserId = WebSecurity.CurrentUserId;
                    Product.ModifiedDate = DateTime.Now;
                    Product.Point = 1;
                    if (model.PriceInbound == null)
                        Product.PriceInbound = 0;

                    if (Request.Files["file-image"] != null)
                    {
                        var file = Request.Files["file-image"];
                        if (file.ContentLength > 0)
                        {
                            FileInfo fi = new FileInfo(Server.MapPath("~/files/product/") + Product.Image_Name);
                            if (fi.Exists)
                            {
                                fi.Delete();
                            }

                            string image_name = "product_" + Helpers.Common.ChuyenThanhKhongDau(Regex.Replace(Product.Code, @"\s+", "_")) + "." + file.FileName.Split('.').Last();

                            file.SaveAs(Server.MapPath("~/files/product/") + image_name);
                            Product.Image_Name = image_name;
                        }
                    }


                    //tạo hoặc cập nhật đặc tính động cho sản phẩm nếu có danh sách đặc tính động truyền vào và đặc tính đó phải có giá trị
                    ObjectAttributeController.CreateOrUpdateForObject(Product.Id, model.AttributeValueList);

                    ProductRepository.UpdateProduct(Product);

                    if (string.IsNullOrEmpty(Request["IsPopup"]) == false)
                    {
                        return RedirectToAction("Edit", new { Id = model.Id, IsPopup = Request["IsPopup"] });
                    }

                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    return RedirectToAction("Index");
                }

                return View(model);
            }

            string errors = string.Empty;
            foreach (var modalState in ModelState.Values)
            {
                errors += modalState.Value + ": ";
                foreach (var error in modalState.Errors)
                {
                    errors += error.ErrorMessage;
                }
            }

            ViewBag.errors = errors;

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
                    var item = ProductRepository.GetProductById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        //if (item.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
                        //{
                        //    TempData["FailedMessage"] = "NotOwner";
                        //    return RedirectToAction("Index");
                        //}

                        item.IsDeleted = true;
                        ProductRepository.UpdateProduct(item);
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

        public ActionResult InfoProduct(int? Id)
        {
            var Product = ProductRepository.GetvwProductById(Id.Value);
            if (Product != null && Product.IsDeleted != true)
            {
                var model = new ProductViewModel();
                AutoMapper.Mapper.Map(Product, model);
                model.ListPriceLog = new List<PriceLogViewModel>();
                var priceLogList = ProductRepository.GetAllPriceLog().Where(x => x.ProductId == Product.Id).OrderByDescending(x => x.StartDateApply);
                AutoMapper.Mapper.Map(priceLogList, model.ListPriceLog);
                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
           
        }
        public void CreateProdeLog(Product Product)
        {
            var PriceLog = new PriceLog();
            PriceLog.IsDeleted = false;
            PriceLog.CreatedUserId = WebSecurity.CurrentUserId;
            PriceLog.ModifiedUserId = WebSecurity.CurrentUserId;
            PriceLog.CreatedDate = DateTime.Now;
            PriceLog.ModifiedDate = DateTime.Now;
            PriceLog.ProductId = Product.Id;
            PriceLog.PriceInbound = Product.PriceInbound;
            PriceLog.PriceOutbound = Product.PriceOutbound;
            PriceLog.StartDateApply = DateTime.Now;
            ProductRepository.InsertPriceLog(PriceLog);
        }

        #region  - Json -
        public JsonResult GetListJson()
        {
            var list = ProductRepository.GetAllProduct().ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
