using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TesInspiro.Business;
using TesInspiro.Domain;
using TesInspiro2023.Models;

namespace TesInspiro2023.Controllers
{
    public class DetailItemController : Controller
    {
        readonly TesInspiroBusinessProcessor _processor = new TesInspiroBusinessProcessor();

        // GET: DetailItemController
        public ActionResult Index()
        {
            //var data = _processor.GetDetailItem(id);
            if (TempData["model"] != null)
            {
                var modelJson = TempData["model"].ToString();
                var data = JsonConvert.DeserializeObject<MsItemsViewModel>(modelJson);
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

        // POST: DetailItemController/Create
        [HttpPost]
        public JsonResult InsertUpdateCart(ShoppingCart_DTO data)
        {
            try
            {
                bool result = _processor.InsertUpdateCart(data);
                return Json(new { result = result });
                //return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // GET: DetailItemController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DetailItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DetailItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DetailItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
