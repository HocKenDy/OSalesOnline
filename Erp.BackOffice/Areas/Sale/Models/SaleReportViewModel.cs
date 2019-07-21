using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class ChartItem
    {
        public string group { get; set; }
        public string group2 { get; set; }
        public string label { get; set; }
        public string label2 { get; set; }
        public object data { get; set; }
    }

    public class DoanhThuTongHopViewModel
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool? IsArchive { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? TotalNoVAT { get; set; }
        public double TaxFee { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public double? AccumulatedPoint { get; set; }
        public double? UsePoint { get; set; }
        public bool? CheckUsePoint { get; set; }
        public decimal? UsePointAmount { get; set; }
        public int? Frequency { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string SalerFullName { get; set; }
    }
    public class DoanhThuTheoLoaiViewModel
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string ProductInvoiceId { get; set; }
        public string SalerName { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal?  Price { get; set; }
        public string Unit { get; set; }
        public decimal Amount { get; set; }
        public double? Point { get; set; }
        
    }
    public class KhoHangNhapXuatTonViewModel
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public int First_Remain { get; set; }
        public int Center_InboundQuantity { get; set; }
        public int Center_OutboundQuantity { get; set; }
        public int Last_Remain { get; set; }
        public int First_Amount { get; set; }
        public int Center_InboundAmount { get; set; }
        public int Center_OutboundAmount { get; set; }
        public int Last_Amount { get; set; }



    }
    public class BaoCaoThuChiViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string CustomerName { get; set; }
        public string MaChungTuGoc { get; set; }
        public string LoaiChungTuGoc { get; set; }
        public string CancelReson { get; set; }
        public string SalerName { get; set; }
        public string BranchName { get; set; }
    }
    public class BaoCaoTongHopTaiChinhViewModel
    {
        public string BranchName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CustomerName { get; set; }
        public double totalRevenue { get; set; }
        public double totalCost { get; set; }
        public double Profit { get; set; }
       
    }
    
}