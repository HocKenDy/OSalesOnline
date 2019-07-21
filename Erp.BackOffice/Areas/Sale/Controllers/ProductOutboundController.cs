using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Sale.Repositories;
using Erp.Domain.Account.Interfaces;
using Erp.Domain.Account.Repositories;
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
using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using Erp.BackOffice.Account.Controllers;
using Erp.BackOffice.Account.Models;
using System.Web;
using Erp.BackOffice.Crm.Controllers;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class ProductOutboundController : Controller
    {
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IWarehouseLocationItemRepository WarehouseLocationItemRepository;
        private readonly IProductInvoiceRepository ProductInvoiceRepository;
        private readonly IPurchaseOrderRepository PurchaseOrderRepository;
        private readonly IInventoryRepository InventoryRepository;
        private readonly IProductRepository ProductRepository;
        private readonly IProductOutboundRepository ProductOutboundRepository;
        private readonly IProductInboundRepository ProductInboundRepository;
        private readonly IUserRepository userRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;
        private readonly IQueryHelper QueryHelper;
        private readonly ICustomerRepository customerRepository;
        private readonly ISettingRepository settingRepository;
        public ProductOutboundController(
             IWarehouseRepository _Warehouse
            , IWarehouseLocationItemRepository _WarehouseLocationItem
            , IInventoryRepository _Inventory
            , IProductInvoiceRepository _ProductInvoice
            , IPurchaseOrderRepository _PurchaseOrder
            , IProductRepository _Product
            , IProductOutboundRepository _ProductOutbound
            , IProductInboundRepository _ProductInbound
            , IUserRepository _user
            , IQueryHelper _QueryHelper
            , ITemplatePrintRepository _templatePrint
            , ICustomerRepository customer
             , ISettingRepository _Setting
            )
        {
            WarehouseRepository = _Warehouse;
            WarehouseLocationItemRepository = _WarehouseLocationItem;
            InventoryRepository = _Inventory;
            ProductInvoiceRepository = _ProductInvoice;
            PurchaseOrderRepository = _PurchaseOrder;
            ProductRepository = _Product;
            ProductOutboundRepository = _ProductOutbound;
            ProductInboundRepository = _ProductInbound;
            userRepository = _user;
            QueryHelper = _QueryHelper;
            templatePrintRepository = _templatePrint;
            customerRepository = customer;
            settingRepository = _Setting;
        }

        #region Index
        public ViewResult Index(string txtCode, string txtMinAmount, string txtMaxAmount, string txtWarehouseDestination, int? warehouseSourceId, string startDate, string endDate)
        {
            IEnumerable<ProductOutboundViewModel> q = ProductOutboundRepository.GetAllvwProductOutbound()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Type != ProductOutboundType.Card)
                .Select(item => new ProductOutboundViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    TotalAmount = item.TotalAmount,
                    InvoiceCode = item.InvoiceCode,
                    PurchaseOrderCode = item.PurchaseOrderCode,
                    WarehouseDestinationName = item.WarehouseDestinationName,
                    WarehouseSourceName = item.WarehouseSourceName,
                    CustomerName = item.CustomerName,
                    IsArchive = item.IsArchive,
                    WarehouseSourceId = item.WarehouseSourceId,
                    WarehouseDestinationId = item.WarehouseDestinationId,
                }).OrderByDescending(m => m.ModifiedDate);

            ////Tìm những phiếu xuất có chứa mã sp
            //if (!string.IsNullOrEmpty(txtProductCode))
            //{
            //    txtProductCode = txtProductCode.Trim();
            //    var productListId = ProductRepository.GetAllvwProduct()
            //        .Where(item => item.Code == txtProductCode).Select(item => item.Id).ToList();

            //    if (productListId.Count > 0)
            //    {
            //        List<int> listProductOutboundId = new List<int>();
            //        foreach (var id in productListId)
            //        {
            //            var list = ProductOutboundRepository.GetAllvwProductOutboundDetailByProductId(id)
            //                .Select(item => item.ProductOutboundId.Value).Distinct().ToList();

            //            listProductOutboundId.AddRange(list);
            //        }

            //        q = q.Where(item => listProductOutboundId.Contains(item.Id));
            //    }
            //}

            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => x.Code.ToLowerOrEmpty().Contains(txtCode));
            }

            if (warehouseSourceId != null)
            {
                q = q.Where(x => x.WarehouseSourceId == warehouseSourceId);
            }

            // lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
                }
            }

            decimal minAmount;
            if (decimal.TryParse(txtMinAmount, out minAmount))
            {
                q = q.Where(x => x.TotalAmount >= minAmount);
            }

            decimal maxAmount;
            if (decimal.TryParse(txtMaxAmount, out maxAmount))
            {
                if (maxAmount > 0)
                {
                    q = q.Where(x => x.TotalAmount <= maxAmount);
                }
            }

            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x=> x.BranchId == Helpers.Common.CurrentUser.BranchId).AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.warehouseList = warehouseList;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        public ViewResult ListForCard(string txtCode, int? warehouseSourceId, string startDate, string endDate)
        {
            IEnumerable<ProductOutboundViewModel> q = ProductOutboundRepository.GetAllvwProductOutbound()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Type == ProductOutboundType.Card)
                .Select(item => new ProductOutboundViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    TotalAmount = item.TotalAmount,
                    InvoiceCode = item.InvoiceCode,
                    PurchaseOrderCode = item.PurchaseOrderCode,
                    WarehouseDestinationName = item.WarehouseDestinationName,
                    WarehouseSourceName = item.WarehouseSourceName,
                    CustomerName = item.CustomerName,
                    IsArchive = item.IsArchive,
                    WarehouseSourceId = item.WarehouseSourceId,
                    WarehouseDestinationId = item.WarehouseDestinationId,
                }).OrderByDescending(m => m.ModifiedDate);


            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => x.Code.ToLowerOrEmpty().Contains(txtCode));
            }

            if (warehouseSourceId != null)
            {
                q = q.Where(x => x.WarehouseSourceId == warehouseSourceId);
            }

            // lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
                }
            }
            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Categories.Contains("CARD")).AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.warehouseList = warehouseList;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }

        public ViewResult ListForGift(string txtCode, int? warehouseSourceId, string startDate, string endDate)
        {
            IEnumerable<ProductOutboundViewModel> q = ProductOutboundRepository.GetAllvwProductOutbound()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Type == ProductOutboundType.Gift)
                .Select(item => new ProductOutboundViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    TotalAmount = item.TotalAmount,
                    InvoiceCode = item.InvoiceCode,
                    PurchaseOrderCode = item.PurchaseOrderCode,
                    WarehouseDestinationName = item.WarehouseDestinationName,
                    WarehouseSourceName = item.WarehouseSourceName,
                    CustomerName = item.CustomerName,
                    IsArchive = item.IsArchive,
                    WarehouseSourceId = item.WarehouseSourceId,
                    WarehouseDestinationId = item.WarehouseDestinationId,
                }).OrderByDescending(m => m.ModifiedDate);


            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => x.Code.ToLowerOrEmpty().Contains(txtCode));
            }

            if (warehouseSourceId != null)
            {
                q = q.Where(x => x.WarehouseSourceId == warehouseSourceId);
            }

            // lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
                }
            }
            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Categories.Contains("CARD")).AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.warehouseList = warehouseList;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ActionResult Create(int? InvoiceId, int? WarehouseSourceId)
        {
            var model = new ProductOutboundViewModel();
            model.CreatedDate = DateTime.Now;
            model.DetailList = new List<ProductOutboundDetailViewModel>();
            model.InvoiceId = InvoiceId;
            model.WarehouseSourceId = WarehouseSourceId;

            model.SelectList_WarehouseSource = WarehouseRepository.GetAllWarehouse()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId)
                .Select(item => new
                {
                    Id = item.Id,
                    Name = item.Name
                }).ToList()
                .Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id + ""
                }).AsEnumerable();

            //Load phiếu xuất theo đơn hàng & Kho hàng
            if (InvoiceId != null)
            {
                var productInvoice = ProductInvoiceRepository.GetvwProductInvoiceById(InvoiceId.Value);
                if (productInvoice != null && WarehouseSourceId != null)
                {
                    //Kiểm tra xem nếu có xuất kho ghi sổ rồi thì return
                    var productOutbound = ProductOutboundRepository.GetAllProductOutbound()
                            .Where(item => item.InvoiceId == productInvoice.Id).FirstOrDefault();
                    if (productOutbound != null && productOutbound.IsArchive)
                        return Content("Đã xuất kho cho đơn hàng!");

                    model.InvoiceCode = productInvoice.Code;

                    //Lấy danh sách chi tiết đơn hàng
                    var listProductInvoiceDetail = ProductInvoiceRepository.GetAllvwInvoiceDetailsByInvoiceId(productInvoice.Id).Where(x=> x.ProductType == ProductType.Product)
                        .Select(item => new
                        {
                            ProductId = item.ProductId,
                            Price = item.Price,
                            ProductCode = item.ProductCode,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity
                        }).ToList();

                    //Tạo danh sách chi tiết phiếu xuất tương ứng                    
                    foreach (var item in listProductInvoiceDetail)
                    {
                        var productOutboundDetailViewModel = model.DetailList.Where(i => i.ProductId == item.ProductId).FirstOrDefault();
                        if (productOutboundDetailViewModel == null)
                        {
                            productOutboundDetailViewModel = new ProductOutboundDetailViewModel();
                            productOutboundDetailViewModel.ProductId = item.ProductId;
                            productOutboundDetailViewModel.ProductCode = item.ProductCode;
                            productOutboundDetailViewModel.ProductName = item.ProductName;
                            productOutboundDetailViewModel.Quantity = item.Quantity;
                            productOutboundDetailViewModel.Price = item.Price;
                            model.DetailList.Add(productOutboundDetailViewModel);
                        }
                        else
                        {
                            productOutboundDetailViewModel.Quantity += item.Quantity;
                        }
                    }

                    //Lấy lô/date. Mặc định lấy theo phương pháp nhập trước xuất trước
                    foreach (var item in model.DetailList)
                    {
                        item.ListWarehouseLocationItemViewModel = new List<WarehouseLocationItemViewModel>();
                        var listLocationItemExits = WarehouseLocationItemRepository.GetAllWarehouseLocationItem()
                            .Where(q => q.ProductId == item.ProductId && q.WarehouseId == WarehouseSourceId && q.IsOut == false)
                            .OrderBy(x => x.ExpiryDate)
                            .Take(item.Quantity.Value);

                        AutoMapper.Mapper.Map(listLocationItemExits, item.ListWarehouseLocationItemViewModel);
                    }

                    model.TotalAmount = model.DetailList.Sum(item => item.Quantity * item.Price);
                }
            }
            model.Code  = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductOutbound");
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProductOutboundViewModel model)
        {
            ProductOutbound productOutbound = null;
            if (model.InvoiceId != null)
            {
                productOutbound = ProductOutboundRepository.GetAllProductOutbound()
                    .Where(item => item.InvoiceId == model.InvoiceId).FirstOrDefault();

                if (productOutbound != null && productOutbound.IsArchive)
                    return Content("Phiếu xuất kho cho đơn hàng này đã được ghi sổ!");
            }

            if (ModelState.IsValid)
            {
                //Lấy danh sách inventory các sản phẩm đang có trong phiếu xuất của kho nguồn nếu có
                var list_InventoryWHSource = InventoryRepository.GetAllInventoryByWarehouseId(model.WarehouseSourceId.Value).ToList();

                //Duyệt qua hết chi tiết phiếu xuất, kiểm tra số lượng tồn kho
                foreach (var item in model.DetailList)
                {
                    //Kiểm tra tình trạng đảm bảo SP và SL của SP trong kho đáp ứng cho đơn hàng. Xảy ra trong tình huống client có tình can thiệp làm sai các thông số đầu vào
                    var product = list_InventoryWHSource.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                    if (product == null || product.Quantity < item.Quantity)
                    {
                        TempData[Globals.FailedMessageKey] = item.ProductCode + " - " + item.ProductName + ": không đáp ứng đủ số lượng";
                        if (model.InvoiceId != null)
                            return RedirectToAction("Create", new { InvoiceId = model.InvoiceId, WarehouseSourceId = model.WarehouseSourceId });
                        else
                            return RedirectToAction("Create");
                    }
                }

                //Nếu đã tạo phiếu xuất rồi thì cập nhật
                if (productOutbound != null)//Cập nhật
                {
                    productOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
                    productOutbound.ModifiedDate = DateTime.Now;
                    productOutbound.TotalAmount = model.DetailList.Sum(x => x.Price * x.Quantity);
                    ProductOutboundRepository.UpdateProductOutbound(productOutbound);

                    //Xóa chi tiết cũ và thêm chi tiết mới
                    var listProductOutboundDetail_old = ProductOutboundRepository.GetAllProductOutboundDetailByOutboundId(productOutbound.Id)
                        .Select(item => item.Id).ToList();
                    foreach (var id in listProductOutboundDetail_old)
                    {
                        ProductOutboundRepository.DeleteProductOutboundDetail(id);
                    }

                    foreach (var item in model.DetailList)
                    {
                        ProductOutboundDetail productOutboundDetail = new ProductOutboundDetail();
                        AutoMapper.Mapper.Map(item, productOutboundDetail);
                        productOutboundDetail.ProductOutboundId = productOutbound.Id;
                        productOutboundDetail.IsDeleted = false;
                        productOutboundDetail.CreatedUserId = WebSecurity.CurrentUserId;
                        productOutboundDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                        productOutboundDetail.CreatedDate = DateTime.Now;
                        productOutboundDetail.ModifiedDate = DateTime.Now;
                        ProductOutboundRepository.InsertProductOutboundDetail(productOutboundDetail);
                    }
                }
                else//Thêm mới
                {
                    productOutbound = new Domain.Sale.Entities.ProductOutbound();
                    AutoMapper.Mapper.Map(model, productOutbound);
                    productOutbound.IsDeleted = false;
                    productOutbound.CreatedUserId = WebSecurity.CurrentUserId;
                    productOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
                    productOutbound.CreatedDate = DateTime.Now;
                    productOutbound.ModifiedDate = DateTime.Now;
                    productOutbound.Type = ProductOutboundType.Invoice;
                    productOutbound.BranchId = Helpers.Common.CurrentUser.BranchId;
                    productOutbound.TotalAmount = model.DetailList.Sum(x => x.Price * x.Quantity);
                    ProductOutboundRepository.InsertProductOutbound(productOutbound);
                    ////Cập nhật lại mã xuất kho
                    productOutbound.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductOutbound");
                    ProductOutboundRepository.UpdateProductOutbound(productOutbound);
                    Erp.BackOffice.Helpers.Common.SetOrderNo("ProductOutbound");
                    foreach (var item in model.DetailList)
                    {
                        ProductOutboundDetail productOutboundDetail = new ProductOutboundDetail();
                        AutoMapper.Mapper.Map(item, productOutboundDetail);
                        productOutboundDetail.IsDeleted = false;
                        productOutboundDetail.CreatedUserId = WebSecurity.CurrentUserId;
                        productOutboundDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                        productOutboundDetail.CreatedDate = DateTime.Now;
                        productOutboundDetail.ModifiedDate = DateTime.Now;
                        productOutboundDetail.ProductOutboundId = productOutbound.Id;
                        ProductOutboundRepository.InsertProductOutboundDetail(productOutboundDetail);
                    }

                    //Thêm vào quản lý chứng từ
                    TransactionController.Create(new TransactionViewModel
                    {
                        TransactionModule = "ProductOutbound",
                        TransactionCode = productOutbound.Code,
                        TransactionName = "Xuất kho"
                    });

                    //Cập nhật hóa đơn là đang xử lý
                    var productInvoice = ProductInvoiceRepository.GetProductInvoiceById(productOutbound.InvoiceId.Value);
                    productInvoice.Status = Wording.OrderStatus_inprogress;
                    productInvoice.ModifiedDate = DateTime.Now;
                    productInvoice.ModifiedUserId = WebSecurity.CurrentUserId;
                    ProductInvoiceRepository.UpdateProductInvoice(productInvoice);

                    //Thêm chứng từ liên quan
                    TransactionController.CreateRelationship(new TransactionRelationshipViewModel
                    {
                        TransactionA = productOutbound.Code,
                        TransactionB = productInvoice.Code
                    });
                }

                //Ghi sổ chứng từ
                Archive(ProductOutboundRepository, productOutbound, TempData);

                return RedirectToAction("Detail", new { Id = productOutbound.Id });
            }

            return RedirectToAction("Index", new { Id = productOutbound.Id });
        }
        #endregion

        public ActionResult Detail(int? Id, string TransactionCode)
        {
            var ProductOutbound = new vwProductOutbound();
            if (Id != null)
                ProductOutbound = ProductOutboundRepository.GetFullvwProductOutboundById(Id.Value);

            if (!string.IsNullOrEmpty(TransactionCode))
                ProductOutbound = ProductOutboundRepository.GetFullvwProductOutboundByCode(TransactionCode);

            if (ProductOutbound != null)
            {
                var model = new ProductOutboundViewModel();
                AutoMapper.Mapper.Map(ProductOutbound, model);
                //Kiểm tra cho phép sửa chứng từ này hay không
                model.AllowEdit = Helpers.Common.KiemTraNgaySuaChungTu(model.CreatedDate.Value);
                model.DetailList = ProductOutboundRepository.GetAllvwProductOutboundDetailByOutboundId(model.Id).AsEnumerable()
                    .Select(x => new ProductOutboundDetailViewModel
                    {
                        ProductId = x.ProductId,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        Unit = x.Unit,
                        ProductName = x.ProductName,
                        ProductCode = x.ProductCode
                    }).ToList();
                model.ModifiedUserName = userRepository.GetUserById(model.ModifiedUserId.Value).FullName;

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.FailedMessage = TempData["FailedMessage"];

                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Archive(int Id)
        {
            if (Request["Submit"] == "Save")
            {
                var productOutbound = ProductOutboundRepository.GetProductOutboundById(Id);

                //Kiểm tra cho phép sửa chứng từ này hay không
                if (Helpers.Common.KiemTraNgaySuaChungTu(productOutbound.CreatedDate.Value) == false)
                {
                    return RedirectToAction("Detail", new { Id = productOutbound.Id });
                }
                Archive(ProductOutboundRepository, productOutbound, TempData);
            }

            return RedirectToAction("Detail", new { Id = Id });
        }

        public static void Archive(IProductOutboundRepository productOutboundRepository,
   ProductOutbound productOutbound,
   TempDataDictionary TempData)
        {
            //Cập nhật lại tồn kho cho những sp trong phiếu này
            var detailList = productOutboundRepository.GetAllvwProductOutboundDetailByOutboundId(productOutbound.Id)
                .Select(item => new
                {
                    ProductName = item.ProductCode + " - " + item.ProductName,
                    ProductId = item.ProductId.Value,
                    Quantity = item.Quantity
                }).ToList();


            //Kiểm tra dữ liệu sau khi bỏ ghi sổ có hợp lệ không
            string check = "";
            foreach (var item in detailList)
            {
                var error = InventoryController.Check(item.ProductName, item.ProductId, productOutbound.WarehouseSourceId.Value, 0, item.Quantity);
                check += error;
            }

            if (string.IsNullOrEmpty(check))
            {
                //Khi đã hợp lệ thì mới update
                foreach (var item in detailList)
                {
                    InventoryController.Update(item.ProductName, item.ProductId, productOutbound.WarehouseSourceId.Value, 0, item.Quantity);
                }

                productOutbound.IsArchive = true;
                productOutboundRepository.UpdateProductOutbound(productOutbound);

                TempData[Globals.SuccessMessageKey] += App_GlobalResources.Wording.ArchiveSuccess;
            }
            else
            {
                TempData[Globals.FailedMessageKey] += App_GlobalResources.Wording.ArchiveFail + check;
            }
        }

        [HttpPost]
        public ActionResult UnArchive(int Id)
        {
            if (Request["Submit"] == "Save")
            {
                var productOutbound = ProductOutboundRepository.GetProductOutboundById(Id);

                //Kiểm tra cho phép sửa chứng từ này hay không
                if (Helpers.Common.KiemTraNgaySuaChungTu(productOutbound.CreatedDate.Value) == false)
                {
                    return RedirectToAction("Detail", new { Id = productOutbound.Id });
                }
                unArchiveFromInvoice(ProductOutboundRepository, productOutbound, TempData);
            }
            return RedirectToAction("Detail", new { Id = Id });
        }

        public static void unArchiveFromInvoice(IProductOutboundRepository productOutboundRepository,
    ProductOutbound productOutbound,
    TempDataDictionary TempData)
        {

            //Cập nhật lại tồn kho cho những sp trong phiếu này
            var detailList = productOutboundRepository.GetAllvwProductOutboundDetailByOutboundId(productOutbound.Id)
                .Select(item => new
                {
                    ProductName = item.ProductCode + " - " + item.ProductName,
                    ProductId = item.ProductId.Value,
                    Quantity = item.Quantity
                }).ToList();
            //Kiểm tra dữ liệu sau khi bỏ ghi sổ có hợp lệ không
            string check = "";
            foreach (var item in detailList)
            {
                var error = InventoryController.Check(item.ProductName, item.ProductId, productOutbound.WarehouseSourceId.Value, item.Quantity, 0);
                check += error;
            }

            if (string.IsNullOrEmpty(check))
            {
                //Khi đã hợp lệ thì mới update
                foreach (var item in detailList)
                {
                    InventoryController.Update(item.ProductName, item.ProductId, productOutbound.WarehouseSourceId.Value, item.Quantity, 0);
                }

                productOutbound.IsArchive = false;
                productOutboundRepository.UpdateProductOutbound(productOutbound);
                TempData[Globals.SuccessMessageKey] = "Đã bỏ ghi sổ";
            }
            else
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.ArchiveFail + check;
            }
        }


        #region Delete
        [HttpPost]
        public ActionResult Delete(int Id, string CancelReason)
        {
            try
            {
                if (Request["Submit"] == "Delete")
                {
                    var productOutbound = ProductOutboundRepository.GetProductOutboundById(Id);

                    //Kiểm tra cho phép sửa chứng từ này hay không
                    if (Helpers.Common.KiemTraNgaySuaChungTu(productOutbound.CreatedDate.Value) == false)
                    {
                        TempData[Globals.FailedMessageKey] = "Qúa hạn sửa chứng từ";
                        return RedirectToAction("Detail", new { Id = productOutbound.Id });
                    }
                    else
                    {
                        productOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
                        productOutbound.ModifiedDate = DateTime.Now;
                        productOutbound.IsDeleted = true;
                        productOutbound.CancelReason = CancelReason;

                        ProductOutboundRepository.UpdateProductOutbound(productOutbound);

                        TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                        return RedirectToAction("Detail", new { Id = productOutbound.Id });
                    }
                }
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Json
        public JsonResult EditInline(int? Id, string fieldName, string value)
        {
            Dictionary<string, object> field_value = new Dictionary<string, object>();
            field_value.Add(fieldName, value);
            field_value.Add("ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
            field_value.Add("ModifiedUserId", WebSecurity.CurrentUserId);

            var flag = QueryHelper.UpdateFields("Sale_ProductOutbound", field_value, Id.Value);
            if (flag == true)
                return Json(new { status = "success", id = Id, fieldName = fieldName, value = value }, JsonRequestBehavior.AllowGet);

            return Json(new { status = "error", id = Id, fieldName = fieldName, value = value }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWarehouseLocationItem(int? warehouseId, int? productId, string serialNumber)
        {
            if (warehouseId == null || productId == null || string.IsNullOrEmpty(serialNumber) == true)
                return Json(new WarehouseLocationItemViewModel(), JsonRequestBehavior.AllowGet);

            var item = WarehouseLocationItemRepository.GetWarehouseLocationItemBySerialNumber(warehouseId.Value, productId.Value, serialNumber);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Print(int? Id, bool ExportExcel = false)
        {
            //lấy phiếu xuất kho
            var ProductOutbound = ProductOutboundRepository.GetvwProductOutboundById(Id.Value);
            //lấy hóa đơn.
            var productInvoice = ProductInvoiceRepository.GetvwProductInvoiceByCode(ProductOutbound.InvoiceCode);

            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            //string vat = Erp.BackOffice.Helpers.Common.GetSetting("VAT");
            //decimal totalVAT = ProductOutbound.TotalAmount.Value * Convert.ToInt32(vat) / 100;
            //var total = ProductOutbound.TotalAmount.Value + totalVAT;

            if (ProductOutbound != null && ProductOutbound.IsDeleted != true)
            {
                var model = new TemplatePrintViewModel();

                //lấy danh sách sản phẩm xuất kho
                var outboundDetails = ProductOutboundRepository.GetAllProductOutboundDetailByOutboundId(Id.Value).AsEnumerable()
                        .Select(x => new ProductOutboundDetailViewModel
                        {
                            Id = x.Id,
                            Price = x.Price,
                            ProductId = x.ProductId,
                            ProductName = ProductRepository.GetProductById(x.ProductId.Value).Name,
                            ProductCode = ProductRepository.GetProductById(x.ProductId.Value).Code,
                            ProductOutboundId = x.ProductOutboundId,
                            Quantity = x.Quantity,
                            Unit = x.Unit,
                        }).ToList();
                foreach (var item in outboundDetails)
                {
                    if (ProductOutbound.InvoiceId != null)
                    {
                        var d = ProductInvoiceRepository.GetAllvwInvoiceDetailsByInvoiceId(ProductOutbound.InvoiceId.Value).Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                        item.DisCount = d.DisCount.HasValue ? d.DisCount.Value : 0;
                        item.DisCountAmount = d.DisCountAmount.HasValue ? d.DisCountAmount : 0;
                    }
                    else
                    {
                        item.DisCount = 0;
                        item.DisCountAmount = 0;
                    }

                    var locationItem = WarehouseLocationItemRepository.GetAllLocationItem().Where(x => x.ProductOutboundDetailId == item.Id && x.ProductId == item.ProductId).OrderBy(x => x.ExpiryDate).FirstOrDefault();
                    if (locationItem != null)
                    {
                        item.ExpiryDate = locationItem.ExpiryDate;
                        item.LoCode = locationItem.LoCode;
                    }
                }

                //tạo dòng của table html danh sách sản phẩm.
                var ListRow = "";
                int tong_tien = 0;
                int da_thanh_toan = 0;
                int con_lai = 0;
                decimal tongTien = 0;

                foreach (var item in outboundDetails)
                {
                    decimal? subTotal = item.Quantity * item.Price.Value;
                    var chiet_khau = item.DisCountAmount.HasValue ? item.DisCountAmount.Value : 0;
                    var thanh_tien = subTotal - chiet_khau;
                    tongTien += thanh_tien.Value;
                    var Row = "<tr>"
                     + "<td class=\"text-center\">" + (outboundDetails.ToList().IndexOf(item) + 1) + "</td>"
                     + "<td class=\"text-left\">" + item.ProductCode + "</td>"
                     + "<td class=\"text-left\">" + item.ProductName + "</td>"
                     + "<td class=\"text-center\">" + item.Unit + "</td>"
                     + "<td class=\"text-right\">" + item.Quantity.Value + "</td>"
                     + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(item.Price).Replace(".", ",") + "</td>"
                     + "<td class=\"text-right\">" + (item.DisCount.HasValue ? item.DisCount.Value : 0) + "</td>"
                     + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(chiet_khau).Replace(".", ",") + "</td>"
                     + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(thanh_tien).Replace(".", ",") + "</td></tr>";
                    ListRow += Row;
                }
                //định dạng table html
                var style = "<style>.invoice-detail{ width:100%;margin-top: 10px;border-spacing: 0px;}"
                    + ".invoice-detail th{border: 1px solid #000;border-right: none;padding: 5px;}"
                    + " .invoice-detail tr th:last-child {border-right: 1px solid #000;}"
                    + " .invoice-detail td{padding: 5px 5px; border-bottom: 1px solid #000; border-left: 1px solid #000; height: 15px;font-size: 12px;}"
                    + " .invoice-detail tr td:last-child {border-right: 1px solid #000;}"
                    + ".invoice-detail tbody tr:last-child td {border-bottom: 1px solid #000;}"
                    + " .invoice-detail tfoot td{font-weight:bold;border-bottom: 1px solid #000;}"
                    + " .invoice-detail tfoot tr:first-child td{border-top: 1px solid #000;}"
                    + ".text-center{text-align:center;}"
                    + ".text-right{text-align:right;}"
                    + " .logo{ width: 100px;float: left; margin: 0 20px;height: 100px;line-height: 100px;}"
                    + ".logo img{ width:100%;vertical-align: middle;}"
                    + "</style>";

                //khởi tạo table html.                
                var table = style + "<table class=\"invoice-detail\"><thead><tr> <th>STT</th> <th>Mã hàng</th><th>Tên mặt hàng</th><th>ĐVT</th><th>Số lượng</th><th>Đơn giá</th><th>% CK</th><th>Trị giá chiết khấu</th><th>Thành tiền</th></tr></thead><tbody>"
                             + ListRow
                             //+ "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>"
                             //+ "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>"
                             + "</tbody><tfoot>"
                             + "<tr><td colspan=\"4\" class=\"text-right\"></td><td class=\"text-right\">"
                             + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(outboundDetails.Sum(x => x.Quantity)).Replace(".", ",")
                             + "</td><td colspan=\"3\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                             + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(tongTien).Replace(".", ",")
                             + "</td></tr>"
                             //  + "<tr><td colspan=\"10\" class=\"text-right\">VAT (" + vat + "%)</td><td class=\"text-right\">"
                             //+ Erp.BackOffice.Helpers.Common.PhanCachHangNgan(totalVAT)
                             //+ "</td></tr>"
                             // + "<tr><td colspan=\"10\" class=\"text-right\">Tổng tiền phải thanh toán</td><td class=\"text-right\">"
                             //+ Erp.BackOffice.Helpers.Common.PhanCachHangNgan(total)
                             //+ "</td></tr>"
                             // + "<tr><td colspan=\"10\" class=\"text-right\">Đã thanh toán</td><td class=\"text-right\">"
                             // + (productInvoice.PaidAmount != null && productInvoice.PaidAmount.Value > 0 ? Erp.BackOffice.Helpers.Common.PhanCachHangNgan(productInvoice.PaidAmount.Value) : "0")
                             //+ "</td></tr>"
                             // + "<tr><td colspan=\"10\" class=\"text-right\">Còn lại phải thu</td><td class=\"text-right\">"
                             //+ Erp.BackOffice.Helpers.Common.PhanCachHangNgan(productInvoice.TotalAmount.Value - (productInvoice.PaidAmount != null && productInvoice.PaidAmount.Value > 0 ? productInvoice.PaidAmount.Value : 0))
                             //+ "</td></tr>"
                             + "</tfoot><table>";

                //lấy template phiếu xuất.
                var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("ProductOutbound")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                //truyền dữ liệu vào template.
                //lấy thông tin khách hàng
                //var customer = customerRepository.GetvwCustomerByCode(productInvoice.CustomerCode);
                //lấy người lập phiếu xuất kho
                //var user = userRepository.GetUserById(productInvoice.SalerId.Value);

                model.Content = template.Content;
                model.Content = model.Content.Replace("{Code}", ProductOutbound.Code);
                model.Content = model.Content.Replace("{Day}", ProductOutbound.CreatedDate.Value.Day.ToString());
                model.Content = model.Content.Replace("{Month}", ProductOutbound.CreatedDate.Value.Month.ToString());
                model.Content = model.Content.Replace("{Year}", ProductOutbound.CreatedDate.Value.Year.ToString());
                //model.Content = model.Content.Replace("{CustomerName}", customer.LastName + " " + customer.FirstName);
                //model.Content = model.Content.Replace("{CustomerPhone}", customer.Phone);
                //model.Content = model.Content.Replace("{CompanyName}", customer.CompanyName);

                //if (!string.IsNullOrEmpty(customer.Address))
                //{
                //    model.Content = model.Content.Replace("{Address}", customer.Address + ", ");
                //}
                //else
                //{
                //    model.Content = model.Content.Replace("{Address}", "");
                //}
                //if (!string.IsNullOrEmpty(customer.DistrictName))
                //{
                //    model.Content = model.Content.Replace("{District}", customer.DistrictName + ", ");
                //}
                //else
                //{
                //    model.Content = model.Content.Replace("{District}", "");
                //}
                //if (!string.IsNullOrEmpty(customer.WardName))
                //{
                //    model.Content = model.Content.Replace("{Ward}", customer.WardName + ", ");
                //}
                //else
                //{
                //    model.Content = model.Content.Replace("{Ward}", "");
                //}
                //if (!string.IsNullOrEmpty(customer.ProvinceName))
                //{
                //    model.Content = model.Content.Replace("{Province}", customer.ProvinceName);
                //}
                //else
                //{
                //    model.Content = model.Content.Replace("{Province}", "");
                //}

                model.Content = model.Content.Replace("{Note}", ProductOutbound.Note);
                if (productInvoice != null)
                {
                    model.Content = model.Content.Replace("{InvoiceCode}", productInvoice.Code);
                }
                else
                {
                    model.Content = model.Content.Replace("{InvoiceCode}", "");
                }
                model.Content = model.Content.Replace("{MoneyText}", Erp.BackOffice.Helpers.Common.ChuyenSoThanhChu(Convert.ToInt32(tongTien)));
                model.Content = model.Content.Replace("{OutboundCode}", ProductOutbound.Code);
                //model.Content = model.Content.Replace("{SaleName}", user.FullName);
                model.Content = model.Content.Replace("{DataTable}", table);
                model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
                model.Content = model.Content.Replace("{System.CompanyName}", company);
                model.Content = model.Content.Replace("{System.AddressCompany}", address);
                model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
                model.Content = model.Content.Replace("{System.Fax}", fax);
                model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
                model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);

                if (ExportExcel)
                {
                    Response.AppendHeader("content-disposition", "attachment;filename=" + ProductOutbound.CreatedDate.Value.ToString("yyyyMMdd") + ProductOutbound.Code + ".xls");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Write(model.Content);
                    Response.End();
                }
                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        #region Tranfer
        public ActionResult Tranfer(int? WarehouseSourceId)
        {
            var warehouseList = WarehouseRepository.GetAllWarehouse().AsEnumerable();
            ViewBag.warehouseSourceList = warehouseList.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId)
                   .Select(item => new SelectListItem
                   {
                       Text = item.Name,
                       Value = item.Id.ToString()
                   });

            ViewBag.warehouseDestinationList = warehouseList
                .Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });

            ProductOutboundTransferViewModel model = new ProductOutboundTransferViewModel();
            model.CreatedUserName = Erp.BackOffice.Helpers.Common.CurrentUser.FullName;
            model.CreatedDate = DateTime.Now;
            model.TotalAmount = 0;
            model.Code = Helpers.Common.GetOrderNo("ProductOutbound", model.Code);
            model.Type = ProductOutboundType.Internal;
            if (WarehouseSourceId != null && WarehouseSourceId > 0)
            {
                model.WarehouseSourceId = WarehouseSourceId;
                var productList = InventoryRepository.GetAllvwInventoryByWarehouseId(WarehouseSourceId.Value)
                    .Where(x => x.ProductType == ProductType.Product && x.Quantity != null && x.Quantity > 0)
                   .Select(item => new ProductViewModel
                   {
                       Id = item.ProductId.Value,
                       Code = item.ProductCode,
                       Name = item.ProductName,
                       CategoryCode = item.CategoryCode,
                       PriceOutbound = item.ProductPriceOutbound,
                       Unit = item.ProductUnit,
                       QuantityTotalInventory = item.Quantity,
                       Image_Name = item.ProductImage
                   }).ToList();

                ViewBag.productList = productList;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Tranfer(ProductOutboundTransferViewModel model)
        {
            if (ModelState.IsValid)
            {
                //duyệt qua danh sách sản phẩm mới xử lý tình huống user chọn 2 sản phầm cùng id
                List<ProductOutboundDetail> outboundDetails = new List<ProductOutboundDetail>();
                foreach (var group in model.DetailList.GroupBy(x => x.ProductId))
                {
                    var product = ProductRepository.GetProductById(group.Key.Value);

                    outboundDetails.Add(new ProductOutboundDetail
                    {
                        ProductId = product.Id,
                        Quantity = group.Sum(x => x.Quantity),
                        Unit = product.Unit,
                        Price = group.FirstOrDefault().Price,
                        IsDeleted = false,
                        CreatedUserId = WebSecurity.CurrentUserId,
                        ModifiedUserId = WebSecurity.CurrentUserId,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    });
                }

                #region Xuất
                //thêm mới phiếu xuất kho theo đơn xuất kho ở trên
                var ProductOutbound = new Domain.Sale.Entities.ProductOutbound();
                AutoMapper.Mapper.Map(model, ProductOutbound);
                ProductOutbound.IsDeleted = false;
                ProductOutbound.CreatedUserId = WebSecurity.CurrentUserId;
                ProductOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
                ProductOutbound.CreatedDate = DateTime.Now;
                ProductOutbound.ModifiedDate = DateTime.Now;
                ProductOutbound.BranchId = Helpers.Common.CurrentUser.BranchId;
                ProductOutbound.IsDone = false;
                ProductOutbound.TotalAmount = outboundDetails.Sum(item => item.Quantity * item.Price);
                ProductOutboundRepository.InsertProductOutbound(ProductOutbound);
                List<WarehouseLocationItemViewModel> LocationItemList = new List<WarehouseLocationItemViewModel>();
                foreach (var item in outboundDetails)
                {
                    ProductOutboundDetail productOutboundDetail = new ProductOutboundDetail();
                    productOutboundDetail.ProductId = item.ProductId;
                    productOutboundDetail.Price = item.Price;
                    productOutboundDetail.Quantity = item.Quantity;
                    productOutboundDetail.Unit = item.Unit;
                    productOutboundDetail.LoCode = item.LoCode;
                    productOutboundDetail.ExpiryDate = item.ExpiryDate;
                    productOutboundDetail.IsDeleted = false;
                    productOutboundDetail.CreatedUserId = WebSecurity.CurrentUserId;
                    productOutboundDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                    productOutboundDetail.CreatedDate = DateTime.Now;
                    productOutboundDetail.ModifiedDate = DateTime.Now;
                    productOutboundDetail.ProductOutboundId = ProductOutbound.Id;
                    ProductOutboundRepository.InsertProductOutboundDetail(productOutboundDetail);
                }

                //cập nhật lại mã xuất kho
                ProductOutbound.Code = Helpers.Common.GetOrderNo("ProductOutbound", model.Code);
                ProductOutboundRepository.UpdateProductOutbound(ProductOutbound);
                Helpers.Common.SetOrderNo("ProductOutbound");

                //Thêm vào quản lý chứng từ
                TransactionController.Create(new TransactionViewModel
                {
                    TransactionModule = "ProductOutbound",
                    TransactionCode = ProductOutbound.Code,
                    TransactionName = "Xuất kho"
                });

                #endregion
                if (ProductOutbound.Type == ProductOutboundType.Internal)
                {
                    #region Nhập
                    var warehouseDestination = WarehouseRepository.GetWarehouseById(model.WarehouseDestinationId.Value);
                    var ProductInbound = new Domain.Sale.Entities.ProductInbound();
                    ProductInbound.IsDone = false;
                    ProductInbound.Note = "Xuất chuyển kho " + DateTime.Now.ToShortDateString();
                    ProductInbound.WarehouseDestinationId = model.WarehouseDestinationId;
                    ProductInbound.IsDeleted = false;
                    ProductInbound.CreatedUserId = WebSecurity.CurrentUserId;
                    ProductInbound.ModifiedUserId = WebSecurity.CurrentUserId;
                    ProductInbound.CreatedDate = DateTime.Now;
                    ProductInbound.ModifiedDate = DateTime.Now;
                    ProductInbound.BranchId = warehouseDestination?.BranchId ?? Helpers.Common.CurrentUser.BranchId;
                    ProductInbound.TotalAmount = decimal.Parse(ProductOutbound.TotalAmount.ToString());
                    ProductInbound.Total = ProductInbound.TotalAmount;
                    ProductInbound.Type = ProductInboundType.Internal;
                    ProductInboundRepository.InsertProductInbound(ProductInbound);
                    //cập nhật lại mã nhập kho
                    ProductInbound.Code = Helpers.Common.GetOrderNo("ProductInbound", model.Code);
                    ProductInboundRepository.UpdateProductInbound(ProductInbound);
                    Erp.BackOffice.Helpers.Common.SetOrderNo("ProductInbound");
                    //Thêm chi tiết phiếu nhập

                    foreach (var item in outboundDetails)
                    {
                        var productInboundDetail = new ProductInboundDetail()
                        {
                            ProductId = item.ProductId.Value,
                            Quantity = item.Quantity == null ? 0 : item.Quantity.Value,
                            Unit = item.Unit,
                            Price = item.Price == null ? 0 : item.Price.Value,
                            IsDeleted = false,
                            CreatedUserId = WebSecurity.CurrentUserId,
                            ModifiedUserId = WebSecurity.CurrentUserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            ProductInboundId = ProductInbound.Id
                        };

                        ProductInboundRepository.InsertProductInboundDetail(productInboundDetail);
                    }
                    //Thêm vào quản lý chứng từ
                    TransactionController.Create(new TransactionViewModel
                    {
                        TransactionModule = "ProductInbound",
                        TransactionCode = ProductInbound.Code,
                        TransactionName = "Nhập kho"
                    });
                    //Thêm chứng từ liên quan
                    TransactionController.CreateRelationship(new TransactionRelationshipViewModel
                    {
                        TransactionA = ProductInbound.Code,
                        TransactionB = ProductOutbound.Code
                    });
                    #endregion
                }

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }

            // phần xử lý cho việc valid sai
            var warehouseList = WarehouseRepository.GetAllWarehouse().AsEnumerable();

            ViewBag.warehouseSourceList = warehouseList.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId)
                   .Select(item => new SelectListItem
                   {
                       Text = item.Name,
                       Value = item.Id.ToString()
                   });

            ViewBag.warehouseDestinationList = warehouseList
                .Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            if (model.WarehouseSourceId > 0)
            {
                var productList = InventoryRepository.GetAllvwInventoryByWarehouseId(model.WarehouseSourceId.Value)
                       .Where(x => x.ProductType == ProductType.Product && x.Quantity != null && x.Quantity > 0)
                      .Select(item => new ProductViewModel
                      {
                          Id = item.ProductId.Value,
                          Code = item.ProductCode,
                          Name = item.ProductName,
                          CategoryCode = item.CategoryCode,
                          PriceOutbound = item.ProductPriceOutbound,
                          Unit = item.ProductUnit,
                          QuantityTotalInventory = item.Quantity,
                          Image_Name = item.ProductImage
                      }).ToList();

                ViewBag.productList = productList;
            }
            return View(model);
        }

        public PartialViewResult LoadProductItemS(int OrderNo, int ProductId, string ProductName, string Unit, int Quantity, decimal Price, string ProductCode, string ProductType, int QuantityInventory, int WarehouseId)
        {
            var model = new ProductOutboundDetailViewModel();
            model.OrderNo = OrderNo;
            model.ProductId = ProductId;
            model.ProductName = ProductName;
            model.Unit = Unit;
            model.Quantity = Quantity;
            model.Price = Price;
            model.ProductCode = ProductCode;
            model.ProductType = ProductType;
            model.QuantityInInventory = QuantityInventory;

            model.ListWarehouseLocationItemViewModel = new List<WarehouseLocationItemViewModel>();

            var listLocationItemExits = WarehouseLocationItemRepository.GetAllWarehouseLocationItem()
                .Where(q => q.ProductId == ProductId && q.WarehouseId == WarehouseId && q.IsOut == false)
                .OrderBy(x => x.ExpiryDate)
                .Take(Quantity)
                .ToList();

            AutoMapper.Mapper.Map(listLocationItemExits, model.ListWarehouseLocationItemViewModel);

            return PartialView(model);
        }
        #endregion

        [HttpPost]
        public ActionResult checkExitsCode(string code, int? id)
        {
            var productoutboud = ProductOutboundRepository.GetProductOutboundByCode(code);
            if (productoutboud != null)
            {
                if (id != null && id.Value > 0)
                {
                    var data = ProductOutboundRepository.GetProductOutboundById(id.Value);
                    if (data.Id == productoutboud.Id)
                    {
                        return Content("success");
                    }
                    return Content("error");
                }
                return Content("error");
            }
            return Content("success");
        }

        public static ProductOutbound CreateFromInvoice(IProductOutboundRepository productOutboundRepository,
                           ProductOutboundViewModel model,
                           string productInvoiceCode, TempDataDictionary TempData)
        {
            var productOutbound = new Domain.Sale.Entities.ProductOutbound();
            AutoMapper.Mapper.Map(model, productOutbound);
            productOutbound.IsDeleted = false;
            productOutbound.CreatedUserId = WebSecurity.CurrentUserId;
            productOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
            productOutbound.CreatedDate = DateTime.Now;
            productOutbound.ModifiedDate = DateTime.Now;
            productOutbound.Type = ProductOutboundType.Invoice;
            productOutbound.BranchId = Helpers.Common.CurrentUser.BranchId;
            productOutbound.TotalAmount = model.DetailList.Sum(x => x.Price * x.Quantity);
            productOutboundRepository.InsertProductOutbound(productOutbound);

            //Cập nhật lại mã xuất kho
            productOutbound.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductOutbound");
            productOutboundRepository.UpdateProductOutbound(productOutbound);

            Erp.BackOffice.Helpers.Common.SetOrderNo("ProductOutbound");
            foreach (var item in model.DetailList)
            {
                ProductOutboundDetail productOutboundDetail = new ProductOutboundDetail();
                AutoMapper.Mapper.Map(item, productOutboundDetail);
                productOutboundDetail.IsDeleted = false;
                productOutboundDetail.CreatedUserId = WebSecurity.CurrentUserId;
                productOutboundDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                productOutboundDetail.CreatedDate = DateTime.Now;
                productOutboundDetail.ModifiedDate = DateTime.Now;
                productOutboundDetail.ProductOutboundId = productOutbound.Id;
                productOutboundRepository.InsertProductOutboundDetail(productOutboundDetail);
            }

            //Thêm vào quản lý chứng từ
            TransactionController.Create(new TransactionViewModel
            {
                TransactionModule = "ProductOutbound",
                TransactionCode = productOutbound.Code,
                TransactionName = "Xuất kho bán hàng"
            });

            //Thêm chứng từ liên quan
            TransactionController.CreateRelationship(new TransactionRelationshipViewModel
            {
                TransactionA = productOutbound.Code,
                TransactionB = productInvoiceCode
            });
            Archive(productOutboundRepository, productOutbound, TempData);
            return productOutbound;
        }

        public static ProductOutbound CreateFromPayPoint(IProductOutboundRepository productOutboundRepository,
                        ProductOutboundViewModel model,
                        string PayPointCode, TempDataDictionary TempData)
        {
            var productOutbound = new Domain.Sale.Entities.ProductOutbound();
            AutoMapper.Mapper.Map(model, productOutbound);
            productOutbound.IsDeleted = false;
            productOutbound.CreatedUserId = WebSecurity.CurrentUserId;
            productOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
            productOutbound.CreatedDate = DateTime.Now;
            productOutbound.ModifiedDate = DateTime.Now;
            productOutbound.Type = ProductOutboundType.Gift;
            productOutbound.BranchId = Helpers.Common.CurrentUser.BranchId;
            productOutbound.TotalAmount = model.DetailList.Sum(x => x.Price * x.Quantity);
            productOutboundRepository.InsertProductOutbound(productOutbound);

            //Cập nhật lại mã xuất kho
            productOutbound.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductOutbound");
            productOutboundRepository.UpdateProductOutbound(productOutbound);
            Erp.BackOffice.Helpers.Common.SetOrderNo("ProductOutbound");

            foreach (var item in model.DetailList)
            {
                ProductOutboundDetail productOutboundDetail = new ProductOutboundDetail();
                AutoMapper.Mapper.Map(item, productOutboundDetail);
                productOutboundDetail.IsDeleted = false;
                productOutboundDetail.CreatedUserId = WebSecurity.CurrentUserId;
                productOutboundDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                productOutboundDetail.CreatedDate = DateTime.Now;
                productOutboundDetail.ModifiedDate = DateTime.Now;
                productOutboundDetail.ProductOutboundId = productOutbound.Id;
                productOutboundRepository.InsertProductOutboundDetail(productOutboundDetail);
            }

            //Thêm vào quản lý chứng từ
            TransactionController.Create(new TransactionViewModel
            {
                TransactionModule = "ProductOutbound",
                TransactionCode = productOutbound.Code,
                TransactionName = "Xuất kho trả điểm"
            });

            //Thêm chứng từ liên quan
            TransactionController.CreateRelationship(new TransactionRelationshipViewModel
            {
                TransactionA = productOutbound.Code,
                TransactionB = PayPointCode
            });
            Archive(productOutboundRepository, productOutbound, TempData);
            return productOutbound;
        }


        public static ProductOutbound CreateForMemberCard(TempDataDictionary TempData, int? CardId, int? BranchId, string CustomerName, string CustomerCode)
        {
            IProductOutboundRepository productOutboundRepository = DependencyResolver.Current.GetService<IProductOutboundRepository>();
            IWarehouseRepository WarehouseRepository = DependencyResolver.Current.GetService<IWarehouseRepository>();
            IInventoryRepository InventoryRepository = DependencyResolver.Current.GetService<IInventoryRepository>();
            var Warehouse = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == BranchId).FirstOrDefault();
            if (Warehouse == null)
            {
                TempData[Globals.FailedMessageKey] += "Người dùng không thuộc chi nhánh có kho! </br>";
                return null;
            }
            var ProductInventory = InventoryRepository.GetAllvwInventoryByWarehouseId(Warehouse.Id).Where(x => x.ProductId == CardId).FirstOrDefault();
            if (ProductInventory == null)
            {
                TempData[Globals.FailedMessageKey] += "Thẻ chưa nhập kho! </br>";
                return null;
            }
            if (ProductInventory.Quantity <= 0)
            {
                TempData[Globals.FailedMessageKey] += "Thẻ đã hết!";
                return null;
            }

            var productOutbound = new Domain.Sale.Entities.ProductOutbound();
            productOutbound.IsDeleted = false;
            productOutbound.CreatedUserId = WebSecurity.CurrentUserId;
            productOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
            productOutbound.CreatedDate = DateTime.Now;
            productOutbound.ModifiedDate = DateTime.Now;
            productOutbound.Type = ProductOutboundType.Card;
            productOutbound.BranchId = Helpers.Common.CurrentUser.BranchId;
            productOutbound.TotalAmount = 0;
            productOutbound.BranchId = BranchId;
            productOutbound.WarehouseSourceId = Warehouse.Id;
            productOutbound.IsArchive = false;
            productOutbound.Note = "Xuất thẻ cho khách hàng: " + CustomerName;
            productOutboundRepository.InsertProductOutbound(productOutbound);

            //Cập nhật lại mã xuất kho
            productOutbound.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductOutbound");
            productOutboundRepository.UpdateProductOutbound(productOutbound);
            Erp.BackOffice.Helpers.Common.SetOrderNo("ProductOutbound");

            // thêm chi tiết
            ProductOutboundDetail productOutboundDetail = new ProductOutboundDetail();
            productOutboundDetail.IsDeleted = false;
            productOutboundDetail.CreatedUserId = WebSecurity.CurrentUserId;
            productOutboundDetail.ModifiedUserId = WebSecurity.CurrentUserId;
            productOutboundDetail.CreatedDate = DateTime.Now;
            productOutboundDetail.ModifiedDate = DateTime.Now;
            productOutboundDetail.ProductOutboundId = productOutbound.Id;
            productOutboundDetail.ProductId = ProductInventory.ProductId;
            productOutboundDetail.Quantity = 1;
            productOutboundDetail.Price = 0;
            productOutboundDetail.Unit = ProductInventory.ProductUnit;
            productOutboundRepository.InsertProductOutboundDetail(productOutboundDetail);


            //Thêm vào quản lý chứng từ
            TransactionController.Create(new TransactionViewModel
            {
                TransactionModule = "ProductOutbound",
                TransactionCode = productOutbound.Code,
                TransactionName = "Xuất kho cấp thẻ"
            });

            //Thêm chứng từ liên quan
            TransactionController.CreateRelationship(new TransactionRelationshipViewModel
            {
                TransactionA = productOutbound.Code,
                TransactionB = CustomerCode
            });
            Archive(productOutboundRepository, productOutbound, TempData);
            return productOutbound;
        }
    }
}
