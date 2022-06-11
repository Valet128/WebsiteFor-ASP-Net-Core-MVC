using ShvedovaAV.Models;
using ShvedovaAV.Services;
using ShvedovaAV.ViewModels;
using System.Security.Claims;

namespace ShvedovaAV.Properties.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class ProfileController : Controller
    {
        ApplicationContext db;
        public ProfileController(ApplicationContext db)
        {
            this.db = db;
        }

        
        public IActionResult Profile()
        {
            Claim? userId = HttpContext.User.FindFirst("ClaimId");
            if (userId != null)
            {
                if(TempData["shortMessage"] != null) 
                {
                    ViewBag.Message = TempData["shortMessage"].ToString();
                }
                
                    return View();
            }
            return NotFound();
        }
        public async Task<IActionResult> Content(int id)
        {
            if (id != 0)
            {
                var content = await db.ContentAccesses.Where(c => c.UserId == id).Include(x=> x.Content).ToListAsync();
                if (content != null)
                {
                    return View(content);
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> Settings(int id)
        {
           var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            return View(user);
        }
        public async Task<IActionResult> Sendoff(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.Sendmail = false;
                await db.SaveChangesAsync();
                TempData["shortMessage"] = "Вы отписались от рассылки!";
                return RedirectToAction("Profile");
            }
            return NotFound();
        }
        public async Task<IActionResult> Subscribe(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.Sendmail = true;
                await db.SaveChangesAsync();
                TempData["shortMessage"] = "Вы подписались на рассылку!";
                return RedirectToAction("Profile");
            }
            return NotFound();
        }

        public IActionResult ChangePassword(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (model != null)
            {
                User? salt = await db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                if (salt == null)
                {
                    ViewBag.Message = "Ошибка, обратитесь в тех.поддержку!";
                    return View(model);
                }
                string password = salt.Salt.ToString() + model.OldPassword;
                
                var user = await db.Users.Where(u => u.Id == model.Id).FirstOrDefaultAsync(u => u.Password == HashService.GetHash(password));
                if (user == null)
                {
                    ViewBag.Message = "Неправильный пароль!";
                    return View(model);
                }
                string newPassword = salt.Salt.ToString() + model.NewPassword;
                user.Password = HashService.GetHash(newPassword);
                await db.SaveChangesAsync();
            }
            TempData["shortMessage"] = "Пароль успешно изменен!";
            return RedirectToAction("Profile");
        }


    }
}
