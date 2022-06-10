using ShvedovaAV.Models;
using ShvedovaAV.ViewModels;

namespace ShvedovaAV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
    {
        private readonly ApplicationContext db;
        private readonly IWebHostEnvironment appEnvironment;
        public FeedbackController(ApplicationContext db, IWebHostEnvironment appEnvironment)
        {
            this.db = db;
            this.appEnvironment = appEnvironment;
        }

        public IActionResult Feedbacks()
        {

            return View(db.Feedbacks.ToList());
        }
        public IActionResult AddFeedback()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback(FeedbackViewModel model)
        {
            if (model != null)
            {
                if (model.ImageFile != null)
                {
                    string fileExtension = Path.GetExtension(model.ImageFile.FileName);
                    string wwwRootPath = appEnvironment.WebRootPath;
                    string fileName = model.Name + "_" + DateTime.Now.ToString("yymmssfff") + fileExtension;

                    string path = Path.Combine(wwwRootPath + "/Files/Images/Feedback/", fileName);
                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    Feedback Feedback = new Feedback
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        Image = "Files/Images/Feedback/" + fileName
                    };

                    db.Feedbacks.Add(Feedback);
                }
                else
                {
                    Feedback Feedback = new Feedback
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        Image = "Image"
                    };

                    db.Feedbacks.Add(Feedback);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Feedbacks");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var Feedback = await db.Feedbacks.FindAsync(id);
            if (Feedback != null)
            {
                var imagePath = Path.Combine(appEnvironment.WebRootPath, Feedback.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                db.Feedbacks.Remove(Feedback);
                await db.SaveChangesAsync();
                return RedirectToAction("Feedbacks");
            }
            return NotFound();
        }
      
        public async Task<IActionResult> EditFeedback(int? id)
        {
            if (id != null)
            {
                var Feedback = await db.Feedbacks.FindAsync(id);
                if (Feedback != null) 
                {
                    FeedbackViewModel viewModel = new() 
                    {
                        Id = Feedback.Id,
                        Name = Feedback.Name,
                        Description = Feedback.Description,
                        Price = Feedback.Price,
                        Image = Feedback.Image,
                        ImageFile = Feedback.ImageFile
                    };
                    return View(viewModel);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditFeedback(Feedback Feedback)
        {
            if (Feedback.ImageFile != null)
            {
                var imagePath = Path.Combine(appEnvironment.WebRootPath, Feedback.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                string fileExtension = Path.GetExtension(Feedback.ImageFile.FileName);
                string wwwRootPath = appEnvironment.WebRootPath;
                string fileName = Feedback.Name + "_" + DateTime.Now.ToString("yymmssfff") + fileExtension;

                string path = Path.Combine(wwwRootPath + "/Files/Images/Feedback/", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await Feedback.ImageFile.CopyToAsync(fileStream);
                }
                Feedback.Image = "Files/Images/Feedback/" + fileName;
            }
            db.Feedbacks.Update(Feedback);
            await db.SaveChangesAsync();
            return RedirectToAction("Feedbacks");
        }
    }
}
