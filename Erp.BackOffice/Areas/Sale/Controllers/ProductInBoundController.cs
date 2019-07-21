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
using Erp.BackOffice.App_GlobalResources;
using Erp.BackOffice.Account.Models;
using Erp.BackOffice.Account.Controllers;
using Erp.Domain.Account.Entities;
using Erp.Domain.Account.Interfaces;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class ProductInboundController : Controller
    {
        private readonly ITransactionLiabilitiesRepository transactionRepository;
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IWarehouseLocationItemRepository WarehouseLocationItemRepository;
        private readonly IInventoryRepository InventoryRepository;
        private readonly IPurchaseOrderRepository PurchaseOrderRepository;
        private readonly IProductRepository ProductRepository;
        private readonly IProductInboundRepository ProductInboundRepository;
        private readonly IProductOutboundRepository ProductOutboundRepository;
        private readonly ISupplierRepository SupplierRepository;
        private readonly IUserRepository userRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly IQueryHelper QueryHelper;
        private readonly ITemplatePrintRepository templatePrintRepository;
        public ProductInboundController(
              ITransactionLiabilitiesRepository _transaction
            , IWarehouseRepository _Warehouse
            , IWarehouseLocationItemRepository _WarehouseLocationItem
            , IInventoryRepository _Inventory
            , IPurchaseOrderRepository _PurchaseOrder
            , IProductRepository _Product
            , IProductInboundRepository _ProductInbound
            , IProductOutboundRepository _ProductOutbound
            , ISupplierRepository _Supplier
            , IUserRepository _user
            , IQueryHelper _QueryHelper
            , IPaymentRepository payment
            , ITemplatePrintRepository _templatePrint
            )
        {
            transactionRepository = _transaction;
            WarehouseRepository = _Warehouse;
            WarehouseLocationItemRepository = _WarehouseLocationItem;
            InventoryRepository = _Inventory;
            PurchaseOrderRepository = _PurchaseOrder;
            ProductRepository = _Product;
            ProductInboundRepository = _ProductInbound;
            ProductOutboundRepository = _ProductOutbound;
            SupplierRepository = _Supplier;
            userRepository = _user;
            QueryHelper = _QueryHelper;
            paymentRepository = payment;
            templatePrintRepository = _templatePrint;
        }

        #region Index
        public ViewResult Index(string txtCode, string txtMinAmount, string txtMaxAmount, int? warehouseDestinationId, string txtWarehouseSource, string startDate, string endDate)
        {
            IEnumerable<ProductInboundViewModel> q = ProductInboundRepository.GetAllvwProductInbound()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Type != ProductInboundType.Card)
                .Select(item => new ProductInboundViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    Total = item.Total,
                    WarehouseDestinationName = item.WarehouseDestinationName,
                    IsDone = item.IsDone,
                    PurchaseOrderId = item.PurchaseOrderId,
                    SupplierId = item.SupplierId,
                    SupplierName = item.SupplierName,
                    WarehouseDestinationId = item.WarehouseDestinationId,
                    Note = item.Note,
                    IsArchive = item.IsArchive,
                }).OrderByDescending(m => m.ModifiedDate);

            //Tìm những phiếu nhập có chứa mã sp
            //if (!string.IsNullOrEmpty(txtProductCode))
            //{
            //    txtProductCode = txtProductCode.Trim();
            //    var productListId = ProductRepository.GetAllvwProduct()
            //        .Where(item => item.Code == txtProductCode).Select(item => item.Id).ToList();

            //    if (productListId.Count > 0)
            //    {
            //        List<int> listProductInboundId = new List<int>();
            //        foreach (var id in productListId)
            //        {
            //            var list = ProductInboundRepository.GetAllvwProductInboundDetailByProductId(id)
            //                .Select(item => item.ProductInboundId).Distinct().ToList();

            //            listProductInboundId.AddRange(list);
            //        }

            //        q = q.Where(item => listProductInboundId.Contains(item.Id));
            //    }
            //}


            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => x.Code.ToLowerOrEmpty().Contains(txtCode));
            }

            if (warehouseDestinationId != null)
            {
                q = q.Where(x => x.WarehouseDestinationId == warehouseDestinationId);
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

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x=> x.BranchId == Helpers.Common.CurrentUser.BranchId).AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.warehouseList = warehouseList;

            return View(q);
        }
        public ViewResult ListForCard(string txtCode, int? warehouseDestinationId, string startDate, string endDate)
        {
            IEnumerable<ProductInboundViewModel> q = ProductInboundRepository.GetAllvwProductInbound()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Type == ProductInboundType.Card)
                .Select(item => new ProductInboundViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    Total = item.Total,
                    WarehouseDestinationName = item.WarehouseDestinationName,
                    IsDone = item.IsDone,
                    PurchaseOrderId = item.PurchaseOrderId,
                    SupplierId = item.SupplierId,
                    SupplierName = item.SupplierName,
                    WarehouseDestinationId = item.WarehouseDestinationId,
                    Note = item.Note,
                    IsArchive = item.IsArchive,
                }).OrderByDescending(m => m.ModifiedDate);
            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => x.Code.ToLowerOrEmpty().Contains(txtCode));
            }

            if (warehouseDestinationId != null)
            {
                q = q.Where(x => x.WarehouseDestinationId == warehouseDestinationId);
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
            var warehouseList = WarehouseRepository.GetAllWarehouse().AsEnumerable()
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
        public ViewResult ListForGift(string txtCode, int? warehouseDestinationId, string startDate, string endDate)
        {
            IEnumerable<ProductInboundViewModel> q = ProductInboundRepository.GetAllvwProductInbound()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Type == ProductInboundType.Gift)
                .Select(item => new ProductInboundViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    Total = item.Total,
                    WarehouseDestinationName = item.WarehouseDestinationName,
                    IsDone = item.IsDone,
                    PurchaseOrderId = item.PurchaseOrderId,
                    SupplierId = item.SupplierId,
                    SupplierName = item.SupplierName,
                    WarehouseDestinationId = item.WarehouseDestinationId,
                    Note = item.Note,
                    IsArchive = item.IsArchive,
                }).OrderByDescending(m => m.ModifiedDate);
            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => x.Code.ToLowerOrEmpty().Contains(txtCode));
            }

            if (warehouseDestinationId != null)
            {
                q = q.Where(x => x.WarehouseDestinationId == warehouseDestinationId);
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
            var warehouseList = WarehouseRepository.GetAllWarehouse().AsEnumerable()
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

        public PartialViewResult LoadProductItem(int OrderNo, int ProductId, string ProductName, string Unit, int Quantity, decimal Price, string ProductCode, string ProductType)
        {
            var model = new ProductInboundDetailViewModel();
            model.OrderNo = OrderNo;
            model.ProductId = ProductId;
            model.ProductName = ProductName;
            model.Unit = Unit;
            model.Quantity = Quantity;
            model.Price = Price;
            model.ProductCode = ProductCode;
            model.ProductType = ProductType;

            return PartialView(model);
        }

        #region Create

        public ActionResult Create(int? PurchaseOrderId)
        {

            var model = new ProductInboundViewModel();
            model.CreatedDate = DateTime.Now;
            model.DetailList = new List<ProductInboundDetailViewModel>();

            if (PurchaseOrderId != null && PurchaseOrderId.Value > 0)
            {
                model.PurchaseOrderId = PurchaseOrderId;
                var aaa = PurchaseOrderRepository.GetvwPurchaseOrderById(PurchaseOrderId.Value);
                model.PurchaseOrderId = aaa.Id;
                model.WarehouseDestinationId = aaa.WarehouseDestinationId;
                model.SupplierId = aaa.SupplierId;
                model.PurchaseOrderCode = aaa.Code;
                var listProductInvoiceDetail = PurchaseOrderRepository.GetvwAllOrderDetailsByOrderId(PurchaseOrderId.Value).Select(i => new
                {
                    Amount = i.Amount,
                    DisCountAmount = i.DisCountAmount,
                    IsArchive = i.IsArchive,
                    CategoryCode = i.CategoryCode,
                    DisCount = i.DisCount,
                    Description = i.Description,
                    Manufacturer = i.Manufacturer,
                    Price = i.Price.Value,
                    ProductCode = i.ProductCode,
                    ProductGroup = i.ProductGroup,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity.Value,
                    ProductId = i.ProductId.Value
                }).OrderByDescending(m => m.ProductCode).ToList();

                //Tạo danh sách chi tiết phiếu xuất tương ứng                    
                foreach (var item in listProductInvoiceDetail)
                {
                    var productOutboundDetailViewModel = model.DetailList.Where(i => i.ProductId == item.ProductId).FirstOrDefault();
                    if (productOutboundDetailViewModel == null)
                    {
                        productOutboundDetailViewModel = new ProductInboundDetailViewModel();
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

                int n = 0;
                foreach (var item in model.DetailList)
                {
                    item.OrderNo = n;
                    n++;
                }
            }


            var orderList = PurchaseOrderRepository.GetAllPurchaseOrder().AsEnumerable()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.SupplierId != null && (x.Status == Wording.OrderStatus_pending || x.Status == Wording.OrderStatus_shipping))
               .Select(item => new PurchaseOrderViewModel
               {
                   Code = item.Code,
                   Id = item.Id,
                   WarehouseDestinationId = item.WarehouseDestinationId,
                   SupplierId = item.SupplierId
               });
            ViewBag.orderList = orderList;

            var supplierList = SupplierRepository.GetAllSupplier().AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.supplierList = supplierList;

            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId).AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.warehouseList = warehouseList;


            var productList = ProductRepository.GetAllProductByType("product").Where(x => x.NoInbound != true)
               .Select(item => new ProductViewModel
               {
                   Code = item.Code,
                   Barcode = item.Barcode,
                   Name = item.Name,
                   Id = item.Id,
                   CategoryCode = item.CategoryCode,
                   PriceInbound = item.PriceInbound,
                   Unit = item.Unit,
                   Image_Name = item.Image_Name
               });
            ViewBag.productList = productList;

            model.CreatedDate = DateTime.Now;
            model.CreatedUserName = Helpers.Common.CurrentUser.FullName;
            model.PaymentViewModel = new PaymentViewModel();
            model.NextPaymentDate = DateTime.Now.AddDays(30);
            //model.PaymentViewModel.Name = TransactionController.TransactionType.ProductInbound.GetName();
            model.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductInbound");
            string vat = Helpers.Common.GetSetting("vat");

            model.VAT = Convert.ToInt32(vat);

            ViewBag.isAdmin = Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId == 1 ? true : false;
            return View(model);
        }

        public ActionResult CreateCard()
        {
            var model = new ProductInboundViewModel();
            model.CreatedDate = DateTime.Now;
            model.DetailList = new List<ProductInboundDetailViewModel>();
            var supplierList = SupplierRepository.GetAllSupplier().AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.supplierList = supplierList;
            var productList = ProductRepository.GetAllProductByType("card").Where(x => x.NoInbound != true)
               .Select(item => new ProductViewModel
               {
                   Code = item.Code,
                   Barcode = item.Barcode,
                   Name = item.Name,
                   Id = item.Id,
                   CategoryCode = item.CategoryCode,
                   PriceInbound = item.PriceInbound,
                   Unit = item.Unit,
                   Image_Name = item.Image_Name
               });
            ViewBag.Categories = "CARD";
            ViewBag.ActionBack = "ListForCard";
            ViewBag.productList = productList;
            model.CreatedDate = DateTime.Now;
            model.CreatedUserName = Helpers.Common.CurrentUser.FullName;
            model.PaymentViewModel = new PaymentViewModel();
            model.NextPaymentDate = DateTime.Now.AddDays(30);
            model.Type = ProductInboundType.Card;
            string vat = Helpers.Common.GetSetting("vat");
            model.VAT = Convert.ToInt32(vat);
            return View("Create", model);
        }
        public ActionResult CreateGift()
        {
            var model = new ProductInboundViewModel();
            model.CreatedDate = DateTime.Now;
            model.DetailList = new List<ProductInboundDetailViewModel>();
            var supplierList = SupplierRepository.GetAllSupplier().AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.supplierList = supplierList;
            var productList = ProductRepository.GetAllProductByType("gift").Where(x => x.NoInbound != true)
               .Select(item => new ProductViewModel
               {
                   Code = item.Code,
                   Barcode = item.Barcode,
                   Name = item.Name,
                   Id = item.Id,
                   CategoryCode = item.CategoryCode,
                   PriceInbound = item.PriceInbound,
                   Unit = item.Unit,
                   Image_Name = item.Image_Name,
                   RedemptionPoints = item.RedemptionPoints
                   
               });
            ViewBag.Categories = "GIFT";
            ViewBag.ActionBack = "ListForGift";
            ViewBag.productList = productList;
            model.CreatedDate = DateTime.Now;
            model.CreatedUserName = Helpers.Common.CurrentUser.FullName;
            model.PaymentViewModel = new PaymentViewModel();
            model.NextPaymentDate = DateTime.Now.AddDays(30);
            model.Type = ProductInboundType.Gift;
            string vat = Helpers.Common.GetSetting("vat");
            model.VAT = Convert.ToInt32(vat);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(ProductInboundViewModel model)
        {
            ProductInbound productInbound = null;
            if (model.PurchaseOrderId != null)
            {
                productInbound = ProductInboundRepository.GetAllProductInbound()
                    .Where(item => item.PurchaseOrderId == model.PurchaseOrderId).FirstOrDefault();

                if (productInbound != null && productInbound.IsArchive)
                    return Content("Phiếu xuất kho cho đơn hàng này đã được ghi sổ!");
            }
            if (ModelState.IsValid)
            {
                var ProductInbound = new Domain.Sale.Entities.ProductInbound();
                AutoMapper.Mapper.Map(model, ProductInbound);
                ProductInbound.IsDeleted = false;
                ProductInbound.CreatedUserId = WebSecurity.CurrentUserId;
                ProductInbound.ModifiedUserId = WebSecurity.CurrentUserId;
                ProductInbound.CreatedDate = DateTime.Now;
                ProductInbound.ModifiedDate = DateTime.Now;

                ProductInbound.BranchId = Helpers.Common.CurrentUser.BranchId;

                //duyệt qua danh sách sản phẩm mới xử lý tình huống user chọn 2 sản phầm cùng id
                List<Domain.Sale.Entities.ProductInboundDetail> listNewCheckSameId = new List<Domain.Sale.Entities.ProductInboundDetail>();
                foreach (var group in model.DetailList.GroupBy(x => x.ProductId))
                {
                    var product = ProductRepository.GetProductById(group.Key);

                    listNewCheckSameId.Add(new Domain.Sale.Entities.ProductInboundDetail
                    {
                        ProductId = group.Key,
                        Quantity = group.Sum(x => x.Quantity),
                        Unit = product.Unit,
                        Price = group.FirstOrDefault().Price,
                        IsDeleted = false,
                        CreatedUserId = WebSecurity.CurrentUserId,
                        ModifiedUserId = WebSecurity.CurrentUserId,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    });
                }

                ProductInbound.TotalAmount = listNewCheckSameId.Sum(x => x.Price * x.Quantity);

                ////lấy thông tin đơn đặt hàng nếu có chọn
                var order = new PurchaseOrder();
                //var orderDetails = new List<PurchaseOrderDetail>();
                if (model.PurchaseOrderId != null)
                {
                    order = PurchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId.Value);
                    //    //lấy danh sách orderdetail từ orderid đã chọn
                    //    orderDetails = PurchaseOrderRepository.GetAllOrderDetailsByOrderId(order.Id).ToList();

                    //    ProductInbound.WarehouseDestinationId = ProductInbound.WarehouseDestinationId == null ? order.WarehouseDestinationId : ProductInbound.WarehouseDestinationId;
                    //    ProductInbound.SupplierId = ProductInbound.SupplierId == null ? order.SupplierId : ProductInbound.SupplierId;
                }


                ProductInbound.Type = model.Type ?? (order.Id == 0 ? ProductInboundType.Manual : (order.SupplierId != null ? ProductInboundType.External : ProductInboundType.Internal));
                //thêm mới phiếu nhập và chi tiết phiếu nhập
                ProductInboundRepository.InsertProductInbound(ProductInbound);
                //cập nhật thông tin đơn nhập hàng nếu có chọn
                if (model.PurchaseOrderId != null)
                {
                    //Cập nhật hóa đơn là đang xử lý
                    var purchaseOrder = PurchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId.Value);
                    purchaseOrder.Status = Wording.OrderStatus_inprogress;
                    purchaseOrder.ModifiedDate = DateTime.Now;
                    purchaseOrder.ModifiedUserId = WebSecurity.CurrentUserId;
                    purchaseOrder.ProductInboundId = ProductInbound.Id;
                    PurchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);
                }
                ////cập nhật lại mã nhập kho

                ProductInbound.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductInbound", model.Code);
                ProductInboundRepository.UpdateProductInbound(ProductInbound);
                Erp.BackOffice.Helpers.Common.SetOrderNo("ProductInbound");
                //Thêm chi tiết phiếu nhập
                foreach (var item in listNewCheckSameId)
                {
                    item.ProductInboundId = ProductInbound.Id;
                    ProductInboundRepository.InsertProductInboundDetail(item);
                }

                //Thêm vào quản lý chứng từ
                TransactionController.Create(new TransactionViewModel
                {
                    TransactionModule = "ProductInbound",
                    TransactionCode = ProductInbound.Code,
                    TransactionName = "Nhập kho"
                });

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Detail", new { Id = ProductInbound.Id });
            }
            return View(model);
        }
        #endregion

        public ActionResult Edit(int? Id, string TransactionCode)
        {
            var ProductInbound = new vwProductInbound();
            if (Id != null)
                ProductInbound = ProductInboundRepository.GetvwProductInboundById(Id.Value);
            if (!string.IsNullOrEmpty(TransactionCode))
                ProductInbound = ProductInboundRepository.GetvwProductInboundByTransactionCode(TransactionCode);

            //  var ProductInbound = ProductInboundRepository.GetvwProductInboundById(Id.Value);
            if (ProductInbound != null && ProductInbound.IsDeleted != true)
            {
                //Nếu đã ghi sổ rồi thì không được sửa
                if (ProductInbound.IsArchive)
                {
                    return RedirectToAction("Detail", new { Id = ProductInbound.Id });
                }

                var model = new ProductInboundViewModel();
                AutoMapper.Mapper.Map(ProductInbound, model);

                //lấy thông tin mã đơn hàng nếu có và kho đích đến theo mã đơn hàng
                if (model.PurchaseOrderId != null)
                {
                    var purchaseOrder = PurchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId.Value);
                    if (purchaseOrder != null)
                    {
                        model.PurchaseOrderCode = purchaseOrder != null ? purchaseOrder.Code : "";
                        var warehouseDestination = WarehouseRepository.GetWarehouseById(purchaseOrder.WarehouseDestinationId.Value);
                        model.WarehouseDestinationName = warehouseDestination != null ? warehouseDestination.Name : "";
                    }
                }

                // lấy danh sách detail
                var Details = ProductInboundRepository.GetAllProductInboundDetailByInboundId(ProductInbound.Id)
                    .Select(x => new ProductInboundDetailViewModel
                    {
                        Id = x.Id,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        ProductInboundId = x.ProductInboundId,
                        Quantity = x.Quantity,
                        ProductCode = "",
                        ExpiryDate = x.ExpiryDate,
                        LoCode = x.LoCode
                    }).ToList();
                model.DetailList = Details;

                foreach (var item in Details)
                {
                    var product = ProductRepository.GetProductById(item.ProductId);
                    item.ProductName = product != null ? product.Name : "";
                    item.ProductCode = product != null ? product.Code : "";
                    var usedQuantity = WarehouseLocationItemRepository.GetAllLocationItem().Where(x => x.ProductId == item.ProductId && x.ProductInboundId == item.ProductInboundId && x.IsOut == true).ToList().Count();
                    item.QuantityUsed = usedQuantity;
                }

                model.CreatedUserName = userRepository.GetUserById(model.CreatedUserId.Value).FullName;

                var productList = ProductRepository.GetAllProduct()
               .Select(item => new ProductViewModel
               {
                   Code = item.Code,
                   Barcode = item.Barcode,
                   Name = item.Name,
                   Id = item.Id,
                   CategoryCode = item.CategoryCode,
                   PriceInbound = item.PriceInbound,
                   Unit = item.Unit
               });
                ViewBag.productList = productList;

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Edit(ProductInboundViewModel model)
        {
            if (Request["Submit"] == "Save")
            {
                var productInbound = ProductInboundRepository.GetProductInboundById(model.Id);

                //Thêm chi tiết phiếu nhập
                foreach (var item in model.DetailList.Where(item => item.Id == 0))
                {
                    var productInboundDetail = new Domain.Sale.Entities.ProductInboundDetail
                    {
                        ProductInboundId = productInbound.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Unit = item.Unit,
                        Price = item.Price,
                        IsDeleted = false,
                        CreatedUserId = WebSecurity.CurrentUserId,
                        ModifiedUserId = WebSecurity.CurrentUserId,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    };

                    ProductInboundRepository.InsertProductInboundDetail(productInboundDetail);

                    //cập nhật vị trí sản phẩm thêm vào kho cho từng sản phẩm riêng biệt
                    for (int i = 1; i <= item.Quantity; i++)
                    {
                        var warehouseLocationItem = new WarehouseLocationItem();
                        warehouseLocationItem.IsDeleted = false;
                        warehouseLocationItem.CreatedUserId = WebSecurity.CurrentUserId;
                        warehouseLocationItem.ModifiedUserId = WebSecurity.CurrentUserId;
                        warehouseLocationItem.CreatedDate = DateTime.Now;
                        warehouseLocationItem.ModifiedDate = DateTime.Now;
                        warehouseLocationItem.ProductId = item.ProductId;
                        warehouseLocationItem.ProductInboundId = productInbound.Id;
                        warehouseLocationItem.ProductInboundDetailId = item.Id;
                        warehouseLocationItem.IsOut = false;
                        warehouseLocationItem.Shelf = "1";
                        warehouseLocationItem.Floor = "1";
                        warehouseLocationItem.WarehouseId = productInbound.WarehouseDestinationId;
                        WarehouseLocationItemRepository.InsertWarehouseLocationItem(warehouseLocationItem);
                    }
                }

                decimal total = 0;
                foreach (var item in model.DetailList)
                {
                    var productInboundDetail = ProductInboundRepository.GetProductInboundDetailById(item.Id);

                    if (productInboundDetail != null)
                    {
                        int first = productInboundDetail.Quantity;
                        int last = item.Quantity;
                        int qIn = 0;
                        int qOut = 0;
                        int qLocationIn = 0;
                        int qLocationOut = 0;

                        productInboundDetail.ExpiryDate = item.ExpiryDate;
                        productInboundDetail.LoCode = item.LoCode;
                        productInboundDetail.Price = item.Price;
                        productInboundDetail.Quantity = last;

                        ProductInboundRepository.UpdateProductInboundDetail(productInboundDetail);
                        var amount = productInboundDetail.Price * productInboundDetail.Quantity;
                        total += amount;

                        var locationItem = WarehouseLocationItemRepository.GetAllLocationItem().Where(x => x.ProductId == item.ProductId && x.ProductInboundDetailId == item.Id).ToList();

                        for (int i = 0; i < locationItem.Count; i++)
                        {
                            locationItem[i].ExpiryDate = item.ExpiryDate;
                            locationItem[i].LoCode = item.LoCode;
                            if (!string.IsNullOrEmpty(item.LoCode) && item.ExpiryDate.HasValue)
                            {
                                locationItem[i].SN = item.ProductCode + item.LoCode + item.ExpiryDate.Value.ToString("yyyyMMdd") + locationItem[i].Id;
                            }
                            WarehouseLocationItemRepository.UpdateWarehouseLocationItem(locationItem[i]);
                        }

                        if (first > last)
                        {
                            qOut = first - last;
                        }
                        else
                        {
                            qIn = last - first;
                        }

                        if (locationItem.Count > last)
                        {
                            qLocationOut = locationItem.Count - last;
                        }
                        else if (locationItem.Count < last)
                        {
                            qLocationIn = last - locationItem.Count;
                        }

                        //Thêm vị trí nếu số lượng tăng lên
                        if (qLocationIn > 0)
                        {
                            for (int i = 1; i <= qLocationIn; i++)
                            {
                                var warehouseLocationItem = new WarehouseLocationItem();
                                warehouseLocationItem.IsDeleted = false;
                                warehouseLocationItem.CreatedUserId = WebSecurity.CurrentUserId;
                                warehouseLocationItem.ModifiedUserId = WebSecurity.CurrentUserId;
                                warehouseLocationItem.CreatedDate = DateTime.Now;
                                warehouseLocationItem.ModifiedDate = DateTime.Now;
                                warehouseLocationItem.ProductId = item.ProductId;
                                warehouseLocationItem.ProductInboundId = productInbound.Id;
                                warehouseLocationItem.ProductInboundDetailId = item.Id;
                                warehouseLocationItem.IsOut = false;
                                warehouseLocationItem.Shelf = "1";
                                warehouseLocationItem.Floor = "1";
                                warehouseLocationItem.ExpiryDate = item.ExpiryDate;
                                warehouseLocationItem.LoCode = item.LoCode;
                                warehouseLocationItem.WarehouseId = productInbound.WarehouseDestinationId;
                                warehouseLocationItem.SN = (item.ProductCode == null ? "" : item.ProductCode) + (warehouseLocationItem.LoCode == null ? "" : warehouseLocationItem.LoCode) + (warehouseLocationItem.ExpiryDate == null ? "" : warehouseLocationItem.ExpiryDate.Value.ToString("yyyyMMdd")) + (warehouseLocationItem.Position == null ? "" : warehouseLocationItem.Position);
                                WarehouseLocationItemRepository.InsertWarehouseLocationItem(warehouseLocationItem);
                            }
                        }

                        //Còn giảm thì xóa đi
                        if (qLocationOut > 0)
                        {
                            var listGiam = WarehouseLocationItemRepository.GetAllLocationItem().Where(i => i.ProductInboundDetailId == item.Id && i.IsOut == false).Take(qOut).ToList();
                            foreach (var i in listGiam)
                            {
                                WarehouseLocationItemRepository.DeleteWarehouseLocationItem(i.Id);
                            }

                            qOut = listGiam.Count;
                        }

                        //cập nhật SL của từng SP trong invetory của SP đó trong kho đích đến
                        //UpdateInventoryWarehouse(productInbound.WarehouseDestinationId, productInboundDetail.ProductId, qIn, 0, qOut);
                    }
                }

                productInbound.Note = model.Note;
                productInbound.TotalAmount = model.TotalAmount;
                productInbound.TotalVAT = model.TotalVAT;
                productInbound.Total = model.Total;
                ProductInboundRepository.UpdateProductInbound(productInbound);
            }

            return RedirectToAction("Detail", new { Id = model.Id });
        }

        public ActionResult Detail(int? Id, string TransactionCode)
        {
            var ProductInbound = new vwProductInbound();
            if (Id != null)
                ProductInbound = ProductInboundRepository.GetvwProductInboundById(Id.Value);
            if (!string.IsNullOrEmpty(TransactionCode))
                ProductInbound = ProductInboundRepository.GetvwProductInboundByTransactionCode(TransactionCode);

            if (ProductInbound != null && ProductInbound.IsDeleted != true)
            {
                var model = new ProductInboundViewModel();

                AutoMapper.Mapper.Map(ProductInbound, model);
                if (model.PurchaseOrderId != null)
                {
                    var purchase = PurchaseOrderRepository.GetPurchaseOrderById(model.PurchaseOrderId.Value);
                    model.PurchaseOrderCode = purchase.Code;
                    model.PurchaseOrderId = purchase.Id;
                }
                //Kiểm tra cho phép sửa chứng từ này hay không
                model.AllowEdit = Helpers.Common.KiemTraNgaySuaChungTu(model.CreatedDate.Value);

                // lấy danh sách detail
                var Details = ProductInboundRepository.GetAllProductInboundDetailByInboundId(ProductInbound.Id)
                    .Select(x => new ProductInboundDetailViewModel
                    {
                        Id = x.Id,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        ProductInboundId = x.ProductInboundId,
                        Quantity = x.Quantity,
                        ProductCode = "",
                        ExpiryDate = x.ExpiryDate,
                        LoCode = x.LoCode
                    }).ToList();
                model.DetailList = Details;

                foreach (var item in Details)
                {
                    var product = ProductRepository.GetProductById(item.ProductId);
                    item.ProductName = product != null ? product.Name : "";
                    item.ProductCode = product != null ? product.Code : "";
                    var usedQuantity = WarehouseLocationItemRepository.GetAllLocationItem().Where(x => x.ProductId == item.ProductId && x.ProductInboundId == item.ProductInboundId && x.IsOut == true).ToList().Count();
                    item.QuantityUsed = usedQuantity;
                }

                model.CreatedUserName = userRepository.GetUserById(model.CreatedUserId.Value).FullName;

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.FailedMessage = TempData["FailedMessage"];
                return View(model);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Archive(int Id)
        {
            if (Request["Submit"] == "Save")
            {
                var productInbound = ProductInboundRepository.GetProductInboundById(Id);

                //Kiểm tra cho phép sửa chứng từ này hay không
                if (Helpers.Common.KiemTraNgaySuaChungTu(productInbound.CreatedDate.Value) == false)
                {
                    return RedirectToAction("Detail", new { Id = productInbound.Id });
                }
                Archive(ProductInboundRepository, productInbound, TempData);
            }

            return RedirectToAction("Detail", new { Id = Id });
        }

        public static void Archive(IProductInboundRepository productInboundRepository,
    ProductInbound productInbound,
    TempDataDictionary TempData)
        {
            //Cập nhật lại tồn kho cho những sp trong phiếu nhập này
            var detailList = productInboundRepository.GetAllvwProductInboundDetailByInboundId(productInbound.Id)
                .Select(item => new
                {
                    ProductName = item.ProductCode + " - " + item.ProductName,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList();

            //Kiểm tra dữ liệu sau khi bỏ ghi sổ có hợp lệ không
            string check = "";
            foreach (var item in detailList)
            {
                var error = InventoryController.Check(item.ProductName, item.ProductId, productInbound.WarehouseDestinationId.Value, item.Quantity, 0);
                check += error;
            }

            if (string.IsNullOrEmpty(check))
            {
                //Khi đã hợp lệ thì mới update
                foreach (var item in detailList)
                {
                    InventoryController.Update(item.ProductName, item.ProductId, productInbound.WarehouseDestinationId.Value, item.Quantity, 0);
                }

                productInbound.IsArchive = true;
                productInboundRepository.UpdateProductInbound(productInbound);
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.ArchiveSuccess;
            }
            else
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.ArchiveFail + check;
            }
        }

        [HttpPost]
        public ActionResult UnArchive(int Id)
        {
            if (Request["Submit"] == "Save")
            {
                var productInbound = ProductInboundRepository.GetProductInboundById(Id);

                //Kiểm tra cho phép sửa chứng từ này hay không
                if (Helpers.Common.KiemTraNgaySuaChungTu(productInbound.CreatedDate.Value) == false)
                {
                    return RedirectToAction("Detail", new { Id = productInbound.Id });
                }

                //Cập nhật lại tồn kho cho những sp trong phiếu nhập này
                var detailList = ProductInboundRepository.GetAllvwProductInboundDetailByInboundId(productInbound.Id)
                    .Select(item => new
                    {
                        ProductName = item.ProductCode + " - " + item.ProductName,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }).ToList();

                //Kiểm tra dữ liệu sau khi bỏ ghi sổ có hợp lệ không
                string check = "";
                foreach (var item in detailList)
                {
                    var error = InventoryController.Check(item.ProductName, item.ProductId, productInbound.WarehouseDestinationId.Value, 0, item.Quantity);
                    check += error;
                }

                if (string.IsNullOrEmpty(check))
                {
                    //Khi đã hợp lệ thì mới update
                    foreach (var item in detailList)
                    {
                        InventoryController.Update(item.ProductName, item.ProductId, productInbound.WarehouseDestinationId.Value, 0, item.Quantity);
                    }

                    productInbound.IsArchive = false;
                    ProductInboundRepository.UpdateProductInbound(productInbound);
                    TempData[Globals.SuccessMessageKey] = "Đã bỏ ghi sổ";
                }
                else
                {
                    TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.ArchiveFail + check;
                }
            }

            return RedirectToAction("Detail", new { Id = Id });
        }

        public ActionResult UpdateLocationItem(int Id)
        {
            ProductInboundViewModel model = new ProductInboundViewModel();
            var item = ProductInboundRepository.GetAllvwProductInbound().Where(x => x.Id == Id).FirstOrDefault();
            if (item != null)
            {
                AutoMapper.Mapper.Map(item, model);
                model.CreatedUserName = userRepository.GetUserById(model.CreatedUserId.Value).FullName;

                var detailList = ProductInboundRepository.GetAllvwProductInboundDetailByInboundId(item.Id);
                model.DetailList = detailList.Select(x => new ProductInboundDetailViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductInboundId = x.ProductInboundId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    ProductCode = x.ProductCode,
                    LoCode = x.LoCode,
                    ExpiryDate = x.ExpiryDate
                }).ToList();

                foreach (var i in model.DetailList)
                {
                    i.ListWarehouseLocationItemViewModel = new List<WarehouseLocationItemViewModel>();
                    var listLocationItemExits = WarehouseLocationItemRepository.GetAllWarehouseLocationItem().Where(q => q.ProductInboundDetailId == i.Id).ToList();
                    AutoMapper.Mapper.Map(listLocationItemExits, i.ListWarehouseLocationItemViewModel);
                }

                //foreach(var g in listLocationItemExits.GroupBy(x => x.ProductId))
                //{
                //    int qty = g.Count();
                //    var itemDetail = model.DetailList.Where(x => x.ProductId == g.FirstOrDefault().ProductId).FirstOrDefault();
                //    if (itemDetail.Quantity == qty)
                //    {
                //        model.DetailList.RemoveAll(x => x.ProductId == g.FirstOrDefault().ProductId);
                //    }
                //    else
                //    {
                //        model.DetailList.Where(x => x.ProductId == g.FirstOrDefault().ProductId).FirstOrDefault().Quantity = itemDetail.Quantity - qty;
                //    }
                //}
            }

            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            if (model.DetailList.Count == 0)
            {
                TempData[Globals.FailedMessageKey] = "Danh sách sản phẩm trong phiếu nhập [" + model.Code + "] đã được nhập vị trí đầy đủ.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateLocationItem(InboundLocationItemsViewModel model)
        {
            //var list = model.LocationItemList
            //    .Where(item => string.IsNullOrEmpty(item.Shelf)
            //        || string.IsNullOrEmpty(item.Floor)
            //        || item.ExpiryDate == null
            //    );
            foreach (var item in model.LocationItemList)
            {
                var warehouseLocationItem = WarehouseLocationItemRepository.GetWarehouseLocationItemById(item.Id.Value);
                warehouseLocationItem.Shelf = item.Shelf;
                warehouseLocationItem.Floor = item.Floor;
                warehouseLocationItem.ExpiryDate = item.ExpiryDate;
                warehouseLocationItem.LoCode = item.LoCode;
                if (!string.IsNullOrEmpty(item.LoCode) && item.ExpiryDate != null)
                {
                    warehouseLocationItem.SN = item.ProductCode + item.LoCode + item.ExpiryDate.Value.ToString("yyyyMMdd") + warehouseLocationItem.Id;
                }
                WarehouseLocationItemRepository.UpdateWarehouseLocationItem(warehouseLocationItem);
            }

            //WarehouseLocationItemRepository.InsertWarehouseLocationItem(list);

            //TempData[Globals.SuccessMessageKey] = "Thêm thành công " + list.Count + " sản phẩm có dữ liệu vị trí.";
            return Redirect("/ProductInbound/Detail/" + model.ProductInboundId);

            //TempData[Globals.FailedMessageKey] = "Bạn chưa điền dữ liệu vị trí cho tất cả dòng.";
            //return RedirectToAction("UpdateLocationItem", new { Id = model.ProductInboundId });
        }

        public ActionResult Print(int? Id)
        {
            var ProductInbound = ProductInboundRepository.GetvwProductInboundById(Id.Value);
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " /></div>";
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            if (ProductInbound != null && ProductInbound.IsDeleted != true)
            {
                var model = new TemplatePrintViewModel();
                var user = userRepository.GetUserById(ProductInbound.CreatedUserId.Value);
                var inboundDetails = ProductInboundRepository.GetAllProductInboundDetailByInboundId(Id.Value).AsEnumerable()
                        .Select(x => new ProductInboundDetailViewModel
                        {
                            Id = x.Id,
                            Price = x.Price,
                            ProductId = x.ProductId,
                            ProductName = ProductRepository.GetProductById(x.ProductId).Name,
                            ProductCode = ProductRepository.GetProductById(x.ProductId).Code,
                            ProductInboundId = x.ProductInboundId,
                            Quantity = x.Quantity,
                            Unit = x.Unit

                        }).ToList();
                var ListRow = "";
                foreach (var item in inboundDetails)
                {
                    decimal? subTotal = item.Quantity * item.Price;
                    var Row = "<tr>"
                     + "<td class=\"text-center\">" + (inboundDetails.ToList().IndexOf(item) + 1) + "</td>"
                     + "<td>" + item.ProductCode + "</td>"
                     + "<td>" + item.ProductName + "</td>"
                     + "<td class=\"text-center\">" + item.Unit + "</td>"
                     + "<td class=\"text-right\">" + item.Quantity + "</td>"
                     + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(item.Price) + "</td>"
                     + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(subTotal) + "</td></tr>";
                    ListRow += Row;
                }
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
                    + " .logo{ width: 100px;float: left; margin: 0 0px;height: 100px;line-height: 100px;}"
                    + ".logo img{ width:100%;vertical-align: middle;}"
                    + "</style>";

                var total = ProductInbound.TotalAmount != null ? ProductInbound.TotalAmount : 0;

                var table = style + "<table class=\"invoice-detail\"><thead><tr> <th>STT</th> <th>Mã hàng</th><th>Tên mặt hàng</th><th>ĐVT</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr></thead><tbody>"
                             + ListRow
                             + "</tbody> <tfoot><tr><td colspan=\"6\" class=\"text-center\">Tổng cộng</td><td class=\"text-right\">"
                             + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(total)
                             + "</td></tr></tfoot></table>";
                var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("ProductInbound")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                model.Content = template.Content;
                model.Content = model.Content.Replace("{Code}", ProductInbound.Code);
                model.Content = model.Content.Replace("{Day}", ProductInbound.CreatedDate.Value.Day.ToString());
                model.Content = model.Content.Replace("{Month}", ProductInbound.CreatedDate.Value.Month.ToString());
                model.Content = model.Content.Replace("{Year}", ProductInbound.CreatedDate.Value.Year.ToString());
                model.Content = model.Content.Replace("{ShipperName}", ProductInbound.ShipperName);
                model.Content = model.Content.Replace("{Phone}", ProductInbound.ShipperPhone);
                model.Content = model.Content.Replace("{Supplier}", ProductInbound.SupplierName);
                model.Content = model.Content.Replace("{Note}", ProductInbound.Note);
                model.Content = model.Content.Replace("{DataTable}", table);
                model.Content = model.Content.Replace("{Logo}", ImgLogo);
                model.Content = model.Content.Replace("{System.CompanyName}", company);
                model.Content = model.Content.Replace("{System.AddressCompany}", address);
                model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
                model.Content = model.Content.Replace("{System.Fax}", fax);
                model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
                model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateData()
        {
            var productInboundDetail = ProductInboundRepository.GetAllvwProductInboundDetail();
            foreach (var item in productInboundDetail)
            {
                var locationItem = WarehouseLocationItemRepository.GetAllLocationItem().Where(x => x.ProductId == item.ProductId && x.ProductInboundDetailId == item.Id).ToList();

                for (int i = 0; i < locationItem.Count(); i++)
                {
                    locationItem[i].ExpiryDate = item.ExpiryDate;
                    locationItem[i].LoCode = item.LoCode;
                    if (!string.IsNullOrEmpty(item.LoCode) && item.ExpiryDate.HasValue)
                    {
                        locationItem[i].SN = item.ProductCode + item.LoCode + item.ExpiryDate.Value.ToString("yyyyMMdd") + locationItem[i].Id;
                    }
                    WarehouseLocationItemRepository.UpdateWarehouseLocationItem(locationItem[i]);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult checkExitsCode(string code, int? id)
        {
            var productinboud = ProductInboundRepository.GetProductInboundByCode(code);
            if (productinboud != null)
            {
                if (id != null && id.Value > 0)
                {
                    var data = ProductInboundRepository.GetProductInboundById(id.Value);
                    if (data.Id == productinboud.Id)
                    {
                        return Content("success");
                    }
                    return Content("error");
                }
                return Content("error");
            }
            return Content("success");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            if (Request["Submit"] == "Delete")
            {
                var productInbound = ProductInboundRepository.GetProductInboundById(Id);
                productInbound.IsDeleted = true;
                productInbound.ModifiedDate = DateTime.Now;
                productInbound.ModifiedUserId = WebSecurity.CurrentUserId;
                ProductInboundRepository.UpdateProductInbound(productInbound);

                var listDetail = ProductInboundRepository.GetAllProductInboundDetailByInboundId(Id).ToList();

                foreach (var productInboundDetail in listDetail)
                {
                    productInboundDetail.IsDeleted = true;
                    productInboundDetail.ModifiedDate = DateTime.Now;
                    productInboundDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                    ProductInboundRepository.UpdateProductInboundDetail(productInboundDetail);
                }

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
