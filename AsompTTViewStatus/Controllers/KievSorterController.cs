using AsompTTViewStatus.Models;
using AsompTTViewStatus.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AsompTTViewStatus.Controllers
{
    public class KievSorterController: Controller
    {
        const int pageSize = 20;

        public ActionResult IndexKievSorter(int? page = 1)
        {
            int pageSize = 20;

            var tt = DAL.DAL.GetListSorterDataOut("TT");

            var items = tt.Skip((page!.Value - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.SendDate).ToList();

            PageViewModel pageViewModel = new PageViewModel(page.Value, tt.Count());

            IndexViewModelSorter model = new IndexViewModelSorter() { ExportApiSorters = items, PageViewModel = pageViewModel };

            return View(model);
        }

        [HttpPost]
        public ActionResult IndexKievSorter(string searchText, int? fileId, int? state, DateTime dateFrom, DateTime dateTo, int? page = 1)
        {
            dateFrom = (DateOnly.Parse(dateFrom.ToShortDateString()) == DateOnly.Parse("01.01.0001")) ? DateTime.Now : dateFrom;
            dateTo = (DateOnly.Parse(dateTo.ToShortDateString()) == DateOnly.Parse("01.01.0001")) ? dateFrom : dateTo;

            var tt = DAL.DAL.GetListSorterDataOut("TT");

            if (DateOnly.Parse(dateFrom.ToShortDateString()) != DateOnly.Parse("01.01.0001")
                &&
                (DateOnly.Parse(dateTo.ToShortDateString()) != DateOnly.Parse("01.01.0001")))
            {

                DateTime dateF = dateFrom;
                DateTime dateT = (dateF > dateTo) ? dateF : dateTo;

                tt = DAL.DAL.GetListSorterDataOut("TT", dateF, dateT);
            }


            if (fileId != null)
                tt = DAL.DAL.GetListSorterDataOut("TT", fileId);

            if (!string.IsNullOrEmpty(searchText))
                tt = DAL.DAL.GetListSorterDataOut("TT", searchText.ToUpper());

            if ((fileId != null) && (!string.IsNullOrEmpty(searchText)))
                tt = DAL.DAL.GetListSorterDataOut("TT", searchText.ToUpper(), fileId);

            var items = tt.Where(s =>
            fileId.HasValue ? s.MrpId == fileId.Value : true
                && !String.IsNullOrEmpty(searchText) ? s.BarcodeMpv.ToLower().Contains(searchText.ToLower()) : true
                && (s.IsSend == state)
                )
                .Skip((page!.Value - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.SendDate)
                .ToList();


            PageViewModel pageViewModel = new PageViewModel(page.Value, tt.Count());

            IndexViewModelSorter model = new IndexViewModelSorter() { ExportApiSorters = items, PageViewModel = pageViewModel };

            return View(model);
        }
    }
}
