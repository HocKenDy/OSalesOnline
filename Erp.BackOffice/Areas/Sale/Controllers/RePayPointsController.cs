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
using Erp.BackOffice.Helpers;
using Erp.BackOffice.Controllers;
using qts.webapp.backend.domain.Services.Sale;
using qts.webapp.backend.domain.Models.Sale;
using Erp.Domain.Account.Interfaces;
using Erp.Domain.Sale.Interfaces;
using qts.webapp.domain.Repositories;
using Erp.BackOffice.Account.Controllers;
using Erp.BackOffice.Account.Models;
using Erp.BackOffice.Crm.Controllers;
using Erp.BackOffice.App_GlobalResources;
using System.Web;

namespace Erp.BackOffice.Sale.Controllers
{
    public class RePayPointsController : BaseController
    {
        private readonly IRePayPointsService RePayPointsService;
        private readonly IRePayPointsDetailService RePayPointsDetailService;
        private readonly IProductRepository ProductRepository;
        private readonly IUserRepository userRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IInventoryRepository InventoryRepository;
        private readonly IProductOutboundRepository ProductOutboundRepository;
        private readonly IvwRePayPointsService vwRePayPointsService;
        private readonly IvwRePayPointsDetailService vwRePayPointsDetailService;
        private readonly ITransactionRepository transactionRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;
        public RePayPointsController(
             IRePayPointsService _RePayPoints
            , IRePayPointsDetailService _rePayPointsDetail
            , IUserRepository _user
            , ICustomerRepository _customerRepository
            , IWarehouseRepository _warehouseRepository
            , IInventoryRepository _inventoryRepository
            , IProductRepository _productRepository
            , IProductOutboundRepository _ProductOutbound
            , IvwRePayPointsService _vwRePayPoints
            , IvwRePayPointsDetailService _vwrePayPointsDetail
            , ITransactionRepository _Transaction
            , ITemplatePrintRepository _TemplatePrint
            )
        {
            RePayPointsService = _RePayPoints;
            RePayPointsDetailService = _rePayPointsDetail;
            userRepository = _user;
            customerRepository = _customerRepository;
            WarehouseRepository = _warehouseRepository;
            InventoryRepository = _inventoryRepository;
            ProductRepository = _productRepository;
            ProductOutboundRepository = _ProductOutbound;
            vwRePayPointsService = _vwRePayPoints;
            vwRePayPointsDetailService = _vwrePayPointsDetail;
            transactionRepository = _Transaction;
            templatePrintRepository = _TemplatePrint;
        }

        #region Index

