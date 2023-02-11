using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TesInspiro.Business;
using TesInspiro.Domain;
using TesInspiro2023.Models;

namespace TesInspiro2023.Controllers
{
    public class ShoppingCartController : Controller
    {
        readonly TesInspiroBusinessProcessor _processor = new TesInspiroBusinessProcessor();

        #region View
        // GET: ShoppingCartController
        public ActionResult Index()
        {
            if (TempData["model"] != null)
            {
                var modelJson = TempData["model"].ToString();
                var data = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(modelJson);
                ViewData["Username"] = TempData["Username"];
                //TempData.Keep("model");
                TempData.Keep();

                return View(data);
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
        public IActionResult ShowPaymentPage(string username, int grandTotal)
        {
            TempData["GrandTotal"] = grandTotal;
            TempData["Username"] = username;
            return RedirectToAction("PaymentPage", "ShoppingCart");
        }
        public ActionResult PaymentPage()
        {
            ViewData["GrandTotal"] = TempData["GrandTotal"];
            ViewData["Username"] = TempData["Username"];
            TempData.Keep();
            return View();
        }
        #endregion

        #region JsonResult
        // POST: ShoppingCartController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteShoppingCart(int id, string username)
        {
            try
            {
                var result = _processor.DeleteShoppingCart(id, username);
                return Json(new { result = result, total = result.Count });
            }
            catch(Exception ex)
            {
                return Json(ex);
            }
        }

        // POST: ShoppingCartController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult UpdateItemCart(int id, int qty, string username)
        {
            try
            {
                var result = _processor.UpdateItemCart(id, qty, username);
                return Json(new { result = result, total = result.Count });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // POST: ShoppingCartController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Payment(string username, int pay, int grandTotal)
        {
            try
            {
                var result = _processor.Payment(username, pay, grandTotal);
                return Json(new { result = result });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        #endregion
    }
}
