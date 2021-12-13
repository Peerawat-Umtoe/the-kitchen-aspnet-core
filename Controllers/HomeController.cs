using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using the_kitchen_aspnet_core.Data;
using the_kitchen_aspnet_core.Models;

namespace the_kitchen_aspnet_core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.action = "Home";
            var applicationDbContext = _context.Products.Include(p => p.Categories).Where(a => a.Amount > 0);
            return View(await applicationDbContext.ToListAsync());
        }

        [Route("[controller]/{category}")]
        public async Task<IActionResult> Index(String category)
        {
            var applicationDbContext = _context.Products.Include(p => p.Categories).Where(a => a.Amount > 0);
            var product = await applicationDbContext.ToListAsync();
            if (category == "CookwareAndCookingCutlery" || category == "1")
            {
                ViewBag.action = "CookwareAndCookingCutlery";
                ViewBag.pageView = "Cookware & Cooking cutlery";
                product = await applicationDbContext.Where(m => m.CategoryId == 1).Where(a => a.Amount > 0).ToListAsync();
            }
            else if (category == "DiningAndKitchenTextile" || category == "2")
            {
                ViewBag.action = "DiningAndKitchenTextile";
                ViewBag.pageView = "Dining & Kitchen Textile";
                product = await applicationDbContext.Where(m => m.CategoryId == 2).Where(a => a.Amount > 0).ToListAsync();
            }
            else if (category == "DiningWare" || category == "3")
            {
                ViewBag.action = "DiningWare";
                ViewBag.pageView = "Dining Ware";
                product = await applicationDbContext.Where(m => m.CategoryId == 3).Where(a => a.Amount > 0).ToListAsync();
            }
            else if (category == "FoodStorage" || category == "4")
            {
                ViewBag.action = "FoodStorage";
                ViewBag.pageView = "Food Storage";
                product = await applicationDbContext.Where(m => m.CategoryId == 4).Where(a => a.Amount > 0).ToListAsync();
            }
            else if (category == "KitchenStorageUtility" || category == "5")
            {
                ViewBag.action = "KitchenStorageUtility";
                ViewBag.pageView = "Kitchen Storage & Utility";
                product = await applicationDbContext.Where(m => m.CategoryId == 5).Where(a => a.Amount > 0).ToListAsync();
            }
            else
            {
                ViewBag.action = "Home";
            }

            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}