using OfficeOpenXml;
using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SkillMapProject.Helper
{
    public static class Utils
    {
        public static string ConvertViewToString(string viewName, object model, ViewDataDictionary ViewData, ControllerContext ControllerContext)
        {
            try
            {
                ViewData.Model = model;
                using (StringWriter writer = new StringWriter())
                {
                    ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                    ViewContext vContext = new ViewContext(ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                    vResult.View.Render(vContext, writer);
                    return writer.ToString();
                }
            }
            catch (Exception)
            {
                return "";
            }
           
        }
    }

    public static class ExportUtils
    {
        private static async Task<int> BindingFormatForExcel(ExcelWorksheet worksheet, List<Certification> list)
        {
            // Set default width cho tất cả column
            //worksheet.DefaultColWidth = 50;
            // Tự động xuống hàng khi text quá dài
            worksheet.Cells.Style.WrapText = true;
            // Tạo header
            worksheet.Cells[1, 1].Value = "Thông tin";
            worksheet.Cells["A1:E1"].Merge = true;
            worksheet.Cells[1, 6].Value = "Cấp độ hiện tại";
            worksheet.Cells["F1:I1"].Merge = true;
            worksheet.Cells[1,10].Value = "Cấp độ I";
            worksheet.Cells["J1:K1"].Merge = true;
            worksheet.Cells[1, 12].Value = "Cấp độ II";
            worksheet.Cells["L1:M1"].Merge = true;
            worksheet.Cells[1, 14].Value = "Cấp độ III";
            worksheet.Cells["N1:O1"].Merge = true;

            worksheet.Cells[2, 1].Value = "Code";
            worksheet.Cells[2, 2].Value = "Họ tên";
            worksheet.Cells[2, 3].Value = "Chức vụ";
            worksheet.Cells[2, 4].Value = "Bộ phận";
            worksheet.Cells[2, 5].Value = "Khách hàng";
            worksheet.Cells[2, 6].Value = "Cấp độ";
            worksheet.Cells[2, 7].Value = "Ngày cấp hiện tại";
            worksheet.Cells[2, 8].Value = "Ngày thi xác nhận";
            worksheet.Cells[2, 9].Value = "Ngày thi thực tế";
            worksheet.Cells[2, 10].Value = "Cấp độ";
            worksheet.Cells[2, 11].Value = "Ngày cấp";
            worksheet.Cells[2, 12].Value = "Cấp độ";
            worksheet.Cells[2, 13].Value = "Ngày cấp";
            worksheet.Cells[2, 14].Value = "Cấp độ";
            worksheet.Cells[2, 15].Value = "Ngày cấp";
            //// Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
            //using (var range = worksheet.Cells["A1:M1"])
            //{
            //    range.AutoFitColumns(20);
            //    // Set PatternType
            //    range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
            //    // Set Màu cho Background
            //    range.Style.Fill.BackgroundColor.SetColor(Color.Aqua);
            //    // Canh giữa cho các text
            //    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            //    // Set Font cho text  trong Range hiện tại
            //    range.Style.Font.SetFromFont(new Font("Arial", 10));
            //    // Set Border
            //    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            //    // Set màu ch Border
            //    range.Style.Border.Bottom.Color.SetColor(Color.Blue);
            //}
            
            await Task.Run(() => {
                // Đỗ dữ liệu từ list vào 
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    worksheet.Cells[i + 3, 1].Value = item.Member.Code;
                    worksheet.Cells[i + 3, 2].Value = item.Member.Name;
                    worksheet.Cells[i + 3, 3].Value = item.Member.Pos;
                    worksheet.Cells[i + 3, 4].Value = item.Member.Dept;
                    worksheet.Cells[i + 3, 5].Value = "";
                    worksheet.Cells[i + 3, 6].Value = item.LevelCurrent;
                    worksheet.Cells[i + 3, 7].Value = item.CurrentGrantDateStr;
                    worksheet.Cells[i + 3, 8].Value = item.NgayThiXacNhanStr;
                    worksheet.Cells[i + 3, 9].Value = item.NgayThiThucTeStr;
                    worksheet.Cells[i + 3, 10].Value = item.CapDo;
                    worksheet.Cells[i + 3, 11].Value = item.NgayCapStr;
                    worksheet.Cells[i + 3, 12].Value = item.NangCap;
                    worksheet.Cells[i + 3, 13].Value = item.CNNguoiDaoTao;
                    worksheet.Cells[i + 3, 14].Value = item.NgayCNNguoiDaoTaoStr;
                   
                }
            });
            return 1;
        }
        public static async Task<Stream> CreateExcelFile(Stream stream = null, List<Certification> list = null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Hanker";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "EPP test background";
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "This is my fucking generated Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("First Sheet");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[0];
                // Đổ data vào Excel file
                // workSheet.Cells[1, 1].LoadFromCollection(list, true, TableStyles.Dark9);
             
                await BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }
    }
}