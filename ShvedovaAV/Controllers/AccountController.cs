using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShvedovaAV.Models;
using ShvedovaAV.Services;
using ShvedovaAV.ViewModels;
using System.Security.Claims;


namespace ShvedovaAV.Controllers
{
    public class AccountController : Controller
    {
        ApplicationContext db;
        
        public AccountController(ApplicationContext db)
        {
            this.db = db;
        }
       
        
        public IActionResult Registration()
        {
            return View();
        }
    
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (model != null)
            {
                var email = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (email != null)
                {
                    ViewBag.Message = "Пользователь с таким Email уже существует!";
                    return View(model);
                }
            }
            if (ModelState.IsValid)
            {
                User user = new User { 
                    Name = model.Name,
                    Email = model.Email,
                    Phone = ""
                };
                string password = user.Salt.ToString() + model.Password;
                var passwordHash = HashService.GetHash(password);
                user.Password = passwordHash;
                await db.Roles.ToListAsync();
                db.Users.Add(user);
                await db.SaveChangesAsync();
                var claims = new List<Claim>
                {
                    new Claim("ClaimId", user.Id.ToString()),
                    new Claim("ClaimName", user.Name),
                    new Claim("ClaimPhone", user.Phone),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                //string textMessage = $"{user.Name}, Вы зарегистрировались на сайте shvedovaav.ru!\nЗаходите к нам почаще!";
                //await EmailService.SendEmailAsync("krskagent@mail.ru", "Регистрация", textMessage);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Profile", "Profile");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            User? salt = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (salt == null) 
            {
                ViewData["Message"] = "Неправильный логин или пароль!";
                return View(model);
            }
            string password = salt.Salt.ToString() + model.Password;
            User? user = await db.Users.Include(r=> r.Role).FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == HashService.GetHash(password));
            if (user != null)
            {
                
                var claims = new List<Claim>
                {
                    new Claim("ClaimId", user.Id.ToString()),
                    new Claim("ClaimName", user.Name),
                    new Claim("ClaimPhone", user.Phone),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Profile", "Profile");
            }
            ViewData["Message"] = "Неправильный логин или пароль!";
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
       
    }
}
