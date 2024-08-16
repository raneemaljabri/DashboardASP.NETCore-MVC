using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class DamagedProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DamagedProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }



        [HttpPost]
        public IActionResult CreateDamagedProducts(DamegedProducts damagedProducts)
        {
            if (ModelState.IsValid)
            {


                var existingProduct = _context.damegedproducts
                    .FirstOrDefault(dp => dp.ProductId == damagedProducts.ProductId);

                if (existingProduct != null)
                {
                    TempData["ErrorMessage"] = "لقد تم اضافة المنتج مسبقا!";
                }
                else
                {
                    _context.damegedproducts.Add(damagedProducts);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "تم اضافة المنتج بنجاح";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "يرجى التحقق من ملء الحقول المطلوبة ";
            }

            return RedirectToAction("DamagedProducts", "Home");
        }






    }
}