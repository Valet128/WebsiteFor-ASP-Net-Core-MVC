using ShvedovaAV.Models;
using ShvedovaAV.Services;
using ShvedovaAV.ViewModels;

namespace ShvedovaAV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContentController : Controller
    {
        private readonly ApplicationContext db;
        private readonly IWebHostEnvironment appEnvironment;
        public ContentController(ApplicationContext db, IWebHostEnvironment appEnvironment)
        {
            this.db = db;
            this.appEnvironment = appEnvironment;
        }
        public async Task<IActionResult> Content()
        {
            return View(await db.Contents.ToListAsync());
        }
        
        public IActionResult AddContent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddContent(ContentViewModel model)
        {
            if (model != null)
            {
                string fileExtension = Path.GetExtension(model.VideoFile.FileName);
                string wwwRootPath = appEnvironment.WebRootPath;
                string fileName = model.Name + "_" + DateTime.Now.ToString("yymmssfff") + fileExtension;

                string path = Path.Combine(wwwRootPath + "/Files/Videos/Content/", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.VideoFile.CopyToAsync(fileStream);
                }

                Content content = new Content()
                {
                    Name = model.Name,
                    Source = "Files/Videos/Content/" + fileName
                };

                db.Contents.Add(content);
                await db.SaveChangesAsync();
                return RedirectToAction("Content");
            }
            ViewBag.Message = "Ошибка, некорретные данные!";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteContent(int id)
        { 
            var content = db.Contents.FirstOrDefault(x => x.Id == id);
            if (content != null)
            {
                var videoPath = Path.Combine(appEnvironment.WebRootPath, content.Source);
                if (System.IO.File.Exists(videoPath))
                {
                    System.IO.File.Delete(videoPath);
                }
                db.Contents.Remove(content);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Content");
        }

        public async Task<IActionResult> AccessContent(int id)
        {
            ViewBag.Id = id;

            var users = await db.Users.ToListAsync();
            var contentAccesses = await db.ContentAccesses.ToListAsync();

            AccessViewModel access = new()
            {
                Users = users,
                ContentAccesses = contentAccesses
            };

            return View(access);
        }

        [HttpPost]
        public async Task<IActionResult> AccessContent(int id, int contentId)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.ContentAccesses.Add(new ContentAccess()
                {
                    ContentId = contentId, 
                    ExpirationDate = DateTime.Now 
                });
                await db.SaveChangesAsync();
                ViewBag.Id = contentId;

                var users = await db.Users.ToListAsync();
                var contentAccesses = await db.ContentAccesses.ToListAsync();

                AccessViewModel access = new()
                {
                    Users = users,
                    ContentAccesses = contentAccesses
                };

                return View(access);
            }
            ViewBag.Id = contentId;
            ViewBag.Message = "Ошибка!";
            return View();
        }
    }
}
