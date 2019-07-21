using Erp.BackOffice.Models;
using Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Erp.BackOffice.Helpers
{
    public class ExcelHelper
    {
        public static Stream CreateExcelFile<T>(IEnumerable<T> models, ExcelExport excelExport, Stream stream = null, bool showTime = false)
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("First Sheet");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var worksheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                // Set default width cho tất cả column
                worksheet.DefaultColWidth = 15;
                // Tự động xuống hàng khi text quá dài
                worksheet.Cells.Style.WrapText = true;

                //Tiêu đề báo cáo
                worksheet.Cells[1, 1].Value = excelExport.getTile();
                var rangeTitle = worksheet.Cells[1, 1, 1, models.FirstOrDefault().GetType().GetProperties().Count() +1];
                rangeTitle.Merge = true;
                //rangeTitle.Style.Fill = ex
                rangeTitle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rangeTitle.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rangeTitle.Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));

                int rowHeader1 = 2;
              //  int rowHeader2 = rowHeader1 + (int)excelExport.MaxHeaderLevel;
                int rowStartItem = rowHeader1 + (int)excelExport.MaxHeaderLevel;
                int nCol = 1;


                foreach (var group in excelExport.ExcelHeaders)
                {
                    group.RowSpan(group);
                    renderHeader(worksheet, group, excelExport.MaxHeaderLevel.Value,  rowHeader1, ref nCol);
                }

                //Đỗ dữ liệu từ list vào
                int nRow = rowStartItem;
                int it = 0;
                var list_number =new List<string> { "System.Int32","System.Int64","System.Float","System.Double","System.Decimal"};
                foreach (var item in models)
                {
                    var pi = item.GetType().GetProperties();
                    int jt = 0;
                    worksheet.Cells[nRow + it, jt + 1].Value = it + 1;
                    worksheet.Cells[nRow + it, jt + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[nRow + it, jt + 1].Style.Font.SetFromFont(new Font("Tahoma", 8, FontStyle.Regular));
                    jt = 1;
                    foreach (var p in pi)
                    {
                        var result = p.GetValue(item);
                        if (result == null)
                        {
                            worksheet.Cells[nRow + it, jt + 1].Value = null;
                        }
                        else if (result.GetType().FullName == "System.DateTime")
                        {
                            worksheet.Cells[nRow + it, jt + 1].Value = ((DateTime)result).ToString("dd/MM/yyyy HH:mm");
                        }
                        else
                        {
                            worksheet.Cells[nRow + it, jt + 1].Value = result ;
                        }
                        worksheet.Cells[nRow + it, jt + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        worksheet.Cells[nRow + it, jt + 1].Style.Font.SetFromFont(new Font("Tahoma", 8, FontStyle.Regular));
                        jt++;
                    }
                    it++;
                }

                excelPackage.Save();
                return excelPackage.Stream;
            }
        }
        private static void formatExcelRange(ExcelRange excelRange)
        {
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            excelRange.Style.Font.SetFromFont(new Font("Tahoma", 8, FontStyle.Bold));
        }
        private static void renderHeader(ExcelWorksheet worksheet, ExcelHeader excelHeader, int maxHeaderLevel,int rowHeader1,ref int nCol)
        {
            if (excelHeader.Width != null)
                worksheet.Column(nCol).Width = excelHeader.Width.Value;
            ExcelRange excelRange = null;
            var colSpanSub = excelHeader.ColSpan(excelHeader);
            
            //var rowSpanSub = maxHeaderLevel - excelHeaders.Level;
            if (colSpanSub == 1)
            {
                var toRow = rowHeader1 + maxHeaderLevel - excelHeader.Level - 1;
                worksheet.Cells[rowHeader1, nCol, toRow, nCol].Value = excelHeader.Name;
                excelRange = worksheet.Cells[rowHeader1, nCol, toRow, nCol];
                excelRange.Merge = true;
                nCol++;
            }
            else
            {
                var rowNext = rowHeader1 + 1;
                var nColNext = nCol + colSpanSub-1;
                worksheet.Cells[excelHeader.Level + rowHeader1, nCol, excelHeader.Level + rowHeader1, nColNext].Value = excelHeader.Name;
                excelRange = worksheet.Cells[excelHeader.Level + rowHeader1, nCol, excelHeader.Level + rowHeader1, nColNext];
                excelRange.Merge = true;
                //nCol = nColNext;
                foreach (var group in excelHeader.SubExcelHeaders)
                {
                    renderHeader(worksheet, group, maxHeaderLevel, rowNext, ref nCol);
                }
               // nCol++;
            }
            formatExcelRange(excelRange);
           
        }
    }
}
