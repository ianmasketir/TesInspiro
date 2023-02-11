using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TesInspiro.Business;
using TesInspiro.Domain;
using TesInspiro2023.Models;

namespace TesInspiro2023.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly TesInspiroBusinessProcessor _processor = new TesInspiroBusinessProcessor();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region View
        public IActionResult Index(string? item_name, int page = 1, int page_size = 5)
        {
            //ViewData["ListItems"] = _processor.GetListItems(item_name)
            var listItems = _processor.GetListItems(item_name)
                                              .Skip((page - 1) * page_size)
                                              .Take(page_size).ToList();
            return View(listItems);
        }
        public IActionResult ShowDetailItem(int id, string? username)
        {
            MsItemsViewModel data = _processor.GetDetailItem(id);
            TempData["model"] = JsonConvert.SerializeObject(data);
            TempData["Username"] = username;
            return RedirectToAction("index", "DetailItem");//, new { id = id, username = username });
            //return Redirect("/DetailItem/index");//, new { id = id, username = username });
        }
        public IActionResult ShowShoppingCart(string? username)
        {
            List<ShoppingCartViewModel> data = _processor.GetListShoppingCart(username);
            TempData["model"] = JsonConvert.SerializeObject(data);
            TempData["Username"] = username;
            return RedirectToAction("index", "ShoppingCart");//, new { id = id, username = username });
            //return Redirect("/DetailItem/index");//, new { id = id, username = username });
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region JsonResult
        [HttpGet]
        public JsonResult GetListItems(string? item_name, int page = 1, int page_size = 5)
        {
            try
            {
                var result = _processor.GetListItems(item_name)
                                       .Skip((page - 1) * page_size)
                                       .Take(page_size).ToList();
                return Json(new { result = result, total = result.Count });
            }
            catch(Exception ex)
            {
                return Json(ex);
            }
        }
        #endregion
    }
}