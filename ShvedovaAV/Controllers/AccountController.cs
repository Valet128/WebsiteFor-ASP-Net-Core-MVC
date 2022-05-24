using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            EmailService emailService = new EmailService();
            if (ModelState.IsValid)
            {
                User user = new User { Name = model.Name, Email = model.Email, Password = model.Password };
                user.Role = new Role { Name = "user"};
                db.Users.Add(user);
                db.Roles.Add(user.Role);
                await db.SaveChangesAsync();
                var claims = new List<Claim>
                {
                    new Claim("ClaimId", user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                string textMessage = $"{user.Name}, Вы зарегистрировались на сайте shvedovaav.ru!\nЗаходите к нам почаще!";
                //await emailService.SendEmailAsync("krskagent@mail.ru", "Регистрация", textMessage);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Profile", "Home");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            User? logUser = db.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (logUser != null)
            {
                var role = db.Users.Include(r => r.Role);
                var userRole = role.FirstOrDefault(r => r.Id == logUser.Id);
                var claims = new List<Claim>
                {
                    new Claim("ClaimId", logUser.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, logUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole.Role.Name)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Profile", "Home");
            }
            return NotFound();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
       
    }
}
