using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using the_kitchen_aspnet_core.Data;
using the_kitchen_aspnet_core.Models;

namespace the_kitchen_aspnet_core
{

    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Products.Include(p => p.Categories);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var product = await applicationDbContext.Where(m => m.ProductId == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.action = "Product";
            ViewBag.pageView = product.Name;
            return View(product);
        }

        [Route("api/[controller]")]
        [HttpGet("api/[controller]/{id}")]
        public async Task<IActionResult> ProductsAsync(int? id)
        {
            var applicationDbContext = _context.Products.Include(p => p.Categories);
            if (id == null)
            {
                return Json(await applicationDbContext.ToListAsync());
            }

            var product = await applicationDbContext.Where(m => m.ProductId == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return Json(new EmptyResult());
            }
            return Json(product);
        }

    }
}
