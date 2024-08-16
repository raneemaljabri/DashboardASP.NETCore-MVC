using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class ProductsDetailsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsDetailsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [Authorize]
        public IActionResult CreateDeatils(ProductsDetails productsDetails, IFormFile photo)
        {
            // محاولة تحويل الكمية إلى عدد صحيح
            if (!int.TryParse(productsDetails.Qty, out int qty))
            {
                TempData["ErrorMessage"] = "يرجى إدخال كمية صحيحة للمنتج";
                return RedirectToAction("ProductsDetails", "Home");
            }

            // التحقق من صحة البيانات المدخلة
            if (photo == null || photo.Length == 0 || productsDetails.Price <= 0 || qty <= 0 || string.IsNullOrEmpty(productsDetails.Color))
            {
                TempData["ErrorMessage"] = "يرجى ملء كامل الحقول المطلوبة للمنتج";
                return RedirectToAction("ProductsDetails", "Home");
            }

            // التحقق مما إذا كان المنتج موجود مسبقًا
            bool productExists = _context.productsDetails.Any(p => p.ProductId == productsDetails.ProductId);
            if (productExists)
            {
                TempData["ErrorMessage"] = "لقد تم إضافة تفاصيل للمنتج مسبقًا!";
                return RedirectToAction("ProductsDetails", "Home");
            }

            // حفظ صورة المنتج
            var fileName = Path.GetFileName(photo.FileName); // احصل على اسم الملف فقط
            var dir = "img";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, dir, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            productsDetails.Images = Path.Combine(dir, fileName);

            // إضافة تفاصيل المنتج إلى قاعدة البيانات
            _context.productsDetails.Add(productsDetails);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "تم إضافة تفاصيل المنتج بنجاح!";
            return RedirectToAction("ProductsDetails", "Home");
        }





        [HttpPost]
        [Authorize]
        public JsonResult EditProductsDetails(int id)
        {
            var product = _context.productsDetails
                  .FirstOrDefault(p => p.ProductId == id);
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
        [Route("update-products-details")]
        public IActionResult UpdateProductsDetails(ProductsDetails productsDetail, IFormFile photo)
        {
            var existingProduct = _context.productsDetails.SingleOrDefault(p => p.ProductId == productsDetail.ProductId);

            if (existingProduct == null)
            {
                return RedirectToAction("ProductsDetails", "Home");
            }

            if (photo != null && photo.Length > 0)
            {
                var fileName = Path.GetFileName(photo.FileName); // احصل على اسم الملف فقط
                var dir = "img";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, dir, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                productsDetail.Images = Path.Combine(dir, fileName);
                existingProduct.Images = productsDetail.Images;
            }

            // محاولة تحويل الكمية إلى عدد صحيح
            if (!int.TryParse(productsDetail.Qty, out int qty))
            {
                TempData["ErrorMessage"] = "يرجى إدخال كمية صحيحة للمنتج";
                return RedirectToAction("ProductsDetails", "Home");
            }

            // التحقق من صحة البيانات المدخلة
            if (productsDetail.Price > 0 && qty > 0 && !string.IsNullOrEmpty(productsDetail.Color))
            {
                existingProduct.Price = productsDetail.Price;
                existingProduct.Qty = qty.ToString(); // احفظ الكمية كقيمة نصية إذا كان ذلك مطلوبًا
                existingProduct.Color = productsDetail.Color;

                _context.productsDetails.Update(existingProduct);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "تم تحديث المنتج بنجاح!";
            }
            else
            {
                TempData["ErrorMessage"] = "يرجى التأكد من المعلومات المدخلة!";
            }

            return RedirectToAction("ProductsDetails", "Home");
        }

        [Authorize]
        [Route("delete-product-details")]
        public IActionResult DeleteProductsDetails(int product_id)
        {

            var productDel = _context.productsDetails.SingleOrDefault(p => p.ProductId == product_id);

            if (productDel != null)
            {

                _context.productsDetails.Remove(productDel);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "تم حذف المنتج بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "لم يتم العثور على المنتج";
            }

            return RedirectToAction("ProductsDetails", "Home");

        }
    }
}