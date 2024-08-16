using Dashboard.Areas.Identity.Data;
using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<DashboardUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<DashboardUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            var user = HttpContext.User.Identity.Name ?? null;
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddMinutes(50);
            Response.Cookies.Append("userdata", user, cookie);

            var users = _userManager.Users.Count();
            var products = _context.products.Count();
            var damagedProducts = _context.damegedproducts.Count();
            ViewBag.User = user;
            ViewBag.users = users;
            ViewBag.products = products;
            ViewBag.damagedProducts = damagedProducts;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        public IActionResult AddNewItem()
        {
            ViewBag.Products = _context.products;
            return View();
        }
        [Authorize]
        [Route("damaged-products")]
        public IActionResult DamagedProducts()
        {

            var damagedProducts = _context.damegedproducts
                          .Join(
                              _context.products,
                              damaged_products => damaged_products.ProductId,
                              products => products.Id,
                              (damaged_products, products) => new
                              {
                                  damaged_products.ProductId,
                                  products.Name,
                                  damaged_products.Qty
                              }
                          )
                          .Join(
                              _context.productsDetails,
                              dp => dp.ProductId,
                              products_details => products_details.ProductId,
                              (dp, products_details) => new
                              {
                                  dp.ProductId,
                                  dp.Name,
                                  products_details.Color,
                                  dp.Qty
                              }
                          )
                          .ToList();


            var products = _context.products.ToList();
            ViewBag.products = products;
            ViewBag.damagedProducts = damagedProducts;
            return View();
        }


        [Route("products-details")]
        public IActionResult ProductsDetails()
        {


            var productsDetails = _context.productsDetails.Join(
                    _context.products,
                    products_details => products_details.ProductId,
                    products => products.Id,

                    (products_details, products) => new
                    {
                        Product_Id = products_details.ProductId,
                        Name = products.Name,
                        Color = products_details.Color,
                        Price = products_details.Price,
                        Qty = products_details.Qty,
                        Images = products_details.Images
                    }

                ).ToList();

            var products = _context.products.ToList();
            ViewBag.products = products;
            ViewBag.productsDetails = productsDetails;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}