using ShvedovaAV.Models;
using ShvedovaAV.Services;
using ShvedovaAV.ViewModels;
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

        public async Task<IActionResult> SM()
        {
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync("info@shvedovaav.ru", "Регистрация", "test");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var products = await db.Products.ToListAsync();
            var sliders = await db.Sliders.ToListAsync();
            var feedbacks = await db.Feedbacks.ToListAsync();
            var models = new MainPageViewModel()
            {
                Products = products, Sliders = sliders, Feedbacks = feedbacks
            };
            return View(models);
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
        public IActionResult Accord()
        {
            return View();
        }
        public IActionResult Denial()
        {
            return View();
        }
        public IActionResult Confidence()
        {
            return View();
        }
        public IActionResult TermsOfUse()
        {
            return View();
        }
        public IActionResult BuyProduct(Product product)
        {
            return View(product);
        }
    }
}
