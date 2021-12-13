using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using the_kitchen_aspnet_core.Data;

namespace the_kitchen_aspnet_core.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            var defaultUser = new ApplicationUser
            {
                UserName = "admin@localhost",
                Email = "admin@localhost",
                Firstname = "Admin",
                Lastname = "Default",
                EmailConfirmed = true,
            };

            if (_userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await _userManager.CreateAsync(defaultUser, "thekitchenadmin");
            }
            defaultUser = await _userManager.FindByEmailAsync("admin@localhost");
            if (!await _userManager.IsInRoleAsync(defaultUser, "Admin"))
            {
                await _userManager.AddToRoleAsync(defaultUser, "Admin");
            }
            return Json(new EmptyResult());
        }
    }
}
