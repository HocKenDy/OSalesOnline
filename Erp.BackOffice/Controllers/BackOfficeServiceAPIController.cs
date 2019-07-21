using Erp.Domain;
using Erp.Domain.Entities;
using Erp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Erp.Domain.Sale;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Repositories;
using Erp.Domain.Account.Entities;
using Erp.Domain.Account.Repositories;
using Erp.BackOffice.Helpers;
using Erp.Domain.Account;
using Erp.Domain.Helper;
using Erp.BackOffice.Sale.Models;
using Erp.Domain.Interfaces;
using qts.webapp.backend.domain.Services.Sale;
using qts.webapp.backend.domain.Models.Sale;
using Newtonsoft.Json;
using System.Net.Http.Headers;
//using Erp.Domain.RealEstate.Repositories;
//using Erp.Domain.RealEstate;
namespace Erp.BackOffice.Controllers
{

    public class BackOfficeServiceAPIController : ApiController
    {
        private readonly ICarLineService CarLineService;
        public BackOfficeServiceAPIController(
            ICarLineService _CarLineService
            )
        {
            CarLineService = _CarLineService;
        }
        #region Save Image Base64
        [HttpPost]
        public static string WriteFileFromBase64String(List<string> val)
        {
            string base64String = val[0];
            string filePath = val[1];
            string fileName = val[2];

            string dataType = fileName.Split('.').ElementAt(1);
            string nameFile = Guid.NewGuid().ToString() + "." + dataType;
            string absoluteFileName = HttpContext.Current.Server.MapPath(filePath) + nameFile;

            string dataBinaryContent = base64String;
            byte[] buffer = Convert.FromBase64String(dataBinaryContent);

            if (WriteFile(buffer, absoluteFileName))
            {
                string relativeFileName = filePath + nameFile;
                return relativeFileName;
            }
            return string.Empty;
        }

