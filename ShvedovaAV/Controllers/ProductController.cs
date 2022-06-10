using ShvedovaAV.Models;
using ShvedovaAV.ViewModels;

namespace ShvedovaAV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationContext db;
        private readonly IWebHostEnvironment appEnvironment;
        public ProductController(ApplicationContext db, IWebHostEnvironment appEnvironment)
        {
            this.db = db;
            this.appEnvironment = appEnvironment;
        }
        public IActionResult Products()
        {
            return View(db.Products.ToList());
        }
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (model != null)
            {
                if (model.ImageFile != null)
                {
                    string fileExtension = Path.GetExtension(model.ImageFile.FileName);
                    string wwwRootPath = appEnvironment.WebRootPath;
                    string fileName = model.Name + "_" + DateTime.Now.ToString("yymmssfff") + fileExtension;

                    string path = Path.Combine(wwwRootPath + "/Files/Images/Product/", fileName);
                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    Product product = new Product
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        Category = model.Category,
                        Image = "Files/Images/Product/" + fileName
                    };

                    db.Products.Add(product);
                }
                else
                {
                    Product product = new Product
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        Category = model.Category,
                        Image = "Image"
                    };

                    db.Products.Add(product);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Products");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product != null)
            {
                var imagePath = Path.Combine(appEnvironment.WebRootPath, product.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Products");
            }
            return NotFound();
        }

        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id != null)
            {
                var product = await db.Products.FindAsync(id);
                if (product != null)
                {
                    ProductViewModel viewModel = new()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Category = product.Category,
                        Image = product.Image,
                        ImageFile = product.ImageFile
                    };
                    return View(viewModel);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (product.ImageFile != null)
            {
                var imagePath = Path.Combine(appEnvironment.WebRootPath, product.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                string fileExtension = Path.GetExtension(product.ImageFile.FileName);
                string wwwRootPath = appEnvironment.WebRootPath;
                string fileName = product.Name + "_" + DateTime.Now.ToString("yymmssfff") + fileExtension;

                string path = Path.Combine(wwwRootPath + "/Files/Images/Product/", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }
                product.Image = "Files/Images/Product/" + fileName;
            }
            db.Products.Update(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Products");
        }
    }
}
