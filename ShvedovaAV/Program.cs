using Microsoft.EntityFrameworkCore;
using ShvedovaAV.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
    
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllerRoute(
        name: "default",
        pattern: "{action=Index}/{id?}",
        defaults: new { controller ="Home"}
    );
app.MapControllerRoute(
        name: "account",
        pattern: "{action}/{id?}",
        defaults: new { controller = "Account" }
    );
app.MapControllerRoute(
        name: "admin",
        pattern: "{action}/{id?}",
        defaults: new { controller = "Admin" }
    );


app.Run();