        public static bool WriteFile(byte[] buffer, string fullname)
        {
            try
            {
                var fileStream = new FileStream(fullname, FileMode.Append);
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Flush();
                fileStream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Save Image PostFileBase
        [HttpPost]
        public static string SaveFile(List<object> value)
        {
            var objFile = (HttpPostedFileBase)value[0];
            string filePath = (string)value[1];
            string strFileName = string.Empty;
            string strTargetFolder;
            if (objFile != null)
            {
                try
                {
                    strTargetFolder = HttpContext.Current.Server.MapPath(filePath);
                    strFileName = Path.GetFileName(objFile.FileName);
                    string fileName = StandardizeFileName(strTargetFolder, strFileName);
                    objFile.SaveAs(strTargetFolder + fileName);
                    return filePath + fileName;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public static string StandardizeFileName(string path, string filename)
        {
            string name = Path.GetFileNameWithoutExtension(filename);
            string ext = Path.GetExtension(filename);
            string result = name;
            int count = 1;
            while (File.Exists(path + filename.Replace(name, result)) == true)
            {
                result = name + "_" + count.ToString(CultureInfo.InvariantCulture);
                count++;
            }
            result = result + ext;
            return result;
        }
        #endregion

        [HttpGet]
        public IEnumerable<Location> FetchLocation(string parentId)
        {
            LocationRepository locationRepository = new LocationRepository(new ErpDbContext());
            List<Location> locationList = locationRepository.GetList(parentId).ToList();
            locationList.Insert(0, new Location() { Id = "", Name = "- Rỗng -" });

            return locationList;
        }

        public IHttpActionResult GetListLocation(string parentId)
        {
            LocationRepository locationRepository = new LocationRepository(new ErpDbContext());
            List<Location> locationList = locationRepository.GetList(parentId).ToList();
            return Ok(locationList);
        }

        public IHttpActionResult GetListLocation2(string parentId)
        {
            LocationRepository locationRepository = new LocationRepository(new ErpDbContext());
            List<Location> locationList = locationRepository.GetList(parentId).ToList();
            return Ok(locationList);
        }
        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> FetchCategoryBy(string value, string getByType)
        {
            return Erp.BackOffice.Helpers.Common.GetSelectList_Category(value, null, getByType);
        }

        [HttpGet]
        public IEnumerable<Contract> FetchContract()
        {
            ContractRepository contractRepository = new ContractRepository(new ErpAccountDbContext());
            List<Contract> ContractList = contractRepository.GetAllContract().AsEnumerable()
                .Select(item => new Contract { Id = item.Id, Code = item.Code }).ToList();
            ContractList.Insert(0, new Contract() { Id = -1, Code = "- Rỗng -" });
            return ContractList;
        }

        [HttpGet]
        public object FetchCarLines(string manufacturerCar)
        {
            var db = CarLineService.GetByManufacturerCar(manufacturerCar).ToList();
            db.Insert(0, new CarLine() { Id = -1, Name = "- Dòng xe -" });
            return db.Select(n => new { n.Id, n.Name });
        }


        //Report React
        [HttpGet]
        public object GetListTransactionLiabilitiesCustomer(string StartDate, string EndDate)
        {
            //    return null;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            var dataTransactionLiabilities = SqlHelper.QuerySP<BaoCaoCongNoKhachHangViewModel>("spSale_BaoCaoCongNoKhachHang", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate
            }).Where(x => x.TargetModule == "Customer").ToList();
            foreach (var item in dataTransactionLiabilities)
            {
                if (item.TongNoDauKy < 0)
                {
                    var a = Erp.BackOffice.Helpers.Common.PhanCachHangNgan(item.TongNoDauKy).Replace("-", "(");
                    item.TongNoDauKy_Text = a + ")";
                }
                else
                {
                    item.TongNoDauKy_Text = Erp.BackOffice.Helpers.Common.PhanCachHangNgan(item.TongNoDauKy);
                }
                if (item.TongNoCuoiKy < 0)
                {
                    var b = Erp.BackOffice.Helpers.Common.PhanCachHangNgan(item.TongNoCuoiKy).Replace("-", "(");
                    item.TongNoCuoiKy_Text = b + ")";
                }
                else
                {
                    item.TongNoCuoiKy_Text = Erp.BackOffice.Helpers.Common.PhanCachHangNgan(item.TongNoCuoiKy);
                }
            }
            return dataTransactionLiabilities;
        }

        #region React
        #region BC Bán Hàng
        public HttpResponseMessage GetListBaoCaoDoanhThuTongHop(string StartDate, string EndDate, int? BranchId, int? SalerId, string PaymentMethod)
        {
            BranchId = BranchId ?? 0;
            SalerId = SalerId ?? 0;
            PaymentMethod = PaymentMethod ?? "";
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var data = SqlHelper.QuerySP<DoanhThuTongHopViewModel>("spSale_BaoCaoDoanhThuTongHop", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SalerId = SalerId,
                PaymentMethod = PaymentMethod
            }).ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region BC Bán Hàng doanh thu sản phẩm
        public HttpResponseMessage GetListBaoCaoDoanhThuSanPham(string StartDate, string EndDate, int? BranchId, int? SalerId)
        {
            BranchId = BranchId ?? 0;
            SalerId = SalerId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var model = SqlHelper.QuerySP<DoanhThuTheoLoaiViewModel>("spSale_BaoCaoDoanhThuSanPham", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SalerId = SalerId,
            }).ToList();
          
            var data = model.GroupBy(x => new { x.BranchName, x.CustomerName, x.Code, x.ProductInvoiceId })
                .Select(item => new
                {
                    CreatedDate = item.FirstOrDefault().CreatedDate,
                    BranchName = item.FirstOrDefault().BranchName,
                    SalerName = item.FirstOrDefault().SalerName,
                    Code = item.FirstOrDefault().Code,
                    CustomerName = item.FirstOrDefault().CustomerName,
                    ProductName = item.FirstOrDefault().ProductName,
                    Quantity = item.FirstOrDefault().Quantity,
                    Price = item.FirstOrDefault().Price,
                    Unit = item.FirstOrDefault().Unit,
                    Amount = item.FirstOrDefault().Amount,
                    Point = item.FirstOrDefault().Point,
                    GroupItem = item
                });


            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region BC Bán Hàng doanh thu dich vụ
        public HttpResponseMessage GetListBaoCaoDoanhThuDichVu(string StartDate, string EndDate, int? BranchId, int? SalerId)
        {
            BranchId = BranchId ?? 0;
            SalerId = SalerId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var model = SqlHelper.QuerySP<DoanhThuTheoLoaiViewModel>("spSale_BaoCaoDoanhThuDichVu", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SalerId = SalerId,

            }).ToList();
            var data = model.GroupBy(x => new { x.BranchName, x.CustomerName, x.Code, x.ProductInvoiceId })
               .Select(item => new
               {
                   CreatedDate = item.FirstOrDefault().CreatedDate,
                   BranchName = item.FirstOrDefault().BranchName,
                   SalerName = item.FirstOrDefault().SalerName,
                   Code = item.FirstOrDefault().Code,
                   CustomerName = item.FirstOrDefault().CustomerName,
                   ProductName = item.FirstOrDefault().ProductName,
                   Quantity = item.FirstOrDefault().Quantity,
                   Price = item.FirstOrDefault().Price,
                   Unit = item.FirstOrDefault().Unit,
                   Amount = item.FirstOrDefault().Amount,
                   Point = item.FirstOrDefault().Point,
                   GroupItem = item
               });

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region BC kho hàng nhập xuất tồn sản phẩm
        public HttpResponseMessage GetListBaoCaoKhoHangTonSanPham(string StartDate, string EndDate, int? BranchId, int? WarehouseId)
        {
            BranchId = BranchId ?? 0;
            WarehouseId = WarehouseId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var data = SqlHelper.QuerySP<KhoHangNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTonSanPham", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                BranchId = BranchId


            }).ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region BC kho hàng nhập xuất tồn thẻ
        public HttpResponseMessage GetListBaoCaoKhoHangTonThe(string StartDate, string EndDate, int? BranchId, int? WarehouseId)
        {
            BranchId = BranchId ?? 0;
            WarehouseId = WarehouseId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var data = SqlHelper.QuerySP<KhoHangNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTonThe", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                BranchId = BranchId


            }).ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region BC kho hàng nhập xuất tồn quà tặng
        public HttpResponseMessage GetListBaoCaoKhoHangTonQuaTang(string StartDate, string EndDate, int? BranchId, int? WarehouseId)
        {
            BranchId = BranchId ?? 0;
            WarehouseId = WarehouseId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var data = SqlHelper.QuerySP<KhoHangNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTonQuaTang", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                BranchId = BranchId


            }).ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region BC tài chính thu chi tiết
        public HttpResponseMessage GetListBaoCaoTaiChinhTheoKhoanThu(string StartDate, string EndDate, int? BranchId, int? SalerId)
        {
            BranchId = BranchId ?? 0;
            SalerId = SalerId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var data = SqlHelper.QuerySP<BaoCaoThuChiViewModel>("spSale_BaoCaoThu", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SalerId = SalerId

            }).ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region BC tài chính chi chi tiết
        public HttpResponseMessage GetListBaoCaoTaiChinhTheoKhoanChi(string StartDate, string EndDate, int? BranchId, int? SalerId)
        {
            BranchId = BranchId ?? 0;
            SalerId = SalerId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var data = SqlHelper.QuerySP<BaoCaoThuChiViewModel>("spSale_BaoCaoChi", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SalerId = SalerId

            }).ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion
       
        #region BC tài chính thu chi tổng hợp
        public HttpResponseMessage GetListBaoCaoTongHopTaiChinh(string StartDate, string EndDate, int? BranchId, int? SalerId)
        {
            BranchId = BranchId ?? 0;
            SalerId = SalerId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var data = SqlHelper.QuerySP<BaoCaoTongHopTaiChinhViewModel>("spSale_BaoCaoTonghopThuChi", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SalerId = SalerId

            }).ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion
        
        #region BC công nợ khách hàng
        public HttpResponseMessage GetListBaoCaoCongNoKhachHang(string StartDate, string EndDate)
        {
            
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(StartDate, EndDate, ref d_startDate, ref d_endDate);
            #endregion 
            var data = SqlHelper.QuerySP<BaoCaoCongNoKhachHangViewModel>("spSale_CongNoCuaKhachHang", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,

            }).ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion
        #endregion
    }
}