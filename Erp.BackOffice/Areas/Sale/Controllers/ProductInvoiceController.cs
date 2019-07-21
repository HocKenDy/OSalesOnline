using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Account.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Sale.Repositories;
using Erp.Domain.Account.Interfaces;
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
using Erp.BackOffice.Account.Controllers;
using Erp.Domain.Account.Entities;
using System.Xml.Linq;
using Erp.BackOffice.Areas.Cms.Models;
using Erp.Domain.Entities;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Web;
using Erp.BackOffice.Crm.Controllers;
using qts.webapp.domain.Repositories;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class ProductInvoiceController : Controller
    {
        private readonly ITransactionLiabilitiesRepository transactionLiabilitiesRepository;
        private readonly IReceiptRepository ReceiptRepository;
        private readonly IProductInvoiceRepository productInvoiceRepository;
        private readonly IUsingServiceRepository UsingServiceRepository;
        private readonly ICommisionRepository CommisionRepository;
        private readonly IProductRepository ProductRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IInventoryRepository InventoryRepository;
        private readonly IProductOutboundRepository ProductOutboundRepository;
        private readonly IUserRepository userRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IWarehouseLocationItemRepository WarehouseLocationItemRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICommisionCustomerRepository commisionCustomerRepository;
        private readonly IReceiptDetailRepository receiptDetailRepository;
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly ISalesReturnsRepository saleReturnRepository;
        public ProductInvoiceController(
            ITransactionLiabilitiesRepository _transactionLiabilities
            , IReceiptRepository _Receipt
            , IProductInvoiceRepository _ProductInvoice
            , IUsingServiceRepository _UsingService
            , ICommisionRepository _Commision
            , IProductRepository _Product
            , ICustomerRepository _Customer
            , IInventoryRepository _Inventory
            , IProductOutboundRepository _ProductOutbound
            , IUserRepository _user
            , ITemplatePrintRepository _templatePrint
            , ICategoryRepository category
            , IWarehouseLocationItemRepository _WarehouseLocationItem
            , ITransactionRepository _transaction
            , ICommisionCustomerRepository _Commision_Customer
            , ISalesReturnsRepository _saleReturn
             , IReceiptDetailRepository _ReceiptDetail
            , IWarehouseRepository _Warehouse
            )
        {
            transactionLiabilitiesRepository = _transactionLiabilities;
            ReceiptRepository = _Receipt;
            productInvoiceRepository = _ProductInvoice;
            UsingServiceRepository = _UsingService;
            CommisionRepository = _Commision;
            ProductRepository = _Product;
            InventoryRepository = _Inventory;
            ProductOutboundRepository = _ProductOutbound;
            customerRepository = _Customer;
            userRepository = _user;
            templatePrintRepository = _templatePrint;
            categoryRepository = category;
            WarehouseLocationItemRepository = _WarehouseLocationItem;
            transactionRepository = _transaction;
            commisionCustomerRepository = _Commision_Customer;
            receiptDetailRepository = _ReceiptDetail;
            saleReturnRepository = _saleReturn;
            WarehouseRepository = _Warehouse;
        }

        #region Funtion Logic
        #endregion

        #region Index By Customer
        public ViewResult IndexByCustomer(int? customerId, int? take)
        {

            IQueryable<ProductInvoiceViewModel> model = productInvoiceRepository.GetAllvwInvoiceByCustomer(customerId).Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Code = item.Code,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ShipCityName = item.ShipCityName,
                TotalAmount = item.TotalAmount,
                Discount = item.Discount,
                TaxFee = item.TaxFee,
                CodeInvoiceRed = item.CodeInvoiceRed,
                Status = item.Status,
                IsArchive = item.IsArchive,
                ProductOutboundId = item.ProductOutboundId,
                ProductOutboundCode = item.ProductOutboundCode,
                Note = item.Note,
                CancelReason = item.CancelReason,
                AccumulatedPoint = item.AccumulatedPoint,
                UsePoint = item.UsePoint,
                Frequency = item.Frequency ?? 0

            }).OrderByDescending(m => m.CreatedDate).AsQueryable();
            if (take != null)
            {
                model = model.Take(take.Value);
            }
            return View(model);
        }
        #endregion

        #region Index

        public ViewResult Index(int? BranchId, string txtCode, string txtMinAmount, string txtMaxAmount, string txtCusName,  string startDate, string endDate)
        {
            var q = productInvoiceRepository.GetAllvwProductInvoiceFull();
            if (!SecurityFilter.IsAdmin())
            {
                q = q.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            }
            else
            {
                if (BranchId != null)
                {
                    q = q.Where(x => x.BranchId == BranchId);
                }
            }
            if (!string.IsNullOrEmpty(txtCode))
            {
                q = q.Where(x => x.Code.Contains(txtCode));
            }

            if (!string.IsNullOrEmpty(txtCusName))
            {
                txtCusName = Helpers.Common.ChuyenThanhKhongDau(txtCusName);
                q = q.Where(x => x.CustomerNameSearch.Contains(txtCusName));
            }
            //Lọc theo ngày
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

            var model = q.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Code = item.Code,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ShipCityName = item.ShipCityName,
                TotalAmount = item.TotalAmount,
                Discount = item.Discount,
                TaxFee = item.TaxFee,
                CodeInvoiceRed = item.CodeInvoiceRed,
                Status = item.Status,
                IsArchive = item.IsArchive,
                ProductOutboundId = item.ProductOutboundId,
                ProductOutboundCode = item.ProductOutboundCode,
                Note = item.Note,
                CancelReason = item.CancelReason,
                BranchName = item.BranchName
            }).OrderByDescending(m => m.ModifiedDate);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            return View(model);
        }
        #endregion

        #region Create
        public ActionResult Create(int? Id)
        {
            ProductInvoiceViewModel model = new ProductInvoiceViewModel();
            model.DetailList = new List<ProductInvoiceDetailViewModel>();
            if (Id.HasValue && Id > 0)
            {
                var productInvoice = productInvoiceRepository.GetvwProductInvoiceById(Id.Value);

                //Nếu đã ghi sổ rồi thì không được sửa
                if (productInvoice.IsArchive)
                {
                    ViewBag.FailedMessage = "Đơn hàng đã ghi sổ!";
                    return RedirectToAction("Detail", new { Id = productInvoice.Id });
                }
                //Kiểm tra xem nếu có xuất kho rồi thì return
                var checkProductOutbound = ProductOutboundRepository.GetAllProductOutbound()
                        .Where(item => item.InvoiceId == productInvoice.Id).FirstOrDefault();
                if (checkProductOutbound != null)
                {
                    ViewBag.FailedMessage = "Đơn hàng đã xuất kho!";
                    return RedirectToAction("Detail", new { Id = productInvoice.Id });
                }

                AutoMapper.Mapper.Map(productInvoice, model);

                var detailList = productInvoiceRepository.GetAllvwInvoiceDetailsByInvoiceId(productInvoice.Id).ToList();
                AutoMapper.Mapper.Map(detailList, model.DetailList);
                model.IsEdited = true;
            }
            else
            {
                //Mặc định chọn khách vãng lai, nhân viên khi thêm mới, nếu có cài đặt == true
                string sale_auto_select_default_customer = Erp.BackOffice.Helpers.Common.GetSetting("sale_auto_select_default_customer");
                if (sale_auto_select_default_customer == "true")
                {
                    //Khách vãng lai
                    string id_default_customer = Helpers.Common.GetSetting("id_default_customer");
                    int deaultCustomerId = string.IsNullOrEmpty(id_default_customer) ? 0 : Convert.ToInt32(id_default_customer);
                    var deaultCustomer = customerRepository.GetCustomerById(deaultCustomerId);

                    if (deaultCustomer != null)
                    {
                        model.CustomerId = deaultCustomer.Id;
                        model.CustomerName = deaultCustomer.Name;
                    }

                    //Nhân viên
                    string id_default_user = Helpers.Common.GetSetting("id_default_user");
                    int deaultUserId = string.IsNullOrEmpty(id_default_user) ? 0 : Convert.ToInt32(id_default_user);
                    var deaultUser = userRepository.GetUserById(deaultUserId);

                    if (deaultUser != null)
                    {
                        model.SalerId = deaultUser.Id;
                    }
                    
                }
                model.WarehouseSourceId = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.IsSale == true).FirstOrDefault()?.Id;
                model.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductInvoice");
                model.CreatedUserName = Erp.BackOffice.Helpers.Common.CurrentUser.FullName;
                model.BranchId = Helpers.Common.CurrentUser.BranchId;
                model.SalerId = Helpers.Common.CurrentUser.Id;
                int taxfee = 0;
                int.TryParse(Helpers.Common.GetSetting("vat"), out taxfee);
                model.TaxFee = taxfee;
                model.TotalNoVAT = 0;
                model.UsePoint = 0;
                model.UsePointAmount = 0;
                model.CheckUsePoint = false;
                model.IsEdited = false;
            }

            var saleDepartmentCode = Erp.BackOffice.Helpers.Common.GetSetting("SaleDepartmentCode");
            string image_folder = Helpers.Common.GetSetting("product-image-folder");

            var productList = ProductRepository.GetAllvwProduct().Where(x => x.Type == "service" || x.QuantityTotalInventory > 0 || x.NoInbound == true)
               .Select(item => new ProductViewModel
               {
                   Type = item.Type,
                   Code = item.Code,
                   Barcode = item.Barcode,
                   Name = item.Name,
                   Id = item.Id,
                   CategoryCode = item.CategoryCode,
                   PriceOutbound = item.PriceOutbound,
                   Unit = item.Unit,
                   QuantityTotalInventory = item.QuantityTotalInventory == null ? 0 : item.QuantityTotalInventory,
                   Image_Name = image_folder + item.Image_Name,
                   IsServicePackage = item.IsServicePackage,
                   ServicesChild = item.ServicesChild,
                   TargetPoint = item.TargetPoint,
                   Point = item.Point
               }).ToList();

            //foreach (var item in productList.Where(x => string.IsNullOrEmpty(x.ServicesChild) == false))
            //{
            //    XElement root = XElement.Parse(item.ServicesChild);
            //    IEnumerable<XElement> Services = from el in root.Elements("Service") select el;

            //    string services_name = string.Empty;
            //    foreach (var el in Services)
            //    {
            //        var serviceItem = productList.Where(x => x.Id == (int)el.Attribute("ProductId")).FirstOrDefault();
            //        if (serviceItem != null)
            //            services_name += serviceItem.Name + " (" + el.Attribute("Quantity").Value + "),";
            //    }

            //    item.ServicesChild = services_name.TrimEnd(',');
            //}

            //ViewBag.productList = productList;

            //Thêm số lượng tồn kho cho chi tiết đơn hàng đã được thêm
            if (model.DetailList != null && model.DetailList.Count > 0)
            {
                foreach (var item in model.DetailList)
                {
                    var product = productList.Where(i => i.Id == item.ProductId).FirstOrDefault();
                    if (product != null)
                    {
                        item.QuantityInInventory = product.QuantityTotalInventory;
                        item.ProductCode = product.Code;
                        item.PriceTest = product.PriceOutbound;
                    }
                    else
                    {
                        item.Id = 0;
                    }
                }
                model.DetailList.RemoveAll(x => x.Id == 0);
                int n = 0;
                foreach (var item in model.DetailList)
                {
                    item.OrderNo = n;
                    n++;
                }
            }


            model.ReceiptViewModel = new ReceiptViewModel();
            model.NextPaymentDate_Temp = DateTime.Now.AddDays(30);
            model.ReceiptViewModel.Name = "Bán hàng - Thu tiền mặt";
            model.ReceiptViewModel.Amount = 0;
            model.ReceiptViewModel.PaymentMethod = "Tiền mặt";
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProductInvoiceViewModel model)
        {
            if (ModelState.IsValid && model.DetailList.Count != 0)
            {
                ProductInvoice productInvoice = null;
                if (model.Id > 0)
                {
                    productInvoice = productInvoiceRepository.GetProductInvoiceById(model.Id);
                }

                if (productInvoice != null)
                {
                    //Nếu đã ghi sổ rồi thì không được sửa
                    if (productInvoice.IsArchive)
                    {
                        return RedirectToAction("Detail", new { Id = productInvoice.Id });
                    }

                    //Kiểm tra xem nếu có xuất kho rồi thì return
                    var checkProductOutbound = ProductOutboundRepository.GetAllProductOutbound()
                            .Where(item => item.InvoiceId == productInvoice.Id).FirstOrDefault();
                    if (checkProductOutbound != null)
                    {
                        return RedirectToAction("Detail", new { Id = productInvoice.Id });
                    }

                    AutoMapper.Mapper.Map(model, productInvoice);
                }
                else
                {
                    productInvoice = new ProductInvoice();
                    AutoMapper.Mapper.Map(model, productInvoice);
                    productInvoice.IsDeleted = false;
                    productInvoice.CreatedUserId = WebSecurity.CurrentUserId;
                    productInvoice.CreatedDate = DateTime.Now;
                    productInvoice.Status = Wording.OrderStatus_pending;
                    productInvoice.BranchId = Helpers.Common.CurrentUser.BranchId;
                    productInvoice.IsArchive = false;
                    productInvoice.IsReturn = false;
                }

                //Duyệt qua danh sách sản phẩm mới xử lý tình huống user chọn 2 sản phầm cùng id
                List<ProductInvoiceDetail> listNewCheckSameId = new List<ProductInvoiceDetail>();
                foreach (var group in model.DetailList)
                {
                    var product = ProductRepository.GetProductById(group.ProductId.Value);
                    listNewCheckSameId.Add(new ProductInvoiceDetail
                    {
                        ProductId = product.Id,
                        ProductType = product.Type,
                        Quantity = group.Quantity,
                        QuantitySerivceRemaining = 0,
                        Unit = product.Unit,
                        Price = group.Price,
                        PromotionDetailId = group.PromotionDetailId,
                        PromotionId = group.PromotionId,
                        PromotionValue = group.PromotionValue,
                        IsDeleted = false,
                        CreatedUserId = WebSecurity.CurrentUserId,
                        ModifiedUserId = WebSecurity.CurrentUserId,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        DisCount = group.DisCount,
                        QuantitySaleReturn = group.Quantity,
                        CheckPromotion = (group.CheckPromotion.HasValue ? group.CheckPromotion.Value : false),
                        IsReturn = false,
                        Point = group.Point
                    });
                }

                //Tính lại chiết khấu
                foreach (var item in listNewCheckSameId)
                {
                    var thanh_tien = item.Quantity * item.Price;
                    item.DisCountAmount = Convert.ToInt32(item.DisCount * thanh_tien / 100);
                }
                //Tính điểm tích lũy

                //productInvoice.TotalAmount = listNewCheckSameId.Sum(item => item.Quantity * item.Price - item.DisCountAmount) + listNewCheckSameId.Sum(item => item.Quantity * item.Price - item.DisCountAmount) * Convert.ToDecimal(productInvoice.TaxFee / 100);
                productInvoice.AccumulatedPoint = listNewCheckSameId.Sum(x => x.Point);
                if (model.CheckUsePoint == false)
                {
                    productInvoice.UsePoint = 0;
                }
                productInvoice.IsReturn = false;
                productInvoice.ModifiedUserId = WebSecurity.CurrentUserId;
                productInvoice.ModifiedDate = DateTime.Now;
                productInvoice.PaidAmount = 0;
                productInvoice.RemainingAmount = productInvoice.TotalAmount;
                productInvoice.PaymentMethod = model.ReceiptViewModel.PaymentMethod;
                if (listNewCheckSameId.Where(x => x.ProductType == ProductType.Service).Count() <= 0)
                {
                    productInvoice.Type = ProductInvoiceType.Product;
                }
                else if (listNewCheckSameId.Where(x => x.ProductType == ProductType.Product).Count() <= 0)
                {
                    productInvoice.Type = ProductInvoiceType.Service;
                }
                else
                {
                    productInvoice.Type = ProductInvoiceType.All;
                }

                if (model.Id > 0)
                {
                    productInvoiceRepository.UpdateProductInvoice(productInvoice);
                    var listDetail = productInvoiceRepository.GetAllInvoiceDetailsByInvoiceId(productInvoice.Id).AsEnumerable();
                    productInvoiceRepository.DeleteProductInvoiceDetail(listDetail);

                    foreach (var item in listNewCheckSameId)
                    {
                        item.ProductInvoiceId = productInvoice.Id;
                        productInvoiceRepository.InsertProductInvoiceDetail(item);
                    }

                    //Thêm vào quản lý chứng từ
                    TransactionController.Create(new TransactionViewModel
                    {
                        TransactionModule = "ProductInvoice",
                        TransactionCode = productInvoice.Code,
                        TransactionName = "Bán hàng"
                    });
                }
                else
                {
                    productInvoiceRepository.InsertProductInvoice(productInvoice, listNewCheckSameId);

                    //Cập nhật lại mã hóa đơn
                    productInvoice.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductInvoice", model.Code);
                    productInvoiceRepository.UpdateProductInvoice(productInvoice);
                    Erp.BackOffice.Helpers.Common.SetOrderNo("ProductInvoice");
                    //Thêm vào quản lý chứng từ
                    TransactionController.Create(new TransactionViewModel
                    {
                        TransactionModule = "ProductInvoice",
                        TransactionCode = productInvoice.Code,
                        TransactionName = "Bán hàng"
                    });
                    PostController.SavePost(productInvoice.Id, "ProductInvoice", "Bán hàng (" + productInvoice.Code + ")");
                }
                //Tạo phiếu nhập, nếu là tự động
                string sale_tu_dong_tao_chung_tu = Erp.BackOffice.Helpers.Common.GetSetting("sale_auto_outbound");
                if (sale_tu_dong_tao_chung_tu == "true")
                {
                    ProductOutboundViewModel productOutboundViewModel = new ProductOutboundViewModel();

                    //Nếu trong đơn hàng có sản phẩm thì xuất kho
                    if (model.WarehouseSourceId != null && listNewCheckSameId.Where(x => x.ProductType == ProductType.Product).Count() > 0)
                    {
                        productOutboundViewModel.InvoiceId = productInvoice.Id;
                        productOutboundViewModel.InvoiceCode = productInvoice.Code;
                        productOutboundViewModel.WarehouseSourceId = model.WarehouseSourceId;
                        productOutboundViewModel.Note = "Xuất kho cho đơn hàng " + productInvoice.Code;
                        // lấy các danh sách không Xuất kho
                        var productNoInbound = ProductRepository.GetAllProduct().Where(x => x.NoInbound == true).ToList();
                        //Lấy dữ liệu cho chi tiết
                        productOutboundViewModel.DetailList = new List<ProductOutboundDetailViewModel>();
                        AutoMapper.Mapper.Map(model.DetailList.Where(x => x.ProductType == ProductType.Product && !productNoInbound.Any(y => y.Id == x.ProductId)), productOutboundViewModel.DetailList);

                        var productOutbound = ProductOutboundController.CreateFromInvoice(ProductOutboundRepository, productOutboundViewModel, productInvoice.Code, TempData);
                        PostController.SavePost(productInvoice.Id, "ProductInvoice", "Xuất kho bán hàng (" + productOutbound.Code + ")");
                    }
                    //Ghi sổ chứng từ bán hàng
                    model.Id = productInvoice.Id;
                    Archive(model);
                }
                return RedirectToAction("Detail", new { Id = productInvoice.Id });
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region LoadProductItem
        public PartialViewResult LoadProductItem(int OrderNo
            , int ProductId
            , string ProductName
            , string Unit
            , int Quantity
            , decimal Price
            , string ProductCode
            , string ProductType
            , string CategoryCode
            , int QuantityInventory
            , bool? CheckPromotion
            , int? CustomerId
            , double? ProductTargetPoint
            , int? ProductPoint
            , string Images
            )
        {
            var model = new ProductInvoiceDetailViewModel();
            model.OrderNo = OrderNo;
            model.ProductId = ProductId;
            model.ProductName = ProductName;
            model.Unit = Unit;
            model.Quantity = Quantity;
            model.Price = Price;
            model.ProductCode = ProductCode;
            model.ProductType = ProductType;
            model.CategoryCode = CategoryCode;
            model.QuantityInInventory = QuantityInventory;
            model.DisCountAmount = 0;
            model.CheckPromotion = CheckPromotion;
            model.PriceTest = Price;
            model.ProductTargetPoint = ProductTargetPoint;
            model.ProductPoint = ProductPoint;
            var commision = commisionCustomerRepository.GetAllCommisionCustomer()
               .Where(item => item.ProductId == ProductId && item.CustomerId == CustomerId && item.IsMoney != true).FirstOrDefault();
            if (commision == null)
            {
                model.DisCount = 0;
            }
            else
            {
                model.DisCount = Convert.ToInt32(commision.CommissionValue);

            }
            model.Images = Images;
            return PartialView(model);
        }
        #endregion

        #region Next
        public ActionResult Next(int? Id)
        {
            var ProductInvoice = productInvoiceRepository.GetvwProductInvoiceById(Id.Value);
            if (ProductInvoice != null && ProductInvoice.IsDeleted != true)
            {
                var model = new ProductInvoiceViewModel();
                AutoMapper.Mapper.Map(ProductInvoice, model);

                //if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                //{
                //    TempData["FailedMessage"] = "NotOwner";
                //    return RedirectToAction("Index");
                //}

                // lấy danh sách invoice detail
                var invoiceDetails = productInvoiceRepository.GetAllInvoiceDetailsByInvoiceId(Id.Value)
                    .Select(x => new ProductInvoiceDetailViewModel
                    {
                        Id = x.Id,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        ProductName = ProductRepository.GetProductById(x.ProductId.Value).Name,
                        ProductInvoiceId = x.ProductInvoiceId,
                        Quantity = x.Quantity,
                        Unit = x.Unit
                    }).ToList();

                //kiểm tra số lượng sản phẩm đa chọn trong tồn kho
                bool flagNotEnoughInInventory = true;
                foreach (var detailItem in invoiceDetails)
                {
                    var inventoryOfItem = InventoryRepository.GetAllInventoryByProductId(detailItem.ProductId.Value);
                    if (inventoryOfItem != null)
                    {
                        detailItem.QuantityInInventory = inventoryOfItem.Sum(x => x.Quantity);
                        if (detailItem.QuantityInInventory < detailItem.Quantity)
                        {
                            flagNotEnoughInInventory = false;
                            break;
                        }
                    }
                    else
                    {
                        flagNotEnoughInInventory = false;
                        break;
                    }

                    var product = ProductRepository.GetProductById(detailItem.ProductId.Value);
                    detailItem.ProductName = product.Name;

                }
                ViewBag.flagNotEnoughInInventory = flagNotEnoughInInventory;

                //gán danh sách sản phẩm vào model
                model.DetailList = invoiceDetails;


                var productList = ProductRepository.GetAllProduct()
                .Select(item => new ProductViewModel
                {
                    Code = item.Code,
                    Barcode = item.Barcode,
                    Name = item.Name,
                    Id = item.Id,
                    CategoryCode = item.CategoryCode,
                    PriceOutbound = item.PriceOutbound,
                    Unit = item.Unit
                });

                ViewBag.productList = productList;

                var customerList = customerRepository.GetAllCustomer()
                   .Select(item => new CustomerViewModel
                   {
                       Id = item.Id,
                       Name = item.Name,
                       Code = item.Code
                   });

                ViewBag.customerList = customerList;

                ViewBag.flagEdit = true;

                var user = userRepository.GetUserById(model.CreatedUserId.Value);
                model.CreatedUserName = user.FullName;

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        #endregion

        #region Print
        public ActionResult Print(int Id, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            //lấy hóa đơn.
            var productInvoice = productInvoiceRepository.GetvwProductInvoiceById(Id);
            //lấy thông tin khách hàng
            var customer = customerRepository.GetvwCustomerByCode(productInvoice.CustomerCode);
            //lấy người lập phiếu xuất kho
            var user = userRepository.GetUserById(productInvoice.SalerId.Value);
            List<ProductInvoiceDetailViewModel> detailList = new List<ProductInvoiceDetailViewModel>();
            if (productInvoice != null && productInvoice.IsDeleted != true)
            {
                //lấy danh sách sản phẩm xuất kho
                detailList = productInvoiceRepository.GetAllvwInvoiceDetailsByInvoiceId(Id)
                        .Select(x => new ProductInvoiceDetailViewModel
                        {
                            Id = x.Id,
                            Price = x.Price,
                            ProductId = x.ProductId,
                            ProductName = x.ProductName,
                            ProductCode = x.ProductCode,
                            Quantity = x.Quantity,
                            Unit = x.Unit,
                            DisCount = x.DisCount.HasValue ? x.DisCount.Value : 0,
                            DisCountAmount = x.DisCountAmount.HasValue ? x.DisCountAmount : 0,
                            ProductGroup = x.ProductGroup,
                            CheckPromotion = x.CheckPromotion,
                            Description = x.Description
                        }).ToList();
            }
            if (productInvoice.ProductOutboundId != null && productInvoice.ProductOutboundId.Value > 0)
            {
                var ProductOutbound = ProductOutboundRepository.GetvwProductOutboundById(productInvoice.ProductOutboundId.Value);
                var listProductOutboundDetail = ProductOutboundRepository.GetAllProductOutboundDetailByOutboundId(ProductOutbound.Id).ToList();
                foreach (var item in detailList)
                {
                    foreach (var i in listProductOutboundDetail.Where(x => x.ProductId == item.ProductId))
                    {
                        var locationItem = WarehouseLocationItemRepository.GetAllLocationItem().Where(x => x.ProductOutboundDetailId == i.Id && x.ProductId == i.ProductId).OrderBy(x => x.ExpiryDate).FirstOrDefault();
                        if (locationItem != null)
                        {
                            item.ExpiryDate = locationItem.ExpiryDate;
                            item.LoCode = locationItem.LoCode;
                        }
                    }

                }
            }
            //tạo dòng của table html danh sách sản phẩm.
            var ListRow = "";
            int tong_tien = 0;
            int da_thanh_toan = 0;
            int con_lai = 0;
            var groupProduct = detailList.GroupBy(x => new { x.ProductGroup }, (key, group) => new ProductInvoiceDetailViewModel
            {
                ProductGroup = key.ProductGroup,
                ProductId = group.FirstOrDefault().ProductId,
                Id = group.FirstOrDefault().Id
            }).ToList();
            var Rows = "";
            var ProductGroupName = new Category();
            foreach (var i in groupProduct)
            {
                var count = detailList.Where(x => x.ProductGroup == i.ProductGroup).ToList();
                var chiet_khau1 = count.Sum(x => x.DisCountAmount.HasValue ? x.DisCountAmount.Value : 0);
                decimal? subTotal1 = count.Sum(x => (x.Quantity) * (x.Price));
                var thanh_tien1 = subTotal1 - chiet_khau1;
                if (!string.IsNullOrEmpty(i.ProductGroup))
                {
                    ProductGroupName = categoryRepository.GetCategoryByCode("Categories_product").Where(x => x.Value == i.ProductGroup).FirstOrDefault();

                    Rows = "<tr style=\"background:#eee;font-weight:bold\"><td colspan=\"6\" class=\"text-left\">" + (i.ProductGroup == null ? "" : i.ProductGroup) + ": " + (ProductGroupName.Name == null ? "" : ProductGroupName.Name) + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(count.Sum(x => x.Quantity)).Replace(".", ",")
                         + "</td><td colspan=\"3\" class=\"text-right\"></td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(thanh_tien1).Replace(".", ",")
                         + "</td></tr>";
                }
                ListRow += Rows;
                int index = 1;
                foreach (var item in detailList.Where(x => x.ProductGroup == i.ProductGroup))
                {
                    decimal? subTotal = item.Quantity * item.Price.Value;
                    var chiet_khau = item.DisCountAmount.HasValue ? item.DisCountAmount.Value : 0;
                    var thanh_tien = subTotal - chiet_khau;

                    var Row = "<tr>"
                     + "<td class=\"text-center\">" + (index++) + "</td>"
                     + "<td class=\"text-right\">" + item.ProductCode + "</td>"
                     //+ "<td class=\"text-left\">" + item.ProductName + "<p><em>" + item.Description + "</em></p>" + (item.CheckPromotion == true ? " (Khuyến mãi)" : "") + "</td>"
                     + "<td class=\"text-left\">" + item.ProductName + " " + Helpers.Common.StripHTML(item.Description) + (item.CheckPromotion == true ? " (Khuyến mãi)" : "") + "</td>"
                     + "<td class=\"text-center\">" + (item.LoCode == null ? "" : item.LoCode) + "</td>"
                     + "<td class=\"text-center\">" + (item.ExpiryDate == null ? "" : item.ExpiryDate.Value.ToShortDateString()) + "</td>"
                     + "<td class=\"text-center\">" + item.Unit + "</td>"
                     + "<td class=\"text-right\">" + item.Quantity.Value + "</td>"
                     + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(item.Price).Replace(".", ",") + "</td>"
                     + "<td class=\"text-right\">" + (item.DisCount.HasValue ? item.DisCount.Value : 0) + "</td>"
                     + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(chiet_khau).Replace(".", ",") + "</td>"
                     + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(thanh_tien).Replace(".", ",") + "</td></tr>";
                    ListRow += Row;
                }
            }

            //khởi tạo table html.                
            var table = "<table class=\"invoice-detail\"><thead><tr> <th>STT</th> <th>Mã hàng</th><th>Tên mặt hàng</th><th>Lô sản xuất</th><th>Hạn dùng</th><th>ĐVT</th><th>Số lượng</th><th>Đơn giá</th><th>% CK</th><th>Trị giá chiết khấu</th><th>Thành tiền</th></tr></thead><tbody>"
                         + ListRow
                         //+ "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>"
                         //+ "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>"
                         + "</tbody><tfoot>"
                         + "</td><td colspan=\"10\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(productInvoice.TotalNoVAT.Value).Replace(".", ",")
                         + "</td></tr>"
                         + "<tr><td colspan=\"10\" class=\"text-right\">VAT (" + productInvoice.TaxFee + "%)</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(productInvoice.TotalAmount - productInvoice.TotalNoVAT)
                         + "</td></tr>"
                          //+ "<tr><td colspan=\"6\" class=\"text-right\"></td><td class=\"text-right\">"
                          //+ Erp.BackOffice.Helpers.Common.PhanCachHangNgan(detailList.Sum(x => x.Quantity)).Replace(".", ",")


                          // + "<tr><td colspan=\"10\" class=\"text-right\">Tổng tiền phải thanh toán</td><td class=\"text-right\">"
                          //+ Erp.BackOffice.Helpers.Common.PhanCachHangNgan(total)
                          //+ "</td></tr>"
                          + "<tr><td colspan=\"10\" class=\"text-right\">Đã thanh toán</td><td class=\"text-right\">"
                          + (productInvoice.PaidAmount != null && productInvoice.PaidAmount.Value > 0 ? Erp.BackOffice.Helpers.Common.PhanCachHangNgan(productInvoice.PaidAmount.Value).Replace(".", ",") : "0")
                         + "</td></tr>"
                          + "<tr><td colspan=\"10\" class=\"text-right\">Còn lại phải thu</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(productInvoice.TotalAmount.Value - (productInvoice.PaidAmount != null && productInvoice.PaidAmount.Value > 0 ? productInvoice.PaidAmount.Value : 0)).Replace(".", ",")
                         + "</td></tr>"
                         + "</tfoot></table>";

            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code == "ProductInvoice").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            string replacement = template.Content;
            Helpers.Common.DienDuLieu(ref replacement, productInvoice, true, true);
            replacement = replacement.Replace("{Code}", productInvoice.Code);
            replacement = replacement.Replace("{Day}", productInvoice.CreatedDate.Value.Day.ToString());
            replacement = replacement.Replace("{Month}", productInvoice.CreatedDate.Value.Month.ToString());
            replacement = replacement.Replace("{Year}", productInvoice.CreatedDate.Value.Year.ToString());
            replacement = replacement.Replace("{CustomerPhone}", customer.Phone);
            replacement = replacement.Replace("{MoneyText}", Erp.BackOffice.Helpers.Common.ChuyenSoThanhChu_2(Convert.ToInt32(productInvoice.TotalAmount.Value)));
            replacement = replacement.Replace("{DataTable}", table);
            replacement = replacement.Replace("{System.Logo}", ImgLogo);
            replacement = replacement.Replace("{System.CompanyName}", company);
            replacement = replacement.Replace("{System.AddressCompany}", address);
            replacement = replacement.Replace("{System.PhoneCompany}", phone);
            replacement = replacement.Replace("{System.Fax}", fax);
            replacement = replacement.Replace("{System.BankCodeCompany}", bankcode);
            replacement = replacement.Replace("{System.BankNameCompany}", bankname);
            model.Content = replacement;
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + productInvoice.CreatedDate.Value.ToString("yyyyMMdd") + productInvoice.Code + ".xlsx");
                Response.Charset = "iso-8859-1";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }

            return View(model);
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(int Id, string CancelReason)
        {
            var productInvoice = productInvoiceRepository.GetProductInvoiceById(Id);
            if (productInvoice != null)
            {
                //Kiểm tra cho phép sửa chứng từ này hay không
                if (!Helpers.Common.KiemTraNgaySuaChungTu(productInvoice.CreatedDate.Value))
                {
                    TempData[Globals.FailedMessageKey] = "Quá hạn xóa chứng từ!";
                    return RedirectToAction("Detail", new { Id = productInvoice.Id });
                }
                var productOutbound = ProductOutboundRepository.GetAllProductOutbound().Where(item => item.InvoiceId == productInvoice.Id).FirstOrDefault();
                if (productOutbound != null)
                {
                    ProductOutboundController.unArchiveFromInvoice(ProductOutboundRepository, productOutbound, TempData);
                    if (TempData[Globals.FailedMessageKey] != null)
                    {
                        return RedirectToAction("Detail", new { Id = productInvoice.Id });
                    }
                    productOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
                    productOutbound.ModifiedDate = DateTime.Now;
                    productOutbound.IsDeleted = true;
                    productOutbound.CancelReason = CancelReason;
                    ProductOutboundRepository.UpdateProductOutbound(productOutbound);
                }
                productInvoice.ModifiedUserId = WebSecurity.CurrentUserId;
                productInvoice.ModifiedDate = DateTime.Now;
                productInvoice.IsDeleted = true;
                productInvoice.CancelReason = CancelReason;
                productInvoice.Status = Wording.OrderStatus_deleted;
                productInvoiceRepository.UpdateProductInvoice(productInvoice);
                PostController.SavePost(productInvoice.Id, "ProductInvoice", "Hủy đơn hàng (" + productInvoice.Code + ")");
                TempData[Globals.SuccessMessageKey] = "Xóa chứng từ thành công!";
                return RedirectToAction("Detail", new { Id = productInvoice.Id });
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Detail
        public ActionResult Detail(int? Id, string TransactionCode)
        {
            var productInvoice = new vwProductInvoice();
            if (Id != null && Id.Value > 0)
            {
                productInvoice = productInvoiceRepository.GetvwProductInvoiceById(Id.Value);
            }

            if (!string.IsNullOrEmpty(TransactionCode))
            {
                productInvoice = productInvoiceRepository.GetvwProductInvoiceByCode(TransactionCode);
            }

            if (productInvoice == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ProductInvoiceViewModel();
            AutoMapper.Mapper.Map(productInvoice, model);

            model.ReceiptViewModel = new ReceiptViewModel();
            model.NextPaymentDate_Temp = DateTime.Now.AddDays(30);
            model.ReceiptViewModel.Name = "Bán hàng - Thu tiền mặt";
            model.ReceiptViewModel.Amount = Convert.ToDouble(productInvoice.RemainingAmount);

            //Lấy thông tin kiểm tra cho phép sửa chứng từ này hay không
            model.AllowEdit = Helpers.Common.KiemTraNgaySuaChungTu(model.CreatedDate.Value);

            //Lấy lịch sử giao dịch thanh toán
            var listTransaction = transactionLiabilitiesRepository.GetAllvwTransaction()
                        .Where(item => item.MaChungTuGoc == productInvoice.Code)
                        .OrderByDescending(item => item.CreatedDate)
                        .ToList();

            model.ListTransactionLiabilities = new List<TransactionLiabilitiesViewModel>();
            AutoMapper.Mapper.Map(listTransaction, model.ListTransactionLiabilities);

            model.Code = productInvoice.Code;
            model.SalerId = productInvoice.SalerId;
            model.SalerName = productInvoice.SalerFullName;
            model.CreatedUserName = Helpers.Common.CurrentUser.FullName;
            var saleDepartmentCode = Erp.BackOffice.Helpers.Common.GetSetting("SaleDepartmentCode");

            //Lấy danh sách chi tiết đơn hàng
            model.DetailList = productInvoiceRepository.GetAllvwInvoiceDetailsByInvoiceId(productInvoice.Id).Select(x =>
                new ProductInvoiceDetailViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    PromotionDetailId = x.PromotionDetailId,
                    PromotionId = x.PromotionId,
                    PromotionValue = x.PromotionValue,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    ProductType = x.ProductType,
                    DisCount = x.DisCount,
                    DisCountAmount = x.DisCountAmount,
                    CategoryCode = x.CategoryCode,
                    ProductInvoiceCode = x.ProductInvoiceCode,
                    ProductName = x.ProductName,
                    ProductCode = x.ProductCode,
                    ProductInvoiceId = x.ProductInvoiceId,
                    ProductGroup = x.ProductGroup,
                    CheckPromotion = x.CheckPromotion,
                    IsReturn = x.IsReturn
                }).ToList();

            model.GroupProduct = model.DetailList.GroupBy(x => new { x.ProductGroup , x.ProductType }, (key, group) => new ProductInvoiceDetailViewModel
            {
                ProductGroup = key.ProductGroup,
                ProductType = key.ProductType,
                ProductId = group.FirstOrDefault().ProductId,
                Id = group.FirstOrDefault().Id
            }).OrderBy(item => item.ProductGroup).ToList();

            foreach (var item in model.GroupProduct)
            {
                if (!string.IsNullOrEmpty(item.ProductGroup))
                {
                    if (item.ProductType == ProductType.Product)
                    {
                        item.ProductGroupName = categoryRepository.GetCategoryByCode("ProductGroup").Where(x => x.Value == item.ProductGroup).FirstOrDefault()?.Name;
                    }
                    else
                    {
                        item.ProductGroupName = categoryRepository.GetCategoryByCode("serviceGroup").Where(x => x.Value == item.ProductGroup).FirstOrDefault()?.Name;
                    }
                }
            }

            //Lấy thông tin phiếu xuất kho
            if (productInvoice.ProductOutboundId != null && productInvoice.ProductOutboundId > 0)
            {
                var productOutbound = ProductOutboundRepository.GetvwProductOutboundById(productInvoice.ProductOutboundId.Value);
                model.ProductOutboundViewModel = new ProductOutboundViewModel();
                AutoMapper.Mapper.Map(productOutbound, model.ProductOutboundViewModel);
            }

            //Lấy danh sách chứng từ liên quan
            model.ListTransactionRelationship = new List<TransactionRelationshipViewModel>();
            var listTransactionRelationship = transactionRepository.GetAllvwTransactionRelationship()
                .Where(item => item.TransactionA == productInvoice.Code
                || item.TransactionB == productInvoice.Code).OrderByDescending(item => item.CreatedDate)
                .ToList();

            AutoMapper.Mapper.Map(listTransactionRelationship, model.ListTransactionRelationship);

            //int taxfee = 0;
            //int.TryParse(Helpers.Common.GetSetting("vat"), out taxfee);
            //model.TaxFee = taxfee;

            ViewBag.isAdmin = Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId == 1 ? true : false;

            model.ModifiedUserName = userRepository.GetUserById(model.ModifiedUserId.Value).FullName;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            model.QuantityCodeSaleReturns = saleReturnRepository.GetAllvwReturnsDetails().Where(x => x.ProductInvoiceId == model.Id).Count();


            return View(model);
        }
        #endregion

        #region LoadDetailInvoice
        public ActionResult LoadInvoice(int CustomerId)
        {
            var model = new List<ProductInvoiceViewModel>();
            var detal = productInvoiceRepository.GetAllvwInvoiceByCustomer(CustomerId).OrderByDescending(x => x.CreatedDate).Take(5);
            AutoMapper.Mapper.Map(detal, model);
            return View(model);
        }
        public ActionResult LoadDetailInvoice(int CustomerId)
        {
            var model = new List<ProductInvoiceDetailViewModel>();
            var detal = productInvoiceRepository.GetAllvwInvoiceDetailsByCustomerId(CustomerId).OrderByDescending(x => x.ProductInvoiceDate).GroupBy(x => x.ProductId).Select(x => x.FirstOrDefault()).Take(10);
            AutoMapper.Mapper.Map(detal, model);
            return View(model);
        }
        #endregion

        #region PayInvoice
        [HttpPost]
        public ActionResult PayInvoice(ProductInvoiceViewModel model)
        {
            if (Request["Submit"] == "Save")
            {
                var productInvoice = productInvoiceRepository.GetProductInvoiceById(model.Id);
                if (productInvoice.IsArchive)
                {
                    //Cập nhật thông tin thanh toán cho đơn hàng
                    productInvoice.ModifiedDate = DateTime.Now;
                    productInvoice.ModifiedUserId = WebSecurity.CurrentUserId;
                    productInvoice.PaidAmount += Convert.ToDecimal(model.ReceiptViewModel.Amount);
                    productInvoice.RemainingAmount = productInvoice.TotalAmount - productInvoice.PaidAmount;
                    if (model.NextPaymentDate_Temp != null)
                    {
                        productInvoice.NextPaymentDate = model.NextPaymentDate_Temp;
                    }
                    productInvoiceRepository.UpdateProductInvoice(productInvoice);
                    //Lấy mã KH
                    var customer = customerRepository.GetCustomerById(productInvoice.CustomerId.Value);
                    //Khách thanh toán ngay
                    if (model.ReceiptViewModel.Amount > 0)
                    {
                        //Lập phiếu thu
                        var receipt = new Receipt();
                        AutoMapper.Mapper.Map(model.ReceiptViewModel, receipt);
                        receipt.IsDeleted = false;
                        receipt.CreatedUserId = WebSecurity.CurrentUserId;
                        receipt.ModifiedUserId = WebSecurity.CurrentUserId;
                        receipt.AssignedUserId = WebSecurity.CurrentUserId;
                        receipt.CreatedDate = DateTime.Now;
                        receipt.ModifiedDate = DateTime.Now;
                        receipt.VoucherDate = DateTime.Now;
                        receipt.CustomerId = customer.Id;
                        receipt.Payer = customer.Name;
                        receipt.PaymentMethod = model.ReceiptViewModel.PaymentMethod;
                        receipt.Address = customer.Address;
                        receipt.MaChungTuGoc = productInvoice.Code;
                        receipt.LoaiChungTuGoc = "ProductInvoice";
                        receipt.Note = receipt.Name;
                        receipt.BranchId = Helpers.Common.CurrentUser.BranchId;
                        ReceiptRepository.InsertReceipt(receipt);

                        receipt.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("Receipt");
                        ReceiptRepository.UpdateReceipt(receipt);
                        Erp.BackOffice.Helpers.Common.SetOrderNo("Receipt");
                        PostController.SavePost(productInvoice.Id, "ProductInvoice", "Thu tiền khách hàng (" + receipt.Code + ")");
                        //Thêm vào quản lý chứng từ
                        TransactionController.Create(new TransactionViewModel
                        {
                            TransactionModule = "Receipt",
                            TransactionCode = receipt.Code,
                            TransactionName = "Thu tiền khách hàng"
                        });

                        //Thêm chứng từ liên quan
                        TransactionController.CreateRelationship(new TransactionRelationshipViewModel
                        {
                            TransactionA = receipt.Code,
                            TransactionB = productInvoice.Code
                        });

                        //Ghi Có TK 131 - Phải thu của khách hàng.
                        Erp.BackOffice.Account.Controllers.TransactionLiabilitiesController.Create(
                            receipt.Code,
                            "Receipt",
                            "Thu tiền khách hàng",
                            customer.Code,
                            "Customer",
                            0,
                            Convert.ToDecimal(model.ReceiptViewModel.Amount),
                            productInvoice.Code,
                            "ProductInvoice",
                            model.ReceiptViewModel.PaymentMethod,
                            null,
                            null);
                    }
                }
            }
            return RedirectToAction("Detail", new { Id = model.Id });
        }
        #endregion


        #region Update
        [HttpPost]
        public ActionResult Update(int ProductInvoiceDetailId, int Discount)
        {
            var productInvoiceDetail = productInvoiceRepository.GetProductInvoiceDetailById(ProductInvoiceDetailId);
            productInvoiceDetail.DisCount = Discount;
            var thanh_tien = productInvoiceDetail.Quantity * productInvoiceDetail.Price;
            productInvoiceDetail.DisCountAmount = Convert.ToInt32(productInvoiceDetail.DisCount * thanh_tien / 100);
            productInvoiceRepository.UpdateProductInvoiceDetail(productInvoiceDetail);

            var productInvoice = productInvoiceRepository.GetProductInvoiceById(productInvoiceDetail.ProductInvoiceId.Value);
            productInvoice.TotalAmount = productInvoiceRepository.GetAllInvoiceDetailsByInvoiceId(productInvoice.Id).Sum(item => (item.Price * item.Quantity) - item.DisCountAmount);
            productInvoiceRepository.UpdateProductInvoice(productInvoice);
            return Content("success");
        }
        #endregion

        #region Archive
        [HttpPost]
        public ActionResult Archive(ProductInvoiceViewModel model)
        {
            if (Request["Submit"] == "Save")
            {
                var productInvoice = productInvoiceRepository.GetProductInvoiceById(model.Id);

                //Kiểm tra cho phép sửa chứng từ này hay không
                if (!Helpers.Common.KiemTraNgaySuaChungTu(productInvoice.CreatedDate.Value))
                {
                    TempData[Globals.FailedMessageKey] = "Quá ngày sửa chứng từ!";
                    return RedirectToAction("Detail", new { Id = productInvoice.Id });
                }

                //Coi thử đã xuất kho chưa mới cho ghi sổ
                bool hasProductOutbound = ProductOutboundRepository.GetAllProductOutbound().Any(item => item.InvoiceId == productInvoice.Id);
                // coi thử đon hàng có sản phẩm không
                bool hasProduct = productInvoiceRepository.GetAllInvoiceDetailsByInvoiceId(productInvoice.Id).Any(x => x.ProductType == ProductType.Product);
                if (hasProduct && !hasProductOutbound)
                {
                    TempData[Globals.FailedMessageKey] = "Chưa lập phiếu xuất kho!";
                    return RedirectToAction("Detail", new { Id = productInvoice.Id });
                }

                if (!productInvoice.IsArchive)
                {
                    //Cập nhật thông tin thanh toán cho đơn hàng
                    productInvoice.PaymentMethod = model.ReceiptViewModel.PaymentMethod;
                    productInvoice.PaidAmount = Convert.ToDecimal(model.ReceiptViewModel.Amount);
                    productInvoice.RemainingAmount = productInvoice.TotalAmount - productInvoice.PaidAmount;
                    productInvoice.NextPaymentDate = model.NextPaymentDate_Temp;

                    productInvoice.ModifiedDate = DateTime.Now;
                    productInvoice.ModifiedUserId = WebSecurity.CurrentUserId;
                    productInvoiceRepository.UpdateProductInvoice(productInvoice);

                    //Tạo chiết khấu cho nhân viên nếu có
                    CommisionSaleController.Create(productInvoice.Id, productInvoice.TotalAmount);

                    //Lấy mã KH
                    var customer = customerRepository.GetCustomerById(productInvoice.CustomerId.Value);

                    var remain = productInvoice.TotalAmount.Value - Convert.ToDecimal(model.ReceiptViewModel.Amount.Value);
                    if (remain > 0)
                    {

                    }
                    else
                    {
                        productInvoice.NextPaymentDate = null;
                        model.NextPaymentDate_Temp = null;
                    }

                    //Ghi Nợ TK 131 - Phải thu của khách hàng (tổng giá thanh toán)
                    Erp.BackOffice.Account.Controllers.TransactionLiabilitiesController.Create(
                        productInvoice.Code,
                        "ProductInvoice",
                        "Bán hàng",
                        customer.Code,
                        "Customer",
                        productInvoice.TotalAmount.Value,
                        0,
                        productInvoice.Code,
                        "ProductInvoice",
                        null,
                        model.NextPaymentDate_Temp,
                        null);

                    //Khách thanh toán ngay
                    if (model.ReceiptViewModel.Amount > 0)
                    {
                        //Lập phiếu thu
                        var receipt = new Receipt();
                        AutoMapper.Mapper.Map(model.ReceiptViewModel, receipt);
                        receipt.IsDeleted = false;
                        receipt.CreatedUserId = WebSecurity.CurrentUserId;
                        receipt.ModifiedUserId = WebSecurity.CurrentUserId;
                        receipt.AssignedUserId = WebSecurity.CurrentUserId;
                        receipt.CreatedDate = DateTime.Now;
                        receipt.ModifiedDate = DateTime.Now;
                        receipt.VoucherDate = DateTime.Now;
                        receipt.CustomerId = customer.Id;
                        receipt.Payer = customer.Name;
                        receipt.PaymentMethod = productInvoice.PaymentMethod;
                        receipt.Address = customer.Address;
                        receipt.MaChungTuGoc = productInvoice.Code;
                        receipt.LoaiChungTuGoc = "ProductInvoice";
                        receipt.Note = receipt.Name;
                        receipt.BranchId = Helpers.Common.CurrentUser.BranchId;
                        if (receipt.Amount > Convert.ToDouble(productInvoice.TotalAmount))
                            receipt.Amount = Convert.ToDouble(productInvoice.TotalAmount);

                        ReceiptRepository.InsertReceipt(receipt);

                        receipt.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("Receipt");
                        ReceiptRepository.UpdateReceipt(receipt);
                        Erp.BackOffice.Helpers.Common.SetOrderNo("Receipt");
                        PostController.SavePost(productInvoice.Id, "ProductInvoice", "Thu tiền khách hàng (" + receipt.Code + ")");
                        //Thêm vào quản lý chứng từ
                        TransactionController.Create(new TransactionViewModel
                        {
                            TransactionModule = "Receipt",
                            TransactionCode = receipt.Code,
                            TransactionName = "Thu tiền khách hàng"
                        });

                        //Thêm chứng từ liên quan
                        TransactionController.CreateRelationship(new TransactionRelationshipViewModel
                        {
                            TransactionA = receipt.Code,
                            TransactionB = productInvoice.Code
                        });

                        //Ghi Có TK 131 - Phải thu của khách hàng.
                        Erp.BackOffice.Account.Controllers.TransactionLiabilitiesController.Create(
                            receipt.Code,
                            "Receipt",
                            "Thu tiền khách hàng",
                            customer.Code,
                            "Customer",
                            0,
                            Convert.ToDecimal(model.ReceiptViewModel.Amount),
                            productInvoice.Code,
                            "ProductInvoice",
                            model.ReceiptViewModel.PaymentMethod,
                            null,
                            null);
                    }
                }

                //Cập nhật đơn hàng
                productInvoice.ModifiedUserId = WebSecurity.CurrentUserId;
                productInvoice.ModifiedDate = DateTime.Now;
                productInvoice.IsArchive = true;
                productInvoice.Status = Wording.OrderStatus_complete;
                productInvoice.Frequency = FrequencyInvoice(productInvoice.CustomerId.Value, productInvoice.CreatedDate.Value);
                productInvoiceRepository.UpdateProductInvoice(productInvoice);

                //Lưu lịch sử tích điểm cho khách hàng
                string sale_tich_diem = Erp.BackOffice.Helpers.Common.GetSetting("sale_earn_points");
                sale_tich_diem = sale_tich_diem ?? "false";
                if (Convert.ToBoolean(sale_tich_diem))
                {
                    RecordPoint(productInvoice);
                }

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.ArchiveSuccess;

                //Cảnh báo cập nhật phiếu xuất kho
                if (hasProductOutbound)
                {
                    TempData[Globals.SuccessMessageKey] += "<br/>Đơn hàng này đã được xuất kho! Vui lòng kiểm tra lại chứng từ xuất kho để tránh sai xót dữ liệu!";
                }
            }

            return RedirectToAction("Detail", new { Id = model.Id });
        }
        #endregion

        public int FrequencyInvoice(int CustomerId, DateTime Date)
        {
            var invoiceBefore = productInvoiceRepository.GetAllProductInvoice().Where(x => x.CustomerId == CustomerId && x.IsArchive).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (invoiceBefore != null)
            {
                return Convert.ToInt32(Helpers.Common.CalculateTwoDates(invoiceBefore.CreatedDate.Value, Date));
            }
            return 0;
        }

        public void RecordPoint(ProductInvoice productInvoice)
        {
            //Lưu lịch sử điểm
            HistoryPointController.CreatePoint(productInvoice.CustomerId, productInvoice.Id, "ProductInvoice", productInvoice.AccumulatedPoint, productInvoice.UsePoint);
            //Ghi nhận điểm
            MemberCardController.SynchUpdateTypeCrad(productInvoice.CustomerId.Value, productInvoice.AccumulatedPoint, productInvoice.UsePoint, productInvoice.Frequency);
        }
        public void DeletePoint(ProductInvoice productInvoice)
        {
            //xóa lịch sử điểm
            HistoryPointController.DeletedPoint(productInvoice.Id, "ProductInvoice");
            //Ghi nhận điểm lại
            MemberCardController.SynchUpdateTypeCrad(productInvoice.CustomerId.Value, productInvoice.AccumulatedPoint, productInvoice.UsePoint, productInvoice.Frequency, false);
        }

        #region UnArchive
        [HttpPost]
        public ActionResult UnArchive(int Id)
        {
            if (Request["Submit"] == "Save")
            {
                var productInvoice = productInvoiceRepository.GetProductInvoiceById(Id);

                //Kiểm tra cho phép sửa chứng từ này hay không
                if (Helpers.Common.KiemTraNgaySuaChungTu(productInvoice.CreatedDate.Value) == false)
                {
                    TempData[Globals.FailedMessageKey] = "Quá hạn sửa chứng từ!";
                }
                else
                {
                    var productOutbound = ProductOutboundRepository.GetAllProductOutbound().Where(item => item.InvoiceId == productInvoice.Id).FirstOrDefault();
                    if (productOutbound != null)
                    {
                        ProductOutboundController.unArchiveFromInvoice(ProductOutboundRepository, productOutbound, TempData);
                        if (TempData[Globals.FailedMessageKey] != null)
                        {
                            return RedirectToAction("Detail", new { Id = productInvoice.Id });
                        }
                        productOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
                        productOutbound.ModifiedDate = DateTime.Now;
                        productOutbound.IsDeleted = true;
                        productOutbound.CancelReason = "Bỏ ghi số - sửa đơn hàng";
                        ProductOutboundRepository.UpdateProductOutbound(productOutbound);
                    }
                    //Kiểm tra nếu có phiếu thu rồi thì không cho bỏ ghi sổ
                    var receipt = ReceiptRepository.GetAllReceipt()
                        .Where(item => item.MaChungTuGoc == productInvoice.Code).ToList();

                    //if (receipt != null)
                    //{
                    //    TempData[Globals.FailedMessageKey] = "Đơn hàng đã phát sinh phiếu thu!";
                    //}
                    //else
                    //{
                    foreach (var item in receipt)
                    {
                        item.IsDeleted = true;
                        item.CancelReason = "Bỏ ghi sổ chứng từ bán hàng: " + productInvoice.Code;
                        ReceiptRepository.UpdateReceipt(item);
                    }


                    //Xóa lịch sử giao dịch
                    var listTransaction = transactionLiabilitiesRepository.GetAllTransaction()
                    .Where(item => item.MaChungTuGoc == productInvoice.Code)
                    .Select(item => item.Id)
                    .ToList();

                    foreach (var item in listTransaction)
                    {
                        transactionLiabilitiesRepository.DeleteTransaction(item);
                    }

                    productInvoice.PaidAmount = 0;
                    productInvoice.RemainingAmount = productInvoice.TotalAmount;
                    productInvoice.NextPaymentDate = null;

                    productInvoice.ModifiedUserId = WebSecurity.CurrentUserId;
                    productInvoice.ModifiedDate = DateTime.Now;
                    productInvoice.IsArchive = false;
                    productInvoice.Status = Wording.OrderStatus_inprogress;
                    productInvoice.Frequency = FrequencyInvoice(productInvoice.CustomerId.Value, productInvoice.CreatedDate.Value);
                    productInvoiceRepository.UpdateProductInvoice(productInvoice);
                    PostController.SavePost(productInvoice.Id, "ProductInvoice", "Bỏ ghi sổ (" + productInvoice.Code + ")");

                    // xóa Point
                    string sale_tich_diem = Erp.BackOffice.Helpers.Common.GetSetting("sale_earn_points");
                    sale_tich_diem = sale_tich_diem ?? "false";
                    if (Convert.ToBoolean(sale_tich_diem))
                    {
                        DeletePoint(productInvoice);
                    }

                    TempData[Globals.SuccessMessageKey] = "Đã bỏ ghi sổ";
                    //}
                }
            }

            return RedirectToAction("Detail", new { Id = Id });
        }
        #endregion

        //#region UpdateQuantitySaleReturn
        //public ActionResult UpdateQuantitySaleReturn()
        //{
        //    var productInvoiceDetail = productInvoiceRepository.GetAllInvoiceDetails().Where(x=>x.QuantitySaleReturn==null).ToList();
        //    for (int i = 0; i < productInvoiceDetail.Count(); i++)
        //    {
        //        productInvoiceDetail[i].QuantitySaleReturn = productInvoiceDetail[i].Quantity;
        //        productInvoiceRepository.UpdateProductInvoiceDetail(productInvoiceDetail[i]);
        //    }
        //    return RedirectToAction("Index");
        //}
        //#endregion

        [HttpPost]
        public ActionResult checkExitsCode(string code, int? id)
        {
            var productinvoice = productInvoiceRepository.GetProductInvoiceByCode(code);
            if (productinvoice != null)
            {
                if (id != null && id.Value > 0)
                {
                    var data = productInvoiceRepository.GetProductInvoiceById(id.Value);
                    if (data.Id == productinvoice.Id)
                    {
                        return Content("success");
                    }
                    return Content("error");
                }
                return Content("error");
            }
            return Content("success");
        }

        #region Edit

        public ActionResult Edit(int? Id)
        {
            ProductInvoiceViewModel model = new ProductInvoiceViewModel();
            var invoice = productInvoiceRepository.GetProductInvoiceById(Id.Value);
            if (invoice != null && invoice.IsDeleted != true)
            {
                AutoMapper.Mapper.Map(invoice, model);
                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(ProductInvoiceViewModel model)
        {
            if (Request["Submit"] == "Save")
            {
                var invoice = productInvoiceRepository.GetProductInvoiceById(model.Id);

                invoice.ModifiedUserId = WebSecurity.CurrentUserId;
                invoice.ModifiedDate = DateTime.Now;
                invoice.Note = model.Note;
                invoice.CodeInvoiceRed = model.CodeInvoiceRed;

                productInvoiceRepository.UpdateProductInvoice(invoice);
                if (Request["IsPopup"] == "true" || Request["IsPopup"] == "True")
                {
                    return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #endregion

        public ViewResult SearchProductInvoice(int? WarehouseId)
        {
            List<ProductViewModel> model = new List<ProductViewModel>();
            //string image_folder = Helpers.Common.GetSetting("product-image-folder");
            var Inventory = InventoryRepository.GetAllvwInventory().Where(x => x.ProductType == ProductType.Product && x.IsSale == true && x.Quantity > 0);
            if (WarehouseId != null)
            {
                Inventory = Inventory.Where(x => x.WarehouseId == WarehouseId);
            }
            model = Inventory.Select(item => new ProductViewModel
            {
                Code = item.ProductCode,
                Barcode = item.ProductBarcode,
                Name = item.ProductName,
                Id = item.ProductId.Value,
                CategoryCode = item.CategoryCode,
                PriceInbound = item.ProductPriceInbound,
                PriceOutbound = item.ProductPriceOutbound,
                Unit = item.ProductUnit,
                QuantityTotalInventory = item.Quantity,
                Image_Name = item.ProductImage,
                TargetPoint = item.TargetPoint,
                Point = item.Point,
                Type = item.ProductType,
            }).ToList();
            // thêm dịch vụ
            var service = ProductRepository.GetAllvwProductByType(ProductType.Service).Select(item => new ProductViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Barcode = item.Barcode,
                Name = item.Name,
                CategoryCode = item.CategoryCode,
                PriceInbound = item.PriceInbound,
                PriceOutbound = item.PriceOutbound,
                Unit = item.Unit,
                QuantityTotalInventory = 0,
                Image_Name = item.Image_Name,
                TargetPoint = item.TargetPoint,
                Point = item.Point,
                Type = item.Type,
            }).AsEnumerable();
            model.AddRange(service);
            return View(model);
        }
    }
}
