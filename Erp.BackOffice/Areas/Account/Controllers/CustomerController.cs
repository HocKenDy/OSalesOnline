using Erp.BackOffice.Filters;
using Erp.BackOffice.Helpers;
using Erp.BackOffice.Account.Models;
using Erp.Domain.Interfaces;
using Erp.Domain.Account.Entities;
using Erp.Domain.Account.Interfaces;
using qts.webapp.backend.domain.Services.Administration;
using qts.webapp.backend.domain.Models.Administration;
using Erp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;
using Erp.BackOffice.Sale.Controllers;
using System.IO;
using Erp.BackOffice.Models;
using System.Data;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using Erp.BackOffice.Sale.Models;
using Erp.Domain.Account.Helper;
using qts.webapp.backend.domain.Services.Sale;
using qts.webapp.domain.Repositories;
using qts.webapp.backend.domain.Models.Sale;

namespace Erp.BackOffice.Account.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository CustomerRepository;
        private readonly IContactRepository ContactRepository;
        private readonly IUserRepository userRepository;
        private readonly IProductInvoiceRepository productInvoiceRepository;
        private readonly IvwProvinceService ProvinceService;
        private readonly IvwDistrictService DistrictService;
        private readonly IMemberCardService MemberCardService;
        private readonly IvwWardService WardService;
        private readonly IvwCarService vwCarsService;
        private readonly ICarLineRepository CarLineRepository;
        private readonly ICarsService CarsService;
        private readonly IMemberCardTypeService MemberCardTypeService;

        public CustomerController(
            ICustomerRepository _Customer
            , IContactRepository _Contact
            , IUserRepository _user
            , IProductInvoiceRepository _ProductInvoice
            , IvwProvinceService _ProvinceService
            , IvwDistrictService _DistrictService
            , IvwWardService _IWardService
            , IMemberCardService _MemberCardService
            , IvwCarService _vwCarService
            , ICarLineRepository _CarLine
            , ICarsService _Cars
            , IMemberCardTypeService _MemberCardTypeService
            )
        {
            ContactRepository = _Contact;
            CustomerRepository = _Customer;
            userRepository = _user;
            productInvoiceRepository = _ProductInvoice;
            ProvinceService = _ProvinceService;
            DistrictService = _DistrictService;
            WardService = _IWardService;
            MemberCardService = _MemberCardService;
            vwCarsService = _vwCarService;
            CarLineRepository = _CarLine;
            CarsService = _Cars;
            MemberCardTypeService = _MemberCardTypeService;
        }

        #region ExportExcel
        [HttpPost]
        public ActionResult ExportExcel(string txtCode, string txtName)
        {
            var q = CustomerRepository.GetAllvwCustomer().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);

            if (string.IsNullOrEmpty(txtCode) == false)
            {
                q = q.Where(x => x.Code.Contains(txtCode));
            }

            if (string.IsNullOrEmpty(txtName) == false)
            {
                q = q.Where(x => x.Name.Contains(txtName));
            }

            var excelExport = new ExcelExport()
            {
                Title = "DANH SÁCH KHÁCH HÀNG",
                StartDate = "",
                EndDate = "",
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Tên", Width=30},
                   new ExcelHeader { Name ="Điện thoại"},
                   new ExcelHeader { Name ="Email", Width=30},
                   new ExcelHeader { Name ="Tỉnh, thành phố", Width=30},
                   new ExcelHeader { Name ="Quận, huyện", Width=30},
                   new ExcelHeader { Name ="Phường, thị trấn, xã", Width=30},
                   new ExcelHeader { Name ="Địa chỉ", Width=40},
                   new ExcelHeader { Name ="Ngày tạo"},
                   new ExcelHeader { Name ="Ngày cập nhật"}
                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.Name,
                        item.Phone,
                        item.Email,
                        item.ProvinceName,
                        item.DistrictName,
                        item.WardName,
                        item.Address,
                        item.CreatedDate,
                        item.ModifiedDate,
                    }), excelExport);
                var buffer = stream as MemoryStream;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = Helpers.Common.ChuyenThanhKhongDau(excelExport.getTile()) + ".xlsx";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.BinaryWrite(buffer.ToArray());
                Response.Flush();
                Response.End();
            }
            TempData[Globals.FailedMessageKey] = "Không có dữ liệu - Xuất lỗi";
            return RedirectToAction("Index", new { txtCode = txtCode, txtName = txtName });
        }
        #endregion

        #region Index

        public ViewResult Index(string txtCode, string txtName, string txtCardCode, int? CardTypeId, string txtPhone, string txtLicensePlates, string txtOption)
        {

            var q = CustomerRepository.GetAllvwCustomer().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            if (!string.IsNullOrEmpty(txtCardCode))
            {
                q = q.Where(x => x.CardCode == txtCardCode);
            }
            if (!string.IsNullOrEmpty(txtCode))
            {
                q = q.Where(x => x.Code.Contains(txtCode));
            }

            if (!string.IsNullOrEmpty(txtName))
            {
                q = q.Where(x => x.Name.Contains(txtName));
            }

            if (!string.IsNullOrEmpty(txtPhone))
            {
                q = q.Where(x => x.Phone.Contains(txtPhone));
            }

            //if (CardTypeId != null)
            //{
            //    q = q.Where(x => x.MemberCardTypeId == CardTypeId);
            //}

            if (!string.IsNullOrEmpty(txtLicensePlates))
            {
                q = SqlHelper.QuerySP<vwCustomer>("spSale_GetCustomerByPlate", new { Plate = txtLicensePlates }).AsQueryable();
            }

            if (!string.IsNullOrEmpty(txtOption))
            {
                bool check = txtOption == "YES" ? true: false;
                if(check)
                {
                    q = q.Where(x => x.MemberCardId != null);
                }
                else
                {
                    q = q.Where(x => x.MemberCardId == null);
                }
            }

            IQueryable<CustomerViewModel> model = q.Select(item => new CustomerViewModel
            {
                Id = item.Id,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Code = item.Code,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                WardName = item.WardName,
                ProvinceName = item.ProvinceName,
                DistrictName = item.DistrictName,
                Point = item.Point,
                MemberCardTypeName = item.MemberCardTypeName,
                CardCode = item.CardCode,
                EndDateProductInvoice = item.EndDateProductInvoice
            }).OrderByDescending(m => m.ModifiedDate);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Customer = CustomerRepository.GetCustomerById(Id.Value);
            if (Customer != null && Customer.IsDeleted != true)
            {
                var model = new CustomerViewModel();
                AutoMapper.Mapper.Map(Customer, model);

                //if (model.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
                //{
                //    TempData["FailedMessage"] = "NotOwner";
                //    return RedirectToAction("Index");
                //}               

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(CustomerViewModel model, bool? IsPopup)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Customer = CustomerRepository.GetCustomerById(model.Id);
                    AutoMapper.Mapper.Map(model, Customer);
                    Customer.ModifiedUserId = WebSecurity.CurrentUserId;
                    Customer.ModifiedDate = DateTime.Now;
                    Customer.NameSearch = Helpers.Common.ChuyenThanhKhongDau(Customer.Name);
                    //tạo đặc tính động cho sản phẩm nếu có danh sách đặc tính động truyền vào và đặc tính đó phải có giá trị
                    //ObjectAttributeController.CreateOrUpdateForObject(Customer.Id, model.AttributeValueList);

                    CustomerRepository.UpdateCustomer(Customer);

                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    if (IsPopup == true)
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

        #region Create
        public ViewResult Create(string Phone)
        {
            var model = new CustomerViewModel();
            //var orderNo = Erp.BackOffice.Helpers.Common.GetCode("orderNo_Customer", 1);
            //model.Code = SaleCurrent.BranchCode + orderNo;
            model.Phone = Phone;
          
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CustomerViewModel model, bool? IsAppend)
        {
            if (ModelState.IsValid)
            {
                //Tạo thẻ cho khách hàng
                if (model.IsCreateCard == true)
                {
                    if (model.MemberCardTypeId == null)
                    {
                        TempData[Globals.FailedMessageKey] = "Vui lòng chọn loại thẻ";
                        ViewBag.FailedMessage = TempData["FailedMessage"];
                        return View(model);
                    }
                    var MemberCard = MemberCardService.GetMemberCardByCode(model.MemberCardCode);
                    if (MemberCard != null)
                    {
                        TempData[Globals.FailedMessageKey] = "Mã thẻ đã đã tồn tại, Vui lòng kiểm tra lại";
                        ViewBag.FailedMessage = TempData["FailedMessage"];
                        return View(model);
                    }
                }
                var Customer = new Domain.Account.Entities.Customer();
                AutoMapper.Mapper.Map(model, Customer);
                Customer.IsDeleted = false;
                Customer.CreatedUserId = WebSecurity.CurrentUserId;
                Customer.ModifiedUserId = WebSecurity.CurrentUserId;
                Customer.CreatedDate = DateTime.Now;
                Customer.ModifiedDate = DateTime.Now;
                Customer.BranchId = Helpers.Common.CurrentUser.BranchId;
                Customer.NameSearch = Helpers.Common.ChuyenThanhKhongDau(Customer.Name);
                Customer.Point = 0;
                Customer.PaidPoint = 0;
                Customer.RemainingPoint = 0;
                Customer.Frequency = 0;
                CustomerRepository.InsertCustomer(Customer);
                Customer.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("Customer");
                CustomerRepository.UpdateCustomer(Customer);
                Erp.BackOffice.Helpers.Common.SetOrderNo("Customer");
                ////tạo liên hệ cho khách hàng
                //if (string.IsNullOrEmpty(model.FirstName) == false && string.IsNullOrEmpty(model.LastName) == false)
                //{
                //    var contact = new Domain.Account.Entities.Contact();
                //    AutoMapper.Mapper.Map(model, contact);
                //    contact.IsDeleted = false;
                //    contact.CreatedUserId = WebSecurity.CurrentUserId;
                //    contact.ModifiedUserId = WebSecurity.CurrentUserId;
                //    contact.CreatedDate = DateTime.Now;
                //    contact.ModifiedDate = DateTime.Now;
                //    contact.CustomerId = Customer.Id;

                //    ContactRepository.InsertContact(contact);
                //}

                ////tạo đặc tính động cho sản phẩm nếu có danh sách đặc tính động truyền vào và đặc tính đó phải có giá trị
                //ObjectAttributeController.CreateOrUpdateForObject(Customer.Id, model.AttributeValueList);

                if (model.IsCreateCard == true)
                {
                    var MemberCardType = MemberCardTypeService.MemberCardTypeGetById(model.MemberCardTypeId.Value);
                    if(MemberCardType != null)
                    {
                        ProductOutboundController.CreateForMemberCard(TempData, MemberCardType.CardId, Helpers.Common.CurrentUser.BranchId, Customer.Name, Customer.Code);
                        if (TempData[Globals.FailedMessageKey] != null)
                        {
                            TempData[Globals.FailedMessageKey] += "Chưa cấp được thẻ cho khách hàng! </br>";
                        }
                        else
                        {
                            var memberCard = new MemberCard();
                            memberCard.IsDeleted = false;
                            memberCard.CreatedUserId = WebSecurity.CurrentUserId;
                            memberCard.CreatedDate = DateTime.Now;
                            memberCard.DateOfIssue = DateTime.Now;
                            memberCard.Status = MemberCardViewModel.Active;
                            memberCard.Code = model.MemberCardCode;
                            memberCard.CustomerId = Customer.Id;
                            memberCard.MemberCardTypeId = MemberCardType.Id;
                            MemberCardService.Create(memberCard);

                            //Thay đổi thẻ cho khách hàng
                            Customer.MemberCardId = memberCard.Id;
                            CustomerRepository.UpdateCustomer(Customer);
                            TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                        }
                    }
                }

                if (IsAppend == true)
                {

                    ViewBag.closePopup = "close and append to page parent";
                    model.Name = model.Name;
                    model.Id = Customer.Id;
                    return View(model);
                }
                if (Request["IsPopup"] != null && Request["IsPopup"].ToString().ToLower().Equals("true"))
                {
                    return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        #endregion

        #region Detail
        public ActionResult Detail(int? Id, string TargetCode, InfoDetailCustomer? infoDetailCustomer)
        {
            ViewBag.InfoDetail = infoDetailCustomer;
            var customer = new vwCustomer();
            if (Id != null)
            {
                customer = CustomerRepository.GetvwCustomerById(Id.Value);
            }
            if (!string.IsNullOrEmpty(TargetCode))
            {
                customer = CustomerRepository.GetAllvwCustomer()
                    .Where(item => item.Code == TargetCode).FirstOrDefault();
            }
            if (customer != null && customer.IsDeleted != true)
            {
                var model = new CustomerViewModel();
                AutoMapper.Mapper.Map(customer, model);
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
                    var item = CustomerRepository.GetCustomerById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        //if (item.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
                        //{
                        //    TempData["FailedMessage"] = "NotOwner";
                        //    return RedirectToAction("Index");
                        //}

                        //Kiểm tra thử có tạo đơn hàng chưa, chưa mới xóa
                        if (!productInvoiceRepository.GetAllvwProductInvoice().Any(x => x.CustomerId == item.Id))
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            item.ModifiedUserId = WebSecurity.CurrentUserId;
                            CustomerRepository.UpdateCustomer(item);
                        }
                        else
                        {
                            TempData[Globals.FailedMessageKey] = "Khách hàng này đã có dữ liệu đơn hàng, nên không thể xóa!";
                            return RedirectToAction("Index");
                        }
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

        #region Import
        public ActionResult Import()
        {
            var data = new DataTable();
            ViewBag.SuccessMessage = TempData[Globals.SuccessMessageKey];
            ViewBag.FailedMessage = TempData[Globals.FailedMessageKey];
            return View(data);
        }

        [HttpPost]
        public ActionResult Import(FormCollection fc)
        {
            try
            {
                var data = new DataTable();
                var tinh_tps = ProvinceService.Get();
                var quan_huyens = DistrictService.Get();
                var phuong_xas = WardService.Get();
                var carLine = CarLineRepository.GetAll();
                if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                    {
                        fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                    }

                    MemoryStream ms = new MemoryStream(fileData);
                    data = ReadFromExcelfile(ms, null, "Danhsach");

                    data.Columns.Add("Ghi chú");
                    var col_HO_TEN_KH = data.Columns.IndexOf("HO_TEN_KH");
                    var col_DIEN_THOAI = data.Columns.IndexOf("DIEN_THOAI");
                    var col_EMAIL = data.Columns.IndexOf("EMAIL");
                    var col_DIA_CHI = data.Columns.IndexOf("SO_NHA_TEN_DUONG");
                    var col_TINH_THANH_PHO = data.Columns.IndexOf("TINH_THANH_PHO");
                    var col_QUAN_HUYEN = data.Columns.IndexOf("QUAN_HUYEN");
                    var col_PHUONG_XA_THI_TRAN = data.Columns.IndexOf("PHUONG_XA_THI_TRAN");
                    var col_NGAY_SINH = data.Columns.IndexOf("NGAY_SINH");
                    var col_GIO_TINH = data.Columns.IndexOf("GIO_TINH");
                    var col_NGHE_NGHIEP = data.Columns.IndexOf("NGHE_NGHIEP");
                    var col_GHI_CHU = data.Columns.IndexOf("GHI_CHU");
                    //var col_HANG_SAN_XUAT = data.Columns.IndexOf("HANG_SAN_XUAT");
                    //var col_DONG_XE = data.Columns.IndexOf("DONG_XE");
                    //var col_MAU_XE = data.Columns.IndexOf("MAU_XE");
                    //var col_SO_KHUNG = data.Columns.IndexOf("SO_KHUNG");
                    //var col_SO_MAY = data.Columns.IndexOf("SO_MAY");
                    //var col_BKS = data.Columns.IndexOf("BKS");

                    var noteColumn = data.Columns.IndexOf("Ghi chú");

                    //tao cờ kiểm tra dữ kiệu
                    bool flag = false;

                    //tao moi 1 danh sách
                    List<ImportViewModel> customerList = new List<ImportViewModel>();
                    foreach (System.Data.DataRow row in data.Rows)
                    {
                        string HO_TEN_KH = col_HO_TEN_KH == -1 ? null : row.Field<string>(col_HO_TEN_KH);
                        string DIEN_THOAI = col_DIEN_THOAI == -1 ? null : row.Field<string>(col_DIEN_THOAI);
                        string EMAIL = col_EMAIL == -1 ? null : row.Field<string>(col_EMAIL);
                        string DIA_CHI = col_DIA_CHI == -1 ? null : row.Field<string>(col_DIA_CHI);
                        string TINH_TP = col_TINH_THANH_PHO == -1 ? null : row.Field<string>(col_TINH_THANH_PHO);
                        string QUAN_HUYEN = col_QUAN_HUYEN == -1 ? null : row.Field<string>(col_QUAN_HUYEN);
                        string PHUONG_XA = col_PHUONG_XA_THI_TRAN == -1 ? null : row.Field<string>(col_PHUONG_XA_THI_TRAN);
                        string NGAY_SINH = col_NGAY_SINH == -1 ? null : row.Field<string>(col_NGAY_SINH);
                        string GHI_CHU = col_GHI_CHU == -1 ? null : row.Field<string>(col_GHI_CHU);

                        string GIO_TINH = col_GIO_TINH == -1 ? null : row.Field<string>(col_GIO_TINH);
                        string NGHE_NGHIEP = col_NGHE_NGHIEP == -1 ? null : row.Field<string>(col_NGHE_NGHIEP);
                        //string HANG_SAN_XUAT = col_HANG_SAN_XUAT == -1 ? null : row.Field<string>(col_HANG_SAN_XUAT);
                        //string DONG_XE = col_DONG_XE == -1 ? null : row.Field<string>(col_DONG_XE);
                        //string MAU_XE = col_MAU_XE == -1 ? null : row.Field<string>(col_MAU_XE);
                        //string SO_KHUNG = col_SO_KHUNG == -1 ? null : row.Field<string>(col_SO_KHUNG);
                        //string SO_MAY = col_SO_MAY == -1 ? null : row.Field<string>(col_SO_MAY);
                        //string BKS = col_SO_MAY == -1 ? null : row.Field<string>(col_BKS);

                        string note = string.Empty;
                        var customer = new Customer
                        {
                            IsDeleted = false,
                            CreatedUserId = WebSecurity.CurrentUserId,
                            ModifiedUserId = WebSecurity.CurrentUserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            Name = HO_TEN_KH,
                            Address = DIA_CHI,
                            Phone = DIEN_THOAI,
                            Email = EMAIL,
                            BranchId = Helpers.Common.CurrentUser.BranchId,
                            Note = GHI_CHU,
                            Occupations = NGHE_NGHIEP,

                        };
                        var Cars = new qts.webapp.backend.domain.Models.Sale.Cars
                        {
                            IsDeleted = false,
                            CreatedUserId = WebSecurity.CurrentUserId,
                            ModifiedUserId = WebSecurity.CurrentUserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            //Color = MAU_XE,
                        };
                        #region Địa chỉ
                        if (!string.IsNullOrEmpty(TINH_TP))
                        {
                            var TINH_TP_UP = TINH_TP.ToUpper();
                            var tp = tinh_tps.Where(n => n.FullName.Contains(TINH_TP_UP) || n.Name.Contains(TINH_TP_UP));
                            if (tp != null && tp.Count() > 0)
                            {
                                customer.CityId = tp.FirstOrDefault().ProvinceId;
                            }
                        }
                        if (!string.IsNullOrEmpty(QUAN_HUYEN))
                        {
                            var QUAN_HUYEN_UP = QUAN_HUYEN.ToUpper();
                            var db = quan_huyens.Where(n => n.FullName.Contains(QUAN_HUYEN_UP) || n.Name.Contains(QUAN_HUYEN_UP));
                            if (db != null && db.Count() > 0)
                            {
                                customer.DistrictId = db.FirstOrDefault().DistrictId;
                            }
                        }
                        if (!string.IsNullOrEmpty(PHUONG_XA))
                        {
                            var PHUONG_XA_UP = PHUONG_XA.ToUpper();
                            var db = phuong_xas.Where(n => n.FullName.Contains(PHUONG_XA_UP) || n.Name.Contains(PHUONG_XA_UP));
                            if (db != null && db.Count() > 0)
                            {
                                customer.WardId = db.FirstOrDefault().WardId;
                            }
                        }
                        #endregion

                        #region thông tin kh
                        if (!string.IsNullOrEmpty(GIO_TINH))
                        {
                            if (GIO_TINH == "Nam")
                            {
                                customer.Gender = false;
                            }
                            else if (GIO_TINH == "Nữ")
                            {
                                customer.Gender = true;
                            }
                            else
                            {

                            }
                        }
                        if (string.IsNullOrEmpty(HO_TEN_KH))
                        {
                            note += "Cột HO_TEN_KH trống";
                            flag = true;
                        }
                        if (string.IsNullOrEmpty(DIEN_THOAI))
                        {
                            note += "Cột DIEN_THOAI trống";
                            flag = true;
                        }

                        if (string.IsNullOrEmpty(NGAY_SINH))
                        {
                            note += "Cột NGAY_SINH trống";
                            flag = true;
                        }
                        else
                        {
                            NGAY_SINH = string.IsNullOrEmpty(NGAY_SINH) ? "" : Regex.Replace(NGAY_SINH, "[^0-9]", "");
                            DateTime date;
                            if (DateTime.TryParseExact(NGAY_SINH, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                            {

                                customer.Birthday = date;
                                row.SetField<string>(col_NGAY_SINH, NGAY_SINH);
                            }
                            else
                            {
                                note += NGAY_SINH + ", NGAY_SINH không đúng định dạng (ví dụ: 30/12/1999 => 30121999)";
                                flag = true;
                            }
                        }
                        #endregion

                        #region Thông tin xe
                        //if (string.IsNullOrEmpty(HANG_SAN_XUAT))
                        //{
                        //    note += "Cột HANG_SAN_XUAT trống";
                        //    flag = true;
                        //}
                        //else
                        //{
                        //    Cars.Manufacturer = HANG_SAN_XUAT;
                        //}
                        //if (string.IsNullOrEmpty(DONG_XE))
                        //{
                        //    note += "Cột DONG_XE trống";
                        //    flag = true;
                        //}
                        //else
                        //{
                        //    var DONG_XE_UP = DONG_XE.ToUpper();
                        //    var db = carLine.Where(n => n.ManufacturerCar == HANG_SAN_XUAT && n.Name.ToUpper() == DONG_XE_UP).FirstOrDefault();
                        //    if (db != null)
                        //    {
                        //        Cars.CarLineId = db.Id;
                        //    }
                        //    else
                        //    {
                        //        note += "Dữ liệu DONG_XE không tồn tại";
                        //        flag = true;
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(SO_KHUNG))
                        //{
                        //    int n;
                        //    bool isNumeric = int.TryParse(SO_KHUNG, out n);
                        //    if (isNumeric)
                        //    {
                        //        Cars.Frames = n;
                        //    }
                        //    else
                        //    {
                        //        note += "Dữ liệu SO_KHUNG phải là số";
                        //        flag = true;
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(SO_MAY))
                        //{
                        //    int n;
                        //    bool isNumeric = int.TryParse(SO_MAY, out n);
                        //    if (isNumeric)
                        //    {
                        //        Cars.Number = n;
                        //    }
                        //    else
                        //    {
                        //        note += "Dữ liệu SO_MAY phải là số";
                        //        flag = true;
                        //    }
                        //}
                        //if (string.IsNullOrEmpty(BKS))
                        //{
                        //    note += "Cột BKS trống";
                        //    flag = true;
                        //}
                        //else
                        //{
                        //    Cars.Plate = BKS;
                        //}
                        #endregion

                        // thông tin xe
                        row.SetField<string>(noteColumn, note);
                        // or if above does not work
                        //myData = tableRow.Field<string>(data.Columns.IndexOf(tinhColumn));
                        ImportViewModel ImportViewModel = new ImportViewModel();
                        ImportViewModel.CustomerList = customer;
                        ImportViewModel.Cars = Cars;
                        customerList.Add(ImportViewModel);
                    }
                    //flag = false;
                    ViewBag.save = flag;
                    Session["ExcelData"] = data;
                    Session["customerList"] = customerList;
                }
                else
                {
                    var check = "";
                    string prefix = Helpers.Common.GetSetting("prefixOrderNo_Customer");
                    int stt = Convert.ToInt32(Helpers.Common.GetSetting("orderNo_Customer"));
                    var BranchCode = Erp.BackOffice.Helpers.Common.CurrentUser.BranchCode;
                    var customerList = (List<ImportViewModel>)Session["customerList"];
                    foreach (var customer in customerList)
                    {
                        customer.CustomerList.Code = string.Format("{0}{1}{2}", prefix, BranchCode, stt.ToString("D4"));
                        customer.CustomerList.Point = 0;
                        customer.CustomerList.PaidPoint = 0;
                        customer.CustomerList.RemainingPoint = 0;
                        CustomerRepository.InsertCustomer(customer.CustomerList);
                        stt++;
                        var cars = customer.Cars;
                        cars.CustomerId = customer.CustomerList.Id;
                        CarsService.Create(cars);
                    }
                    Helpers.Common.SetSetting("orderNo_Customer", stt.ToString());

                    Session["ExcelData"] = null;
                    Session["customerList"] = null;
                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                    if (!string.IsNullOrEmpty(check))
                        TempData[Globals.FailedMessageKey] = "Lỗi ghi sổ: " + check;
                    return RedirectToAction("Index");
                }


                return View(data);
            }
            catch (Exception ex)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.InsertFailed;
                return RedirectToAction("Import");
            }


        }

        private DataTable ReadFromExcelfile(MemoryStream ms, string path, string sheetName)
        {
            // Khởi tạo data table
            DataTable dt = new DataTable();

            ExcelPackage package;
            if (path != null)
                package = new ExcelPackage(new FileInfo(path));
            else
                package = new ExcelPackage(ms);

            // Load file excel và các setting ban đầu
            using (package)
            {
                if (package.Workbook.Worksheets.Count < 1)
                {
                    // Log - Không có sheet nào tồn tại trong file excel của bạn 
                    return null;
                }

                // Lấy Sheet đầu tiện trong file Excel để truy vấn 
                // Truyền vào name của Sheet để lấy ra sheet cần, nếu name = null thì lấy sheet đầu tiên 
                ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == sheetName)
                    ?? package.Workbook.Worksheets.FirstOrDefault();
                // Đọc tất cả các header
                foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                {
                    dt.Columns.Add(firstRowCell.Text);
                }

                // Đọc tất cả data bắt đầu từ row thứ 2
                for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                {
                    // Lấy 1 row trong excel để truy vấn
                    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];

                    //tao cờ kiểm tra dòng cuối
                    int flag_count_empty = 0;

                    // tạo 1 row trong data table
                    var newRow = dt.NewRow();
                    foreach (var cell in row)
                    {
                        if (string.IsNullOrEmpty(cell.Text.Trim()))
                            flag_count_empty++;
                        try
                        {
                            newRow[cell.Start.Column - 1] = cell.Text;
                        }
                        catch (Exception ex) { break; }
                    }
                    if (flag_count_empty >= dt.Columns.Count || flag_count_empty >= row.Count())
                        break;
                    else
                        dt.Rows.Add(newRow);
                }

                return dt;
            }
        }

        #endregion


        [AllowAnonymous]
        public JsonResult GetAllCustomerByPhone(string Phone)
        {
            var option = new List<CustomerJsonModel>
                    {
                        new CustomerJsonModel
                        {
                            Id =0,
                            Value = "THÊM KHÁCH HÀNG MỚI",
                            Phone = Phone,
                        }
                    };
            if (!string.IsNullOrEmpty(Phone) && Phone.Length > 3)
            {
                var listCustomer = SqlHelper.QuerySP<CustomerJsonModel>("spAccount_SearchCustomer", new
                {
                    Key = Phone
                }).ToList();
                if (listCustomer.Count() > 0)
                {
                    var data = new
                    {
                        results = listCustomer.Select(x => new
                        {
                            id = x.Id,
                            text = x.Value,
                            name = x.Name,
                            point = x.RemainingPoint,
                            html = Helpers.Common.RenderPartialViewToString(this, "InfoForInvoice", x),
                        })
                    };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var data = new
                    {
                        results = option.Select(x => new
                        {
                            id = x.Id,
                            text = x.Value,
                            phone = x.Phone
                        })
                    };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            var dataAdd = new
            {
                results = option.Select(x => new
                {
                    id = x.Id,
                    text = x.Value,
                    //type = x.Type,
                    phone = x.Phone
                })
            };
            return Json(dataAdd, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetCustomerById(int Id)
        {
            var customer = CustomerRepository.GetvwCustomerForInvoiceById(Id);
            if (customer != null)
            {
                var model = new CustomerJsonModel();
                AutoMapper.Mapper.Map(customer, model);
                var data = new
                {
                    id = customer.Id,
                    text = customer.Phone + " - " + customer.Name,
                    //type = "Customer",
                    name = customer.Name,
                    point = customer.RemainingPoint,
                    html = Helpers.Common.RenderPartialViewToString(this, "InfoForInvoice", model),
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetCustomerByCardCode(string Code)
        {
            var customer = CustomerRepository.GetvwCustomerForInvoiceByCardCode(Code);
            if (customer != null)
            {
                var model = new CustomerJsonModel();
                AutoMapper.Mapper.Map(customer, model);
                var data = new
                {
                    id = model.Id,
                    text = model.Phone + " - " + model.Name,
                    name = model.Name,
                    point = model.RemainingPoint,
                    html = Helpers.Common.RenderPartialViewToString(this, "InfoForInvoice", model),
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("failed", JsonRequestBehavior.AllowGet);
        }
    }
}
