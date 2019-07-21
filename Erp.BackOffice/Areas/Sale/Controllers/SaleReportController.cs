using System.Globalization;
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
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Helpers;
using Erp.Domain.Helper;
using Newtonsoft.Json;
using Erp.Domain.Account.Interfaces;
using Erp.BackOffice.Account.Models;
using System.Data;
using System.Web;
using Erp.BackOffice.Areas.Administration.Models;
using Erp.BackOffice.Models;
using System.IO;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class SaleReportController : Controller
    {
        private readonly IBranchRepository BranchRepository;
        private readonly IUserRepository userRepository;
        private readonly IBranchDepartmentRepository branchDepartmentRepository;
        private readonly ISaleReportRepository saleReportRepository;
        private readonly IProductInvoiceRepository invoiceRepository;
        private readonly IPurchaseOrderRepository purchaseOrderRepository;
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IProductInboundRepository inboundRepository;
        private readonly IProductOutboundRepository outboundRepository;
        private readonly ISalesReturnsRepository salesReturnsRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IQueryHelper QueryHelper;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IProductRepository ProductRepository;
        private readonly IPageMenuRepository PageMenuRepository;

        public SaleReportController(
            IBranchRepository _Branch
            , IUserRepository _user
            , IBranchDepartmentRepository branchDepartment
            , ISaleReportRepository saleReport
            , IProductInvoiceRepository invoice
            , IPurchaseOrderRepository purchaseOrder
            , IWarehouseRepository warehouse
            , IProductInboundRepository inbound
            , IProductOutboundRepository outbound
            , ISalesReturnsRepository salesReturns
            , IQueryHelper _QueryHelper
            , ICustomerRepository _Customer
            , IInventoryRepository inventory
            , IWarehouseRepository _Warehouse
            , IProductRepository _Product
             , IPageMenuRepository _PageMenuRepository
            )
        {
            BranchRepository = _Branch;
            userRepository = _user;
            branchDepartmentRepository = branchDepartment;
            saleReportRepository = saleReport;
            invoiceRepository = invoice;
            purchaseOrderRepository = purchaseOrder;
            warehouseRepository = warehouse;
            inboundRepository = inbound;
            outboundRepository = outbound;
            salesReturnsRepository = salesReturns;
            customerRepository = _Customer;
            QueryHelper = _QueryHelper;
            inventoryRepository = inventory;
            WarehouseRepository = _Warehouse;
            ProductRepository = _Product;
            PageMenuRepository = _PageMenuRepository;
        }

        #region Biểu đồ
        public ActionResult Summary(bool single, int? year, int? month, int? quarter, string group, string manufacturer)
        {
            SaleReportSumaryViewModel model = new SaleReportSumaryViewModel();

            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            group = string.IsNullOrEmpty(group) ? "" : group;
            manufacturer = string.IsNullOrEmpty(manufacturer) ? "" : manufacturer;

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value);

            int branchId = Helpers.Common.CurrentUser.BranchId.Value;

            var qThongKeBanHang = invoiceRepository.GetAllvwInvoiceDetails()
                    .Where(x => x.IsArchive
                        && x.ProductInvoiceDate > StartDate
                        && x.ProductInvoiceDate < EndDate);

            //Thống kê đơn hàng khởi tạo/đang xử lý
            model.NumberOfProductInvoice_Pendding = invoiceRepository.GetAllvwProductInvoice().Where(item => item.Status == App_GlobalResources.Wording.OrderStatus_pending).Count();
            model.NumberOfProductInvoice_InProgress = invoiceRepository.GetAllvwProductInvoice().Where(item => item.Status == App_GlobalResources.Wording.OrderStatus_inprogress).Count();

            if (!string.IsNullOrEmpty(manufacturer))
            {
                qThongKeBanHang = qThongKeBanHang.Where(item => item.Manufacturer == manufacturer);
            }

            if (!string.IsNullOrEmpty(group))
            {
                qThongKeBanHang = qThongKeBanHang.Where(item => item.ProductGroup == group);
            }

            if (string.IsNullOrEmpty(group) && string.IsNullOrEmpty(manufacturer))
            {
                model.IsFilter_ByProductProperty = true;
                //Thống kê doanh thu bán hàng
                var qProductInvoice = invoiceRepository.GetAllvwProductInvoice()
                    .Where(x => x.BranchId == branchId
                        && x.IsArchive
                        && x.CreatedDate > StartDate
                        && x.CreatedDate < EndDate);

                var Revenue = qProductInvoice.Sum(item => item.TotalAmount);
                var NumberOfProductInvoice = qProductInvoice.Count();
                model.Revenue = Revenue == null ? 0 : Convert.ToDouble(Revenue.Value);
                model.NumberOfProductInvoice = NumberOfProductInvoice;

                //Thống kê hàng bán trả lại
                var qSalesReturns = salesReturnsRepository.GetAllvwSalesReturns()
                    .Where(x => x.BranchId == branchId
                    && x.CreatedDate > StartDate
                    && x.CreatedDate < EndDate);

                var SalesReturnAmount = qSalesReturns.Sum(item => item.TotalAmount);
                var NumberOfSalesReturn = qSalesReturns.Count();

                model.SalesReturnAmount = SalesReturnAmount == null ? 0 : Convert.ToDouble(SalesReturnAmount.Value);
                model.NumberOfSalesReturn = NumberOfSalesReturn;
            }
            else
            {
                //Thống kê theo nhóm sản phẩm
                model.IsFilter_ByProductProperty = false;

                var Revenue = qThongKeBanHang.Sum(item => (item.Quantity * item.Price) - item.DisCountAmount);
                var NumberOfProductInvoice = qThongKeBanHang.Sum(item => item.Quantity.Value);
                model.Revenue = Revenue == null ? 0 : Convert.ToDouble(Revenue.Value);
                model.NumberOfProductInvoice = NumberOfProductInvoice;
                model.ProductGroup = group;
                model.Manufacturer = manufacturer;
            }

            //Thống kê bán hàng theo nhân viên bán
            var qThongKeBanHang_TheoNhanVien = qThongKeBanHang.Where(item => item.Amount > 0).Select(item => new { item.SalerId, item.SalerFullName, item.Amount })
                .ToList()
                .GroupBy(l => l.SalerId)
                .Select(cl => new ChartItem
                {
                    label = cl.FirstOrDefault().SalerFullName,
                    data = cl.Sum(i => i.Amount).ToString(),
                }).ToList();

            ViewBag.jsonThongKeBanHang_TheoNhanVien = JsonConvert.SerializeObject(qThongKeBanHang_TheoNhanVien);

            //Thống kê bán hàng theo khách hàng
            var qThongKeBanHang_TheoKhachHang = qThongKeBanHang.Where(item => item.Amount > 0).Select(item => new { item.CustomerId, item.CustomerName, item.Amount })
                .ToList()
                .GroupBy(l => l.CustomerId)
                .Select(cl => new ChartItem
                {
                    label = cl.FirstOrDefault().CustomerName,
                    data = cl.Sum(i => i.Amount).ToString(),
                }).ToList();

            ViewBag.jsonThongKeBanHang_TheoKhachHang = JsonConvert.SerializeObject(qThongKeBanHang_TheoKhachHang);

            return View(model);
        }

        public ActionResult ChartInvoiceDayInMonth(bool single, int? year, int? month, int? quarter, string group, string manufacturer)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            group = string.IsNullOrEmpty(group) ? "" : group;
            manufacturer = string.IsNullOrEmpty(manufacturer) ? "" : manufacturer;

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value);

            if (single)
            {
                var data = SqlHelper.QuerySQL<ChartItem>(string.Format(@"SELECT convert(varchar, day(ProductInvoiceDate)) + '/' + convert(varchar, month(ProductInvoiceDate)) As label, sum(Amount) As data
                                                  FROM [vwSale_ProductInvoiceDetail] where IsArchive = 1 and (Manufacturer = '{2}' or '' = '{2}') and (ProductGroup = '{3}' or '' = '{3}')
                                                  and ProductInvoiceDate > '{0}' and ProductInvoiceDate < '{1}'
                                                  GROUP BY convert(varchar, day(ProductInvoiceDate)) + '/' + convert(varchar, month(ProductInvoiceDate))", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"), manufacturer, group));

                var jsonData = new List<ChartItem>();
                for (int i = StartDate.Day; i <= EndDate.Day; i++)
                {
                    string label = i + "/" + month.Value;
                    var obj = data.Where(item => item.label == label).FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new ChartItem();
                        obj.label = label;
                        obj.data = 0;
                    }

                    jsonData.Add(obj);
                }

                string json = JsonConvert.SerializeObject(jsonData);
                ViewBag.json = json;
            }
            else
            {
                var data = SqlHelper.QuerySQL<ChartItem>(string.Format(@"SELECT convert(varchar, month(ProductInvoiceDate)) + '/' + convert(varchar, year(ProductInvoiceDate)) As label, sum(Amount) As data
                                                  FROM [vwSale_ProductInvoiceDetail] where IsArchive = 1 and (Manufacturer = '{2}' or '' = '{2}') and (ProductGroup = '{3}' or '' = '{3}')
                                                  and ProductInvoiceDate > '{0}' and ProductInvoiceDate < '{1}'
                                                  GROUP BY convert(varchar, month(ProductInvoiceDate)) + '/' + convert(varchar, year(ProductInvoiceDate))", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"), manufacturer, group));

                var jsonData = new List<ChartItem>();
                for (int i = StartDate.Month; i <= EndDate.Month; i++)
                {
                    string label = i + "/" + year.Value;
                    var obj = data.Where(item => item.label == label).FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new ChartItem();
                        obj.label = label;
                        obj.data = 0;
                    }

                    jsonData.Add(obj);
                }

                string json = JsonConvert.SerializeObject(jsonData);
                ViewBag.json = json;
            }

            return View();
        }

        public ActionResult ChartProductSaleInMonth(bool single, int? year, int? month, int? quarter, string group, string manufacturer)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            group = string.IsNullOrEmpty(group) ? "" : group;
            manufacturer = string.IsNullOrEmpty(manufacturer) ? "" : manufacturer;

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value);

            var data = SqlHelper.QuerySQL<ChartItem>(string.Format(@"SELECT TOP 10 *
                                                                    FROM 
                                                                    (
	                                                                    SELECT ProductCode as label, ProductName as label2, sum(Quantity) As data
	                                                                    FROM [vwSale_ProductInvoiceDetail]
	                                                                    WHERE IsArchive = 1 and (Manufacturer = '{3}' or '' = '{3}') and (ProductGroup = '{2}' or '' = '{2}') and CreatedDate > '{0}' and CreatedDate < '{1}'
	                                                                    GROUP BY ProductId, ProductCode, ProductName
                                                                    ) as Tbx
                                                                    order by data desc", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"), group, manufacturer));

            string json = JsonConvert.SerializeObject(data);
            ViewBag.json = json;

            return View();
        }

        public ActionResult ChartProductInboundOfCard(int? branchId, string StartDate, string EndDate)
        {
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);

            var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            var data = SqlHelper.QuerySQL<ChartItem>(string.Format(@"exec spSale_ChartProductInboundOfCard '{0}', '{1}', '{2}' ", d_startDate, d_endDate, branchId));

            string json = JsonConvert.SerializeObject(data);
            ViewBag.json = json;

            return View();
        }
        public ActionResult ChartProductOutboundOfCard(int? branchId, string StartDate, string EndDate)
        {
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);

            var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            var data = SqlHelper.QuerySQL<ChartItem>(string.Format(@"exec spSale_ChartProductOutboundOfCard '{0}', '{1}', '{2}' ", d_startDate, d_endDate, branchId));

            string json = JsonConvert.SerializeObject(data);
            ViewBag.jsonOut = json;

            return View();
        }
        public ActionResult ChartServiceSaleInMonth(string noLayout)
        {

            var data = SqlHelper.QuerySQL<ChartItem>(string.Format(@"SELECT TOP 10 *
                                                                    FROM 
                                                                    (
	                                                                    SELECT Sale_Product.Code as label, Sale_Product.Name as label2, sum(Quantity * Price) As data
	                                                                    FROM [Sale_ProductInvoiceDetail] as PInD
                                                                        left join Sale_Product on Sale_Product.Id = PInD.ProductId
	                                                                    WHERE month(PInD.CreatedDate) = {0} and Sale_Product.Type = 'service'
	                                                                    GROUP BY ProductId, Sale_Product.Code, Sale_Product.Name
                                                                    ) as Tbx
                                                                    order by data desc", DateTime.Now.Month));

            string json = JsonConvert.SerializeObject(data);
            ViewBag.json = json;

            ViewBag.noLayout = noLayout;
            return View();
        }

        public ActionResult InventoryQueryExpiryDate()
        {
            var warehouseList = warehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            ViewBag.warehouseList = warehouseList.AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
            return View();
        }

        public ActionResult ChartInboundAndOutboundInMonth(bool single, int? year, int? month, int? quarter, string group, string manufacturer)
        {
            var model = new ChartInboundAndOutboundInMonthViewModel();

            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            group = string.IsNullOrEmpty(group) ? "" : group;
            manufacturer = string.IsNullOrEmpty(manufacturer) ? "" : manufacturer;
            int MonthOfQuarter1 = 0, MonthOfQuarter2 = 0, MonthOfQuarter3 = 0;

            switch (quarter)
            {
                case 1:
                    MonthOfQuarter1 = 1;
                    MonthOfQuarter2 = 2;
                    MonthOfQuarter3 = 3;
                    model.Quarter = "I";
                    break;
                case 2:
                    MonthOfQuarter1 = 4;
                    MonthOfQuarter2 = 5;
                    MonthOfQuarter3 = 6;
                    model.Quarter = "II";
                    break;
                case 3:
                    MonthOfQuarter1 = 7;
                    MonthOfQuarter2 = 8;
                    MonthOfQuarter3 = 9;
                    model.Quarter = "III";
                    break;
                case 4:
                    MonthOfQuarter1 = 10;
                    MonthOfQuarter2 = 11;
                    MonthOfQuarter3 = 12;
                    model.Quarter = "IV";
                    break;
            }

            var dataInbound = SqlHelper.QuerySP<ChartItem>("spSale_ReportProductInbound", new
            {
                SingleMonth = single,
                Month = month,
                Year = year,
                MonthOfQuarter1 = MonthOfQuarter1,
                MonthOfQuarter2 = MonthOfQuarter2,
                MonthOfQuarter3 = MonthOfQuarter3,
                ProductGroup = group,
                Manufacturer = manufacturer
            });

            var dataOutbound = SqlHelper.QuerySP<ChartItem>("spSale_ReportProductOutbound", new
            {
                SingleMonth = single,
                Month = month,
                Year = year,
                MonthOfQuarter1 = MonthOfQuarter1,
                MonthOfQuarter2 = MonthOfQuarter2,
                MonthOfQuarter3 = MonthOfQuarter3,
                ProductGroup = group,
                Manufacturer = manufacturer
            });

            //Xử lý dữ liệu
            foreach (var item in dataInbound)
            {
                if (!string.IsNullOrEmpty(item.label))
                {
                    item.label = item.label.Trim().Replace("\t", "");
                }
                else
                {
                    item.label = "";
                }
            }

            foreach (var item in dataOutbound)
            {
                if (!string.IsNullOrEmpty(item.label))
                {
                    item.label = item.label.Trim().Replace("\t", "");
                }
                else
                {
                    item.label = "";
                }
            }

            var category = dataInbound.Select(item => item.label).Union(dataOutbound.Select(item => item.label));
            var qGroupTemp = dataInbound.Select(item => new { GroupName = item.group, GroupName2 = item.group2, NumberOfInbound = Convert.ToInt32(item.data), NumberOfOutbound = 0 })
                .Union(dataOutbound.Select(item => new { GroupName = item.group, GroupName2 = item.group2, NumberOfInbound = 0, NumberOfOutbound = Convert.ToInt32(item.data) }));

            //Thống kế theo nhóm sản phẩm
            var qGroup = qGroupTemp.GroupBy(
                item => item.GroupName,
                (key, g) => new
                {
                    GroupName = key,
                    NumberOfInbound = g.Sum(i => i.NumberOfInbound),
                    NumberOfOutbound = g.Sum(i => i.NumberOfOutbound)
                }
            );

            model.GroupList = qGroup.ToDataTable();

            //Thống kế theo nhà sản xuất
            var qManufacturer = qGroupTemp.GroupBy(
                item => item.GroupName2,
                (key, g) => new
                {
                    GroupName = key,
                    NumberOfInbound = g.Sum(i => i.NumberOfInbound),
                    NumberOfOutbound = g.Sum(i => i.NumberOfOutbound)
                }
            );

            model.ManufacturerList = qManufacturer.ToDataTable();

            model.jsonInbound = JsonConvert.SerializeObject(dataInbound);

            model.jsonOutbound = JsonConvert.SerializeObject(dataOutbound);

            model.jsonCategory = JsonConvert.SerializeObject(category);

            if (dataInbound.Count() > 0)
            {
                model.TongNhap = dataInbound.Sum(item => Convert.ToInt32(item.data));
            }
            else
            {
                model.TongNhap = 0;
            }

            if (dataOutbound.Count() > 0)
            {
                model.TongXuat = dataOutbound.Sum(item => Convert.ToInt32(item.data));
            }
            else
            {
                model.TongXuat = 0;
            }

            model.Year = year.Value;
            model.Month = month.Value;
            model.Single = single;

            return View(model);
        }
        #endregion

        #region Báo cáo + export bán hàng doanh thu tổng hợp
        public ActionResult BCBH_DoanhThuTongHop()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCBH_DoanhThuTongHop(string startDate, string endDate, int? branchId, int? salerId, string paymentMethod)
        {
            branchId = branchId ?? 0;
            salerId = salerId ?? 0;
            paymentMethod = paymentMethod ?? "";
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<DoanhThuTongHopViewModel>("spSale_BaoCaoDoanhThuTongHop", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                SalerId = salerId,
                PaymentMethod = paymentMethod

            });

            var excelExport = new ExcelExport()
            {
                Title = "DOANH THU TỔNG HƠP",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Ngày hóa đơn", Width=15},
                   new ExcelHeader { Name ="Chi nhánh", Width=15},
                   new ExcelHeader { Name ="Khách hàng", Width=15},
                   new ExcelHeader { Name ="Hóa đơn"},
                   new ExcelHeader { Name ="Điểm tích"},
                   new ExcelHeader { Name ="Điểm sử dụng"},
                   new ExcelHeader { Name ="Tiền sử dụng"},
                   new ExcelHeader { Name ="Tiền đơn hàng", Width=15},
                   new ExcelHeader { Name ="Đã thanh toán", Width=20},
                   new ExcelHeader { Name ="Còn lại", Width=15},
                   new ExcelHeader { Name ="Nhân viên", Width=15},
                   new ExcelHeader { Name ="Ghi chú", Width=15}
                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.CreatedDate,
                        item.BranchName,
                        item.CustomerName,
                        item.Code,
                        item.AccumulatedPoint,
                        item.UsePoint,
                        item.UsePointAmount,
                        item.TotalAmount,
                        item.PaidAmount,
                        item.RemainingAmount,
                        item.SalerFullName,
                        item.Note

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
            return RedirectToAction("BCBH_DoanhThuTongHop");

        }
        #endregion
        #region Báo cáo + export doanh thu bán hàng theo SP
        public ActionResult BCBH_DoanhThuSanPham()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCBH_DoanhThuSanPham(string startDate, string endDate, int? branchId, int? salerId)
        {
            branchId = branchId ?? 0;
            salerId = salerId ?? 0;

            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<DoanhThuTheoLoaiViewModel>("spSale_BaoCaoDoanhThuSanPham", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                SalerId = salerId,

            });

            var excelExport = new ExcelExport()
            {
                Title = "DOANH THU THEO SẢN PHẨM",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Ngày hóa đơn", Width=15},
                   new ExcelHeader { Name ="Chi nhánh", Width=15},
                   new ExcelHeader { Name ="Khách hàng", Width=15},
                   new ExcelHeader { Name ="Hóa đơn"},
                   new ExcelHeader { Name ="Sản phẩm", Width = 30},
                   new ExcelHeader { Name ="Đơn vị", Width=15},
                   new ExcelHeader { Name ="Số lượng"},
                   new ExcelHeader { Name ="Giá"},
                   new ExcelHeader { Name ="Thành tiền", Width=15},
                   new ExcelHeader { Name ="Điểm", Width=15},
                   new ExcelHeader { Name ="Nhân viên", Width=15},

                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.CreatedDate,
                        item.BranchName,
                        item.CustomerName,
                        item.Code,
                        item.ProductName,
                        item.Unit,
                        item.Quantity,
                        item.Price,
                        item.Amount,
                        item.Point,
                        item.SalerName

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
            return RedirectToAction("BCBH_DoanhThuSanPham");

        }
        #endregion
        #region Báo cáo + export doanh thu bán hàng theo DV
        public ActionResult BCBH_DoanhThuDichVu()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCBH_DoanhThuDichVu(string startDate, string endDate, int? branchId, int? salerId)
        {
            branchId = branchId ?? 0;
            salerId = salerId ?? 0;

            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<DoanhThuTheoLoaiViewModel>("spSale_BaoCaoDoanhThuDichVu", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                SalerId = salerId,

            });

            var excelExport = new ExcelExport()
            {
                Title = "DOANH THU THEO DỊCH VỤ",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Ngày hóa đơn", Width=15},
                   new ExcelHeader { Name ="Chi nhánh", Width=15},
                   new ExcelHeader { Name ="Khách hàng", Width=15},
                   new ExcelHeader { Name ="Hóa đơn"},
                   new ExcelHeader { Name ="Sản phẩm", Width = 30},
                   new ExcelHeader { Name ="Đơn vị", Width=15},
                   new ExcelHeader { Name ="Số lượng"},
                   new ExcelHeader { Name ="Giá"},
                   new ExcelHeader { Name ="Thành tiền", Width=15},
                   new ExcelHeader { Name ="Điểm", Width=15},
                   new ExcelHeader { Name ="Nhân viên", Width=15},

                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.CreatedDate,
                        item.BranchName,
                        item.CustomerName,
                        item.Code,
                        item.ProductName,
                        item.Unit,
                        item.Quantity,
                        item.Price,
                        item.Amount,
                        item.Point,
                        item.SalerName

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
            return RedirectToAction("BCBH_DoanhThuDichVu");

        }
        #endregion
        #region Báo cáo + export kho hàng nhập xuất tồn theo sản phẩm
        public ActionResult BCKH_NhapXuatTonSanPham()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCKH_NhapXuatTonSanPham(string startDate, string endDate, int? branchId, int? warehouseId)
        {
            branchId = branchId ?? 0;
            warehouseId = warehouseId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<KhoHangNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTonSanPham", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                WarehouseId = warehouseId

            });

            var excelExport = new ExcelExport()
            {
                Title = "TỒN KHO SẢN PHẨM",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Mã sản phẩm"},
                   new ExcelHeader { Name ="Tên sản phẩm", Width=30},
                   new ExcelHeader { Name ="Đơn vị"},
                   new ExcelHeader { Name ="Đầu kỳ"},
                   new ExcelHeader { Name ="Tổng tiền đầu kỳ", Width=20},
                   new ExcelHeader { Name ="Nhập trong kỳ"},
                   new ExcelHeader { Name ="Tổng tiền nhập trong kỳ", Width=25},
                   new ExcelHeader { Name ="Xuất trong kỳ"},
                   new ExcelHeader { Name ="Tổng tiền xuất trong kỳ", Width=25},
                   new ExcelHeader { Name ="Cuối kỳ"},
                   new ExcelHeader { Name ="Tổng tiền cuối kỳ", Width=25}

                    
                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.ProductCode, 
                        item.ProductName, 
                        item.ProductUnit, 
                        item.First_Remain,
                        item.First_Amount, 
                        item.Center_InboundQuantity,
                        item.Center_InboundAmount,
                        item.Center_OutboundQuantity,
                        item.Center_OutboundAmount, 
                        item.Last_Remain,
                        item.Last_Amount
                        
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
            return RedirectToAction("BCKH_NhapXuatTonSanPham");

        }
        #endregion
        #region Báo cáo + export kho hàng nhập xuất tồn theo thẻ
        public ActionResult BCKH_NhapXuatTonThe()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCKH_NhapXuatTonThe(string startDate, string endDate, int? branchId, int? warehouseId)
        {
            branchId = branchId ?? 0;
            warehouseId = warehouseId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<KhoHangNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTonThe", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                WarehouseId = warehouseId

            });

            var excelExport = new ExcelExport()
            {
                Title = "TỒN KHO THẺ",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Mã thẻ"},
                   new ExcelHeader { Name ="Tên thẻ", Width=30},
                   new ExcelHeader { Name ="Đơn vị"},
                   new ExcelHeader { Name ="Đầu kỳ"},
                   new ExcelHeader { Name ="Nhập trong kỳ"},
                   new ExcelHeader { Name ="Xuất trong kỳ"},
                   new ExcelHeader { Name ="Cuối kỳ"},
                  
                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.ProductCode,
                        item.ProductName,
                        item.ProductUnit,
                        item.First_Remain,
                        item.Center_InboundQuantity,     
                        item.Center_OutboundQuantity,
                        item.Last_Remain
                     
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
            return RedirectToAction("BCKH_NhapXuatTonThe");

        }
        #endregion
        #region Báo cáo kho hàng nhập xuất tồn quà tặng
        public ActionResult BCKH_NhapXuatTonQuaTang()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCKH_NhapXuatTonQuaTang(string startDate, string endDate, int? branchId, int? warehouseId)
        {
            branchId = branchId ?? 0;
            warehouseId = warehouseId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<KhoHangNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTonQuaTang", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                WarehouseId = warehouseId

            });

            var excelExport = new ExcelExport()
            {
                Title = "TỒN KHO THẺ",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Mã quà tặng"},
                   new ExcelHeader { Name ="Tên quà tặng", Width=30},
                   new ExcelHeader { Name ="Đơn vị"},
                   new ExcelHeader { Name ="Đầu kỳ"},
                   new ExcelHeader { Name ="Nhập trong kỳ"},
                   new ExcelHeader { Name ="Xuất trong kỳ"},
                   new ExcelHeader { Name ="Cuối kỳ"},

                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.ProductCode,
                        item.ProductName,
                        item.ProductUnit,
                        item.First_Remain,
                        item.Center_InboundQuantity,
                        item.Center_OutboundQuantity,
                        item.Last_Remain

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
            return RedirectToAction("BCKH_NhapXuatTonQuaTang");

        }
        #endregion

        #region Báo cáo + export tài chính thu chi tiết
        public ActionResult BCTC_BaoCaoThu()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCTC_BaoCaoThu(string startDate, string endDate, int? branchId, int? salerId)
        {
            branchId = branchId ?? 0;
            salerId = salerId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<BaoCaoThuChiViewModel>("spSale_BaoCaoThu", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                SalerId = salerId

            });

            var excelExport = new ExcelExport()
            {
                Title = "TÀI CHÍNH THU CHI TIẾT",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Chi nhánh", Width=15},
                   new ExcelHeader { Name ="Mã thu", Width=15},
                   new ExcelHeader { Name ="Ngày chứng từ"},
                   new ExcelHeader { Name ="Mã chứng từ"},
                   new ExcelHeader { Name ="Loại chứng từ"},
                    new ExcelHeader { Name ="Khách hàng"},
                   new ExcelHeader { Name ="Tổng tiền", Width=15},
                   new ExcelHeader { Name ="Hình thức thanh toán", Width=20},
                   new ExcelHeader { Name ="Lý do hủy", Width=15},
                   new ExcelHeader { Name ="Nhân viên", Width=15}
                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.BranchName,
                        item.Code,
                        item.VoucherDate,
                        item.MaChungTuGoc,
                        item.LoaiChungTuGoc,
                        item.CustomerName,
                        item.Amount,
                        item.PaymentMethod,
                        item.CancelReson,
                        item.SalerName
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
            return RedirectToAction("BCTC_BaoCaoChi");

        }
        #endregion

        #region Báo cáo + export tài chính chi chi tiết
        public ActionResult BCTC_BaoCaoChi()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCTC_BaoCaoChi(string startDate, string endDate, int? branchId, int? salerId)
        {
            branchId = branchId ?? 0;
            salerId = salerId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<BaoCaoThuChiViewModel>("spSale_BaoCaoChi", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                SalerId = salerId

            });

            var excelExport = new ExcelExport()
            {
                Title = "TÀI CHÍNH CHI CHI TIẾT",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Chi nhánh", Width=15},
                   new ExcelHeader { Name ="Mã thu", Width=15},
                   new ExcelHeader { Name ="Ngày chứng từ"},
                   new ExcelHeader { Name ="Mã chứng từ"},
                   new ExcelHeader { Name ="Loại chứng từ"},
                   new ExcelHeader { Name ="Tổng tiền", Width=15},
                   new ExcelHeader { Name ="Hình thức thanh toán", Width=20},
                   new ExcelHeader { Name ="Lý do hủy", Width=15},
                   new ExcelHeader { Name ="Nhân viên", Width=15}
                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.BranchName,
                        item.Code,
                        item.VoucherDate,
                        item.MaChungTuGoc,
                        item.LoaiChungTuGoc,
                        item.Amount,
                        item.PaymentMethod,
                        item.CancelReson,
                        item.SalerName
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
            return RedirectToAction("BCTC_BaoCaoChi");

        }
        #endregion
        #region Báo cáo + export tài chính tổng hợp thu chi
        public ActionResult BCTC_BaoCaoTongHopThuChi()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCTC_BaoCaoTongHopThuChi(string startDate, string endDate, int? branchId, int? salerId)
        {
            branchId = branchId ?? 0;
            salerId = salerId ?? 0;
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<BaoCaoTongHopTaiChinhViewModel>("spSale_BaoCaoTonghopThuChi", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = branchId,
                SalerId = salerId

            });

            var excelExport = new ExcelExport()
            {
                Title = "TÀI CHÍNH TỔNG HỢP",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Chi nhánh", Width=15},
                   new ExcelHeader { Name ="Ngày"},
                   new ExcelHeader { Name ="Tổng thu", Width=15},
                   new ExcelHeader { Name ="Tổng chi", Width=15},
                   new ExcelHeader { Name ="Lợi nhuận", Width=15}
                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.BranchName,
                        item.CreatedDate,
                        item.totalRevenue,
                        item.totalCost,
                        item.Profit
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
            return RedirectToAction("BCTC_BaoCaoTongHopThuChi");

        }
        #endregion

        #region Báo cáo + export công nợ khách hàng
        public ActionResult BCCN_KhachHang()
        {

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel_BCCN_KhachHang(string startDate, string endDate)
        {
            #region Get beweent day
            var d_startDate = "";
            var d_endDate = "";
            Helpers.Common.ParseBetweenDate(startDate, endDate, ref d_startDate, ref d_endDate);
            #endregion 
            var q = SqlHelper.QuerySP<BaoCaoCongNoKhachHangViewModel>("spSale_CongNoCuaKhachHang", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,

            });

            var excelExport = new ExcelExport()
            {
                Title = "CÔNG NỢ KHÁCH HÀNG",
                StartDate = startDate,
                EndDate = endDate,
                ExcelHeaders = new List<ExcelHeader> {
                   new ExcelHeader { Name ="STT", Width =5},
                   new ExcelHeader { Name ="Mã khách hàng", Width=30},
                   new ExcelHeader { Name ="Tên khách hàng"},
                   new ExcelHeader { Name ="Số điện thoại", Width=15},
                   new ExcelHeader { Name ="Công nợ", Width=30}
                },
            };
            if (q != null && q.Count() > 0)
            {
                var stream = ExcelHelper.CreateExcelFile(
                    q.Select(item => new
                    {
                        item.TargetCode,
                        item.TargetName,
                        item.Phone,
                        item.Debt
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
            return RedirectToAction("BCCN_KhachHang");

        }
        #endregion
        public ActionResult BaoCaoBanHang_DoanhThuTongHop()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            //Danh sách nhân viên sale
            var SaleList = userRepository.GetUserbyUserType("Sales Excutive").Select(item => new { item.FullName, item.Id }).ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.Id + ""
                });

            ViewBag.SaleList = SaleList;
            //Danh sách khách hàng
            ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }

        public ActionResult BaoCaoBanHang_HangBanTraLai()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }

        public ActionResult BaoCaoBanHang_SoLuongBan()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }


     
        #region Báo cáo công nợ
        public ActionResult BaoCaoCongNoKH_ChiTiet()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }
        public ActionResult BaoCaoCongNoKH_QuaHan()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }

        public ActionResult BaoCaoCongNoKH_TongHop()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            //Danh sách nhân viên sale
            var SaleList = userRepository.GetUserbyUserType("Sales Excutive").Select(item => new { item.FullName, item.Id }).ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.Id + ""
                });

            ViewBag.SaleList = SaleList;
            //Danh sách khách hàng
            ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }
        public ActionResult BaoCaoCongNoNCC_ChiTiet()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            ////Danh sách khách hàng
            //ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }

        public ActionResult BaoCaoCongNoNCC_QuaHan()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            ////Danh sách khách hàng
            //ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }

        public ActionResult BaoCaoCongNoNCC_TongHop()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            ////Danh sách nhân viên sale
            //var SaleList = userRepository.GetUserbyUserType("Sales Excutive").Select(item => new { item.FullName, item.Id }).ToList()
            //    .Select(x => new SelectListItem
            //    {
            //        Text = x.FullName,
            //        Value = x.Id + ""
            //    });

            //ViewBag.SaleList = SaleList;
            //Danh sách khách hàng
            //ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }
        #endregion

        #region Báo cáo kho
        public ActionResult BaoCaoKho_CanhBaoTonKho(int? WarehouseId)
        {
            var warehouse = warehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            WarehouseId = WarehouseId ?? warehouse.FirstOrDefault().Id;
            IQueryable<InventoryViewModel> inventoryP = inventoryRepository.GetAllvwInventoryByWarehouseId(WarehouseId.Value)
                .Where(u => u.CBTK > 0)
            .Select(itemV => new InventoryViewModel
            {
                Id = itemV.Id,
                ProductId = itemV.ProductId,
                Quantity = itemV.Quantity,
                WarehouseId = itemV.WarehouseId,
                ProductCode = itemV.ProductCode,
                ProductName = itemV.ProductName,
                WarehouseName = itemV.WarehouseName,
                CBTK = itemV.CBTK,
                StatusInventory = itemV.StatusInventory
            }).OrderBy(x => x.Quantity);
            ViewBag.WarehouseName = warehouse.Where(x => x.Id == WarehouseId).FirstOrDefault().Name;
            return View(inventoryP);
        }

        public ActionResult BaoCaoKho_NhapXuatTon()
        {
            var warehouseList = warehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            ViewBag.warehouseList = warehouseList.AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
            return View();
        }

        public ActionResult BaoCaoKho_TonKho()
        {
            var warehouseList = warehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            ViewBag.warehouseList = warehouseList.AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
            return View();
        }

        public ActionResult BaoCaoKho_TonKhoTheoNgay(string group, string category, string manufacturer, int? page, string StartDate, string EndDate, int? WarehouseId)
        {
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);

            var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            var data = SqlHelper.QuerySP<spBaoCaoNhapXuatTon_TuanViewModel>("spSale_BaoCaoNhapXuatTon_Tuan", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = category,
                ProductGroup = group,
                Manufacturer = manufacturer
            });
            var product_outbound = SqlHelper.QuerySP<spBaoCaoXuatViewModel>("spSale_BaoCaoXuat", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = category,
                ProductGroup = group,
                Manufacturer = manufacturer
            }).ToList();
            //var pager = new Pager(data.Count(), page, 20);


            var Items = data.OrderBy(m => m.ProductCode)
              //.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
              .Select(item => new spBaoCaoNhapXuatTon_TuanViewModel
              {
                  CategoryCode = item.CategoryCode,
                  First_InboundQuantity = item.First_InboundQuantity,
                  First_OutboundQuantity = item.First_OutboundQuantity,
                  First_Remain = item.First_Remain,
                  Last_InboundQuantity = item.Last_InboundQuantity,
                  Last_OutboundQuantity = item.Last_OutboundQuantity,
                  ProductCode = item.ProductCode,
                  ProductId = item.ProductId,
                  ProductName = item.ProductName,
                  ProductUnit = item.ProductUnit,
                  Remain = item.Remain,
                  ProductMinInventory = item.ProductMinInventory
              }).ToList();

            ViewBag.productInvoiceDetailList = product_outbound;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];


            return View(Items);
        }
        #endregion
        #region Báo cáo xuất nhập tồn thẻ
        public ActionResult BaoCaoNhapXuat_TonTheoThe(int? branchId, string StartDate, string EndDate, int? WarehouseId)
        {

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            branchId = branchId ?? Helpers.Common.CurrentUser.BranchId;
            var warehouse = warehouseRepository.GetAllWarehouse().Where(x => x.BranchId == branchId);
            WarehouseId = WarehouseId ?? warehouse.FirstOrDefault().Id;

            var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            var data = SqlHelper.QuerySP<spBaoCaoNhapXuatTonTheViewModel>("spSale_BaoCaoNhapXuatTonThe", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                BranchId = branchId
            });


            //var pager = new Pager(data.Count(), page, 20);

            var Items = data.OrderBy(m => m.ProductCode)
              //.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
              .Select(item => new spBaoCaoNhapXuatTonTheViewModel
              {
                  ProductId = item.ProductId,
                  ProductCode = item.ProductCode,
                  ProductName = item.ProductName,
                  ProductUnit = item.ProductUnit,
                  First_Remain = item.First_Remain,
                  Center_InboundQuantity = item.Center_InboundQuantity,
                  Center_OutboundQuantity = item.Center_OutboundQuantity,
                  Last_Remain = item.Last_Remain

              }).ToList();

            return View(Items);
        }
        #endregion
        #region Báo cáo thu/chi
        public ActionResult BaoCaoTaiChinh_ChiTienChiTiet()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }
        public ActionResult BaoCaoTaiChinh_ChiTienTongHop()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }
        public ActionResult BaoCaoTaiChinh_ThuTienChiTiet()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }
        #endregion        

        #region Báo cáo mua hàng
        public ActionResult PurchaseOrderBySupplier()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
                SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }
        #endregion

        public ActionResult XuatExcel(string html)
        {
            Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoTonKho" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(html);
            Response.End();
            return Content("success");
        }

        #region Báo cáo
        public ActionResult RABCBH_BaoCaoNhapXuatTon()
        {
            return View();
        }
        #endregion

    }
}
