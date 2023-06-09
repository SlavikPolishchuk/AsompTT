﻿using AsompTTViewStatus.Models;
using AsompTTViewStatus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;

namespace AsompTTViewStatus.Controllers
{
    public class TTController : Controller
    {
        const int pageSize = 20;

        public ActionResult IndexTT(int? page = 1)
        {
            int pageSize = 20;

            var tt = DAL.DAL.GetListAsrkFile("TT");

            var items = tt.Skip((page!.Value - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.Dateins).ToList();

            PageViewModel pageViewModel = new PageViewModel(page.Value, tt.Count());

            IndexViewModel model = new IndexViewModel() { ExportApis = items, PageViewModel = pageViewModel };

            return View(model);
        }

        [HttpPost]
        public ActionResult IndexTT(string searchText, int? fileId, int? state, DateTime dateFrom, DateTime dateTo, int? page = 1)
        {
            dateFrom = (DateOnly.Parse(dateFrom.ToShortDateString()) == DateOnly.Parse("01.01.0001")) ? DateTime.Now : dateFrom;
            dateTo = (DateOnly.Parse(dateTo.ToShortDateString()) == DateOnly.Parse("01.01.0001")) ? dateFrom : dateTo;

            var tt = DAL.DAL.GetListAsrkFile("TT");

            if (DateOnly.Parse(dateFrom.ToShortDateString()) != DateOnly.Parse("01.01.0001")
                &&
                (DateOnly.Parse(dateTo.ToShortDateString()) != DateOnly.Parse("01.01.0001")))
            {

                DateTime dateF = dateFrom;
                DateTime dateT = (dateF > dateTo) ? dateF : dateTo;

                tt = DAL.DAL.GetListAsrkFile("TT", dateF, dateT);
            }


            if (fileId != null)
                tt = DAL.DAL.GetListAsrkFile("TT", fileId);

            if (!string.IsNullOrEmpty(searchText))
                tt = DAL.DAL.GetListAsrkFile("TT", searchText.ToUpper());

            if ((fileId != null) && (!string.IsNullOrEmpty(searchText)))
                tt = DAL.DAL.GetListAsrkFile("TT", searchText.ToUpper(), fileId);

            var items = tt.Where(s =>
            fileId.HasValue ? s.File_Id == fileId.Value : true
                && !String.IsNullOrEmpty(searchText) ? s.Barcode.ToLower().Contains(searchText.ToLower()) : true
                && (s.State == state)
                )
                .Skip((page!.Value - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Dateins)
                .ToList();


            PageViewModel pageViewModel = new PageViewModel(page.Value, tt.Count());

            IndexViewModel model = new IndexViewModel() { ExportApis = items, PageViewModel = pageViewModel };

            return View(model);
        }

        public ActionResult DownloadToExcel()
        {
            byte[] stream = DownloadFilteredData.
                CreatePackageAsBytes(DAL.DAL.GetListAsrkFile("TT"));
            return File(stream, "application/xlsx", string.Format("{0}-({1}).xlsx",
                "ASOMP_TT", DateTime.Now.ToString("dd MMM yyyy HH:mm",
                CultureInfo.CreateSpecificCulture("uk-UA"))));
        }
    }
}
