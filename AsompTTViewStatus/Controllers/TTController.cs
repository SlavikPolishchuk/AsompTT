using AsompTTViewStatus.Models;
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

        //List<ExportApi> exportApis = DAL.DAL.GetListAsrkFile();
        public ActionResult IndexTT(int? page = 1)
        {
            int pageSize = 20;

            var tt = DAL.DAL.GetListAsrkFile();

            var items = tt.Skip((page!.Value - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.Dateins).ToList();

            PageViewModel pageViewModel = new PageViewModel(page.Value, tt.Count());

            IndexViewModel model = new IndexViewModel() { ExportApis = items, PageViewModel = pageViewModel };

            return View(model);
        }

        [HttpPost]
        public ActionResult IndexTT(string searchText, int? fileId, int? state, int? page = 1)
        {
            //DateTime df =(!String.IsNullOrEmpty(datefrom))? DateTime.Parse(datefrom): DateTime.Now.AddDays(-1);
            //DateTime dt = (!String.IsNullOrEmpty(dateto)) ? DateTime.Parse(dateto) : DateTime.Now;

            var tt = DAL.DAL.GetListAsrkFile();
            //var items = tt.Where(s =>
            //((fileId.HasValue ? s.File_Id == fileId : false)
            //|| (!string.IsNullOrEmpty(searchText) ? s.Barcode.ToLower().Contains(searchText.ToLower()):false)


            ////&& (DateTime.Parse(s.Dateins) >= df && DateTime.Parse(s.Dateins) <= dt)
            //&& (s.State == state)))
            //    .Skip((page!.Value - 1) * pageSize)
            //    .Take(pageSize)
            //    .OrderByDescending(x=>x.Dateins)
            //    .ToList();

            var items = tt.Where(s =>
            fileId.HasValue ? s.File_Id == fileId.Value :true
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
                CreatePackageAsBytes(DAL.DAL.GetListAsrkFile());
            return File(stream, "application/xlsx", string.Format("{0}-({1}).xlsx",
                "ASOMP_TT", DateTime.Now.ToString("dd MMM yyyy HH:mm",
                CultureInfo.CreateSpecificCulture("uk-UA"))));
        }
    }
}
