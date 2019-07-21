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
//using Erp.Domain.RealEstate.Repositories;
//using Erp.Domain.RealEstate;
namespace Erp.BackOffice.Controllers
{
    public class ReportAPIController : ApiController
    {
        #region Báo cáo nhập xuất tồn
        [HttpGet]
        public object BaoCaoNhapXuatTon(string StartDate, string EndDate, string ProductCode, int? WarehouseId, int? BranchId)
        {
            ProductCode = ProductCode == null ? "" : ProductCode;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            BranchId = BranchId == null ? 0 : BranchId;
            //    return null;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var data = SqlHelper.QuerySP<BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon_New", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductCode = ProductCode,
                WarehouseId = WarehouseId,
                BranchId = BranchId
            }).ToList();

            return data;
        }
        #endregion
    }
}