using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class AddNewItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AddNewItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Create(Products products)
        {

            _context.products.Add(products);
            _context.SaveChanges();
            return RedirectToAction("AddNewItem", "Home");
        }
        [Authorize]
        [HttpPost]
        public JsonResult Edit(int id)
        {
            var product = _context.products
                  .FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                return Json(product);
            }
            else
            {
                return Json(null);
            }


        }

        [Authorize]
        [HttpPost]
        [Route("update")]
        public IActionResult Update(Products product)
        {
            if (ModelState.IsValid)
            {
                _context.products.Update(product);
                _context.SaveChanges();
            }

            return RedirectToAction("AddNewItem", "Home");
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            var productDel = _context.products.SingleOrDefault(p => p.Id == id);
            if (productDel != null)
            {
                _context.products.Remove(productDel);
                _context.SaveChanges();
                TempData["msg"] = "Ok";
            }
            else
            {
                TempData["msg"] = "Bad";
            }
            return RedirectToAction("AddNewItem", "Home");

        }


    }
}