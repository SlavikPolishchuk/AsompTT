using AsompTTViewStatus.Models;
using AsompTTViewStatus.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AsompTTViewStatus.Controllers
{
    public class LvivController : Controller
    {
        const int pageSize = 20;

        //List<ExportApi> exportApis = DAL.DAL.GetListAsrkFile();
        public ActionResult IndexLviv(int? page = 1)
        {
            int pageSize = 20;

            var tt = DAL.DAL.GetListAsrkFile("Lviv");

            var items = tt.Skip((page!.Value - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.Dateins).ToList();

            PageViewModel pageViewModel = new PageViewModel(page.Value, tt.Count());

            IndexViewModel model = new IndexViewModel() { ExportApis = items, PageViewModel = pageViewModel };

            return View(model);
        }

        [HttpPost]
        public ActionResult IndexLviv(string searchText, int? fileId, int? state, DateTime dateFrom, DateTime dateTo, int? page = 1)
        {
            dateFrom = (DateOnly.Parse(dateFrom.ToShortDateString()) == DateOnly.Parse("01.01.0001")) ? DateTime.Now : dateFrom;
            dateTo = (DateOnly.Parse(dateTo.ToShortDateString()) == DateOnly.Parse("01.01.0001")) ? dateFrom : dateTo;
            var tt = DAL.DAL.GetListAsrkFile("Lviv");
            if (DateOnly.Parse(dateFrom.ToShortDateString()) != DateOnly.Parse("01.01.0001")
            &&
                (DateOnly.Parse(dateTo.ToShortDateString()) != DateOnly.Parse("01.01.0001")))
            {
                DateTime dateF = dateFrom;
                DateTime dateT = (dateF > dateTo) ? dateF : dateTo;

                tt = DAL.DAL.GetListAsrkFile("Lviv", dateF, dateT);
            }


            if (fileId != null)
                tt = DAL.DAL.GetListAsrkFile("Lviv", fileId);

            if (!string.IsNullOrEmpty(searchText))
                tt = DAL.DAL.GetListAsrkFile("Lviv", searchText.ToUpper());

            if ((fileId != null) && (!string.IsNullOrEmpty(searchText)))
                tt = DAL.DAL.GetListAsrkFile("Lviv", searchText.ToUpper(), fileId);

            var items = tt.Where(s =>
            fileId.HasValue ? s.File_Id == fileId.Value : true
                && !System.String.IsNullOrEmpty(searchText) ? s.Barcode.ToLower().Contains(searchText.ToLower()) : true
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
                CreatePackageAsBytes(DAL.DAL.GetListAsrkFile("Lviv"));
            return File(stream, "application/xlsx", string.Format("{0}-({1}).xlsx",
                "ASOMP_TT", DateTime.Now.ToString("dd MMM yyyy HH:mm",
                CultureInfo.CreateSpecificCulture("uk-UA"))));
        }
    }
}
