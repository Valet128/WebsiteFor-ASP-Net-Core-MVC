using ShvedovaAV.Models;
using ShvedovaAV.Services;

namespace ShvedovaAV.Controllers
{
    [Authorize(Roles = "admin")]
    public class SendingController : Controller
    {
        private readonly ApplicationContext db;
        public SendingController(ApplicationContext db)
        {
            this.db = db;
        }
        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(string title, string message)
        {
            if (title != null && message != null)
            {
                var users = await db.Users.Where(u => u.Sendmail == true).ToListAsync();
                string[] emails = new string[users.Count()];
                for (int i = 0; i < users.Count(); i++)
                {
                    emails[i] = users[i].Email;
                }
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(emails, title, message);
                ViewBag.Message = "Почта отправлена!";
                return View();
            }
            ViewBag.Message = "Ошибка, введите название и текст для отправки!";
            return View();
        }
        
    }
}