        public ViewResult Index(int? BranchId,string txtCode, string txtCusName, string startDate, string endDate)
        {
            IEnumerable<RePayPointsViewModel> q = vwRePayPointsService.Get()
                .Select(item => new RePayPointsViewModel
                {
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    Status = item.Status,
                    IsArchive = item.IsArchive,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    BranchId = item.BranchId,
                    BranchName = item.BranchName,
                    Note = item.Note,
                    SaleName = item.SaleName,
                    CancelReason = item.CancelReason,
                    AvailabilityPoint = item.AvailabilityPoint,
                    TotalPoint = item.TotalPoint,
                    WarehouseSourceName = item.WarehouseSourceName
                }).OrderByDescending(m => m.ModifiedDate);
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
            if (!string.IsNullOrEmpty(txtCode))
            {
                q = q.Where(x => x.Code == txtCode);
            }
            if (!string.IsNullOrEmpty(txtCusName))
            {
                q = q.Where(x => x.CustomerName.Contains(txtCusName));
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion
        #region Index

        public ViewResult IndexByCustomer(int CustomerId)
        {
            IEnumerable<RePayPointsViewModel> q = vwRePayPointsService.Get()
                .Where(x=> x.CustomerId == CustomerId)
                .Select(item => new RePayPointsViewModel
                {
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    Status = item.Status,
                    IsArchive = item.IsArchive,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    BranchId = item.BranchId,
                    BranchName = item.BranchName,
                    Note = item.Note,
                    SaleName = item.SaleName,
                    CancelReason = item.CancelReason,
                    AvailabilityPoint = item.AvailabilityPoint,
                    TotalPoint = item.TotalPoint,
                    WarehouseSourceName = item.WarehouseSourceName
                }).OrderByDescending(m => m.ModifiedDate);
            return View(q);
        }
        #endregion
        #region Create
        public ActionResult Create(int? Id)
        {
            var model = new RePayPointsViewModel();
            model.DetailList = new List<RePayPointsDetailViewModel>();
            if (Id.HasValue && Id > 0)
            {
                #region Cập nhật
                var rePayPoint = RePayPointsService.Get(Id.Value);
                //Nếu đã ghi sổ rồi thì không được sửa
                if (rePayPoint.IsArchive == true)
                {
                    TempData[Globals.FailedMessageKey] = "Đã ghi sổ. không được chỉnh sửa";
                    return RedirectToAction("Detail", new { Id = rePayPoint.Id });
                }

                AutoMapper.Mapper.Map(rePayPoint, model);

                var detailList = vwRePayPointsDetailService.GetAllvwRePayPointsDetailByRePayPointId(rePayPoint.Id);
                AutoMapper.Mapper.Map(detailList, model.DetailList);
                #endregion
            }
            else
            {
                //Nhân viên
                string id_default_user = Helpers.Common.GetSetting("id_default_user");
                int deaultUserId = string.IsNullOrEmpty(id_default_user) ? 0 : Convert.ToInt32(id_default_user);
                var deaultUser = userRepository.GetUserById(deaultUserId);

                if (deaultUser != null)
                {
                    model.SaleId = deaultUser.Id;
                }
                model.TotalPoint = 0;
                model.WarehouseSourceId = WarehouseRepository.GetAllWarehouse()
                    .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Categories.Contains("GIFT")).FirstOrDefault()?.Id;
            }
            var productList = ProductRepository.GetAllvwProduct().Where(x => x.Type == "gift" || x.QuantityTotalInventory > 0 || x.NoInbound == true);
            if (model.DetailList != null && model.DetailList.Count > 0)
            {
                foreach (var item in model.DetailList)
                {
                    var product = productList.Where(i => i.Id == item.GiftId).FirstOrDefault();
                    if (product != null)
                    {
                        item.GiftCode = product.Code;
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
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RePayPointsViewModel model)
        {
            if (ModelState.IsValid && model.DetailList.Count != 0)
            {
                RePayPoints rePayPoints = null;
                if (model.Id > 0)
                {
                    rePayPoints = RePayPointsService.Get(model.Id);
                }

                if (rePayPoints != null)
                {
                    #region Cập nhật
                    //Nếu đã ghi sổ rồi thì không được sửa
                    if (rePayPoints.IsArchive == true)
                    {
                        TempData[Globals.FailedMessageKey] = "Đã ghi sổ. không được chỉnh sửa";
                        return RedirectToAction("Detail", new { Id = rePayPoints.Id });
                    }

                    //Kiểm tra xem nếu có xuất kho rồi thì return
                    var checkProductOutbound = ProductOutboundRepository.GetAllProductOutbound()
                            .Where(item => item.Type == ProductOutboundType.Gift && item.PayPointId == rePayPoints.Id).FirstOrDefault();
                    if (checkProductOutbound != null)
                    {
                        TempData[Globals.FailedMessageKey] = "Phiếu xuất kho đã có!";
                        return RedirectToAction("Detail", new { Id = rePayPoints.Id });
                    }

                    AutoMapper.Mapper.Map(model, rePayPoints);
                    rePayPoints.ModifiedUserId = WebSecurity.CurrentUserId;
                    rePayPoints.ModifiedDate = DateTime.Now;
                    rePayPoints.Status = RePayPointsStatus.Pending;
                    rePayPoints.BranchId = Helpers.Common.CurrentUser.BranchId;
                    rePayPoints.IsArchive = false;
                    RePayPointsService.Update(rePayPoints);

                    //Xóa chi tiết cũ và thêm chi tiết mới
                    var listDetail_old = RePayPointsDetailService.GetRePayPointsDetailByPayPointId(rePayPoints.Id).ToList();
                    foreach (var item in listDetail_old)
                    {
                        RePayPointsDetailService.Delete(item);
                    }

                    foreach (var item in model.DetailList)
                    {
                        RePayPointsDetail rePayPointsDetail = new RePayPointsDetail();
                        AutoMapper.Mapper.Map(item, rePayPointsDetail);
                        rePayPointsDetail.RePayPointId = rePayPoints.Id;
                        rePayPointsDetail.IsDeleted = false;
                        rePayPointsDetail.CreatedUserId = WebSecurity.CurrentUserId;
                        rePayPointsDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                        rePayPointsDetail.CreatedDate = DateTime.Now;
                        rePayPointsDetail.ModifiedDate = DateTime.Now;
                        RePayPointsDetailService.Create(rePayPointsDetail);
                    }

                    #endregion
                }
                else// Thêm mới
                {
                    #region MyRegion
                    rePayPoints = new RePayPoints();
                    AutoMapper.Mapper.Map(model, rePayPoints);
                    rePayPoints.IsDeleted = false;
                    rePayPoints.CreatedUserId = WebSecurity.CurrentUserId;
                    rePayPoints.ModifiedUserId = WebSecurity.CurrentUserId;
                    rePayPoints.CreatedDate = DateTime.Now;
                    rePayPoints.ModifiedDate = DateTime.Now;
                    rePayPoints.Status = RePayPointsStatus.Pending;
                    rePayPoints.BranchId = Helpers.Common.CurrentUser.BranchId;
                    rePayPoints.IsArchive = false;
                    RePayPointsService.Create(rePayPoints);
                    //Cập nhật lại mã xuất kho
                    rePayPoints.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("RepayPoints", model.Code);
                    RePayPointsService.Update(rePayPoints);
                    Erp.BackOffice.Helpers.Common.SetOrderNo("RepayPoints");

                    foreach (var item in model.DetailList)
                    {
                        RePayPointsDetail rePayPointsDetail = new RePayPointsDetail();
                        AutoMapper.Mapper.Map(item, rePayPointsDetail);
                        rePayPointsDetail.IsDeleted = false;
                        rePayPointsDetail.CreatedUserId = WebSecurity.CurrentUserId;
                        rePayPointsDetail.ModifiedUserId = WebSecurity.CurrentUserId;
                        rePayPointsDetail.CreatedDate = DateTime.Now;
                        rePayPointsDetail.ModifiedDate = DateTime.Now;
                        rePayPointsDetail.RePayPointId = rePayPoints.Id;
                        RePayPointsDetailService.Create(rePayPointsDetail);
                    }

                    //Thêm vào quản lý chứng từ
                    TransactionController.Create(new TransactionViewModel
                    {
                        TransactionModule = "RePayPoints",
                        TransactionCode = rePayPoints.Code,
                        TransactionName = "Trả điểm"
                    });
                    #endregion
                }
                //Tạo phiếu nhập, nếu là tự động
                string sale_tu_dong_tao_chung_tu = Erp.BackOffice.Helpers.Common.GetSetting("sale_auto_outbound");
                if (sale_tu_dong_tao_chung_tu == "true")
                {
                    ProductOutboundViewModel productOutboundViewModel = new ProductOutboundViewModel();
                    // var warehouseDefault = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.IsSale == true).FirstOrDefault();

                    //Nếu trong đơn hàng có sản phẩm thì xuất kho
                    if (model.WarehouseSourceId != null)
                    {
                        productOutboundViewModel.PayPointId = rePayPoints.Id;
                        productOutboundViewModel.PayPointCode = rePayPoints.Code;
                        productOutboundViewModel.WarehouseSourceId = model.WarehouseSourceId;
                        productOutboundViewModel.Note = "Xuất kho cho đơn trả điểm " + rePayPoints.Code;
                        // lấy các danh sách không Xuất kho
                        var productNoInbound = ProductRepository.GetAllProduct().Where(x => x.Type == ProductType.Gift && x.NoInbound == true).ToList();
                        //Lấy dữ liệu cho chi tiết
                        productOutboundViewModel.DetailList = model.DetailList.Where(x => !productNoInbound.Any(y => y.Id == x.GiftId)).Select(x => new ProductOutboundDetailViewModel
                        {
                            ProductId = x.GiftId,
                            Quantity = x.Quantity,
                            Price = 0,
                            Unit = x.Unit,
                        }).ToList();

                        var productOutbound = ProductOutboundController.CreateFromPayPoint(ProductOutboundRepository, productOutboundViewModel, rePayPoints.Code, TempData);
                        PostController.SavePost(rePayPoints.Id, "RePayPoints", "Xuất kho trả điểm (" + productOutbound.Code + ")");
                    }
                    //Ghi sổ chứng từ bán hàng
                    model.Id = rePayPoints.Id;
                    Archive(model);
                }
                return RedirectToAction("Detail", new { Id = rePayPoints.Id });

            }
            return View(model);

        }
        #endregion


        #region Archive
        [HttpPost]
        public ActionResult Archive(RePayPointsViewModel model)
        {
            if (Request["Submit"] == "Save")
            {
                var rePayPoints = RePayPointsService.Get(model.Id);

                //Kiểm tra cho phép sửa chứng từ này hay không
                if (!Helpers.Common.KiemTraNgaySuaChungTu(rePayPoints.CreatedDate.Value))
                {
                    return RedirectToAction("Detail", new { Id = rePayPoints.Id });
                }

                //Coi thử đã xuất kho chưa mới cho ghi sổ
                var hasProductOutbound = ProductOutboundRepository.GetAllProductOutbound().Any(item => item.Type == ProductOutboundType.Gift && item.PayPointId == rePayPoints.Id);

                if (!hasProductOutbound)
                {
                    TempData[Globals.FailedMessageKey] = "Chưa lập phiếu xuất kho!";
                    return RedirectToAction("Detail", new { Id = rePayPoints.Id });
                }

                if (rePayPoints.IsArchive == false)
                {

                    //Cập nhật đơn hàng
                    rePayPoints.ModifiedUserId = WebSecurity.CurrentUserId;
                    rePayPoints.ModifiedDate = DateTime.Now;
                    rePayPoints.IsArchive = true;
                    rePayPoints.Status = RePayPointsStatus.Complete;
                    RePayPointsService.Update(rePayPoints);
                    RecordPoint(rePayPoints);
                }

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.ArchiveSuccess;

                //Cảnh báo cập nhật phiếu xuất kho
                if (hasProductOutbound)
                {
                    TempData[Globals.SuccessMessageKey] += "<br/>Đơn quà này đã được xuất kho! Vui lòng kiểm tra lại chứng từ xuất kho để tránh sai xót dữ liệu!";
                }
            }

            return RedirectToAction("Detail", new { Id = model.Id });
        }
        #endregion

        public void RecordPoint(RePayPoints RePayPoints)
        {
            //Lưu lịch sử điểm
            HistoryPointController.CreatePoint(RePayPoints.CustomerId, RePayPoints.Id, "RePayPoints", 0, RePayPoints.TotalPoint);
            //Ghi nhận điểm
            MemberCardController.SynchUpdateTypeCrad(RePayPoints.CustomerId.Value, 0, RePayPoints.TotalPoint, null);
        }
        public void DeletePoint(RePayPoints RePayPoints)
        {
            //xóa lịch sử điểm
            HistoryPointController.DeletedPoint(RePayPoints.Id, "RePayPoints");
            //Ghi nhận điểm lại
            MemberCardController.SynchUpdateTypeCrad(RePayPoints.CustomerId.Value, 0, RePayPoints.TotalPoint, null, false);
        }

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var RePayPoints = RePayPointsService.Get(Id.Value);
            if (RePayPoints != null && RePayPoints.IsDeleted != true)
            {
                var model = new RePayPointsViewModel();
                AutoMapper.Mapper.Map(RePayPoints, model);

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
        public ActionResult Edit(RePayPointsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var RePayPoints = RePayPointsService.Get(model.Id);
                    AutoMapper.Mapper.Map(model, RePayPoints);
                    SetModifier(RePayPoints, true);
                    RePayPointsService.Update(RePayPoints);

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
            var RePayPoints = vwRePayPointsService.Get(Id.Value);
            if (RePayPoints != null && RePayPoints.IsDeleted != true)
            {
                var model = new RePayPointsViewModel();
                AutoMapper.Mapper.Map(RePayPoints, model);
                model.DetailList = new List<RePayPointsDetailViewModel>();
                var listDetail = vwRePayPointsDetailService.GetAllvwRePayPointsDetailByRePayPointId(RePayPoints.Id);
                AutoMapper.Mapper.Map(listDetail, model.DetailList);

                //Lấy danh sách chứng từ liên quan
                model.ListTransactionRelationship = new List<TransactionRelationshipViewModel>();
                var listTransactionRelationship = transactionRepository.GetAllvwTransactionRelationship()
                    .Where(item => item.TransactionA == RePayPoints.Code
                    || item.TransactionB == RePayPoints.Code).OrderByDescending(item => item.CreatedDate)
                    .ToList();
                AutoMapper.Mapper.Map(listTransactionRelationship, model.ListTransactionRelationship);

                var productOutbound = ProductOutboundRepository.GetAllvwProductOutbound().Where(x => x.Type == ProductOutboundType.Gift && x.PayPointId == RePayPoints.Id).FirstOrDefault();
                if (productOutbound != null)
                {
                    model.ProductOutboundViewModel = new ProductOutboundViewModel();
                    AutoMapper.Mapper.Map(productOutbound, model.ProductOutboundViewModel);
                }
                //Lấy thông tin kiểm tra cho phép sửa chứng từ này hay không
                model.AllowEdit = Helpers.Common.KiemTraNgaySuaChungTu(model.CreatedDate.Value);
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.FailedMessage = TempData["FailedMessage"];
                ViewBag.AlertMessage = TempData["AlertMessage"];
                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        #endregion


        #region UnArchive
        [HttpPost]
        public ActionResult UnArchive(int Id)
        {
            if (Request["Submit"] == "Save")
            {
                var rePayPoints = RePayPointsService.Get(Id);

                //Kiểm tra cho phép sửa chứng từ này hay không
                if (Helpers.Common.KiemTraNgaySuaChungTu(rePayPoints.CreatedDate.Value) == false)
                {
                    TempData[Globals.FailedMessageKey] = "Quá hạn sửa chứng từ!";
                }
                else
                {
                    //cập nhật trạng thái
                    rePayPoints.ModifiedUserId = WebSecurity.CurrentUserId;
                    rePayPoints.ModifiedDate = DateTime.Now;
                    rePayPoints.IsArchive = false;
                    rePayPoints.Status = RePayPointsStatus.Inprogress;
                    RePayPointsService.Update(rePayPoints);

                    // xoá lịch sử điểm và cập nhật lại điểm cho khách hàng
                    DeletePoint(rePayPoints);
                    // bỏ ghi sổ phiếu xuất kho
                    var ProductOutbound = ProductOutboundRepository.GetAllProductOutbound().Where(item => item.IsArchive == true && item.Type == ProductOutboundType.Gift && item.PayPointId == rePayPoints.Id).FirstOrDefault();
                    if (ProductOutbound != null)
                    {
                        ProductOutboundController.unArchiveFromInvoice(ProductOutboundRepository, ProductOutbound, TempData);
                        // xoá phiếu xuất kho
                        ProductOutbound.ModifiedUserId = WebSecurity.CurrentUserId;
                        ProductOutbound.ModifiedDate = DateTime.Now;
                        ProductOutbound.IsDeleted = true;
                        ProductOutbound.CancelReason = "Bỏ ghi số đơn quà tặng - " + rePayPoints.Code;
                        ProductOutboundRepository.UpdateProductOutbound(ProductOutbound);
                    }

                    PostController.SavePost(rePayPoints.Id, "RePayPoints", "Bỏ ghi sổ (" + rePayPoints.Code + ")");

                    TempData[Globals.SuccessMessageKey] = "Đã bỏ ghi sổ";
                }
            }

            return RedirectToAction("Detail", new { Id = Id });
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
                    var item = RePayPointsService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        RePayPointsService.DeleteRs(item);
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
            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code == "RePayPoints").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (template != null)
            {
                //lấy hóa đơn.
                var RePayPoints = vwRePayPointsService.Get(Id);
                //lấy thông tin khách hàng
                var customer = customerRepository.GetvwCustomerById(RePayPoints.CustomerId.Value);
                List<RePayPointsDetailViewModel> detailList = new List<RePayPointsDetailViewModel>();
                //lấy danh sách sản phẩm xuất kho
                detailList = vwRePayPointsDetailService.GetAllvwRePayPointsDetailByRePayPointId(Id)
                        .Select(x => new RePayPointsDetailViewModel
                        {
                            Id = x.Id,
                            Point = x.Point,
                            GiftId = x.GiftId,
                            GiftName = x.GiftName,
                            GiftCode = x.GiftCode,
                            Quantity = x.Quantity,
                            Unit = x.Unit,
                            TotalPoint = x.TotalPoint,
                            CategoryCode = x.CategoryCode
                        }).ToList();
                //tạo dòng của table html danh sách sản phẩm.
                var ListRow = "";
                int index = 1;
                foreach (var item in detailList)
                {
                    var Row = "<tr>"
                     + "<td class=\"text-center\">" + index + "</td>"
                     + "<td class=\"text-left\">" + item.GiftCode + "</td>"
                     + "<td class=\"text-left\">" + item.GiftName + "</td>"
                     + "<td class=\"text-center\">" + item.Unit + "</td>"
                     + "<td class=\"text-right\">" + item.Quantity.Value + "</td>"
                     + "<td class=\"text-right\">" + Helpers.Common.PhanCachHangNgan(item.Point).Replace(".", ",") + "</td>"
                     + "<td class=\"text-right\">" + Helpers.Common.PhanCachHangNgan(item.TotalPoint).Replace(".", ",") + "</td></tr>";
                    ListRow += Row;
                    index++;
                }
                //khởi tạo table html.                
                var table = "<table class=\"invoice-detail\"><thead><tr> <th style=\"width: 30px;\">STT</th><th style=\"width: 100px;\">Mã quà tặng</th><th>Tên quà tặng</th><th style=\"width: 80px;\">ĐVT</th><th style=\"width: 80px;\">Số lượng</th><th style=\"width: 100px;\">Điểm quy đổi</th><th style=\"width: 100px;\">Tổng điểm</th></tr></thead><tbody>"
                             + ListRow
                             + "</tbody><tfoot>"
                             + "</td><td colspan=\"4\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                             + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(detailList.Sum(x => x.Quantity)).Replace(".", ",")
                             + "</td><td></td><td class=\"text-right\">"
                             + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(RePayPoints.TotalPoint).Replace(".", ",")
                             + "</td></tr>"
                             + "</tfoot></table>";

                //truyền dữ liệu vào template.
                string replacement = template.Content;
                Helpers.Common.DienDuLieu(ref replacement, RePayPoints, true, true);
                replacement = replacement.Replace("{Day}", RePayPoints.CreatedDate.Value.Day.ToString());
                replacement = replacement.Replace("{Month}", RePayPoints.CreatedDate.Value.Month.ToString());
                replacement = replacement.Replace("{Year}", RePayPoints.CreatedDate.Value.Year.ToString());
                replacement = replacement.Replace("{CustomerPhone}", customer.Phone);
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
                    Response.AppendHeader("content-disposition", "attachment;filename=" + RePayPoints.CreatedDate.Value.ToString("yyyyMMdd") + RePayPoints.Code + ".xlsx");
                    Response.Charset = "iso-8859-1";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Write(model.Content);
                    Response.End();
                }
            }

            return View(model);
        }
        #endregion
        public ViewResult SearchProductForGift(int? WarehouseId)
        {
            List<ProductViewModel> model = new List<ProductViewModel>();
            //string image_folder = Helpers.Common.GetSetting("product-image-folder");
            var Inventory = InventoryRepository.GetAllvwInventory().Where(x => x.ProductType == ProductType.Gift && x.IsSale == true && x.Quantity > 0);
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
                RedemptionPoints = item.RedemptionPoints
            }).ToList();

            return View(model);
        }

        #region LoadProductItem
        public PartialViewResult LoadProductItem(int OrderNo, int GiftId, string GiftName, string GiftCode, int Quantity, int QuantityInInventory, string Unit, string Images, double? Point)
        {
            var model = new RePayPointsDetailViewModel();
            model.OrderNo = OrderNo;
            model.GiftId = GiftId;
            model.GiftName = GiftName;
            model.GiftCode = GiftCode;
            model.Quantity = Quantity;
            model.QuantityInInventory = QuantityInInventory;
            model.Unit = Unit;
            model.Images = Images;
            model.Point = Point;
            model.TotalPoint = Point * Quantity;
            return PartialView(model);
        }
        #endregion
    }
}
