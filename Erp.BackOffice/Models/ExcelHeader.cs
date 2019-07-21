using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.BackOffice.Models
{
    public class ExcelHeader
    {
        public string Name { get; set; }
        public int? Width { get; set; }
        public string DataType { get; set; }
        public int? Id { get; set; }
        public int? OrderNo { get; set; }
        public int Level { get; set; }
        //public int? ParentId { get; set; }
        //public bool? HasSubList { get;  set; }
        public List<ExcelHeader> SubExcelHeaders { get; set; }
        public ExcelHeader()
        {
            //NumberOfSubList = 0;
            //HasSubList = false;
            Level = 0;
            SubExcelHeaders = new List<ExcelHeader>();
        }
        public int ColSpan(ExcelHeader header)
        {
            int cells = 1;
            if (header.SubExcelHeaders.Count() > 0)
                foreach (var sub in header.SubExcelHeaders)
                {
                    cells += header.SubExcelHeaders.IndexOf(sub) == 0 ? 0 : ColSpan(sub);
                }

            return cells;
            //return SubExcelHeaders.Count();
        }
        public int RowSpan(ExcelHeader header)
        {
            int rows = 1;
            if (header.SubExcelHeaders.Count() > 0)
            {
                rows += 1;
                foreach (var sub in header.SubExcelHeaders)
                {
                    rows += RowSpan(sub);
                    sub.Level = header.Level + 1;
                }
            }
            return rows;
        }
    }

    public class ExcelExport
    {

        public string Title { private get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<ExcelHeader> ExcelHeaders { get; set; }
        public bool? Merge { get; set; }
        /// <summary>
        /// Số dòng header để group. Mặc định = 1 (chỉ có 1 dòng header) 
        /// </summary>
        public int? MaxHeaderLevel { get; set; }
        public string getTile()
        {
            return string.Format("BÁO CÁO {0} TỪ NGÀY {1} ĐẾN NGÀY {2}", Title, StartDate, EndDate);
        }
        public ExcelExport()
        {
            Merge = false;
            MaxHeaderLevel = 1;
        }
       
    }
}
