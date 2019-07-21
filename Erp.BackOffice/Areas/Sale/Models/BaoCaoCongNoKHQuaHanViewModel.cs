
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class BaoCaoCongNoKHQuaHanViewModel
    {
        public int? CustomerId { get; set; }
public string MaChungTuGoc { get; set; }
public decimal? TotalAmount { get; set; }
public decimal? DaThu { get; set; }
public decimal? ConLai { get; set; }
public string CustomerCode { get; set; }
public string CustomerName { get; set; }
public int? NgayTra { get; set; }

    }
}