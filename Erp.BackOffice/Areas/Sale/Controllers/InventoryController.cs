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
using Erp.Domain.Sale.Repositories;
using Erp.BackOffice.Areas.Cms.Models;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class InventoryController : Controller
    {
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IProductRepository ProductRepository;
        private readonly IInventoryRepository InventoryRepository;
        private readonly IProductInboundRepository ProductInboundRepository;
        private readonly IProductOutboundRepository ProductOutboundRepository;
        private readonly IPhysicalInventoryRepository PhysicalInventoryRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository _categoryRepository;
        public InventoryController(
            IInventoryRepository _Inventory
            , IProductRepository _Product
            , IWarehouseRepository _Warehouse
            , IProductInboundRepository _ProductInbound
            , IProductOutboundRepository _ProductOutbound
            , IPhysicalInventoryRepository _PhysicalInventory
            , IUserRepository _user
            , ICategoryRepository categorys
            )
        {
            WarehouseRepository = _Warehouse;
            ProductRepository = _Product;
            InventoryRepository = _Inventory;
            ProductInboundRepository = _ProductInbound;
            ProductOutboundRepository = _ProductOutbound;
            userRepository = _user;
            PhysicalInventoryRepository = _PhysicalInventory;
            _categoryRepository = categorys;
        }

        #region Index
        public ViewResult Index(string category, string manufacturer, string txtSearch, string txtCode, int? page, string status)
        {

            List<string> ListCheck = new List<string>();
            var status_check = Request["status_check"];
            if (!string.IsNullOrEmpty(status_check))
            {
                ListCheck = status_check.Split(',').ToList();
            }

            //List<vwProduct> listProduct = new List<vwProduct>();
            var listProduct = ProductRepository.GetAllvwProductByType("product").Where(x => x.NoInbound != true);

            if (ListCheck.Count() > 0)
            {
                listProduct = listProduct.Where(id1 => ListCheck.Contains(id1.ProductGroup));
            }
            if (string.IsNullOrEmpty(category) == false)
            {
                listProduct = listProduct.Where(x => x.CategoryCode == category);
            }
            if (string.IsNullOrEmpty(manufacturer) == false)
            {
                listProduct = listProduct.Where(x => x.Manufacturer == manufacturer);
            }
            if (string.IsNullOrEmpty(txtSearch) == false)
            {
                txtSearch = Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtSearch);
                listProduct = listProduct.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.Name).Contains(txtSearch));
            }

            if (string.IsNullOrEmpty(txtCode) == false)
            {
                txtCode = txtCode.ToLower();
                listProduct = listProduct.Where(x => x.Code.ToLower().Contains(txtCode));
            }
            if (!string.IsNullOrEmpty(status))
            {
                listProduct = listProduct.Where(x => x.StatusInventory == status);
            }

            var pager = new Pager(listProduct.Count(), page, 20);

            var viewModel = new IndexViewModel<ProductViewModel>
            {
                Items = listProduct.OrderBy(m => m.Code).Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
                .Select(item => new ProductViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    PriceInbound = item.PriceInbound,
                    Type = item.Type,
                    Unit = item.Unit,
                    ModifiedDate = item.ModifiedDate,
                    MinInventory = item.MinInventory,
                    MinInventoryAlarms = item.MinInventoryAlarms,
                    QuantityTotalInventory = item.QuantityTotalInventory,
                    CategoryCode = item.CategoryCode,
                    ProductGroup = item.ProductGroup
                }).ToList(),
                Pager = pager
            };

            List<InventoryViewModel> inventoryList = new List<InventoryViewModel>();
            foreach (var item in viewModel.Items)
            {
                List<InventoryViewModel> inventoryP = InventoryRepository.GetAllInventoryByProductId(item.Id)
                .Select(itemV => new InventoryViewModel
                {
                    ProductId = itemV.ProductId,
                    Quantity = itemV.Quantity,
                    WarehouseId = itemV.WarehouseId
                }).ToList();

                if (inventoryP.Count > 0)
                    inventoryList.AddRange(inventoryP);
            }

            List<WarehouseViewModel> warehouseList = WarehouseRepository.GetAllWarehouse()
                .Select(item => new WarehouseViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    Address = item.Address,
                    Categories = item.Categories
                }).ToList();
            if (ListCheck.Count() > 0)
            {
                warehouseList = warehouseList.Where(id1 => ListCheck.Any(id2 => id1.Categories != null && id1.Categories.Contains(id2.ToString()))).ToList();
            }
            //lấy danh sách loại kho
            var categoryList = _categoryRepository.GetCategoryByCode("Categories_product").Select(x => new CategoryViewModel
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Value = x.Value,
                OrderNo = x.OrderNo
            }).ToList();
            ViewBag.Category = categoryList.OrderBy(x => x.OrderNo).ToList();

            ViewBag.inventoryList = inventoryList;
            ViewBag.warehouseList = warehouseList;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];


            ViewBag.listcheck = ListCheck;
            return View(viewModel);
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

                var inboundDetails = ProductInboundRepository.GetAllvwProductInboundDetailByProductId(Id.Value)
                    .Where(item => item.IsArchive).OrderBy(x => x.ProductInboundId).OrderByDescending(x => x.CreatedDate);
                var outboundDetails = ProductOutboundRepository.GetAllvwProductOutboundDetailByProductId(Id.Value)
                    .Where(item => item.IsArchive)
                    .OrderBy(x => x.ProductOutboundId)
                    .OrderByDescending(x => x.CreatedDate);

                ViewBag.inboundDetails = inboundDetails;
                ViewBag.outboundDetails = outboundDetails;

                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        #endregion

        #region - Json -
        [AllowAnonymous]
        public JsonResult GetListProductJsonByWarehouseId(int? warehouseId, string manufacturer)
        {
            if (warehouseId == null && manufacturer == "")
                return Json(new List<int>(), JsonRequestBehavior.AllowGet);
            if (warehouseId != null && manufacturer == "")
            {
                var list = InventoryRepository.GetAllvwInventoryByWarehouseId(warehouseId.Value).Where(x => x.ProductType == ProductType.Product)
                .Select(item => new
                {
                    item.CategoryCode,
                    item.ProductId,
                    item.ProductCode,
                    item.ProductName,
                    item.Quantity,
                    item.ProductManufacturer
                }).OrderBy(item => item.CategoryCode).ThenBy(item => item.ProductCode).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else if (warehouseId == null && manufacturer != "")
            {
                var list = InventoryRepository.GetAllvwInventoryByProductManufacturer(manufacturer).Where(x => x.ProductType == ProductType.Product)
                .Select(item => new
                {
                    item.CategoryCode,
                    item.ProductId,
                    item.ProductCode,
                    item.ProductName,
                    item.Quantity,
                    item.ProductManufacturer
                }).OrderBy(item => item.CategoryCode).ThenBy(item => item.ProductCode).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var list = InventoryRepository.GetAllvwInventoryByWarehouseIdAndProductManufacturer(warehouseId.Value, manufacturer).Where(x => x.ProductType == ProductType.Product)
                .Select(item => new
                {
                    item.CategoryCode,
                    item.ProductId,
                    item.ProductCode,
                    item.ProductName,
                    item.Quantity,
                    item.ProductManufacturer
                }).OrderBy(item => item.CategoryCode).ThenBy(item => item.ProductCode).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        public JsonResult GetListCardJsonByWarehouseId(int? warehouseId)
        {
            if (warehouseId != null)
            {
                var list = InventoryRepository.GetAllvwInventoryByWarehouseId(warehouseId.Value).Where(x => x.ProductType == ProductType.Card)
                .Select(item => new
                {
                    item.CategoryCode,
                    item.ProductId,
                    item.ProductCode,
                    item.ProductName,
                    item.Quantity,
                    item.ProductManufacturer
                }).OrderBy(item => item.CategoryCode).ThenBy(item => item.ProductCode).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new List<int>(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetListGiftJsonByWarehouseId(int? warehouseId)
        {
            if (warehouseId != null)
            {
                var list = InventoryRepository.GetAllvwInventoryByWarehouseId(warehouseId.Value).Where(x => x.ProductType == ProductType.Gift)
                .Select(item => new
                {
                    item.CategoryCode,
                    item.ProductId,
                    item.ProductCode,
                    item.ProductName,
                    item.Quantity,
                    item.ProductManufacturer
                }).OrderBy(item => item.CategoryCode).ThenBy(item => item.ProductCode).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new List<int>(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Check or Update
        public static string Check(string productName, int productId, int warehouseId, int quantityIn, int quantityOut)
        {
            return Update(productName, productId, warehouseId, quantityIn, quantityOut, false);
        }

        public static string Update(string productName, int productId, int warehouseId, int quantityIn, int quantityOut, bool isArchive = true)
        {
            string error = "";
            IProductInboundRepository productInboundRepository = DependencyResolver.Current.GetService<IProductInboundRepository>();
            IProductOutboundRepository productOutboundRepository = DependencyResolver.Current.GetService<IProductOutboundRepository>();
            IInventoryRepository inventoryRepository = DependencyResolver.Current.GetService<IInventoryRepository>();

            var inbound = productInboundRepository.GetAllvwProductInboundDetail()
                    .Where(x => x.IsArchive && x.ProductId == productId && x.WarehouseDestinationId == warehouseId);

            var outbound = productOutboundRepository.GetAllvwProductOutboundDetail()
                .Where(x => x.IsArchive && x.ProductId == productId && x.WarehouseSourceId == warehouseId);

            var inventory = (inbound.Count() == 0 ? 0 : inbound.Sum(x => x.Quantity)) - (outbound.Count() == 0 ? 0 : outbound.Sum(x => x.Quantity)) + quantityIn - quantityOut;

            var inventoryCurrent = inventoryRepository.GetInventoryByWarehouseIdAndProductId(warehouseId, productId);

            //Sau khi thay đổi dữ liệu phải đảm bảo tồn kho >= 0
            if (inventory >= 0)
            {
                if (isArchive)
                {
                    if (inventoryCurrent != null)
                    {
                        if (inventoryCurrent.Quantity != inventory)
                        {
                            inventoryCurrent.Quantity = inventory;
                            inventoryRepository.UpdateInventory(inventoryCurrent);
                        }
                    }
                    else
                    {
                        inventoryCurrent = new Inventory();
                        inventoryCurrent.IsDeleted = false;
                        inventoryCurrent.CreatedUserId = WebSecurity.CurrentUserId;
                        inventoryCurrent.ModifiedUserId = WebSecurity.CurrentUserId;
                        inventoryCurrent.CreatedDate = DateTime.Now;
                        inventoryCurrent.ModifiedDate = DateTime.Now;
                        inventoryCurrent.WarehouseId = warehouseId;
                        inventoryCurrent.ProductId = productId;
                        inventoryCurrent.Quantity = inventory;
                        inventoryRepository.InsertInventory(inventoryCurrent);
                    }
                }
            }
            else
            {
                error += string.Format("<br/>Dữ liệu tồn kho của sản phẩm \"{0}\" là {1}: không hợp lệ!", productName, inventory);
            }

            return error;
        }
        #endregion

        #region UpdateAll
        public ActionResult UpdateAll(string url)
        {
            string rs = "";
            var inventoryList = InventoryRepository.GetAllInventory().ToList();
            foreach (var item in inventoryList)
            {
                var warehouseId = item.WarehouseId.Value;
                var productId = item.ProductId.Value;
                var inbound = ProductInboundRepository.GetAllvwProductInboundDetail()
                    .Where(x => x.IsArchive && x.ProductId == productId && x.WarehouseDestinationId == warehouseId)
                    .Sum(x => x.Quantity);

                var outbound = ProductOutboundRepository.GetAllvwProductOutboundDetail()
                    .Where(x => x.IsArchive && x.ProductId == productId && x.WarehouseSourceId == warehouseId)
                    .Sum(x => x.Quantity);

                var inventory = (inbound == null ? 0 : inbound) - (outbound == null ? 0 : outbound);

                if (item.Quantity != inventory)
                {
                    rs += "<br/>" + item.ProductId + " | " + item.Quantity + " => " + inventory;
                    item.Quantity = inventory;
                    InventoryRepository.UpdateInventory(item);
                }
            }

            TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess + rs;
            return Redirect(url);
        }

        #endregion
        #region ListByCard
        public ViewResult ListByCard(string txtSearch, string txtCode, int? page, string status)
        {

            //List<vwProduct> listProduct = new List<vwProduct>();
            var listProduct = ProductRepository.GetAllvwProductByType("card").Where(x => x.NoInbound != true);

            if (string.IsNullOrEmpty(txtSearch) == false)
            {
                txtSearch = Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtSearch);
                listProduct = listProduct.Where(x => x.Name.Contains(txtSearch));
            }

            if (string.IsNullOrEmpty(txtCode) == false)
            {
                txtCode = txtCode.ToLower();
                listProduct = listProduct.Where(x => x.Code.ToLower().Contains(txtCode));
            }
            if (!string.IsNullOrEmpty(status))
            {
                listProduct = listProduct.Where(x => x.StatusInventory == status);
            }

            var pager = new Pager(listProduct.Count(), page, 20);

            var viewModel = new IndexViewModel<ProductViewModel>
            {
                Items = listProduct.OrderBy(m => m.Code).Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
                .Select(item => new ProductViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    PriceInbound = item.PriceInbound,
                    Type = item.Type,
                    Unit = item.Unit,
                    ModifiedDate = item.ModifiedDate,
                    MinInventory = item.MinInventory,
                    MinInventoryAlarms = item.MinInventoryAlarms,
                    QuantityTotalInventory = item.QuantityTotalInventory,
                    CategoryCode = item.CategoryCode,
                    ProductGroup = item.ProductGroup
                }).ToList(),
                Pager = pager
            };

            List<InventoryViewModel> inventoryList = new List<InventoryViewModel>();
            foreach (var item in viewModel.Items)
            {
                List<InventoryViewModel> inventoryP = InventoryRepository.GetAllInventoryByProductId(item.Id)
                .Select(itemV => new InventoryViewModel
                {
                    ProductId = itemV.ProductId,
                    Quantity = itemV.Quantity,
                    WarehouseId = itemV.WarehouseId
                }).ToList();

                if (inventoryP.Count > 0)
                    inventoryList.AddRange(inventoryP);
            }

            List<WarehouseViewModel> warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.Categories.Contains("CARD"))
                .Select(item => new WarehouseViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    Address = item.Address,
                    Categories = item.Categories
                }).ToList();
            //lấy danh sách loại kho
            //var categoryList = _categoryRepository.GetCategoryByCode("Categories_product").Select(x => new CategoryViewModel
            //{
            //    Code = x.Code,
            //    Description = x.Description,
            //    Id = x.Id,
            //    Name = x.Name,
            //    Value = x.Value,
            //    OrderNo = x.OrderNo
            //}).ToList();
            //ViewBag.Category = categoryList.OrderBy(x => x.OrderNo).ToList();

            ViewBag.inventoryList = inventoryList;
            ViewBag.warehouseList = warehouseList;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            return View(viewModel);
        }
        #endregion

        #region ListByGift
        public ViewResult ListByGift(string txtSearch, string txtCode, int? page, string status)
        {

            //List<vwProduct> listProduct = new List<vwProduct>();
            var listProduct = ProductRepository.GetAllvwProductByType("gift").Where(x => x.NoInbound != true);

            if (string.IsNullOrEmpty(txtSearch) == false)
            {
                txtSearch = Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtSearch);
                listProduct = listProduct.Where(x => x.Name.Contains(txtSearch));
            }

            if (string.IsNullOrEmpty(txtCode) == false)
            {
                txtCode = txtCode.ToLower();
                listProduct = listProduct.Where(x => x.Code.ToLower().Contains(txtCode));
            }
            if (!string.IsNullOrEmpty(status))
            {
                listProduct = listProduct.Where(x => x.StatusInventory == status);
            }

            var pager = new Pager(listProduct.Count(), page, 20);

            var viewModel = new IndexViewModel<ProductViewModel>
            {
                Items = listProduct.OrderBy(m => m.Code).Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
                .Select(item => new ProductViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    PriceInbound = item.PriceInbound,
                    Type = item.Type,
                    Unit = item.Unit,
                    ModifiedDate = item.ModifiedDate,
                    MinInventory = item.MinInventory,
                    MinInventoryAlarms = item.MinInventoryAlarms,
                    QuantityTotalInventory = item.QuantityTotalInventory,
                    CategoryCode = item.CategoryCode,
                    ProductGroup = item.ProductGroup
                }).ToList(),
                Pager = pager
            };

            List<InventoryViewModel> inventoryList = new List<InventoryViewModel>();
            foreach (var item in viewModel.Items)
            {
                List<InventoryViewModel> inventoryP = InventoryRepository.GetAllInventoryByProductId(item.Id)
                .Select(itemV => new InventoryViewModel
                {
                    ProductId = itemV.ProductId,
                    Quantity = itemV.Quantity,
                    WarehouseId = itemV.WarehouseId
                }).ToList();

                if (inventoryP.Count > 0)
                    inventoryList.AddRange(inventoryP);
            }

            List<WarehouseViewModel> warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.Categories.Contains("GIFT"))
                .Select(item => new WarehouseViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    Address = item.Address,
                    Categories = item.Categories
                }).ToList();
            //lấy danh sách loại kho
            //var categoryList = _categoryRepository.GetCategoryByCode("Categories_product").Select(x => new CategoryViewModel
            //{
            //    Code = x.Code,
            //    Description = x.Description,
            //    Id = x.Id,
            //    Name = x.Name,
            //    Value = x.Value,
            //    OrderNo = x.OrderNo
            //}).ToList();
            //ViewBag.Category = categoryList.OrderBy(x => x.OrderNo).ToList();

            ViewBag.inventoryList = inventoryList;
            ViewBag.warehouseList = warehouseList;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            return View(viewModel);
        }
        #endregion
    }
}
