using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using the_kitchen_aspnet_core.Data;
using the_kitchen_aspnet_core.Models;

namespace the_kitchen_aspnet_core
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            ViewBag.action = "Cart";
            ViewBag.pageView = "Shopping Cart";
            var user = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Carts.Include(c => c.Products).Include(c => c.Products.Categories).Where(c => c.User == user);
            if (user == null)
            {
                
                return View(await applicationDbContext.ToListAsync());
            }
            
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> subQuantity(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (id == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            if (user.Id != cart.User.Id)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                if(cart.Quantity == 1)
                {
                    _context.Remove(cart);
                }
                else
                {
                    cart.Quantity -= 1;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(cart.CartId))
                {
                    return RedirectToAction("Index", "Cart");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", "Cart"); ;
        }

        public async Task<IActionResult> addQuantity(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Index", "Home");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (id == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            if(user.Id != cart.User.Id)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                cart.Quantity += 1;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(cart.CartId))
                {
                        return RedirectToAction("Index", "Cart");
                    }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", "Cart"); ;
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Products)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Cart/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,UserID,ProductId,Quantity,Timestamp")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", cart.ProductId);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", cart.UserID);
            return View(cart);
        }

        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", cart.ProductId);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", cart.UserID);
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,UserID,ProductId,Quantity,Timestamp")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", cart.ProductId);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", cart.UserID);
            return View(cart);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Products)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}
