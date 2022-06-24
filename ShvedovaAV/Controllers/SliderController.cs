using ShvedovaAV.Models;
using ShvedovaAV.ViewModels;

namespace ShvedovaAV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly ApplicationContext db;
        private readonly IWebHostEnvironment appEnvironment;
        public SliderController(ApplicationContext db, IWebHostEnvironment appEnvironment)
        {
            this.db = db;
            this.appEnvironment = appEnvironment;
        }

        public IActionResult Sliders()
        {

            return View(db.Sliders.ToList());
        }
        public IActionResult AddSlider()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSlider(SliderViewModel model)
        {
            if (model != null)
            {
                if (model.ImageFile != null)
                {
                    string fileExtension = Path.GetExtension(model.ImageFile.FileName);
                    string wwwRootPath = appEnvironment.WebRootPath;
                    string fileName = model.Name + "_" + DateTime.Now.ToString("yymmssfff") + fileExtension;

                    string path = Path.Combine(wwwRootPath + "/Files/Images/Slider/", fileName);
                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    Slider Slider = new Slider
                    {
                        Name = model.Name,
                        Image = "Files/Images/Slider/" + fileName
                    };

                    db.Sliders.Add(Slider);
                }
                else
                {
                    Slider Slider = new Slider
                    {
                        Name = model.Name,
                        Image = "Image"
                    };

                    db.Sliders.Add(Slider);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Sliders");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            var Slider = await db.Sliders.FindAsync(id);
            if (Slider != null)
            {
                var imagePath = Path.Combine(appEnvironment.WebRootPath, Slider.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                db.Sliders.Remove(Slider);
                await db.SaveChangesAsync();
                return RedirectToAction("Sliders");
            }
            return NotFound();
        }
      
        public async Task<IActionResult> EditSlider(int? id)
        {
            if (id != null)
            {
                var Slider = await db.Sliders.FindAsync(id);
                if (Slider != null) 
                {
                    SliderViewModel viewModel = new() 
                    {
                        Id = Slider.Id,
                        Name = Slider.Name,
                        Image = Slider.Image,
                        ImageFile = Slider.ImageFile
                    };
                    return View(viewModel);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditSlider(Slider Slider)
        {
            if (Slider.ImageFile != null)
            {
                var imagePath = Path.Combine(appEnvironment.WebRootPath, Slider.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                string fileExtension = Path.GetExtension(Slider.ImageFile.FileName);
                string wwwRootPath = appEnvironment.WebRootPath;
                string fileName = Slider.Name + "_" + DateTime.Now.ToString("yymmssfff") + fileExtension;

                string path = Path.Combine(wwwRootPath + "/Files/Images/Slider/", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await Slider.ImageFile.CopyToAsync(fileStream);
                }
                Slider.Image = "Files/Images/Slider/" + fileName;
            }
            db.Sliders.Update(Slider);
            await db.SaveChangesAsync();
            return RedirectToAction("Sliders");
        }
    }
}
