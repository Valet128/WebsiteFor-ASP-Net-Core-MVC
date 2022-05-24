using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShvedovaAV.Models;
using System.Drawing;
using System.Security.Claims;

namespace ShvedovaAV.Properties.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        public HomeController(ApplicationContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }
        public IActionResult Product(int id)
        {
            if (id != 0)
            {
                var product = db.Products.FirstOrDefault(p => p.Id == id);
                return View(product);
            }
            return NotFound();
        }
        public IActionResult Profile()
        {
            Claim? userId = HttpContext.User.FindFirst("ClaimId");
            if (userId != null)
            {
                    return View();
            }
            return NotFound();
            
        }
        public IActionResult Description()
        {
            return View();
        }
        public IActionResult BuyProduct(Product product)
        {
            return View(product);
        }
    }
}
