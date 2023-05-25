using AsompTTViewStatus.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AsompTTViewStatus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            List<ExportApi> _exportApi = DAL.DAL.GetListAsrkFile("TT", DateTime.Now, DateTime.Now);


            //if (!String.IsNullOrEmpty(_searchText))
            //{
            //    _exportApi.Where(t => t.Barcode!.Contains(_searchText));
            //}
            


            return View(_exportApi.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}