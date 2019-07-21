using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Erp.BackOffice.Sale.Models
{
    public class BaoCaoCongNoKhachHangViewModel
    {
        public string TargetModule { get; set; }
        public string TargetCode { get; set; }
        public string TargetName { get; set; }
        public decimal TangDauKy { get; set; }
        public decimal GiamDauKy { get; set; }
        public decimal TangGiuaKy { get; set; }
        public decimal GiamGiuaKy { get; set; }
        public decimal TongNoGiuaKy { get; set; }
        public decimal TongNoDauKy { get; set; }
        public decimal TongNoCuoiKy { get; set; }
        public string TongNoDauKy_Text { get; set; }
        public string TongNoCuoiKy_Text { get; set; }
        public string Phone { get; set; }
        public decimal Debt { get; set; }

    }
}