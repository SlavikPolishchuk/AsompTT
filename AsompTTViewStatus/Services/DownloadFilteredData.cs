using AsompTTViewStatus.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AsompTTViewStatus.Services
{
    public class DownloadFilteredData
    {
        public static byte[] ReportVerifyCreatePackageAsBytes(List<ExportApi> messagesList,
            DateTime dateStart, DateTime dateEnd)
        {
            using (var mstm = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excel = new ExcelPackage(mstm))
                {
                    CreateExcelDocReprotVerify(messagesList, excel, dateStart, dateEnd);
                }

                mstm.Flush();
                mstm.Close();
                return mstm.ToArray();
            }
        }

        private static byte[] CreateExcelDocReprotVerify(List<ExportApi> messagesList,
            ExcelPackage document, DateTime dateStart, DateTime dateEnd)
        {
            document.Workbook.Worksheets.Add("Лист1");

            var workSheet = document.Workbook.Worksheets["Лист1"];
            workSheet.Column(1).Width = 2;
            workSheet.Column(2).Width = 10;      //№ B
            workSheet.Column(3).Width = 15;     //Дата C
            workSheet.Column(4).Width = 15;     //Время D
            workSheet.Column(5).Width = 20;     //ШКІ E
            workSheet.Column(6).Width = 20;     //№ телефона F 
            workSheet.Column(7).Width = 35;     //Статус G

            workSheet.Column(3).Style.Numberformat.Format = "dd-MM-yyyy";
            workSheet.Column(4).Style.Numberformat.Format = "HH:mm:ss";

            using (var range = workSheet.Cells[1, 1, 1, 1])
            {
                range.Style.Font.Bold = true;
            }

            var currIdx = 2;
            workSheet.Cells["B" + currIdx + ":G" + currIdx].Merge = true;
            workSheet.Cells["B" + currIdx].Value = "Вручення за кодом СМС";
            workSheet.Cells["B" + currIdx].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["B" + currIdx].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["B" + currIdx].Style.Font.SetFromFont(new Font("Arial", 14));

            currIdx++;

            workSheet.Cells["B" + currIdx + ":G" + currIdx].Merge = true;
            workSheet.Cells["B" + currIdx].Value = " за період з " + dateStart.ToShortDateString()
                + " по " + dateEnd.ToShortDateString();
            workSheet.Cells["B" + currIdx].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["B" + currIdx].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["B" + currIdx].Style.Font.SetFromFont(new Font("Arial", 12));


            /*using (var range = workSheet.Cells["B" + currIdx])
            {
                range.Style.Font.Italic = true;
                range.Style.Font.UnderLine = true;
                range.Style.Font.Bold = true;
            }*/
            currIdx += 2;
            int stratIdx, endIdx;
            stratIdx = currIdx;

            if (messagesList.Any())
            {
                workSheet.Cells["B" + currIdx].Value = "№";
                workSheet.Cells["C" + currIdx].Value = "Дата";
                workSheet.Cells["D" + currIdx].Value = "Время";
                workSheet.Cells["E" + currIdx].Value = "ШКІ";
                workSheet.Cells["F" + currIdx].Value = "№ телефона";
                workSheet.Cells["G" + currIdx].Value = "Статус";
                ++currIdx;

                foreach (var messsage in messagesList)
                {
                    workSheet.Cells["B" + currIdx].Value = messsage.File_Id;
                    workSheet.Cells["C" + currIdx].Value = messsage.Barcode;
                    workSheet.Cells["D" + currIdx].Value = messsage.DataStr;
                    workSheet.Cells["E" + currIdx].Value = messsage.Dateins;
                    workSheet.Cells["F" + currIdx].Value = messsage.State;
                    workSheet.Cells["G" + currIdx].Value = messsage.Message;

                    ++currIdx;

                }
                endIdx = --currIdx;
            }
            else
            {
                workSheet.Cells["B" + currIdx + ":G" + currIdx].Merge = true;
                workSheet.Cells["B" + currIdx].Value = "Повідомлення відсутні";
                endIdx = currIdx;
            }

            workSheet.Cells["B" + stratIdx + ":G" + endIdx].Style.WrapText = true;
            workSheet.Cells["B" + stratIdx + ":G" + endIdx].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["B" + stratIdx + ":G" + endIdx].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["B" + stratIdx + ":G" + endIdx].Style.Font.SetFromFont(new Font("Arial", 10));

            ReportVerifySetCellBorder(workSheet, "B" + stratIdx, "G" + endIdx, "Thin");
            return document.GetAsByteArray();
        }

        private static void ReportVerifySetCellBorder(ExcelWorksheet workSheet, string startIdx, string endIdx, string borderType)
        {
            if (borderType == "Medium")
            {
                foreach (var cell in workSheet.Cells[startIdx + ":" + endIdx])
                {
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                }
            }
            else
            {
                foreach (var cell in workSheet.Cells[startIdx + ":" + endIdx])
                {
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }
        }


        public static byte[] CreatePackageAsBytes(List<ExportApi> messages)
        {
            using (var mstm = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excel = new ExcelPackage(mstm))
                {
                    CreateExcelDoc(messages, excel);
                }

                mstm.Flush();
                mstm.Close();
                return mstm.ToArray();
            }
        }

        private static byte[] CreateExcelDoc(List<ExportApi> messages, ExcelPackage document)
        {
            document.Workbook.Worksheets.Add("Лист1");

            var workSheet = document.Workbook.Worksheets["Лист1"];
            workSheet.Column(1).Width = 2;
            workSheet.Column(2).Width = 13;     
            workSheet.Column(3).Width = 15;     
            workSheet.Column(4).Width = 50;     
            workSheet.Column(5).Width = 10;     
            workSheet.Column(6).Width = 10;     
            workSheet.Column(7).Width = 10;     


            workSheet.Column(4).Style.Numberformat.Format = "dd-MM-yyyy HH:mm";

            using (var range = workSheet.Cells[1, 1, 1, 1])
            {
                range.Style.Font.Bold = true;
            }

            var currIdx = 2;
            int stratIdx, endIdx;

            workSheet.Cells["B" + currIdx].Value = "Id пакету відправлення";
            workSheet.Cells["C" + currIdx].Value = "ШКІ";
            workSheet.Cells["D" + currIdx].Value = "Дані відправлення";
            workSheet.Cells["E" + currIdx].Value = "Дата створення";
            workSheet.Cells["F" + currIdx].Value = "Статус відправлення";
            workSheet.Cells["G" + currIdx].Value = "Повідомлення коду відправлення";
            

            foreach (var item in messages)
            {

                stratIdx = currIdx;

                
                ++currIdx;

                workSheet.Cells["B" + currIdx].Value = item.File_Id;
                workSheet.Cells["C" + currIdx].Value = item.Barcode;
                workSheet.Cells["D" + currIdx].Value = item.DataStr;
                workSheet.Cells["E" + currIdx].Value = item.Dateins;
                workSheet.Cells["F" + currIdx].Value = item.State;
                workSheet.Cells["G" + currIdx].Value = item.Message;
            


                endIdx = --currIdx;

                workSheet.Cells["B" + stratIdx + ":G" + endIdx].Style.WrapText = true;
                //workSheet.Cells["B" + stratIdx + ":G" + endIdx].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //workSheet.Cells["B" + stratIdx + ":G" + endIdx].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //workSheet.Cells["B" + stratIdx + ":G" + endIdx].Style.Font.SetFromFont(new Font("Arial", 10));

                SetCellBorder(workSheet, "B" + stratIdx, "G" + endIdx, "Thin");

                currIdx++;

            }
            return document.GetAsByteArray();
        }

        private static void SetCellBorder(ExcelWorksheet workSheet, string startIdx, string endIdx, string borderType)
        {
            if (borderType == "Medium")
            {
                foreach (var cell in workSheet.Cells[startIdx + ":" + endIdx])
                {
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                }
            }
            else
            {
                foreach (var cell in workSheet.Cells[startIdx + ":" + endIdx])
                {
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }
        }
    }
}